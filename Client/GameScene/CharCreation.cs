using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Controls;
using MMORpgmaker.Helper;
using MMORpgmaker_Client.Controls;
using Packet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client.GameScene
{
    public class CharCreation
    {
        GraphicsDevice d;
        ContentManager Content;
        SkinSystem skin;
        SpriteFont font;
        SpriteFont Symb;
        Game1 game;
        GameClient client;
        Utils u = new Utils();
        TextBox tx;
        MouseState m;

        public int account_id = 0;
        public int slot = 0;

        public string SEX = "M";

        Texture2D bg;

        Texture2D @base;
        Texture2D sx, dx;
        Texture2D panel;
        Texture2D crea;

        TexturedButton str_sx, str_dx, agi_sx, agi_dx, vit_sx, vit_dx, int_sx, int_dx,
            de_sx, de_dx,lu_sx,lu_dx;
        TexturedButton bt_crea;

        TexturedButton hair_left, hair_right;

        struct hairs
        {
            public int id;
            public Texture2D tx;
        }

        List<hairs> hairpart = new List<hairs>();
        int hair_sel = 0;

        int str = 5, agi = 5, vit = 5, ints = 5, dex = 5, luk = 5;

        int counter_press = 0;
        int point_left = 0;

        public CharCreation(GraphicsDevice dev, ContentManager c, SkinSystem sk, SpriteFont f, SpriteFont s, Game1 g, GameClient cli)
        {
            d = dev;
            Content = c;
            skin = sk;
            font = f;
            Symb = s;
            game = g;
            client = cli;
        }

        public void LoadContent()
        {
            bg = u.LoadTextureFromFile("win_make.png", d, Utils.TextureType.SystemSkin);
            tx = new TextBox(new Vector2(100, 435), "", font, skin,180);

            //Loading Chara Template
            @base = u.LoadTextureFromFile("Novice.png", d, Utils.TextureType.Charaset);
            sx = u.LoadTextureFromFile("arw_sx.png", d, Utils.TextureType.SystemSkin);
            dx = u.LoadTextureFromFile("arw_dx.png", d, Utils.TextureType.SystemSkin);
            panel = u.LoadTextureFromFile("textbox.png", d, Utils.TextureType.SystemSkin);
            crea = u.LoadTextureFromFile("btn_crea.png", d, Utils.TextureType.SystemSkin);


            //Button

            bt_crea = new TexturedButton(crea, 365, 460);

            str_sx = new TexturedButton(sx, 658, 80,15,15);
            str_dx = new TexturedButton(dx, 715, 80, 15, 15);

            agi_sx  = new TexturedButton(sx, 658, 107, 15, 15);
            agi_dx  = new TexturedButton(dx, 715, 107, 15, 15);

            vit_sx = new TexturedButton(sx, 658, 135, 15, 15);
            vit_dx = new TexturedButton(dx, 715, 135, 15, 15);

            int_sx = new TexturedButton(sx, 658, 161, 15, 15);
            int_dx = new TexturedButton(dx, 715, 161, 15, 15);

            de_sx = new TexturedButton(sx, 658, 192, 15, 15);
            de_dx = new TexturedButton(dx, 715, 192, 15, 15);
            
            lu_sx = new TexturedButton(sx, 658, 220, 15, 15);
            lu_dx = new TexturedButton(dx, 715, 220, 15, 15);
            //352 x 290

            hair_left = new TexturedButton(sx,352, 290, 20, 20);
            hair_right = new TexturedButton(dx, 440, 290, 20, 20);

            //event
            str_sx.OnMouseDown += Str_sx_OnMouseDown;
            str_dx.OnMouseDown += Str_dx_OnMouseDown;
            agi_sx.OnMouseDown += Agi_sx_OnMouseDown;
            agi_dx.OnMouseDown += Agi_dx_OnMouseDown;
            vit_sx.OnMouseDown += Vit_sx_OnMouseDown;
            vit_dx.OnMouseDown += Vit_dx_OnMouseDown;
            int_sx.OnMouseDown += Int_sx_OnMouseDown;
            int_dx.OnMouseDown += Int_dx_OnMouseDown;
            de_sx.OnMouseDown += De_sx_OnMouseDown;
            de_dx.OnMouseDown += De_dx_OnMouseDown;
            lu_sx.OnMouseDown += Lu_sx_OnMouseDown;
            lu_dx.OnMouseDown += Lu_dx_OnMouseDown;

            bt_crea.OnMouseDown += Bt_crea_OnMouseDown;

            hair_left.OnMouseDown += Hair_left_OnMouseDown;
            hair_right.OnMouseDown += Hair_right_OnMouseDown;

            //Loading Hairs
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + $"/Content/Class/Hair");
            foreach(string s in files)
            {
                if(s.Contains("_" + SEX))
                {
                    string a = s.Replace('\\', '/');
                    string[] f = a.Split('/');
                    string[] res = f[f.Length - 1].Split('_');
                    hairs h = new hairs();
                    h.id = int.Parse(res[0]);
                   h.tx = u.LoadTextureFromFile(f[f.Length-1], d, Utils.TextureType.Hair);
                    hairpart.Add(h);
                }
            }

        }

        private void Hair_right_OnMouseDown()
        {
            if(hair_sel < hairpart.Count-1 && counter_press ==0)
            {
                hair_sel++;
                counter_press = 10;
            }
        }

        private void Hair_left_OnMouseDown()
        {
            if(hair_sel > 0 && counter_press ==0)
            {
                hair_sel--;
                counter_press = 10;
            }
        }

        private void Bt_crea_OnMouseDown()
        {
            if (point_left > 0)
            {
                game.m.MessageText = "Please use all Stats Point";
                game.m.ShowMessage();
            }
            else if(tx.Text.Length < 5)
            {
                game.m.MessageText = "Character Name nimimum 5 Char";
                game.m.ShowMessage();
            }
            else
            {
                CharPaket ch = new CharPaket();
                ch.Command = (uint)PacketHeader.HeaderCommand.ACT_CREATE_CHAR;
                ch.CharNum = slot;
                ch.accountID = account_id;
                ch.CharName = tx.Text;
                ch.Class = "Novice";
                ch.exp = 0;
                ch.agi = agi;
                ch.luk = luk;
                ch.hp = 100;
                ch.max_hp = 100;
                ch.sp = 50;
                ch.max_sp = 50;
                ch.str = str;
                ch.vit = vit;
                ch.intel = ints;
                ch.dex = dex;
                ch.hair_id = hairpart[hair_sel].id;

                //Send Packet
                byte[] data = u.AssemblyPacket(PacketHeader.PacketType.CharPacket, ch.Serialize());
                object t = client.SendGetPacket(data);

                PacketData p = (PacketData)t;
                if(p.Command == (uint)PacketHeader.HeaderCommand.ACT_CONFIRM)
                {
                    if(p.Argument1.ToLower().Contains("true"))
                    {
                        game.gamestate._GameState = Enums.GameState.gameState.CharSelection;
                        game.SwitchScene(game.gamestate);
                    }
                }

            }
        }

        private void Lu_dx_OnMouseDown()
        {
            if (counter_press == 0 && point_left > 0)
            {
                luk++;
                point_left--;
                counter_press = 10;
            }
        }

        private void Lu_sx_OnMouseDown()
        {
            if (luk > 0 && counter_press == 0)
            {
                luk--;
                point_left++;
                counter_press = 10;
            }
        }

        private void De_dx_OnMouseDown()
        {
            if (counter_press == 0 && point_left > 0)
            {
                dex++;
                point_left--;
                counter_press = 10;
            }
        }

        private void De_sx_OnMouseDown()
        {
            if (dex > 0 && counter_press == 0)
            {
                dex--;
                point_left++;
                counter_press = 10;
            }
        }

        private void Int_dx_OnMouseDown()
        {
            if (counter_press == 0 && point_left > 0)
            {
                ints++;
                point_left--;
                counter_press = 10;
            }
        }

        private void Int_sx_OnMouseDown()
        {
            if (ints > 0 && counter_press == 0)
            {
                ints--;
                point_left++;
                counter_press = 10;
            }
        }

        private void Vit_dx_OnMouseDown()
        {
            if (counter_press == 0 && point_left > 0)
            {
                vit++;
                point_left--;
                counter_press = 10;
            }
        }

        private void Vit_sx_OnMouseDown()
        {
            if (vit > 0 && counter_press == 0)
            {
                vit--;
                point_left++;
                counter_press = 10;
            }
        }

        private void Agi_dx_OnMouseDown()
        {
            if (counter_press == 0 && point_left > 0)
            {
                agi++;
                point_left--;
                counter_press = 10;
            }
        }

        private void Agi_sx_OnMouseDown()
        {
            if (agi > 0 && counter_press == 0)
            {
                agi--;
                point_left++;
                counter_press = 10;
            }
        }

        private void Str_dx_OnMouseDown()
        {
            if (counter_press == 0 && point_left > 0)
            {
                str++;
                point_left--;
                counter_press = 10;
            }
        }

        private void Str_sx_OnMouseDown()
        {
            if (str > 0 && counter_press==0)
            {
                str--;
                point_left++;
                counter_press = 10;
            }
        }

        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            tx.Update(gameTime, kb, ms);
            m = ms;

            if (counter > 10)
            {
                frame += 32;
                if (frame > 32 * 3)
                    frame = 0;
                counter = 0;
            }

            counter++;

            if (counter_press > 0)
                counter_press--;

            str_sx.Update(gameTime, ms, kb);
            str_dx.Update(gameTime, ms, kb);
            agi_sx.Update(gameTime, ms, kb);
            agi_dx.Update(gameTime, ms, kb);
            vit_dx.Update(gameTime, ms, kb);
            vit_sx.Update(gameTime, ms, kb);
            int_sx.Update(gameTime, ms, kb);
            int_dx.Update(gameTime, ms, kb);
            de_dx.Update(gameTime, ms, kb);
            de_sx.Update(gameTime, ms, kb);
            lu_dx.Update(gameTime, ms, kb);
            lu_sx.Update(gameTime, ms, kb);
            bt_crea.Update(gameTime, ms, kb);
            hair_left.Update(gameTime, ms, kb);
            hair_right.Update(gameTime, ms, kb);
        }


        int frame = 0;
        int counter = 0;
        public void Draw(SpriteBatch spriteBatch)
        {
            

            spriteBatch.Draw(bg, new Rectangle(0, 0, d.PresentationParameters.BackBufferWidth, d.PresentationParameters.BackBufferHeight), Color.White);

            tx.Draw(spriteBatch);
            spriteBatch.Draw(@base,new Rectangle(100,250,32*2,48*2), new Rectangle(frame,0,32,48), Color.White);

            //Draw Hair
            spriteBatch.Draw(hairpart[hair_sel].tx, new Rectangle(100, 250, 32 * 2, 48 * 2), new Rectangle(frame, 0, 32, 48), Color.White);


            //Hair Selection
            spriteBatch.Draw(hairpart[hair_sel].tx, new Rectangle(375, 270, 32 * 2, 48 * 2), new Rectangle(0, 0, 32, 48), Color.White);

            #region Draw Stats

            if (point_left > 0)
            {
                spriteBatch.DrawString(font, str.ToString(), new Vector2(690, 78), Color.Red);
                spriteBatch.DrawString(font, agi.ToString(), new Vector2(690, 105), Color.Red);
                spriteBatch.DrawString(font, vit.ToString(), new Vector2(690, 134), Color.Red);
                spriteBatch.DrawString(font, ints.ToString(), new Vector2(690, 160), Color.Red);
                spriteBatch.DrawString(font, dex.ToString(), new Vector2(690, 190), Color.Red);
                spriteBatch.DrawString(font, luk.ToString(), new Vector2(690, 218), Color.Red);
            }
            else
            {
                if(str>0)
                    spriteBatch.DrawString(font, str.ToString(), new Vector2(690, 78), Color.Black);
                else
                    spriteBatch.DrawString(font, str.ToString(), new Vector2(690, 78), Color.Red);

                if(agi>0)
                    spriteBatch.DrawString(font, agi.ToString(), new Vector2(690, 105), Color.Black);
                else
                    spriteBatch.DrawString(font, agi.ToString(), new Vector2(690, 105), Color.Red);

                if(vit>0)
                    spriteBatch.DrawString(font, vit.ToString(), new Vector2(690, 134), Color.Black);
                else
                    spriteBatch.DrawString(font, vit.ToString(), new Vector2(690, 134), Color.Red);

                if(ints>0)
                    spriteBatch.DrawString(font, ints.ToString(), new Vector2(690, 160), Color.Black);
                else
                    spriteBatch.DrawString(font, ints.ToString(), new Vector2(690, 160), Color.Red);

                if(dex>0)
                    spriteBatch.DrawString(font, dex.ToString(), new Vector2(690, 190), Color.Black);
                else
                    spriteBatch.DrawString(font, dex.ToString(), new Vector2(690, 190), Color.Red);

                if(luk>0)
                        spriteBatch.DrawString(font, luk.ToString(), new Vector2(690, 218), Color.Black);
                    else
                        spriteBatch.DrawString(font, luk.ToString(), new Vector2(690, 218), Color.Red);
            }

            #endregion

            #region Draw Arrow

            if (str >0)
            str_sx.Draw(spriteBatch);

            if (agi > 0)
                agi_sx.Draw(spriteBatch);

            if (vit > 0)
                vit_sx.Draw(spriteBatch);

            if (ints > 0)
                int_sx.Draw(spriteBatch);

            if (dex > 0)
                de_sx.Draw(spriteBatch);

            if (luk > 0)
                lu_sx.Draw(spriteBatch);


            //if have point left
            if (point_left > 0)
            {
                str_dx.Draw(spriteBatch);
                agi_dx.Draw(spriteBatch);
                vit_dx.Draw(spriteBatch);
                int_dx.Draw(spriteBatch);
                de_dx.Draw(spriteBatch);
                lu_dx.Draw(spriteBatch);
            }

            #endregion

            if(hair_sel > 0)
            hair_left.Draw(spriteBatch);

            if(hair_sel < hairpart.Count-1)
            hair_right.Draw(spriteBatch);

            bt_crea.Draw(spriteBatch);

            //Panel
            spriteBatch.Draw(panel,new Rectangle(635,250,120,20), Color.White);
            spriteBatch.DrawString(font, $"Point Left: {point_left}", new Vector2(638, 252), Color.Black);




            spriteBatch.DrawString(font, m.Position.ToVector2().ToString(), new Vector2(2, 30), Color.Yellow);

        }



    }
}
