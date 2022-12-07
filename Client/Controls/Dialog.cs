using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker_Client;

namespace MMORpgmaker.Controls
{
    public class Dialog
    {

        public delegate void TalkCompleted();
        public delegate void TalkShow();

        public event TalkCompleted OnTalkCompleted;
        public event TalkShow OnTalkShow;

        private Vector2 _position;


        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = new Vector2(value.X - stringSize.X / 2, value.Y - 64);

            }
        }

        Vector2 origin;

        private GraphicsDevice d;
        private SpriteFont f;
        private bool show = false;
        private string tx;
        Vector2 stringSize = new Vector2(0, 0);

        Color color = new Color(255, 255, 255, 255);
        //Textures
        Texture2D top_left, top, top_right, left, right, bottom_left, bottom, bottom_right, background, cursor;

        float timer = 5;         //Initialize a 10 second timer
        const float TIMER = 5;
        private bool autohide = true;

        public string Text
        {
            get { return tx; }
            set { tx = value; }
        }


        public bool AutoHide
        {
            get { return autohide; }
            set { autohide = value; }
        }


        public Dialog(GraphicsDevice dev, SpriteFont font)
        {
            d = dev;
            f = font;

            Utils u = new Utils();

            top_left = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_top_left.PNG", dev);
            top_right = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_top_right.PNG", dev);
            top = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_top.PNG", dev);
            background = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_bg.PNG", dev);
            left = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_left.PNG", dev);
            right = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_right.PNG", dev);
            bottom_left = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_bottom_left.PNG", dev);
            bottom_right = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_bottom_right.PNG", dev);
            bottom = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_bottom.PNG", dev);
            cursor = u.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/msg_cursor.PNG", dev);

            OnTalkCompleted += Dialog_OnTalkCompleted;
            OnTalkShow += Dialog_OnTalkShow;
        }

        private void Dialog_OnTalkShow()
        {
            
        }

        private void Dialog_OnTalkCompleted()
        {
            
        }

        public void Show(string text, Vector2 position)
        {
            Text = text;
            stringSize = f.MeasureString(text);

            if(stringSize.X < 48)
            {
                stringSize = new Vector2(48, stringSize.Y);
            }

            Position = position;
            origin = position;
            show = true;
            OnTalkShow();
            timer = TIMER;

        }

        public void Hide()
        {
            show = false;
            OnTalkCompleted();
        }

        public void Draw(SpriteBatch sprite)
        {
            if (show)
            {

                sprite.Draw(background, new Rectangle((int)Position.X + 6, (int)Position.Y + 6, (int)stringSize.X + 12, (int)stringSize.Y + 6), color);

                sprite.Draw(top_left, new Vector2(Position.X, Position.Y), color);
                sprite.Draw(top, new Rectangle((int)Position.X + 13, (int)Position.Y, (int)stringSize.X, 13), color);
                sprite.Draw(top_right, new Vector2((int)(Position.X + 13) + stringSize.X, Position.Y), color);

                //---
                sprite.Draw(left, new Vector2(Position.X, Position.Y + 13), color);

                sprite.DrawString(f, Text, new Vector2(Position.X + 13, Position.Y + 13), Color.Black);

                sprite.Draw(right, new Vector2((int)Position.X + 13 + stringSize.X, Position.Y + 13), color);
                //----

                sprite.Draw(bottom_left, new Vector2(Position.X, Position.Y + 26), color);
                sprite.Draw(bottom, new Rectangle((int)Position.X + 13, (int)Position.Y + 26, (int)stringSize.X, 13), color);
                sprite.Draw(bottom_right, new Vector2((int)(Position.X + 13) + stringSize.X, Position.Y + 26), color);

                //--
                
                sprite.Draw(cursor, new Vector2((origin.X - stringSize.X / 2) + stringSize.X / 2, Position.Y + 38), color);
            }


        }



        public void Update(GameTime gameTime, KeyboardState kb, MouseState ms,Vector2 pos)
        {
            if (show)
            {
                Position = pos;
                origin = pos;
                Position = new Vector2(Position.X, Position.Y + 20);
                origin = new Vector2(origin.X - 32, origin.Y + 5);
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {

                Hide();

            }


        }

    }
}
