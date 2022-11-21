using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

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
        bool ispass = false;
        float timer = 1;         //Initialize a 10 second timer
        const float TIMER = 1;


        public bool IsPassword { get { return ispass; }set { ispass = value; } }


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
            p.FillRectangle(new Rectangle(X, Y, Width, Height), Color.White);
            p.DrawRectangle(new Rectangle(X, Y, Width, Height), Color.Black);
            
            p.End();
            string pass = "";
            foreach(char c in text.ToCharArray())
            {
                pass = pass + "*";
            }

            if (!check)
            {

                if(!ispass)
                sprite.DrawString(Font, text, new Vector2(X + 1, Y + 1), Color.Black);
                else
                    sprite.DrawString(Font, pass, new Vector2(X + 1, Y + 1), Color.Black);
            }
            else
            {
                if (!ispass)
                    sprite.DrawString(Font, text, new Vector2(X + 1, Y + 1), Color.Black);
                else
                    sprite.DrawString(Font, pass, new Vector2(X + 1, Y + 1), Color.Black);

                if (blink)
                {
                    if (!ispass)
                        sprite.DrawString(Font, text + "|", new Vector2(X + 1, Y + 1), Color.Black);
                    else
                        sprite.DrawString(Font, pass + "|", new Vector2(X + 1, Y + 1), Color.Black);

                }
            }
            
        }

        int counter = 0;
        public override void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            if (counter > 0)
                counter--;

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


            if(check)
            {
                if (counter == 0)
                {
                    Keys[] l = kb.GetPressedKeys();


                    if (l.Length > 1)
                    {
                        if (l.Contains(Keys.LeftShift) || l.Contains(Keys.RightShift))
                        {
                          

                            Text = Text + l[0].ToString();
                            counter = 10;

                        }
                    }
                    else
                    {
                        if (l.Contains(Keys.Q))
                        {
                            Text = Text + "q";
                            counter = 10;
                        }
                        if (l.Contains(Keys.W))
                        {
                            Text = Text + "w";
                            counter = 10;
                        }
                        if (l.Contains(Keys.E))
                        {
                            Text = Text + "e";
                            counter = 10;
                        }
                        if (l.Contains(Keys.R))
                        {
                            Text = Text + "r";
                            counter = 10;
                        }
                        if (l.Contains(Keys.T))
                        {
                            Text = Text + "t";
                            counter = 10;
                        }
                        if (l.Contains(Keys.Y))
                        {
                            Text = Text + "y";
                            counter = 10;
                        }
                        if (l.Contains(Keys.U))
                        {
                            Text = Text + "u";
                            counter = 10;
                        }
                        if (l.Contains(Keys.I))
                        {
                            Text = Text + "i";
                            counter = 10;
                        }
                        if (l.Contains(Keys.O))
                        {
                            Text = Text + "o";
                            counter = 10;
                        }
                        if (l.Contains(Keys.P))
                        {
                            Text = Text + "p";
                            counter = 10;
                        }
                        if (l.Contains(Keys.A))
                        {
                            Text = Text + "a";
                            counter = 10;
                        }
                        if (l.Contains(Keys.S))
                        {
                            Text = Text + "s";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D))
                        {
                            Text = Text + "d";
                            counter = 10;
                        }
                        if (l.Contains(Keys.F))
                        {
                            Text = Text + "f";
                            counter = 10;
                        }
                        if (l.Contains(Keys.G))
                        {
                            Text = Text + "g";
                            counter = 10;
                        }
                        if (l.Contains(Keys.H))
                        {
                            Text = Text + "h";
                            counter = 10;
                        }
                        if (l.Contains(Keys.J))
                        {
                            Text = Text + "j";
                            counter = 10;
                        }
                        if (l.Contains(Keys.K))
                        {
                            Text = Text + "k";
                            counter = 10;
                        }
                        if (l.Contains(Keys.L))
                        {
                            Text = Text + "l";
                            counter = 10;
                        }
                        if (l.Contains(Keys.Z))
                        {
                            Text = Text + "z";
                            counter = 10;
                        }
                        if (l.Contains(Keys.X))
                        {
                            Text = Text + "x";
                            counter = 10;
                        }
                        if (l.Contains(Keys.C))
                        {
                            Text = Text + "c";
                            counter = 10;
                        }
                        if (l.Contains(Keys.V))
                        {
                            Text = Text + "v";
                            counter = 10;
                        }
                        if (l.Contains(Keys.B))
                        {
                            Text = Text + "b";
                            counter = 10;
                        }
                        if (l.Contains(Keys.N))
                        {
                            Text = Text + "n";
                            counter = 10;
                        }
                        if (l.Contains(Keys.M))
                        {
                            Text = Text + "m";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad0))
                        {
                            Text = Text + "0";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad1))
                        {
                            Text = Text + "1";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad2))
                        {
                            Text = Text + "2";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad3))
                        {
                            Text = Text + "3";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad4))
                        {
                            Text = Text + "4";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad5))
                        {
                            Text = Text + "5";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad6))
                        {
                            Text = Text + "6";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad7))
                        {
                            Text = Text + "7";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad8))
                        {
                            Text = Text + "8";
                            counter = 10;
                        }
                        if (l.Contains(Keys.NumPad9))
                        {
                            Text = Text + "9";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D0))
                        {
                            Text = Text + "0";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D1))
                        {
                            Text = Text + "1";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D2))
                        {
                            Text = Text + "2";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D3))
                        {
                            Text = Text + "3";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D4))
                        {
                            Text = Text + "4";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D5))
                        {
                            Text = Text + "5";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D6))
                        {
                            Text = Text + "6";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D7))
                        {
                            Text = Text + "7";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D8))
                        {
                            Text = Text + "8";
                            counter = 10;
                        }
                        if (l.Contains(Keys.D9))
                        {
                            Text = Text + "9";
                            counter = 10;
                        }


                    }

                }



  

            }

           

        }

    }
}
