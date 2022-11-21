using MySql.Data.MySqlClient;
using Packet;
using System.Net;
using System.Net.Sockets;

MMORpgmakerServer.Server s = new MMORpgmakerServer.Server();
s.Main();



namespace MMORpgmakerServer
{

    public class Server
    {
        public TcpListener tcpListener;
        public Thread listenThread;


        string host_ip;
        int host_port = 6500;
        string mysql_host = "";
        int mysql_host_port = 3306;
        string mysql_user = "";
        string mysql_pass = "";
        string mysql_db = "";
        ConsoleColor Fore = ConsoleColor.Red;
        ConsoleColor Back = ConsoleColor.Black;

        static MySqlConnection cn;
        const int PacketSize = 120;


        PacketData p = new PacketData();
        NetworkStream clientStream;
        public List<Cliente> Clients = new List<Cliente>();

        /// <summary>
        /// Client Structure
        /// </summary>
        public struct Cliente
        {
            public short id;
            public string ip;
            public TcpClient sock;
            public string username;
            public int accid;
            public int char_id;
            public string map_loc;
            public float pos_x;
            public float pos_y;
        }


        public void Main()
        {
            //connessione = "server=" + host + ";uid=" + user + ";pwd=" + psw + ";database=" + db;
            Back = Console.BackgroundColor;
            Fore = Console.ForegroundColor;

            Crediti();

            CheckConfig();
            

            Notice("Preparing Server");

            ShowPacketVer();

            MySqlConnection conn;
            string connection = $"server={mysql_host};uid={mysql_user};pwd={mysql_pass};database={mysql_db}";
            cn = new MySqlConnection();
            cn.ConnectionString = connection;

            try
            {
                cn.Open();
            }
            catch
            {
                Error($"Can't connect to MySQL server on IP: {host_ip}:{host_port}");
            }


            IPAddress ip = IPAddress.Any;
            // IPEndPoint host = new IPEndPoint(IPAddress.Parse("25.87.68.41"), 1234);
            IPEndPoint host = new IPEndPoint(IPAddress.Parse(host_ip), host_port);
            tcpListener = new TcpListener(host);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();

            //ConnectToCoreClient();

            Console.ReadKey();
        }



        /// <summary>
        /// Listen all connection
        /// and separe socket in new Thread
        /// </summary>
        public void ListenForClients()
        {
            tcpListener.Start();

            Notice("Server is Online");
            GetUserOnline();

            while (true)
            {

                tcpListener.Start();
                //blocks until a client has connected to the server
                TcpClient client = tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
                Notice("Client Connesso...");


            }


        }


        /// <summary>
        /// Listening clients on Thread
        /// </summary>
        /// <param name="client">Client Object</param>
        public void HandleClientComm(object client)
        {
            bool whilestatus = true;
            TcpClient tcpClient = (TcpClient)client;
            clientStream = tcpClient.GetStream();
            string clientIP = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();


            Cliente c = new Cliente();
            c.sock = tcpClient;
            c.id = tcpClient.Client.Ttl;
            c.ip = clientIP;
            c.pos_x = 100;
            c.pos_y = 100;
            c.map_loc = "lobby";
            Clients.Add(c);



            Notice($"Connessione accettata da parte di [{clientIP}]");
            Notice($"User Connected: {Clients.Count}");

            while (whilestatus)
            {
                try
                {

                    byte[] data = new byte[110]; //120 byte 
                    int byteres = clientStream.Read(data, 0, data.Length);

                    object t = DisassemblyPacket(data);

                    clientStream.Flush();

                    #region PacketData

                    if (t.GetType() == typeof(PacketData))
                    {
                        #region ACT LOGIN
                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_LOGIN)
                        {
                            Notice("Tentativo di acceso da parte di '" + ((PacketData)t).Argument1 + "'   IP: [ " + clientIP + " ]");

                            bool check = Login(((PacketData)t).Argument1, ((PacketData)t).Argument2);

                            if (check)
                            {
                                PacketData p = new PacketData();
                                p.Command = (uint)PacketHeader.HeaderCommand.ACT_LOGIN;
                                p.Argument1 = "true";

                                data = p.Serialize();
                                data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);

                                clientStream.Flush();
                                clientStream.Write(data, 0, data.Length);

                                Notice("Accesso eseguito da " + ((PacketData)t).Argument1);
                            }
                            else
                            {
                                clientStream.WriteByte(0x02);
                                Warining("Accesso rifiutato per '" + ((PacketData)t).Argument1 + "'    IP: [ " + clientIP + " ]");
                            }
                        }
                        #endregion



  

        
                    }


                    #endregion




                    Console.WriteLine();


                }
                catch
                {

                    var cli = Clients.Where(x => x.ip == clientIP).FirstOrDefault();
                    Clients.Remove(cli);
                    Notice($"TTL: {cli.id} [{cli.ip}] Client Disconnesso...");
                    tcpClient.Close();
                    //Thread.CurrentThread.Abort();
                    whilestatus = false;
                    Notice($"User Connected: {Clients.Count}");


                }


            }

