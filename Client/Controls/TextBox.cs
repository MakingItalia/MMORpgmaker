using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MMORpgmaker.Helper;
using MMORpgmaker_Client.Controls;
using MMORpgmaker_Client;

namespace MMORpgmaker.Controls
{
    public class TextBox_EventArgs : EventArgs
    {
        string text;
        public TextBox_EventArgs(string text)
        {
            this.text = text;
        }

        public string Text { get { return text; } }
    }

    public class TextBox : IControls
    {

        Vector2 pos;
        SkinSystem Skin;
        Texture2D TxTex;
        string text;
        SpriteFont Font;
        Utils ut = new Utils();
        Rectangle txbox_area = new Rectangle(44, 6, 230, 18);
        int counter = 0;
        bool caret = false;
        bool focus = false;
        KeyboardMapper keyboard;
        private bool password = false;
        public string passwords;
        bool enabled = true;
        int counter2 = 0;
        private bool draw_border = true;
        private bool is_dialogobx = false;

        public bool Enabled { get => enabled; set => enabled = value; }
        public bool DrawBorder { get => draw_border; set => draw_border = value; }
       
        /// <summary>
        /// At Enter press, Text is Clear
        /// </summary>
        public bool Is_DialogBox { get => is_dialogobx; set => is_dialogobx = value; }
        bool send = false;

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }


        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool IsPassword
        {
            get { return password; }
            set { password = value; }
        }


        public delegate void MouseDown();
        public delegate void KeyDown();
        public delegate void TextChanged(TextBox_EventArgs e);

        public event MouseDown OnMouseDown;
        public event KeyDown OnKeyDown;
        public event TextChanged OnTextChanged;

        public TextBox(Vector2 position,string text, SpriteFont font, SkinSystem skin,int width=230)
        {
            Position = position;
            Skin = skin;
            Text = text;
            Font = font;
            TxTex = ut.LoadFromFileStream(skin.SkinBase + "/basic_interface/chatwin0_bg.png", skin.graphicsDevice);
            txbox_area = new Rectangle(44, 6, width, 18);
            OnMouseDown += new MouseDown(TextBox_OnMouseDown);

            keyboard = new KeyboardMapper(Text);
            OnTextChanged += TextBox_OnTextChanged1;
        }

        private void TextBox_OnTextChanged1(TextBox_EventArgs e)
        {
            if (IsPassword)
            {
                string ntx = string.Empty;
                for (int i = 0; i < Text.Length - 1; i++)
                {
                    ntx = ntx + "*";
                }
                passwords = Text;
                Text = ntx;
            }
        }

  

        void TextBox_OnMouseDown()
        {
            if(send)
            {
                Text = "";
                send = false;
            }

            if (Enabled && counter2 == 0) 
            {
                if (focus)
                {
                    focus = false; caret = true;
                    counter2 = 15;
                }
                else
                {
                    focus = true; caret = false;
                    counter2 = 15;
                }
            }

            
         
        }



        public void Initialize()
        {
            
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {

           if(DrawBorder)
            sprite.Draw(TxTex, Position, txbox_area, Color.White);
            
            
            if(!caret)
                sprite.DrawString(Font, Text, new Vector2(Position.X + 5, Position.Y ), Color.Black);
            else
                sprite.DrawString(Font, Text + "|", new Vector2(Position.X + 5, Position.Y ), Color.Black);


        }


        string oldtx = "";
        public void Update(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState kb, Microsoft.Xna.Framework.Input.MouseState ms)
        {
            counter++;
            if (counter >= 50 && focus)
            {
                caret = caret ? false : true;
                counter = 0;
            }

            if (counter2 > 0)
                counter2--;


            if (ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Rectangle r = new Rectangle((int)Position.X, (int)Position.Y, 230, 18);

                if(r.Contains(new Point(ms.X,ms.Y)) && Enabled)
                {
                    OnMouseDown();
                }
            }


            //If mouse leave from Controls, Automatically unfocus
            Rectangle r2 = new Rectangle((int)Position.X, (int)Position.Y, 230, 18);
            if(!r2.Contains(new Point(ms.X,ms.Y)) && !is_dialogobx)
            {
                focus = false;
                caret = false;
            }


            if (focus)
            {
                oldtx = Text;
                Text = keyboard.Update(kb);

                if (oldtx != Text)
                {
                    TextBox_EventArgs ev = new TextBox_EventArgs(Text);
                    OnTextChanged(ev);
                }
            }


            if (keyboard.enter)
            {
                focus = false; 
                caret = false;
                keyboard.enter = false;
                send = true;
            }
        }
    }
}
