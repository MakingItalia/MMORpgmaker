using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MMORpgmaker_Client.Controls
{
    public class SkinSystem
    {
        Utils u = new Utils();
        
        string skinbase = "";
        public string SkinBase { get { return skinbase; } set { skinbase = value; } }
        public GraphicsDevice graphicsDevice;
        

        public SkinSystem(GraphicsDevice device, string SkinBase)
        {
            skinbase = Environment.CurrentDirectory + "\\Content\\SystemSkin\\" + SkinBase; 
            graphicsDevice = device;
        }



    }
}
