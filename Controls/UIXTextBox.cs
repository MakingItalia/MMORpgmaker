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
    public class UIXTextBox : Controls
    {
        SpriteFont Font;
        Primitive p;
        bool focus = false;
        bool check = false;
        string text = "";
        bool blink = false;

        float timer = 1;         //Initialize a 10 second timer
        const float TIMER = 1;

        /// <summary>
        /// Get or Set Text from UIXTextBox
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        public UIXTextBox(GraphicsDevice dev, SpriteFont font, Vector2 position, int width = 130, int height = 20) 
            : base(dev)
        {
            Initialize(typeof(UIXTextBox));
            base.X = (int)position.X;
            base.Y = (int)position.Y;
            base.Width = width;
            base.Height = height;
            Font = font;
            p = new Primitive(dev);
        }


        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);
            p.Begin();
            p.DrawRectangle(new Rectangle(X, Y, Width, Height), Color.Black);
            p.End();

            if (!check)
            {


                sprite.DrawString(Font, text, new Vector2(X + 1, Y + 1), Color.Black);
            }
            else
            {
                if(blink)
                sprite.DrawString(Font, text + "|", new Vector2(X + 1, Y + 1), Color.Black);
            }
            
        }


        public override void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            base.Update(gameTime, ms, kb);
            if (new Rectangle(X, Y, Width, Height).Contains(new Point(ms.X, ms.Y)))
            {

                focus = true;
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    check = true;
                    blink = true;
                    timer = 1;
                }
            }
            else
            {
                focus = false;
                check = false;
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                //Timer expired, execute action
                blink = blink ? false : true;
                timer = TIMER;   //Reset Timer
            }
            else
            {
               
            }
        }

    }
}
