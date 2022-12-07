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
    public class CharSelect
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
        public int select_id = 1;

        int acc_id = 0;

        //Button
        Texture2D bt_crea, bt_usa;

        TexturedButton bt_create1, bt_create2, bt_create3;
        TexturedButton bt_usa1, bt_usa2, bt_usa3;

        //Information
        CharPaket char1, char2, char3;

        //Information
        string name = "", job = "";
        int level=0, exp=0, hp=0, sp=0;
        int str = 0, agi = 0, vit = 0, ints=0, dex=0, luk=0;

        //Graphics
        Texture2D char1_body, char1_head;
        Texture2D char2_body,char2_head;
        Texture2D char3_body,char3_head;


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

            bt_usa1 = new TexturedButton(bt_usa, 135, 192);
            bt_usa2 = new TexturedButton(bt_usa, 370, 192);
            bt_usa3 = new TexturedButton(bt_usa, 590, 192);

            bt_create1.OnMouseDown += Bt_create_OnMouseDown;
            bt_create2.OnMouseDown += Bt_create2_OnMouseDown;
            bt_create3.OnMouseDown += Bt_create3_OnMouseDown;

            bt_usa1.OnMouseDown += Bt_usa1_OnMouseDown;
            bt_usa2.OnMouseDown += Bt_usa2_OnMouseDown;
            bt_usa3.OnMouseDown += Bt_usa3_OnMouseDown;

            //Get Char From Account ID;

            //count char
            PacketData p = new PacketData();
            p.Command = (uint)PacketHeader.HeaderCommand.ACT_COUNT_CHAR;
            p.Argument1 = acc_id.ToString();

            byte[] data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());
            object t = client.SendGetPacket(data);
            int char_tot = 0;
            string charpos="";
            string[] char_block= new string[0];

            if (t.GetType() == typeof(PacketData))
            {
                PacketData packet = (PacketData)t;
                char_tot = int.Parse(packet.Argument1);
                charpos = packet.Argument2;
                char_block = charpos.Split(',');

            }

 
             for (int i = 1; i < char_tot+1; i++)
             {
                 try
                 {
                     p = new PacketData();
                     p.Command = (uint)PacketHeader.HeaderCommand.ACT_GET_CHAR;
                     p.Argument1 = acc_id.ToString();
                     p.Argument2 = char_block[i-1];
                     data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());

                     t = client.SendGetPacket(data);

                     if (t.GetType() == typeof(CharPaket) && ((CharPaket)t).Command != (uint)PacketHeader.HeaderCommand.CHAR_EMPTY)
                     {
                         if(((CharPaket)t).CharNum == 1)
                         char1 = (CharPaket)t;
                     }

                     if (t.GetType() == typeof(CharPaket) && ((CharPaket)t).Command != (uint)PacketHeader.HeaderCommand.CHAR_EMPTY)
                     {
                         if (((CharPaket)t).CharNum == 2)
                             char2 = (CharPaket)t;
                     }

                     if (t.GetType() == typeof(CharPaket) && ((CharPaket)t).Command != (uint)PacketHeader.HeaderCommand.CHAR_EMPTY)
                     {
                         if (((CharPaket)t).CharNum == 3)
                             char3 = (CharPaket)t;

                     }

                 }
                 catch { }

                #region Loading Graphics

                if (char1.Command != (uint)25)
                {
                    char1.sex = 'M';

                    char1_body = u.LoadTextureFromFile(char1.Class + ".png", d, Utils.TextureType.Charaset);
                    char1_head = u.LoadTextureFromFile($"{char1.hair_id}_{char1.sex}.png", d, Utils.TextureType.Hair);

                }

                if(char2.Command !=(uint)25)
                {
                    char2.sex = 'M';

                    char2_body = u.LoadTextureFromFile(char2.Class + ".png", d, Utils.TextureType.Charaset);
                    char2_head = u.LoadTextureFromFile($"{char2.hair_id}_{char2.sex}.png", d, Utils.TextureType.Hair);

                }

                if (char3.Command != (uint)25)
                {
                    //test
                    char3.sex = 'M';

                    char3_body = u.LoadTextureFromFile(char3.Class + ".png", d, Utils.TextureType.Charaset);
                    char3_head = u.LoadTextureFromFile($"{char3.hair_id}_{char3.sex}.png", d, Utils.TextureType.Hair);
                }

                #endregion

            }


        }

        private void Bt_usa3_OnMouseDown()
        {
            if (char3.Command != (uint)25)
            {
                game.SelectedChar = char3;
                game.gamestate._GameState = Enums.GameState.gameState.Game;
                game.SwitchScene(game.gamestate);
            }
        }

        private void Bt_usa2_OnMouseDown()
        {
            if(char2.Command != (uint)25)
            {
                game.SelectedChar = char2;
                game.gamestate._GameState = Enums.GameState.gameState.Game;
                game.SwitchScene(game.gamestate);
            }
        }

        private void Bt_usa1_OnMouseDown()
        {
            if(char1.Command != (uint)25)
            {
                game.SelectedChar = char1;
                game.gamestate._GameState = Enums.GameState.gameState.Game;
                game.SwitchScene(game.gamestate);
            }
        }

        private void Bt_create3_OnMouseDown()
        {
            if (char3.Command == (uint)25)
            {
                game.gamestate._GameState = Enums.GameState.gameState.CharCreation;
                game.charcreation.account_id = acc_id;
                game.charcreation.slot = 3;
                game.SwitchScene(game.gamestate);
            }
        }

        private void Bt_create2_OnMouseDown()
        {
            if (char2.Command == (uint)25)
            {
                game.gamestate._GameState = Enums.GameState.gameState.CharCreation;
                game.charcreation.account_id = acc_id;
                game.charcreation.slot = 2;
                game.SwitchScene(game.gamestate);
            }
        }

        private void Bt_create_OnMouseDown()
        {
            if (char1.Command == (uint)25)
            {
                game.gamestate._GameState = Enums.GameState.gameState.CharCreation;
                game.charcreation.account_id = acc_id;
                game.charcreation.slot = 1;
                game.SwitchScene(game.gamestate);
            }
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

            bt_usa1.Update(gameTime, ms, kb);
            bt_usa2.Update(gameTime, ms, kb);
            bt_usa3.Update(gameTime, ms, kb);

            #region Recover Character information

            if (char1.Command != (uint)PacketHeader.HeaderCommand.CHAR_EMPTY && select_id == 1)
            {
                name = char1.CharName;
                job = char1.Class;
                level = char1.level;
                exp = char1.exp;
                hp = char1.hp;
                sp = char1.sp;
                str = char1.str;
                agi = char1.agi;
                vit = char1.vit;
                ints = char1.intel;
                dex = char1.dex;
                luk = char1.luk;

            }
            if (char1.Command == (uint)PacketHeader.HeaderCommand.CHAR_EMPTY && select_id == 1)
            {
                name = "";
                job = "";
                level = 0;
                exp = 0;
                hp = 0;
                sp = 0;
                str = 0;
                agi = 0;
                vit = 0;
                ints = 0;
                dex = 0;
                luk = 0;
            }


            if (char2.Command != (uint)PacketHeader.HeaderCommand.CHAR_EMPTY && select_id == 2)
            {
                name = char2.CharName;
                job = char2.Class;
                level = char2.level;
                exp = char2.exp;
                hp = char2.hp;
                sp = char2.sp;
                str = char2.str;
                agi = char2.agi;
                vit = char2.vit;
                ints = char2.intel;
                dex = char2.dex;
                luk = char2.luk;

            }
            if (char2.Command == (uint)PacketHeader.HeaderCommand.CHAR_EMPTY && select_id == 2)
            {
                name = "";
                job = "";
                level = 0;
                exp = 0;
                hp = 0;
                sp = 0;
                str = 0;
                agi = 0;
                vit = 0;
                ints = 0;
                dex = 0;
                luk = 0;
            }


            if (char3.Command != (uint)PacketHeader.HeaderCommand.CHAR_EMPTY && select_id == 3)
            {
                name = char3.CharName;
                job = char3.Class;
                level = char3.level;
                exp = char3.exp;
                hp = char3.hp;
                sp = char3.sp;
                str = char3.str;
                agi = char3.agi;
                vit = char3.vit;
                ints = char3.intel;
                dex = char3.dex;
                luk = char3.luk;

            }
            if(char3.Command == (uint)PacketHeader.HeaderCommand.CHAR_EMPTY && select_id == 3)
            {
                   name = "";
                    job = "";
                    level = 0;
                    exp = 0;
                    hp = 0;
                    sp = 0;
                    str = 0;
                    agi = 0;
                    vit = 0;
                    ints = 0;
                    dex = 0;
                    luk = 0;
            }

            #endregion

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, new Rectangle(0, 0, d.PresentationParameters.BackBufferWidth, d.PresentationParameters.BackBufferHeight), Color.White);


            spriteBatch.DrawString(font, name, new Vector2(93, 365), Color.Black);
            spriteBatch.DrawString(font, job, new Vector2(93, 393), Color.Black);
            spriteBatch.DrawString(font, level.ToString(), new Vector2(93, 421), Color.Black);
            spriteBatch.DrawString(font, exp.ToString(), new Vector2(93, 449), Color.Black);
            spriteBatch.DrawString(font, hp.ToString(), new Vector2(93, 477), Color.Black);
            spriteBatch.DrawString(font, sp.ToString(), new Vector2(93, 505), Color.Black);

            spriteBatch.DrawString(font, str.ToString(), new Vector2(295, 365), Color.Black);
            spriteBatch.DrawString(font, agi.ToString(), new Vector2(295, 393), Color.Black);
            spriteBatch.DrawString(font, vit.ToString(), new Vector2(295, 421), Color.Black);
            spriteBatch.DrawString(font, ints.ToString(), new Vector2(295, 449), Color.Black);
            spriteBatch.DrawString(font, dex.ToString(), new Vector2(295, 477), Color.Black);
            spriteBatch.DrawString(font, luk.ToString(), new Vector2(295, 505), Color.Black);

            //spriteBatch.DrawString(font, name, new Vector2(m.Position.X,m.Position.Y), Color.Black);


            //Selector
            switch (select_id)
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


            //Draw Graphics
            if (char1.Command != (uint)25)
            {
                spriteBatch.Draw(char1_body, new Vector2(155, 140), new Rectangle(0, 0, 32, 48), Color.White);
                spriteBatch.Draw(char1_head, new Vector2(155, 140), new Rectangle(0, 0, 32, 48), Color.White);
            }


            if (char2.Command != (uint)25)
            {
                spriteBatch.Draw(char2_body, new Vector2(390, 140), new Rectangle(0, 0, 32, 48), Color.White);
                spriteBatch.Draw(char2_head, new Vector2(390, 140), new Rectangle(0, 0, 32, 48), Color.White);
            }

            if (char3.Command != (uint)25)
            {
                spriteBatch.Draw(char3_body, new Vector2(615, 140), new Rectangle(0, 0, 32, 48), Color.White);
                spriteBatch.Draw(char3_head, new Vector2(615, 140), new Rectangle(0, 0, 32, 48), Color.White);
            }



            if (char1.Command == (uint)PacketHeader.HeaderCommand.CHAR_EMPTY)
                bt_create1.Draw(spriteBatch);
            else
                bt_usa1.Draw(spriteBatch);

            if (char2.Command == (uint)PacketHeader.HeaderCommand.CHAR_EMPTY)
                bt_create2.Draw(spriteBatch);
            else
                bt_usa2.Draw(spriteBatch);

            if (char3.Command == (uint)PacketHeader.HeaderCommand.CHAR_EMPTY)
                bt_create3.Draw(spriteBatch);
            else
                bt_usa3.Draw(spriteBatch);

            //spriteBatch.DrawString(font,m.Position.ToVector2().ToString(), new Vector2(2, 30), Color.Yellow);
        }


        
    }
}
