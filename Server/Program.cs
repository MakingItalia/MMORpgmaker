using MySql.Data.MySqlClient;
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

        MySqlConnection cn;
        const int PacketSize = 120;





        public void Main()
        {
            //connessione = "server=" + host + ";uid=" + user + ";pwd=" + psw + ";database=" + db;
            Crediti();
           
            Console.ReadKey();
        }



        public static void Crediti()
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



    }

}

