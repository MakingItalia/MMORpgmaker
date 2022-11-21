using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Packet;
using System.Threading;

namespace MMORpgmaker.Helper
{
    public class GameClient
    {
        //Private field
        TcpClient client;
        IPEndPoint serverEndPoint;
        NetworkStream ns;
        public uint command;
        Thread t;
        PacketData packets = new PacketData();
        private int accountID;
        const int PacketSize = 120;
        public int AccountID { get => accountID; set => accountID = value; }


        public GameClient(string ip, int port)
        {
            try
            {
                serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                client = new TcpClient();
                //client.Connect(serverEndPoint);
            }
            catch
            { }
            //t = new Thread(MainSocketLoop);
            //t.Start();

        }


        public void StartLooping()
        {
            t = new Thread(MainSocketLoop);
            t.Start();
        }


        public void MainSocketLoop()
        {



            try
            {


                ns = client.GetStream();


         

                if (command == (uint)PacketHeader.HeaderCommand.ACT_MSG)
                {

                    PacketData p = new PacketData();
                    p.Command = command;
                    p.Argument1 = "ciao";

                    byte[] b = p.Serialize();
                    ns.Write(b, 0, b.Length);
                }



            }
            catch (Exception a)
            {

                client.Close();
                ns.Close();
            }


        }


        public void Connect()
        {
            try
            {
                client.Connect(serverEndPoint);
            }
            catch { }
        }

        public TcpClient Client
        {
            get { return client; }
            set { client = value; }
        }


        public IPEndPoint EndPoint
        {
            get { return serverEndPoint; }
            set { serverEndPoint = value; }
        }

        public uint Command
        {
            get { return command; }
            set { command = value; }
        }


        /// <summary>
        /// Send Packet
        /// </summary>
        /// <param name="Command">Command Header for packet</param>
        /// <param name="arg1">Arguments</param>
        /// <param name="arg2">Arguments</param>
        /// <returns>Get uint Respose from server</returns>
        public uint SendPacket(PacketHeader.HeaderCommand Command, string arg1, string arg2)
        {
            ns = client.GetStream();
            PacketData paket = new PacketData();
            paket.Command = (uint)Command;
            paket.Argument1 = arg1;
            paket.Argument2 = arg2;

            byte[] data = paket.Serialize();
            ns.Write(data, 0, data.Length);

            uint val = (uint)ns.ReadByte();

            ns.Flush();

            return val;
        }


        /// <summary>
        /// Invia e restituisce un Pacchetto
        /// </summary>
        /// <typeparam name="T">Tipo di Ritorno</typeparam>
        /// <typeparam name="I">Tipo dell'argomento</typeparam>
        /// <param name="strict">Argomento</param>
        /// <returns></returns>
        public T SendGetPacket<T,I>(I strict)
        {
            PacketData p = new PacketData();
            CharPaket ch = new CharPaket();
            
            if(typeof(I) == typeof(PacketData))
            {
                object pdata = strict;
                if(((PacketData)pdata).Command == (uint)PacketHeader.HeaderCommand.ACT_LOGIN)
                {
                    //
                }

                if(((PacketData)pdata).Command == (uint)PacketHeader.HeaderCommand.ACT_GET_CHAR)
                {
                    ch.CharName = "OOOK";
                }
            }

            //CharPacket
            if(typeof(I) == typeof(CharPaket))
            {
                object datas = strict;
                if(((CharPaket)datas).Command == (uint)PacketHeader.HeaderCommand.ACT_CREATE_CHAR)
                {
                    ns = client.GetStream();

                    byte[] data = ((CharPaket)datas).Serialize();
                    ns.Write(data, 0, data.Length);


                    byte[] rd = new byte[100];
                    ns.Read(rd, 0, rd.Length);                   
                    p.Deserialize(ref rd);

                }
            }

            if (typeof(T) == typeof(PacketData))
            {
                return (T)Convert.ChangeType(p, typeof(T));
            }
            
            if(typeof(T) == typeof(CharPaket))
            {
                return (T)Convert.ChangeType(ch, typeof(T));
            }

            return (T)Convert.ChangeType(p, typeof(T));
        }




        /// <summary>
        /// Assembla un Pacchetto inserendo nell'Header a posizione 0
        /// il tipo di pacchetto che viene assemblato
        /// </summary>
        /// <param name="type">Tipo di pacchetto</param>
        /// <param name="data">array serializato</param>
        /// <returns>Array Serializzato con Header</returns>
        public byte[] AssemblyPacket(PacketHeader.PacketType type, byte[] data)
        {

            byte[] final_send = new byte[data.Length + 1];

            Array.Copy(data, 0, final_send, 1, final_send.Length - 1);

            //Assegno tipo di pacchetto
            final_send[0] = (byte)type;

            return final_send;
        }


        public object DisassemblyPacket(byte[] data)
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



        public object SendGetPacket(PacketData p)
        {

            ns = client.GetStream();

            byte[] data = p.Serialize();

            data = AssemblyPacket(PacketHeader.PacketType.PacketData, data);



            ns.Write(data, 0, data.Length);

            //Risposta
            //--- Get Response

            byte[] rd = new byte[120];
            ns.Read(rd, 0, rd.Length);

            object tipo = DisassemblyPacket(rd);

            ns.Flush();
           
            return tipo;
        }


        public object SendGetPacket(CharPaket p)
        {

