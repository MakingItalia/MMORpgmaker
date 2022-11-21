using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MMORpgmaker_Client.Controls;
using MMORpgmaker_Client;

namespace MMORpgmaker.Controls
{

    public class MouseMoveArgs : EventArgs
    {
        private int X;
        private int Y;

        public Vector2 Position;
        public MouseState mouseState;

        public MouseMoveArgs(MouseState m)
        {
            X = (int)m.X;
            Y = (int)m.Y;
            Position = new Vector2(X, Y);
            mouseState = m;
        }
    }


    public abstract class Control : IControls
    {

        private Vector2 position;
        private SkinSystem skin;
        private int size_w = 0;
        private int size_h = 0;
        private Texture2D tex;
        MouseMoveArgs MouseMoveEventArgs;
        private Rectangle inter_area;
        private GraphicsDevice dev;

        /// <summary>
        /// Get or Set Position
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        /// <summary>
        /// Get or Set SystemSkin
        /// </summary>
        public SkinSystem Skin
        {
            get { return skin; }
            set { skin = value; }
        }

        /// <summary>
        /// Get or Set Control Width
        /// </summary>
        public int Width
        {
            get { return size_w; }
            set { size_w = value; }
        }


        /// <summary>
        /// Get or Set Control Height
        /// </summary>
        public int Height
        {
            get { return size_h; }
            set { size_h = value; }
        }


        /// <summary>
        /// Get or Set Texture of Control
        /// </summary>
        public Texture2D Texture
        {
            get { return tex; }
            set { tex = value; }
        }


        /// <summary>
        /// Get or Set Interactive area of Control
        /// </summary>
        public Rectangle InteractiveArea
        {
            get { return inter_area; }
            set { inter_area = value; }
        }

        public delegate void MouseDown(MouseMoveArgs m);
        public delegate void MouseUp(MouseMoveArgs m);
        public delegate void MouseMove(MouseMoveArgs m);


        public event MouseDown OnMouseDown;
        public event MouseUp OnMouseUp;
        public event MouseMove OnMouseMove;


        public Control(Vector2 position,SkinSystem skin,GraphicsDevice device)
        {
            Position = position;
            Skin = skin;
            dev = device;
            OnMouseDown += new MouseDown(Control_OnMouseDown);
            OnMouseMove += new MouseMove(Control_OnMouseMove);
            OnMouseUp += new MouseUp(Control_OnMouseUp);

        }


        void Control_OnMouseUp(MouseMoveArgs m)
        {
            
        }


        public void LoadResource(string resourceName)
        {
            Utils u = new Utils();
            Texture = u.LoadFromFileStream(Environment.CurrentDirectory + resourceName, dev);
        }

        void Control_OnMouseMove(MouseMoveArgs m)
        {
            
        }

        void Control_OnMouseDown(MouseMoveArgs m)
        {
            
        }


        public void Initialize()
        {
            
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            try
            {
                sprite.Draw(Texture, Position, Color.White);
            }
            catch { }
        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState kb, Microsoft.Xna.Framework.Input.MouseState ms)
        {
            MouseMoveEventArgs = new MouseMoveArgs(ms);

            if (InteractiveArea.Contains(new Point(ms.X, ms.Y)))
            {
                OnMouseMove(MouseMoveEventArgs);
                OnMouseUp(MouseMoveEventArgs);
                OnMouseDown(MouseMoveEventArgs);
            }
            

        }
    }
}
