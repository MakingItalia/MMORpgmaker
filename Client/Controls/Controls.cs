using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

/* UIX SDK BY Thejuster
 * see my other project on pierotofy.it and Github
 * */


namespace UIXControls
{


    public abstract class Controls : IControl
    {

        #region Private Field

        private int x=0, y=0,w=0,h=0;
        private Color higlight = Color.FromNonPremultiplied(211, 232, 250, 255);
        private bool mdown = false;

        #endregion

        #region Delegates

        public delegate void MouseDown();
        public delegate void MouseUp();
        public delegate void MouseLeftDown();
        public delegate void MouseLeftUp();
        public delegate void MouseRightDown();
        public delegate void MouseRightUp();
        

        #endregion

        #region Events

        public event MouseDown OnMouseDown;
        public event MouseUp OnMouseUp;
        public event MouseLeftDown OnMouseLeftDown;
        public event MouseLeftUp OnMouseLeftUp;
        public event MouseRightDown OnMouseRightDown;
        public event MouseRightUp OnMouseRightUp;

        #endregion

        #region Public Events

        /// <summary>
        /// Get or Set X Coordinates
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Get or Set Y Coordinates
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Get or Set Width
        /// </summary>
        public int Width
        {
            get { return w; }
            set { w = value; }
        }

        /// <summary>
        /// Get or Set Height
        /// </summary>
        public int Height
        {
            get { return h; }
            set { h = value; }
        }

        /// <summary>
        /// Get or Set if control are focused
        /// </summary>
        public bool Focused { get; set; }

        /// <summary>
        /// Get or Set Highlight Color
        /// </summary>
        public Color HighlightColor { get { return higlight; } set { higlight = value; } }

        /// <summary>
        /// Get or Set Position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Get or Set Controls Graphics
        /// </summary>
        public Texture2D ControlGraphic;

        /// <summary>
        /// Get or Set Name of Control
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set Text of Control
        /// </summary>
        public string Text { get; set; }

        
        //Device
        public GraphicsDevice Device; 


#endregion


        /// <summary>
        /// Constructor of Controls
        /// </summary>
        /// <param name="device">Graphics Device</param>
        public Controls(GraphicsDevice device)
        {
            Device = device;
            Width = 0;
            Height = 0;
        }


        /// <summary>
        /// Initialize Method
        /// </summary>
        /// <param name="ControlType">Set a type of Control</param>
        public void Initialize(object ControlType)
        {
         
          

            OnMouseDown += new MouseDown(Controls_OnMouseDown);
            OnMouseUp += new MouseUp(Controls_OnMouseUp);
            OnMouseLeftDown += new MouseLeftDown(Controls_OnMouseLeftDown);
            OnMouseLeftUp += new MouseLeftUp(Controls_OnMouseLeftUp);
            OnMouseRightDown += new MouseRightDown(Controls_OnMouseRightDown);
            OnMouseRightUp += new MouseRightUp(Controls_OnMouseRightUp);
            
        }


        void Controls_OnMouseRightUp() {}

        void Controls_OnMouseRightDown() {}

        void Controls_OnMouseLeftUp() { }

        void Controls_OnMouseLeftDown() { }

        void Controls_OnMouseUp() { }

        void Controls_OnMouseDown() { }


        /// <summary>
        /// Draw Method
        /// </summary>
        /// <param name="sprite">SpriteBatch</param>
        public virtual void Draw(SpriteBatch sprite)
        {
            
        }


        /// <summary>
        /// Update Game Cycle
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="ms">Mouse State</param>
        /// <param name="kb">Keyboard State</param>
        public virtual void Update(GameTime gameTime,MouseState ms,KeyboardState kb)
        {
            if (new Rectangle(X, Y, Width, Height).Contains(new Point(ms.X, ms.Y)))
            {
                Focused = true;
                if (ms.LeftButton == ButtonState.Pressed && !mdown)
                {
                    OnMouseDown();
                    mdown = true;
                }

                if (ms.LeftButton == ButtonState.Released && mdown)
                {
                    OnMouseLeftUp();
                    mdown = false;
                }

               /* if (ms.RightButton == ButtonState.Pressed && !mdown)
                {
                    OnMouseRightDown();
                    mdown = true;
                }

                if (ms.RightButton == ButtonState.Released && mdown)
                {
                    OnMouseRightUp();
                    mdown = false;
                }*/
            }
            else
            {
                Focused = false;
            }
        }
    }
}
