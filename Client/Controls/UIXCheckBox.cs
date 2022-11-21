using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/* UIX SDK BY Thejuster
 * see my other project on pierotofy.it and Github
 * */


namespace UIXControls
{
    public class UIXCheckBox : Controls
    {
        SpriteFont font, symb;
        Primitive p;
        bool check, mdown = false;


        public bool Checked
        {
            get { return check; }
            set { check = value; }
        }

        public UIXCheckBox(GraphicsDevice dev, SpriteFont DefaultFont, SpriteFont SymbolFont, Vector2 position, string Text)
            : base(dev)
        {
            font = DefaultFont;
            symb = SymbolFont;
            base.Position = position;
            base.Text = Text;
            Vector2 sz = font.MeasureString(Text);
            base.Width = (int)sz.X + 20;
            base.Height = (int)sz.Y;
            p = new Primitive(dev);
        }


        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);

            p.Begin();
            p.DrawRectangle(new Rectangle((int)Position.X, (int)Position.Y+3, 10, 10), Color.Black);

            if(Checked)
            p.DrawString(symb, "b", new Vector2((int)Position.X-2, (int)Position.Y+1), Color.Black);

            p.End();

            sprite.DrawString(font, Text, new Vector2((int)Position.X + 20, (int)Position.Y), Color.Black);
        }



        public override void Update(GameTime gameTime,MouseState ms, KeyboardState kb)
        {
            base.Update(gameTime, ms, kb);

            if (new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height).Contains(new Point(ms.X, ms.Y)))
            {
                if (!mdown && ms.LeftButton == ButtonState.Pressed)
                {
                    mdown = true;
                    if (Checked)
                        Checked = false;
                    else
                        Checked = true;
                }
            }

            if (mdown && ms.LeftButton == ButtonState.Released)
                mdown = false;
        }
    }
}