            ns = client.GetStream();

            byte[] data = p.Serialize();

            data = AssemblyPacket(PacketHeader.PacketType.CharPacket, data);



            ns.Write(data, 0, data.Length);

            //Risposta
            //--- Get Response

            byte[] rd = new byte[120];
            ns.Read(rd, 0, rd.Length);

            object tipo = DisassemblyPacket(rd);

            ns.Flush();
            return tipo;
        }



        /*
        public object SendGetPacket<T>(T t)
        {
            PacketData p = new PacketData();
            object _ret = null;
            if (typeof(T) == typeof(CharPaket))
            {
                               
                object ot = t;
                if(((CharPaket)ot).hp <= 0)
                {
                    p.Command = (uint)PacketHeader.HeaderCommand.ACT_CHAR_DEATH;
                    p.Argument1 = ((CharPaket)ot).accountID.ToString();
                    p.Argument2 = ((CharPaket)ot).CharNum.ToString();

                    
                }

                _ret = p;

            }
            return (T)_ret;
        }

        */
        /// <summary>
        /// Send Packet and Get Packet Response
        /// </summary>
        /// <param name="Command">Command Header for Packet</param>
        /// <param name="arg1">Argument1</param>
        /// <param name="arg2">Argument2</param>
        /// <returns>Return Packet Structure</returns>
        public PacketData SendPacket(PacketData _paket)
        {
            ns = client.GetStream();

            PacketData paket = new PacketData();
            paket.Command = (uint)_paket.Command;
            paket.Argument1 = _paket.Argument1;
            paket.Argument2 = _paket.Argument2;

            byte[] data = paket.Serialize();
             byte[] newValues = new byte[data.Length + 1];
            newValues[0] = 0x00; //PacketData                                
            Array.Copy(data, 0, newValues, 1, data.Length); 


            ns.Write(newValues, 0, newValues.Length);

            //--- Get Response
             //Rimuovo il byte iniziale per identificare il tipo
            byte[] rd = new byte[100];
            ns.Read(rd, 0, rd.Length);
            byte[] end = new byte[rd.Length];
            PacketData p = new PacketData();
            Array.Copy(rd, 1, end, 0, rd.Length - 1);
            p.Deserialize(ref end);
 
            return p;
        }

        public bool SendPacket(CharPaket charPaket)
        {
            ns = client.GetStream();

            byte[] data = charPaket.Serialize();

            byte[] newValues = new byte[data.Length + 1];
            newValues[0] = 0x01;                                
            Array.Copy(data, 0, newValues, 1, data.Length); 


            ns.Write(newValues, 0, newValues.Length);

            return true;
        }
        

        /// <summary>
        /// Invia un Pacchetto semplice richiedendo al server
        /// un Packet di tipo Char per le informazioni basi del personaggio
        /// </summary>
        /// <param name="p">Packet Semplice</param>
        /// <returns>CharPaket</returns>
        public CharPaket GetCharInfo(PacketData p)
        {
            
            ns = client.GetStream();
            CharPaket ch = new CharPaket();
            //send
        
            object t = SendGetPacket(p);

            /*
            byte[] newValues = new byte[data.Length + 1];
            newValues[0] = 0x00; //PacketData
            Array.Copy(data, 0, newValues, 1, data.Length);


            ns.Write(newValues, 0, newValues.Length);

            //--- Get Response
            //Rimuovo il byte iniziale per identificare il tipo
            byte[] rd = new byte[100];
            ns.Read(rd, 0, rd.Length);
            byte[] end = new byte[rd.Length];
            CharPaket ch = new CharPaket();
            Array.Copy(rd, 1, end, 0, rd.Length - 1);
            ch.Deserialize(ref end);*/

            return (CharPaket)t;

        }


    



            /// <summary>
            /// Try to login to game
            /// </summary>
            /// <param name="username">Username</param>
            /// <param name="password">Password</param>
            /// <returns>Return if user and password is correct</returns>
            public bool Login(string username, string password)
            {

            ns = client.GetStream();
            PacketData p = new PacketData();
            p.Command = (uint)PacketHeader.HeaderCommand.ACT_LOGIN;
            p.Argument1 = username;
            p.Argument2 = password;

            object r = SendGetPacket(p);

            if(r.GetType() == typeof(PacketData))
            {
                if(((PacketData)r).Argument1 == "true")
                {
                    return true;
                }else
                {
                    return false;
                }
            }else
            {
                return false;
            }


            #region old
            /*
            ns = client.GetStream();

            PacketData login = new PacketData();
            login.Command = (uint)PacketHeader.HeaderCommand.ACT_LOGIN;
            login.Argument1 = username;
            login.Argument2 = password;


            ns = client.GetStream();


            //---- Example Packet ---   Metodo Serializzato
            byte[] data = login.Serialize();


            byte[] newValues = new byte[data.Length + 1];
            newValues[0] = 0x00; //PacketData
            Array.Copy(data, 0, newValues, 1, data.Length);


            ns.Write(newValues, 0, newValues.Length);
            ns.Write(data, 0, data.Length);

            uint val = (uint)ns.ReadByte();

            if (val == 0x01)
            {
                return true;
            }
            else
            {
                return false;
            }

            ns.Flush();
            */
            #endregion

            //------
        }


        public void Disconnect()
        {
            Client.Close();

        }
    }
}
