using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client.Controls
{
    public class TexturedButton : UIXControls.IControl
    {
        private int _x, _y;
        private Vector2 _pos;
        private Texture2D _tex;

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public Vector2 Position { get => _pos; set => _pos = new Vector2(X, Y); }
        public Texture2D Texture { get => _tex; set => _tex = value; }

        Rectangle sz;
        public delegate void MouseDown();
        public event MouseDown OnMouseDown;

        public TexturedButton(Texture2D tex, int x, int y)
        {
            OnMouseDown += TexturedButton_OnMouseDown;
            _tex = tex;
            _pos = new Vector2(x, y);
            sz = new Rectangle(x, y, tex.Width, tex.Height);
        }

        public TexturedButton(Texture2D tex, int x, int y,int w,int h)
        {
            OnMouseDown += TexturedButton_OnMouseDown;
            _tex = tex;
            _pos = new Vector2(x, y);
            sz = new Rectangle(x, y, w, h);
        }

        private void TexturedButton_OnMouseDown()
        {
            
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Texture,new Rectangle((int)Position.X,(int)Position.Y,sz.Width,sz.Height), Color.White);
        }

        public void Initialize(object ControlType = null)
        {
            
        }

        public void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            if(ms.LeftButton == ButtonState.Pressed)
            {
                if(sz.Contains(ms.Position))
                {
                    OnMouseDown();
                }
            }
        }
    }
}
