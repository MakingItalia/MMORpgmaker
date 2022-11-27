using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Helper;
using MMORpgmaker_Client.Controls;
using Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client.GameScene
{
    internal class CharSelect
    {
        Game1 game;
        GraphicsDevice d;
        SpriteFont font, symb;
        ContentManager content;
        SkinSystem skin;
        MouseState m;
        KeyboardState k;
        Utils u = new Utils();
        GameClient client;
        Texture2D bg; //Background

        Texture2D sel; //Selector --
        Vector2 sel_p1 = new Vector2(80, 73);
        Vector2 sel_p2 = new Vector2(306, 73);
        Vector2 sel_p3 = new Vector2(533, 73);
        const int sel_w = 247;
        const int sel_h = 188;
        Rectangle box1, box2, box3;
        int select_id = 1;

        int acc_id = 0;

        //Button
        Texture2D bt_crea, bt_usa;

        TexturedButton bt_create1, bt_create2,bt_create3,bt_use;

        //Information
        CharPaket char1, char2, char3;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dev">GraphicsDevice</param>
        /// <param name="f">Font</param>
        /// <param name="s">Symbol Font</param>
        /// <param name="g">Main Game Class</param>
        public CharSelect(GraphicsDevice dev,ContentManager c,SkinSystem sk,SpriteFont f,SpriteFont s,Game1 g,GameClient cli)
        {
            game = g;
            d = dev;
            font = f;
            symb = s;
            content = c;
            skin = sk;
            client = cli;

            char1 = new CharPaket();
            char1.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;

            char2 = new CharPaket();
            char2.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;

            char3 = new CharPaket();
            char3.Command = (uint)PacketHeader.HeaderCommand.CHAR_EMPTY;
        }


        public void LoadContent()
        {
            acc_id = game.account_id;

            bg = u.LoadTextureFromFile("win_select.png", d, Utils.TextureType.SystemSkin);
            sel = u.LoadTextureFromFile("box_select.png", d, Utils.TextureType.SystemSkin);

            box1 = new Rectangle((int)sel_p1.X, (int)sel_p1.Y, sel_w, sel_h);
            box2 = new Rectangle((int)sel_p2.X, (int)sel_p2.Y, sel_w, sel_h);
            box3 = new Rectangle((int)sel_p3.X, (int)sel_p3.Y, sel_w, sel_h);

            bt_crea = u.LoadTextureFromFile("btn_crea.png", d, Utils.TextureType.SystemSkin);
            bt_usa = u.LoadTextureFromFile("btn_usa.png", d, Utils.TextureType.SystemSkin);

            bt_create1 = new TexturedButton(bt_crea, 135, 192);
            bt_create2 = new TexturedButton(bt_crea, 370, 192);
            bt_create3 = new TexturedButton(bt_crea, 590, 192);
            bt_create1.OnMouseDown += Bt_create_OnMouseDown;

            //Get Char From Account ID;

            //count char
            PacketData p = new PacketData();
            p.Command = (uint)PacketHeader.HeaderCommand.ACT_COUNT_CHAR;
            p.Argument1 = acc_id.ToString();

            byte[] data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());
            object t = client.SendGetPacket(data);
            int char_tot = 0;
            if(t.GetType() == typeof(PacketData))
            {
                PacketData packet = (PacketData)t;
                char_tot = int.Parse(packet.Argument1);

            }

            #region Get information about Character

            //1
            try
            {
                p = new PacketData();
                p.Command = (uint)PacketHeader.HeaderCommand.ACT_GET_CHAR;
                p.Argument1 = acc_id.ToString(); //Account ID
                p.Argument2 = "1";
                data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());
                t = client.SendGetPacket(data);

                if (t.GetType() == typeof(CharPaket))
                {
                    char1 = (CharPaket)t;                   
                }
            }
            catch { }

            //1
            try
            {
                p = new PacketData();
                p.Command = (uint)PacketHeader.HeaderCommand.ACT_GET_CHAR;
                p.Argument1 = acc_id.ToString(); //Account ID
                p.Argument2 = "2";
                data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());
                t = client.SendGetPacket(data);

                if (t.GetType() == typeof(CharPaket))
                {
                    char2 = (CharPaket)t;
                }
            }
            catch { }

            //3
            try
            {
                p = new PacketData();
                p.Command = (uint)PacketHeader.HeaderCommand.ACT_GET_CHAR;
                p.Argument1 = acc_id.ToString(); //Account ID
                p.Argument2 = "3";
                data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());
                t = client.SendGetPacket(data);

                if (t.GetType() == typeof(CharPaket))
                {
                    char3 = (CharPaket)t;
                }
            }
            catch { }

            #endregion


        }

        private void Bt_create_OnMouseDown()
        {
           
        }

        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            k = kb;
            m = ms;

            //Box Selector
            if(ms.LeftButton == ButtonState.Pressed)
            {
                if(box1.Contains(m.Position))
                {
                    select_id = 1;
                }

                if(box2.Contains(m.Position))
                {
                    select_id = 2;
                }

                if(box3.Contains(m.Position))
                {
                    select_id = 3;
                }
            }

            bt_create1.Update(gameTime, ms, kb);
            bt_create2.Update(gameTime, ms, kb);
            bt_create3.Update(gameTime, ms, kb);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, new Rectangle(0, 0, d.PresentationParameters.BackBufferWidth, d.PresentationParameters.BackBufferHeight), Color.White);

            //Selector
            switch(select_id)
            {
                case 1:
                    spriteBatch.Draw(sel, sel_p1, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(sel, sel_p2, Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(sel, sel_p3, Color.White);
                    break;
            }


            bt_create1.Draw(spriteBatch);
            bt_create2.Draw(spriteBatch);
            bt_create3.Draw(spriteBatch);

            spriteBatch.DrawString(font,m.Position.ToVector2().ToString(), new Vector2(2, 30), Color.Yellow);
        }


        
    }
}
