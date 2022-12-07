using MySql.Data.MySqlClient;
using Packet;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

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
        string mysql_host = "127.0.0.1";
        int mysql_host_port = 3306;
        string mysql_user = "root";
        string mysql_pass = "";
        string mysql_db = "mmorpgmaker";
        ConsoleColor Fore = ConsoleColor.Red;
        ConsoleColor Back = ConsoleColor.Black;
        bool TXTServer = false;
        static MySqlConnection cn;
        const int PacketSize = 120;


        PacketData p = new PacketData();
        NetworkStream clientStream;
        public List<Cliente> Clients = new List<Cliente>();


        public struct account
        {
            public int id;
            public string username;
            public string password;
            public string email;
            public string level;
        }
        public List<account> Account = new List<account>();
        
        public struct chars
        {
            public int char_id;
            public int account_id;
            public int char_num;
            public string name;
            public string char_class;
            public int base_leve;
            public int job_level;
            public int base_exp;
            public int job_exp;
            public int gold;
            public int str;
            public int agi;
            public int vit;
            public int ints;
            public int dex;
            public int luk;
            public int max_hp, hp;
            public int max_sp, sp;
            public int status_point;
            public int skill_point;
            public string option;
            public string karma;
            public int party_id;
            public int guild_id;
            public int pet_id;
            public int body, weapon, shield, head, robe, accessory1, accessory2;
            public string last_map;
            public int last_x, last_y;
            public string save_map;
            public int save_x, save_y;
            public int partner_id;
            public bool online;
            public int unban_time;
            public string sex;
            public int clan_id;
            public bool show_equip;
        }

        public List<chars> Chars = new List<chars>();

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
                Warining("Switching to TXT Server Mode");
                LoadTXTData();
                TXTServer = true;
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
        /// Loading TXT Data file
        /// </summary>
        public void LoadTXTData()
        {
            //Loading Accounts
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/data/accounts.txt"))
                {
                    account acc = new account();
                    string ln;
                    while ((ln = sr.ReadLine()) != null)
                    {
                        if (ln.StartsWith("//"))
                            continue;

                        string[] data = ln.Split(',');
                        acc.id = int.Parse(data[0]);
                        acc.username = data[1];
                        acc.password = data[2];
                        acc.email = data[3];
                        acc.level = data[4];
                        Account.Add(acc);
                    }
                } 
            }

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                using (StreamReader sr = new StreamReader("data/accounts.txt"))
                {
                    account acc = new account();
                    string ln;
                    while ((ln = sr.ReadLine()) != null)
                    {
                        if (ln.StartsWith("//"))
                            continue;

                        string[] data = ln.Split(',');
                        acc.id = int.Parse(data[0]);
                        acc.username = data[1];
                        acc.password = data[2];
                        acc.email = data[3];
                        acc.level = data[4];
                        Account.Add(acc);
                    }
                }
            }

            //Loading Chars
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/data/chars.txt"))
                {
                    chars c = new chars();
                    string ln;
                    while ((ln = sr.ReadLine()) != null)
                    {
                        if (ln.StartsWith("//"))
                            continue;

                        if (ln.Length < 10)
                            continue;

                        if (ln.StartsWith(";"))
                            continue;
                        string[] linee = ln.Split(';');

                        foreach (var s in linee)
                        {
                            string[] data = s.Split(',');
                            if (data[0].Length <= 0)
                                continue;

                            //char_id,account_id,char_num,name,class,base_level,job_level
                            c.char_id = int.Parse(data[0]);
                            c.account_id = int.Parse(data[1]);
                            c.char_num = int.Parse(data[2]);
                            c.name = data[3];
                            c.char_class = data[4];
                            c.base_leve = int.Parse(data[5]);
                            c.job_level = int.Parse(data[6]);
                            c.base_exp = int.Parse(data[7]);
                            c.job_exp = int.Parse(data[8]);
                            c.gold = int.Parse(data[9]);
                            c.str = int.Parse(data[10]);
                            c.agi = int.Parse(data[11]);
                            c.vit = int.Parse(data[12]);
                            c.ints = int.Parse(data[13]);
                            c.dex = int.Parse(data[14]);
                            c.luk = int.Parse(data[15]);
                            c.max_hp = int.Parse(data[16]);
                            c.hp = int.Parse(data[17]);
                            c.max_sp = int.Parse(data[18]);
                            c.sp = int.Parse(data[19]);
                            c.status_point = int.Parse(data[20]);
                            c.skill_point = int.Parse(data[21]);
                            c.option = data[22];
                            c.karma = data[23];
                            c.party_id = int.Parse(data[24]);
                            c.guild_id = int.Parse(data[25]);
                            c.pet_id = int.Parse(data[26]);
                            c.body = int.Parse(data[27]);
                            c.weapon = int.Parse(data[28]);
                            c.shield = int.Parse(data[29]);
                            c.head = int.Parse(data[30]);
                            c.robe = int.Parse(data[31]);
                            c.accessory1 = int.Parse(data[32]);
                            c.accessory2 = int.Parse(data[33]);
                            c.last_map = data[34];
                            c.last_x = int.Parse(data[35]);
                            c.last_y = int.Parse(data[36]);
                            c.save_map = data[37];
                            c.save_x = int.Parse(data[38]);
                            c.save_y = int.Parse(data[39]);
                            c.partner_id = int.Parse(data[40]);
                            c.online = bool.Parse(data[41]);
                            c.unban_time = int.Parse(data[42]);
                            c.sex = data[43];
                            c.clan_id = int.Parse(data[44]);
                            c.show_equip = bool.Parse(data[45]);

                            Chars.Add(c);
                        }

                    }
                }
            }

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "data/chars.txt"))
                {
                    chars c = new chars();
                    string ln;
                    while ((ln = sr.ReadLine()) != null)
                    {
                        if (ln.StartsWith("//"))
                            continue;

                        if (ln.Length < 10)
                            continue;

                        if (ln.StartsWith(";"))
                            continue;
                        string[] linee = ln.Split(';');

                        foreach (var s in linee)
                        {
                            string[] data = s.Split(',');
                            if (data[0].Length <= 0)
                                continue;

                            //char_id,account_id,char_num,name,class,base_level,job_level
                            c.char_id = int.Parse(data[0]);
                            c.account_id = int.Parse(data[1]);
                            c.char_num = int.Parse(data[2]);
                            c.name = data[3];
                            c.char_class = data[4];
                            c.base_leve = int.Parse(data[5]);
                            c.job_level = int.Parse(data[6]);
                            c.base_exp = int.Parse(data[7]);
                            c.job_exp = int.Parse(data[8]);
                            c.gold = int.Parse(data[9]);
                            c.str = int.Parse(data[10]);
                            c.agi = int.Parse(data[11]);
                            c.vit = int.Parse(data[12]);
                            c.ints = int.Parse(data[13]);
                            c.dex = int.Parse(data[14]);
                            c.luk = int.Parse(data[15]);
                            c.max_hp = int.Parse(data[16]);
                            c.hp = int.Parse(data[17]);
                            c.max_sp = int.Parse(data[18]);
                            c.sp = int.Parse(data[19]);
                            c.status_point = int.Parse(data[20]);
                            c.skill_point = int.Parse(data[21]);
                            c.option = data[22];
                            c.karma = data[23];
                            c.party_id = int.Parse(data[24]);
                            c.guild_id = int.Parse(data[25]);
                            c.pet_id = int.Parse(data[26]);
                            c.body = int.Parse(data[27]);
                            c.weapon = int.Parse(data[28]);
                            c.shield = int.Parse(data[29]);
                            c.head = int.Parse(data[30]);
                            c.robe = int.Parse(data[31]);
                            c.accessory1 = int.Parse(data[32]);
                            c.accessory2 = int.Parse(data[33]);
                            c.last_map = data[34];
                            c.last_x = int.Parse(data[35]);
                            c.last_y = int.Parse(data[36]);
                            c.save_map = data[37];
                            c.save_x = int.Parse(data[38]);
                            c.save_y = int.Parse(data[39]);
                            c.partner_id = int.Parse(data[40]);
                            c.online = bool.Parse(data[41]);
                            c.unban_time = int.Parse(data[42]);
                            c.sex = data[43];
                            c.clan_id = int.Parse(data[44]);
                            c.show_equip = bool.Parse(data[45]);

                            Chars.Add(c);
                        }

                    }
                }
            }

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
                if (!TXTServer) //if server is in SQL
                {
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                    Notice("Client Connected...");
                }else
                {
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientTXT));
                    clientThread.Start(client);
                    Notice("Client Connected...");
                }

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

                            bool check = LoginSQL(((PacketData)t).Argument1, ((PacketData)t).Argument2);

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
                                PacketData p = new PacketData();
                                p.Command = (uint)PacketHeader.HeaderCommand.ACT_LOGIN;
                                p.Argument1 = "false";
                                data = p.Serialize();
                                data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);

                                clientStream.Flush();
                                clientStream.Write(data, 0, data.Length);
                                //clientStream.WriteByte(0x02);
                                Warining("Accesso rifiutato per '" + ((PacketData)t).Argument1 + "'    IP: [ " + clientIP + " ]");
                            }
                        }
                        #endregion


                        #region GET ACCOUNT ID
                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_ACC_ID)
                        {
#if DEBUG
                            Debug("Richiesta di ottenere account id con user " + ((PacketData)t).Argument1);
#endif
                            try
                            {
                                //----
                                MySqlCommand cmd;
                                cmd = new MySqlCommand($"select * from login where username='{((PacketData)t).Argument1}';", cn);
                                MySqlDataReader rd = cmd.ExecuteReader();
                                rd.Read();
                                //----

                                p = new Packet.PacketData();
                                p.Command = (uint)Packet.PacketHeader.HeaderCommand.ACT_REC_ACC_ID;
                                p.Argument1 = rd.GetInt16("id").ToString();

                                data = p.Serialize();
                                data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);

                                rd.Close();

                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();

                            }
                            catch (Exception ex)
                            {
                                Error(ex.Message);
                            }

                        }

                        #endregion

                        #region ACT_COUNT_CHAR

                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_COUNT_CHAR)
                        {

                            MySqlCommand cmd;
                            cmd = new MySqlCommand($"select * from chars where account_id={int.Parse(((PacketData)t).Argument1)};", cn);

                            try
                            {
                                MySqlDataReader rd = cmd.ExecuteReader();
                           

                            int count = 0;
                            string char_id = "";
                            while (rd.Read())
                            {
                                count++;
                                char_id = char_id + rd.GetInt16("char_num") + ",";
                            }

                            rd.Close();

                            p = new PacketData();
                            p.Command = (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_CHARCOUNT;
                            p.Argument1 = count.ToString();
                            p.Argument2 = char_id;

                            data = p.Serialize();

                            data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);
                            clientStream.Write(data, 0, data.Length);
                            clientStream.Flush();

                            }
                            catch (Exception a)
                            {
                                Error(a.Message);
                            }
                        }

                        #endregion


                        if(((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_ALL_CHAR)
                        {
                            try
                            {
                                MySqlCommand cmd;
                                cmd = new MySqlCommand($"select * from chars where account_id={int.Parse(((PacketData)t).Argument1)};", cn);
                                MySqlDataReader rd = cmd.ExecuteReader();
                                string dt = "";
                                while(rd.Read())
                                {
                                    dt = dt + rd.GetInt16("char_num") + ",";
                                }

                                rd.Close();
                                rd.Dispose();
                                cmd.Dispose();

                            }
                            catch(Exception ex)
                            {
                                Error(ex.Message);
                                CharPaket ch = new CharPaket();
                                ch.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;
                                data = ch.Serialize();

                                data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);

                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();
                            }
                        }

                        #region ACT_GET_CHAR
                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_CHAR)
                        {
                            try
                            {
                                MySqlCommand cmd;
                                cmd = new MySqlCommand($"select * from chars where account_id={int.Parse(((PacketData)t).Argument1)};", cn);
                                MySqlDataReader rd = cmd.ExecuteReader();
                                while (rd.Read())
                                {
                                    if (rd.GetInt16("char_num") == Convert.ToInt16(((PacketData)t).Argument2))
                                    {
                                        CharPaket ch = new CharPaket();
                                        ch.Command = 0;
                                        ch.CharNum = rd.GetInt16("char_num");
                                        ch.accountID = int.Parse(((PacketData)t).Argument1);
                                        ch.CharName = rd.GetString("name");
                                        ch.Class = rd.GetString("class");
                                        ch.level = rd.GetInt16("base_level");
                                        ch.exp = rd.GetInt16("base_exp");
                                        ch.max_hp = rd.GetInt16("max_hp");
                                        ch.max_sp = rd.GetInt16("max_sp");
                                        ch.hp = rd.GetInt16("hp");
                                        ch.sp = rd.GetInt16("sp");
                                        ch.str = rd.GetInt16("str");
                                        ch.agi = rd.GetInt16("agi");
                                        ch.vit = rd.GetInt16("vit");
                                        ch.intel = rd.GetInt16("inte");
                                        ch.dex = rd.GetInt16("dex");
                                        ch.luk = rd.GetInt16("luk");
                                        ch.hair_id = rd.GetInt16("head");

                                        data = ch.Serialize();

                                        data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);
                                        clientStream.Write(data, 0, data.Length);

                                        clientStream.Flush();

                                    }
                                }
                                rd.Close();
                            }
                            catch (Exception ex)
                            {
                                Error(ex.Message);
                                CharPaket ch = new CharPaket();
                                ch.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;
                                data = ch.Serialize();

                                data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);

                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();

                            }
                        }

                        #endregion
                    }


                    #endregion


                    if(t.GetType() == typeof(CharPaket))
                    {
                        if(((CharPaket)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_CREATE_CHAR)
                        {
                            CharPaket cc = (CharPaket)t;

                            //Execute Query
                            MySqlCommand cmd;
                            cmd = new MySqlCommand($"insert into chars (account_id,char_num,name,class,base_level,job_level,gold,str,agi,vit,inte,dex,luk,max_hp,hp,max_sp,sp,head,save_map,save_x,save_y) VALUES ('{cc.accountID}','{cc.CharNum}','{cc.CharName}','{cc.Class}','1','1','1000','{cc.str}','{cc.agi}','{cc.vit}','{cc.intel}','{cc.dex}','{cc.luk}','100','100','50','50','{cc.hair_id}','main','100','100');", cn);
                            MySqlDataReader rd = cmd.ExecuteReader();


                            PacketData p = new PacketData();
                            p.Command = (uint)PacketHeader.HeaderCommand.ACT_CONFIRM;
                            p.Argument1 = "true";

                            data = AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());

                            rd.Close();
                            
                            
                            clientStream.Write(data, 0, data.Length);
                            clientStream.Flush();
                        }
                    }


                   
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



        public void HandleClientTXT(object client)
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
                        if(t.GetType() == typeof(PacketData))
                        {

                        #region ACT LOGIN
                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_LOGIN)
                            {
                                Notice("Tentativo di acceso da parte di '" + ((PacketData)t).Argument1 + "'   IP: [ " + clientIP + " ]");

                                bool check = LoginTXT(((PacketData)t).Argument1, ((PacketData)t).Argument2);

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
                                    PacketData p = new PacketData();
                                    p.Command = (uint)PacketHeader.HeaderCommand.ACT_LOGIN;
                                    p.Argument1 = "false";
                                    data = p.Serialize();
                                    data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);

                                    clientStream.Flush();
                                    clientStream.Write(data, 0, data.Length);
                                    //clientStream.WriteByte(0x02);
                                    Warining("Accesso rifiutato per '" + ((PacketData)t).Argument1 + "'    IP: [ " + clientIP + " ]");
                                }
                            }

                        #endregion


                        #region GET ACCOUNT ID
                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_ACC_ID)
                        {
#if DEBUG
                            Debug("Richiesta di ottenere account id con user " + ((PacketData)t).Argument1);
#endif
                            try
                            {
                                var acc = Account.Where(x => x.username == ((PacketData)t).Argument1).FirstOrDefault();
                              

                                p = new Packet.PacketData();
                                p.Command = (uint)Packet.PacketHeader.HeaderCommand.ACT_REC_ACC_ID;
                                p.Argument1 = acc.id.ToString();

                                data = p.Serialize();
                                data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);
                             
                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();

                            }
                            catch (Exception ex)
                            {
                                Error(ex.Message);
                            }

                        }

                        #endregion


                        #region ACT_COUNT_CHAR

                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_COUNT_CHAR)
                        {
                                                        
                          
                            try
                            {
                                

                                int count = 0;
                                string char_id = "";

                                for(int i = 0; i < Chars.Count;i++)
                                {
                                    if (Chars[i].account_id == int.Parse(((PacketData)t).Argument1))
                                    {
                                        count++;
                                        char_id = char_id + Chars[i].char_num + ",";
                                    }
                                }


                                p = new PacketData();
                                p.Command = (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_CHARCOUNT;
                                p.Argument1 = count.ToString();
                                p.Argument2 = char_id;

                                data = p.Serialize();

                                data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);
                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();

                            }
                            catch (Exception a)
                            {
                                Error(a.Message);
                            }
                        }

                        #endregion


                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_ALL_CHAR)
                        {
                            try
                            {
                                MySqlCommand cmd;
                                cmd = new MySqlCommand($"select * from chars where account_id={int.Parse(((PacketData)t).Argument1)};", cn);
                                MySqlDataReader rd = cmd.ExecuteReader();
                                string dt = "";

                                for(int i = 0; i < Chars.Count;i++)
                                {
                                    if (Chars[i].account_id == int.Parse(((PacketData)t).Argument1))
                                    {
                                        dt = dt + Chars[i].char_num + ",";
                                    }
                                }

                            
     
                            }
                            catch (Exception ex)
                            {
                                Error(ex.Message);
                                CharPaket ch = new CharPaket();
                                ch.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;
                                data = ch.Serialize();

                                data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);

                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();
                            }
                        }


                        #region ACT_GET_CHAR
                        if (((PacketData)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_GET_CHAR)
                        {

                            try
                            {
                                var cc = Chars.Where(x => x.account_id == int.Parse(((PacketData)t).Argument1) && x.char_num == int.Parse(((PacketData)t).Argument2)).FirstOrDefault();
                               

                                    CharPaket ch = new CharPaket();
                                    ch.Command = 0;
                                    ch.CharNum = cc.char_num;
                                    ch.accountID = cc.account_id;
                                    ch.CharName = cc.name;
                                    ch.Class = cc.char_class;
                                    ch.level = cc.base_leve;
                                    ch.exp = cc.base_exp;
                                    ch.max_hp = cc.max_hp;
                                    ch.max_sp = cc.max_sp;
                                    ch.hp = cc.hp;
                                    ch.sp = cc.sp;
                                    ch.str = cc.str;
                                    ch.agi = cc.agi;
                                    ch.vit = cc.vit;
                                    ch.intel = cc.ints;
                                    ch.dex = cc.dex;
                                    ch.luk = cc.luk;
                                    ch.hair_id = cc.head;

                                        data = ch.Serialize();

                                        data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);
                                        clientStream.Write(data, 0, data.Length);

                                        clientStream.Flush();

                                 
                               
                            }
                            catch (Exception ex)
                            {
                                Error(ex.Message);
                                CharPaket ch = new CharPaket();
                                ch.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;
                                data = ch.Serialize();

                                data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);

                                clientStream.Write(data, 0, data.Length);
                                clientStream.Flush();

                            }
                        }

                        #endregion

                    }



                    #endregion


                    #region CharPacket
                    if (t.GetType() == typeof(CharPaket))
                    {
                        if (((CharPaket)t).Command == (uint)Packet.PacketHeader.HeaderCommand.ACT_CREATE_CHAR)
                        {
                            CharPaket cc = (CharPaket)t;


                            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "/data/chars.txt", true))
                            {
                                sw.WriteLine($"{Chars.Count + 1},{cc.accountID},{cc.CharNum},{cc.CharName},{cc.Class},1,0,0,0,0,{cc.str},{cc.agi},{cc.vit},{cc.intel},{cc.dex},{cc.luk},100,100,50,50,0,0,,,0,0,0,0,0,0,{cc.hair_id},0,0,0,main,100,100,main,100,100,0,false,0,M,0,false;");
                            }

                            //
                            PacketData p = new PacketData();
                            p.Command = (uint)PacketHeader.HeaderCommand.ACT_CONFIRM;
                            p.Argument1 = "true";

                            data = AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());

                           
                            clientStream.Write(data, 0, data.Length);
                            clientStream.Flush();
                            LoadTXTData();
                        }
                    }


                    #endregion

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
        }




        private bool LoginTXT(string argument1, string argument2)
        {
            bool found = false;
            for(int i = 0; i < Account.Count; i++)
            {             
                if(argument1 == Account[i].username && argument2 == Account[i].password)
                {
                    found = true;
                }else
                {
                    found = false;
                }
            }
            return found;
        }



        public bool LoginSQL(string user, string pass)
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
                Console.WriteLine("");
                Console.WriteLine("");
                Console.Write("Want to use SQL SERVER?");
                Console.Write("[Yes] or [No] type Y / N: ");
                r = Console.ReadLine();

                if (r.ToLower() == "y")
                {
                    ConfigureServer();
                }
                if (r.ToLower() == "n")
                {

                    Console.WriteLine("");
                    Console.WriteLine("Want to use TXT based SERVER?");
                    Console.WriteLine("[Yes] or [No] type Y / N");
                    r = Console.ReadLine();
                    if(r.ToLower() == "y")
                    {
                        ConfigureTXTServer();
                    }else
                    {
                        Environment.Exit(0);
                    }
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



        public void ConfigureTXTServer()
        {
            Notice("Configuring Mode... press any key to continue...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(" Please input ip address to remote host (default is: 127.0.0.1");
            Console.Write(" IP:> ");
            string _ip = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("");
            

            Console.WriteLine(" Please input port to host (default: 6400)");
            Console.Write(" Port:> ");
            int port = int.Parse(Console.ReadLine());
            Console.WriteLine("");
            Console.WriteLine("");

            Notice("Writing configuration files...");

            StreamWriter sw;

            //File.Create(Environment.CurrentDirectory + @"\data\config.txt");
            sw = new StreamWriter(Environment.CurrentDirectory + "/data/config.txt");

            sw.WriteLine("server_type:TXT");
            sw.WriteLine($"server_ip:{_ip}");
            sw.WriteLine($"server_port:{port}");
            
            sw.Close();

            Console.ReadKey();
            Warining("Writing Default txt server files..");

            sw = new StreamWriter(Environment.CurrentDirectory + "/data/accounts.txt");
            sw.WriteLine("//Account ID,Username,Password,Email,Access Level");
            sw.WriteLine("1,server,server,server@server.it,99");
            sw.Close();

            sw = new StreamWriter(Environment.CurrentDirectory + "/data/chars.txt");
            sw.WriteLine("//char_id,account_id,char_num,name,class,base_level,job_level,base_exp,job_exp,gold,str,agi,vit,int,dex,luk,max_hp,hp,max_sp,sp,status_point,skill_point,options,karma,party_id,guild_id,pet_id,body,weapon,shield,head,robe,accessory1,accessory2,last_map,last_x,last_y,save_map,save_x,save_y,partner_id,online,unban_time,sex,clan_id,show_equip");
            sw.Close();
            sw.Dispose();

            Warining("For apply all changes, need to restart server. Playse restart the server");
            sw.Close();

            Environment.Exit(0);

        }


        /// <summary>
        /// Configure the server at First Start
        /// </summary>
        public void ConfigureServer()
        {
            Notice("Configuring Mode... press any key to continue.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(" Please input ip address to remote host (default: 127.0.0.1)");
            Console.Write(" IP:> ");
            string _ip = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Please input port to host (default: 6400)");
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
            Console.WriteLine(" Please input ip address to remote MySQL Host (default: 127.0.0.1)");
            Console.Write(" IP:> ");
            string _ipsql = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Please input port to host (default port is: 3306)");
            Console.Write(" Port:> ");
            int portsql = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            Console.WriteLine(" Please input Username for MySQL");
            Console.Write(" User:> ");
            string user = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Please input Password for MySQL");
            Console.Write(" Password:> ");
            string pass = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine(" Please input Database name for MMORpgmaker");
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
            
                //File.Create(Environment.CurrentDirectory + @"\data\config.txt");
                sw = new StreamWriter(Environment.CurrentDirectory + @"\data\config.txt");

            sw.WriteLine("server_type:SQL");
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
            sw.Close();
            
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

