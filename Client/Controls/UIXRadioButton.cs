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
    public class UIXRadioButton : Controls
    {

        SpriteFont font, symb;
        bool check = false;
        bool mdown = false;



        public bool Checked
        {
            get { return check; }
            set { check = value; }
        }


        public UIXRadioButton(GraphicsDevice dev, SpriteFont DefaultFont,SpriteFont SymbolFont, Vector2 position,string Text)
            : base(dev)
        {
            font = DefaultFont;
            symb = SymbolFont;
            base.Position = position;
            base.Text = Text;

            Vector2 sz = font.MeasureString(Text);
            base.Width = (int)sz.X + 20;
            base.Height = (int)sz.Y;
            OnMouseDown += new MouseDown(UIXRadioButton_OnMouseDown);
        }

        void UIXRadioButton_OnMouseDown()
        {
            Checked = Checked ? false : true;
        }

        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);
            sprite.DrawString(symb, "n", Position, Color.DarkGray);
            sprite.DrawString(symb, "j", Position, Color.Black);
            sprite.DrawString(symb, "k", Position, Color.Black);
            sprite.DrawString(font, Text, new Vector2(Position.X +20,Position.Y), Color.Black);

            if(Checked)
            sprite.DrawString(symb, "h", Position, Color.White);
            

        }

        public override void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            base.Update(gameTime, ms, kb);
            if (new Rectangle((int)Position.X, (int)Position.Y, Width, Height).Contains(new Point(ms.X, ms.Y)))
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

            if (ms.LeftButton == ButtonState.Released)
                mdown = false;
            
        }
    }
}
