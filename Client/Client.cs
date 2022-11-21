//using MMORpgmaker_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;


namespace MMORpgmaker
{
    public class Client
    {
        Socket sender;
       public TcpClient client;
       public int ClientID = 0;
       public bool connected = false;
        public bool identified = false;
        public NetworkStream ns;


        public int PositionX, PositionY;

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="Address">Host Address</param>
        /// <param name="port">Port</param>
        public bool Connect(string Address, int port)
        {
            try
            {
                client = new TcpClient();
                client.Connect(Address,port);
                connected = true;

                return true;


            }catch
            {
                return false;
            }

        }

        /// <summary>
        /// Update location to server
        /// </summary>
        public void UpdateLocation()
        {
            byte[] bytes = new byte[1024];

            byte[] packets = Encoding.ASCII.GetBytes($"UPDLCT|{ClientID}|{PositionX}|{PositionY}");
            sender.Send(packets);
        }


        public void SendPacket(Packet.PacketData packet)
        {

            byte[] data = packet.Serialize();
            ns = client.GetStream();
            ns.Write(data, 0, data.Length);
            ns.Flush();
        }



        public bool SendPacketGetByte(Packet.PacketData packet)
        {
            
                byte[] data = packet.Serialize();
                ns = client.GetStream();
                ns.Write(data, 0, data.Length);

                uint rec = (uint)ns.ReadByte();

                if (rec == 2)
                    return false;
                else
                    return true;
           
        }

        /// <summary>
        /// Send Packet
        /// </summary>
        /// <param name="packet">Packet</param>
        /// <param name="response">need response</param>
        public void SendPacket(string packet,bool response = false)
        {
            byte[] bytes = new byte[1024];



            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes("ident");

            // Send the data through the socket.  
            int bytesSent = sender.Send(msg);

            

            if (response)
            {
                int bytesRec = sender.Receive(bytes);
                ClientID = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                //MessageBox.Show(Encoding.ASCII.GetString(bytes, 0, bytesRec));
            }


        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            // Release the socket.  
            //sender.Shutdown(SocketShutdown.Both);
            //sender.Close();

            client.Close();
        }



    }
}
