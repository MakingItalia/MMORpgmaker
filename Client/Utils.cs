using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Packet;

namespace MMORpgmaker_Client
{
    public class Utils
    {

        public enum TextureType
        {
            Texture,
            Charaset,
            Tileset,
            SystemSkin,
            Picture,
            Background

        }




        /// <summary>
        /// Load Music file from BGM Folder
        /// </summary>
        /// <param name="filename">Filename whit extension</param>
        /// <returns></returns>
        public Song LoadMusic(string filename)
        {
            return Song.FromUri("titolo", new System.Uri(Environment.CurrentDirectory + $"/data/BGM/{filename}"));
        }



        /// <summary>
        /// Load Texture from Texture Folder
        /// </summary>
        /// <param name="filename">Filename whit extension</param>
        /// <param name="graphicsDevice">GraphicsDevice</param>
        /// <returns>Texure2D</returns>
        public Texture2D LoadTextureFromFile(string filename, GraphicsDevice graphicsDevice, TextureType textureType)
        {
            Texture2D tx = null;
            FileStream fss;

            switch (textureType)
            {
                case TextureType.Texture:
                    fss = new FileStream(Environment.CurrentDirectory + $"/data/Texture/{filename}", FileMode.Open, FileAccess.Read);
                    tx = Texture2D.FromStream(graphicsDevice, fss);
                    fss.Close();
                    break;
                case TextureType.Picture:
                    fss = new FileStream(Environment.CurrentDirectory + $"/data/Picture/{filename}", FileMode.Open, FileAccess.Read);
                    tx = Texture2D.FromStream(graphicsDevice, fss);
                    fss.Close();
                    break;
                case TextureType.SystemSkin:
                    fss = new FileStream(Environment.CurrentDirectory + $"/Content\\SystemSkin\\{filename}", FileMode.Open, FileAccess.Read);
                    tx = Texture2D.FromStream(graphicsDevice, fss);
                    fss.Close();
                    break;

                case TextureType.Charaset:
                    fss = new FileStream(Environment.CurrentDirectory + $"/data/Charaset/{filename}", FileMode.Open, FileAccess.Read);
                    tx = Texture2D.FromStream(graphicsDevice, fss);
                    fss.Close();
                    break;

            }
            return tx;
        }



        /// <summary>
        /// This method get Bytes from files
        /// </summary>
        /// <param name="fullFilePath">Startup Directory</param>
        /// <returns>return bytes of file</returns>
        public byte[] GetBytesFromFile(string fullFilePath)
        {
            //Questo metodo è limitato a 2^32 byte del file pari a (4.2 GB) massimi
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }


        /// <summary>
        /// This method write to a file
        /// </summary>
        /// <param name="_FileName">Filename</param>
        /// <param name="_ByteArray">Array di Byte</param>
        /// <returns></returns>
        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Apro il file per la lettura
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);

                //Scrivi dei blocchi di bytes in questo stream
                //utilizzando l'array dei bytes
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // Chido lo stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Errore
                Console.WriteLine("Errore nel processo: {0}",
                                  _Exception.ToString());
            }

            // Ad un errore riscontrato, restituisce false
            return false;
        }


        /// <summary>
        /// This Method create a Texture From file
        /// </summary>
        /// <param name="dev">Device</param>
        /// <param name="files">File</param>
        /// <returns>return compiled Texture</returns>
        public Texture2D TextureFromBytes(GraphicsDevice dev, byte[] files)
        {
            MemoryStream ms = new MemoryStream(files);
            return Texture2D.FromStream(dev, ms);
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



        /// <summary>
        /// Disassembla un pacchetto di bytes
        /// Ne ricava il tipo e restutisce la struttura
        /// </summary>
        /// <param name="data">Buffer</param>
        /// <returns>Tipo Disassemblato in Object</returns>
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




        public Texture2D LoadFromFileStream(string fileName, GraphicsDevice graphicsDevice)
        {
            Texture2D file;
            RenderTarget2D result;

            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                file = Texture2D.FromStream(graphicsDevice, fileStream);
            }

            //Setup a render target to hold our final texture which will have premulitplied alpha values
            result = new RenderTarget2D(graphicsDevice, file.Width, file.Height);
            graphicsDevice.SetRenderTarget(result);
            graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            //Multiply each color by the source alpha, and write in just the color values into the final texture
            var blendColor = new BlendState
            {
                ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue,
                AlphaDestinationBlend = Blend.Zero,
                ColorDestinationBlend = Blend.Zero,
                AlphaSourceBlend = Blend.SourceAlpha,
                ColorSourceBlend = Blend.SourceAlpha
            };

            var spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
            spriteBatch.Draw(file, file.Bounds, Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();

            //Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
            var blendAlpha = new BlendState
            {
                ColorWriteChannels = ColorWriteChannels.Alpha,
                AlphaDestinationBlend = Blend.Zero,
                ColorDestinationBlend = Blend.Zero,
                AlphaSourceBlend = Blend.One,
                ColorSourceBlend = Blend.One
            };

            spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
            spriteBatch.Draw(file, file.Bounds, Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();

            //Release the GPU back to drawing to the screen
            graphicsDevice.SetRenderTarget(null);

            return result as Texture2D;
        }

      
    }
}
