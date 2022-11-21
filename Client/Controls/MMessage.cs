using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MMORpgmaker_Client;
using MMORpgmaker_Client.Controls;

namespace MMORpgmaker.Controls
{
    public class Message : Control
    {
        Button ok;
        string message;
        SpriteFont Font;
        public bool OkPressed = false;
        private bool show;

        /// <summary>
        /// Show Message Dialog
        /// </summary>
        public bool Show
        {
            get { return show;  }
            set { show = value; }
        }

        public string MessageText
        {
            get { return message; }
            set { message = value; }
        }


        public Message(SkinSystem skin,GraphicsDevice device,SpriteFont font,string Message)
            : base(new Vector2((device.PresentationParameters.BackBufferWidth / 2) - 140, (device.PresentationParameters.BackBufferHeight / 2) - 60), skin, device)
        {
           
            Utils u = new Utils();
            base.Texture = u.LoadFromFileStream(Environment.CurrentDirectory + "\\Content\\SystemSkin\\Eau\\win_msgbox.png", device);
            
           ok = new Button(Button.Button_Type.ok, new Vector2(base.Position.X + 230,base.Position.Y + (Texture.Height - 25)), skin);
           ok.ButtonClick += new Button.OnClick(ok_ButtonClick);
           message = Message;
           Font = font;
        }

        void ok_ButtonClick()
        {
            Show = false;
            OkPressed = true;
        }


        public override void Draw(SpriteBatch sprite)
        {
            if (Show)
            {
                base.Draw(sprite);
                ok.Draw(sprite);
                sprite.DrawString(Font, message, new Vector2(base.Position.X + 8, base.Position.Y + 23), Color.Black);
            }
        }

        public void ShowMessage()
        {
            Show = true;
        }

        public override void Update(GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState kb, Microsoft.Xna.Framework.Input.MouseState ms)
        {
            base.Update(gameTime, kb, ms);
           
            ok.Update(gameTime, kb, ms);
        }

    }
}