            Thread.CurrentThread.Interrupt();

        }


        public bool Login(string user, string pass)
        {
            bool val = false;
            try
            {

                MySqlConnection conn;
                string connection = $"server={mysql_host};uid={mysql_user};pwd={mysql_pass};database={mysql_db}";

                conn = new MySqlConnection(connection);
                conn.Open();

                MySqlCommand cmd;
                cmd = new MySqlCommand($"select * from login where username='{user}';", conn);
                MySqlDataReader rd = cmd.ExecuteReader();

                rd.Read();

                if (rd.GetString("username") == user && rd.GetString("password") == pass)
                {
                    val = true;

                }




                conn.Close();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                val = false;
            }

            return val;
        }


        /// <summary>
        /// EN
        /// Assembly a Packet inser on Header at offset 0
        /// type of packet assembled
        /// IT
        /// Assembla un Pacchetto inserendo nell'Header a posizione 0
        /// il tipo di pacchetto che viene assemblato
        /// </summary>
        /// <param name="type">Tipo di pacchetto</param>
        /// <param name="data">array serializato</param>
        /// <returns>Array Serializzato con Header</returns>
        byte[] AssemblyPacket(PacketHeader.PacketType type, byte[] data)
        {

            byte[] final_send = new byte[data.Length + 1];

            Array.Copy(data, 0, final_send, 1, final_send.Length - 1);

            //Assign type of packet
            final_send[0] = (byte)type;

            return final_send;
        }


        /// <summary>
        /// EN
        /// Disassemble a packet of bytes
        /// Then get type and return structure
        /// IT
        /// Disassembla un pacchetto di bytes
        /// Ne ricava il tipo e restutisce la struttura
        /// </summary>
        /// <param name="data">Buffer</param>
        /// <returns>Tipo Disassemblato in Object</returns>
        object DisassemblyPacket(byte[] data)
        {

            object ret = null;
            byte[] unpacked = new byte[120];

            if (data[0] == (byte)PacketHeader.PacketType.PacketData)
            {

                Array.Copy(data, 1, unpacked, 0, data.Length - 1);
                ret = new PacketData();
                PacketData p = new PacketData();
                p.Deserialize(ref unpacked);

                ret = new PacketData();
                ret = p;

            }

            if (data[0] == (byte)PacketHeader.PacketType.CharPacket)
            {
                Array.Copy(data, 1, unpacked, 0, data.Length - 1);
                ret = new CharPaket();
                CharPaket p = new CharPaket();
                p.Deserialize(ref unpacked);

                ret = new CharPaket();
                ret = p;
            }



            return ret;
        }


        /// <summary>
        /// Checking file integration
        /// </summary>
        public void CheckConfig()
        {
            Console.WriteLine("");
            Notice("Checking configuration file...");
            string r;

            if (!File.Exists(Environment.CurrentDirectory + @"\data\config.txt"))
            {

                Error("Cannot find main configuration file...");
                Warining("Want to configure now the server?");

                Console.Write("[Yes] or [No] type Y / N: ");
                r = Console.ReadLine();

                if (r.ToLower() == "y")
                {
                    ConfigureServer();
                }
                if (r.ToLower() == "n")
                {
                    Environment.Exit(0);
                }

                if (r.ToLower() != "y" || r.ToLower() != "n")
                {
                    Environment.Exit(0);
                }
            }

            #region Configuration File

            try
            {
                using (var reader = new StreamReader(Environment.CurrentDirectory + "\\data\\config.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] st = line.Split(':');

                        switch (st[0]) //Read configuration file
                        {
                            case "server_ip":
                                host_ip = st[1];
                                break;
                            case "server_port":
                                host_port = int.Parse(st[1]);
                                break;
                            case "mysql_ip":
                                mysql_host = st[1];
                                break;
                            case "mysql_port":
                                mysql_host_port = int.Parse(st[1]);
                                break;
                            case " mysql_db":
                                mysql_db = st[1];
                                break;
                            case "mysql_user":
                                mysql_user = st[1];
                                break;
                            case "mysq_pass":
                                mysql_pass = st[1];
                                break;
                        }
                    }

                }

                Notice("Configuratioin loaded.");
                Notice("Ready...");

            }
            catch
            {
                Error("Damaged main configuration file.");
                Warining("Want to configure now the server?");
                Console.Write("[Yes] or [No] type Y / N: ");
                r = Console.ReadLine();

                if (r.ToLower() == "y")
                {
                    ConfigureServer();
                }
                if (r.ToLower() == "n")
                {
                    Environment.Exit(0);
                }

                if (r.ToLower() != "y" || r.ToLower() != "n")
                {
                    Environment.Exit(0);
                }
            }

            #endregion

        }



        /// <summary>
        /// Configure the server at First Start
        /// </summary>
        public void ConfigureServer()
        {
            Notice("Configuring Mode... press any key to continue.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(" Plase input ip address to remote host (default: 127.0.0.1)");
            Console.Write(" IP:> ");
            string _ip = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Plase input port to host (default: 6400)");
            Console.Write(" Port:> ");
            int port = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            Notice(" Host server completed.");
            Console.WriteLine(" Configuring MySQL Connection");
            Console.WriteLine(" Press any key to continue...");

            Console.ReadKey();
            Console.Clear();
            //-------  MySQL
            confsql:
            Console.WriteLine(" Plase input ip address to remote MySQL Host (default: 127.0.0.1)");
            Console.Write(" IP:> ");
            string _ipsql = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Plase input port to host (default port is: 3306)");
            Console.Write(" Port:> ");
            int portsql = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            Console.WriteLine(" Plase input Username for MySQL");
            Console.Write(" User:> ");
            string user = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Plase input Password for MySQL");
            Console.Write(" Password:> ");
            string pass = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Plase input Database name for MMORpgmaker");
            Console.Write(" Database:> ");
            string db = Console.ReadLine();
            Console.WriteLine("");

            Notice("MySQL Configuration Completed.");
            Notice("Trying connect to MySQL Server");
            Notice("Press any key to continues");
            Console.ReadKey();

            MySqlConnection conn;
            string connection = $"server={_ipsql};uid={user};pwd={pass};database={db}";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connection;
                conn.Open();
                Notice("Connection Successfull!");

                Notice("Checking Table...");
                try
                {
                    MySqlCommand cmd = new MySqlCommand("select * from login where id=0", conn);
                    var r = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Error(ex.Message);
                    Console.ReadKey();
                    Notice("Creating database table..");
                    CreateTable(conn, connection);
                }


            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Error(ex.Message);
                Console.WriteLine("");
                Console.WriteLine("Plase. configure again and check username or password");
                Console.ReadKey();
                Console.Clear();
                goto confsql;
            }


            Notice("Writing configuration files...");

            StreamWriter sw;
            
                File.Create(Environment.CurrentDirectory + @"\data\config.txt");
                sw = new StreamWriter(Environment.CurrentDirectory + @"\data\config.txt");
           

            sw.WriteLine($"server_ip:{_ip}");
            sw.WriteLine($"server_port:{port}");
            sw.WriteLine($"mysql_ip:{_ipsql}");
            sw.WriteLine($"mysql_port:{portsql}");
            sw.WriteLine($"mysql_db:{db}");
            sw.WriteLine($"mysql_user:{user}");
            sw.WriteLine($"mysq_pass:{pass}");
            sw.Close();

            Console.ReadKey();

            Warining("For apply all changes, need to restart server. Playse restart the server");
            Environment.Exit(0);


        }



        /// <summary>
        /// Creating a MySQL Data Table
        /// </summary>
        /// <param name="conn">MySQL Connection</param>
        /// <param name="connection">Connection String</param>
        public void CreateTable(MySqlConnection conn, string connection)
        {
            string sq = File.ReadAllText(Environment.CurrentDirectory + @"\data\login.sql");

            MySqlCommand cmd;
            cmd = new MySqlCommand(sq, conn);
            cmd.ExecuteNonQuery();

            Notice("TABLE [ login ] created.");
            cmd = new MySqlCommand("insert into login (username,password,email,level) VALUES ('server','server','server@server',99);", conn);
            cmd.ExecuteNonQuery();
            Notice("Default account 'Server' created.");

            sq = File.ReadAllText(Environment.CurrentDirectory + @"\data\chars.sql");
            cmd = new MySqlCommand(sq, conn);
            cmd.ExecuteNonQuery();
            Notice("Table [ Chars ] created.");

            Console.ReadKey();

            conn.Close();
        }


        /// <summary>
        /// Show Main Credits
        /// </summary>
        public void Crediti()
        {
            ConsoleColor c = Console.ForegroundColor;
            ConsoleColor bg = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;

            Console.WriteLine(@"                                                                                            ");
            Console.WriteLine(@"   _____      _____   ________ __________                               __                  ");
            Console.WriteLine(@"  /     \    /     \  \_____  \\______   \______   ____   _____ _____  |  | __ ___________  ");
            Console.WriteLine(@" /  \ /  \  /  \ /  \  /   |   \|       _/\____ \ / ___\ /     \\__  \ |  |/ // __ \_  __ \ ");
            Console.WriteLine(@"/    Y    \/    Y    \/    |    \    |   \|  |_> > /_/  >  Y Y  \/ __ \|    <\  ___/|  | \/ ");
            Console.WriteLine(@"\____|__  /\____|__  /\_______  /____|_  /|   __/\___  /|__|_|  (____  /__|_ \\___  >__|    ");
            Console.WriteLine(@"        \/         \/         \/       \/ |__|  /_____/       \/     \/     \/    \/        ");
            Console.WriteLine(@"                                                                                            ");
            Console.WriteLine(@" By Thejuster - (C) Copyright Making Italia                                                 ");
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(@$" Core Server  - Hostin to {host_ip} : {host_port}");
            Console.WriteLine("");

            Console.ForegroundColor = c;
            Console.BackgroundColor = bg;

        }


        /// <summary>
        /// Print Number of all Clients Connected
        /// </summary>
        public void GetUserOnline()
        {
            Console.WriteLine("User Connected: " + Clients.Count);
        }


        /// <summary>
        /// Display a Notice Message
        /// </summary>
        /// <param name="msg">Message</param>
        public void Notice(string msg)
        {

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[NOTICE]: ");
            Console.ForegroundColor = Fore;
            Console.WriteLine(msg);
        }


        /// <summary>
        /// Display a Debug Message
        /// </summary>
        /// <param name="msg">Message</param>
        public void Debug(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.Write("[DEBUG]: ");
            Console.ForegroundColor = Fore;
            Console.WriteLine(msg);
        }


        /// <summary>
        /// Show PacketVersion for the server of PacketData
        /// </summary>
        public void ShowPacketVer()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            string ss = string.Join(",", Enum.GetNames(typeof(Packet.PacketHeader.HeaderCommand)));

            Packet.PacketHeader.HeaderCommand packet = new Packet.PacketHeader.HeaderCommand();
            foreach (var s in typeof(Packet.PacketHeader.HeaderCommand).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                Console.WriteLine("{0} = {1}", s.Name, s.GetValue(packet));
            }

            Console.WriteLine("[Packet Version]: " + "0x" + ss.Length);
            Console.ForegroundColor = Fore;
            Console.WriteLine();
        }


        /// <summary>
        /// Display a Debug Message
        /// </summary>
        /// <param name="msg">Message</param>
        public void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write("[ERROR]: ");
            Console.ForegroundColor = Fore;
            Console.WriteLine(msg);
        }


        /// <summary>
        /// Display a Debug Message
        /// </summary>
        /// <param name="msg">Message</param>
        public void Warining(string msg)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write(@"[/!\ WARNING]: ");
            Console.ForegroundColor = Fore;
            Console.WriteLine(msg);
        }



    }

}

