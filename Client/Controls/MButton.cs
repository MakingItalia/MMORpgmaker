using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker_Client;
using MMORpgmaker_Client.Controls;

namespace MMORpgmaker.Controls
{
    public class Button : IControls
    {

        public enum Button_Type
        {
            back, //
            buy, //
            cancel,
            close,
            edit,
            exchange,
            next,
            ok,
            rewrite,
            sell,
            send,
            use,
            view,
            open
        }

        Utils ut = new Utils();

        private Button_Type btype;
        bool focus = false;

        const int sz_w = 40;
        const int sz_h = 20;

        private Vector2 pos;
        private SkinSystem skin;

        public delegate void OnClick();
        public event OnClick ButtonClick;

        Rectangle bounds;

        Texture2D BtTexture;
        Texture2D BtIn;
        Texture2D BtOut;

        /// <summary>
        /// Type of Button
        /// </summary>
        public Button_Type ButtonType
        {
            get { return btype; }
            set { btype = value; }
        }


        /// <summary>
        /// Location of Button
        /// </summary>
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }


        /// <summary>
        /// Get or Set GameSkin
        /// </summary>
        public SkinSystem GameSkin
        {
            get { return skin; }
            set { skin = value; }
        }



        public Button(Button_Type buttonType,Vector2 position,SkinSystem skin)
        {
            ButtonClick += new OnClick(Button_ButtonClick);
            ButtonType = buttonType;
            Position = position;
            GameSkin = skin;
            LoadTexture();

            bounds = new Rectangle((int)position.X, (int)position.Y, sz_w, sz_h);
        }



        void LoadTexture()
        {
            switch (ButtonType)
            {
                case Button_Type.back :
                    BtTexture = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_back.png", GameSkin.graphicsDevice);
                    BtIn = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_back_a.png", GameSkin.graphicsDevice);
                    BtOut = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_back_b.png", GameSkin.graphicsDevice);
                  break;
                case Button_Type.buy:
                     BtTexture = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_buy.png", GameSkin.graphicsDevice);
                     BtIn = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_buy_a.png", GameSkin.graphicsDevice);
                     BtOut = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_buy_b.png", GameSkin.graphicsDevice);
                  break;
                case Button_Type.ok:
                     BtTexture = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_ok.png", GameSkin.graphicsDevice);
                     BtIn = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_ok_a.png", GameSkin.graphicsDevice);
                     BtOut = ut.LoadFromFileStream(GameSkin.SkinBase + "/btn_ok_b.png", GameSkin.graphicsDevice);
                  break;
            }
        }


        void Button_ButtonClick() { }      
        



        public void Initialize()
        {
            
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            if (!focus)
                sprite.Draw(BtTexture, Position, Color.White);
            else
                sprite.Draw(BtIn, Position, Color.White);
        }

        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            if (bounds.Contains(new Point(ms.X, ms.Y)))
            {
                focus = true;
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    ButtonClick();
                }
            }
            else
            {
                focus = false;
            }

            

            bounds = new Rectangle((int)Position.X, (int)Position.Y, sz_w, sz_h);
        }
    }
}
