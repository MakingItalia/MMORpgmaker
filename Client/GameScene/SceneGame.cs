using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Controls;
using MMORpgmaker.Helper;
using MMORpgmaker.Work;
using MMORpgmaker_Client.Controls;
using Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client.GameScene
{
    public class SceneGame
    {

        Game1 game;
        GraphicsDevice d;
        SpriteFont font, symb,font2;
        ContentManager content;
        SkinSystem skin;
        MouseState m;
        KeyboardState k;
        Utils u = new Utils();
        GameClient client;


        public int acc_id = 0;
        public int char_num = 0;
        CharPaket chara;
        Dialog talk;
        Texture2D MainPlayer_head,MainPlayer_body;
        Player MainPlayer;
        DialogBox dialogbox;

        //Texture2D DialogBox;

        public SceneGame(GraphicsDevice dev, ContentManager c, SkinSystem sk, SpriteFont f, SpriteFont s, Game1 g, GameClient cli)
        {
            d = dev;
            content = c;
            skin = sk;
            font = f;
            symb = s;
            game = g;
            client = cli;

            font2 = c.Load<SpriteFont>("Segoe2");

        }


        public void LoadContent(int account_id)
        {
            acc_id = account_id;
            
            char_num = game.charselection.select_id;

            PacketData p = new PacketData();
            p.Command = (uint)PacketHeader.HeaderCommand.ACT_GET_CHAR;
            p.Argument1 = acc_id.ToString();
            p.Argument2 = char_num.ToString();

            byte[] data = u.AssemblyPacket(PacketHeader.PacketType.PacketData, p.Serialize());
            object t = client.SendGetPacket(data);
            chara = (CharPaket)t;

            //DialogBox = u.LoadTextureFromFile("Eau/basic_interface/dialog_bg.png", d, Utils.TextureType.SystemSkin);
            dialogbox = new DialogBox(d, content, skin, font2, symb, game);

            

            chara.sex = 'M';
            MainPlayer_body = u.LoadTextureFromFile(chara.Class + ".png", d, Utils.TextureType.Charaset);
            MainPlayer_head = u.LoadTextureFromFile($"{chara.hair_id}_{chara.sex}.png", d, Utils.TextureType.Hair);

            MainPlayer = new Player(d, MainPlayer_body,MainPlayer_head, chara, font2, new Vector2(100, 100),client,game.cam);

            game.cam.Pos = new Vector2(MainPlayer.X, MainPlayer.Y);
            MainPlayer.talk.OnTalkCompleted += Talk_OnTalkCompleted;
            
        }


        private void Talk_OnTalkCompleted()
        {
            dialogbox.Reset();
        }

        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            MainPlayer.Update(gameTime, kb, ms);

            if(kb.IsKeyDown(Keys.Enter))
            {
                MainPlayer.Talk(dialogbox.Text);
                
            }

            if (kb.IsKeyDown(Keys.Right))
                MainPlayer.MoveRight();

            if (kb.IsKeyDown(Keys.Left))
                MainPlayer.MoveLeft();

            if (kb.IsKeyDown(Keys.Up))
                MainPlayer.MoveUp();

            if (kb.IsKeyDown(Keys.Down))
                MainPlayer.MoveDown();


            if(kb.IsKeyDown(Keys.F12) && game.edit_mode)
            {
                game.m.MessageText = "You are on Edit Mode";
                game.m.ShowMessage();
            }

            dialogbox.Update(gameTime, kb, ms);
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            MainPlayer.Draw(spriteBatch);

        }

        public void DrawUI(SpriteBatch sprite)
        {
            dialogbox.Draw(sprite);
            //sprite.Draw(DialogBox, new Vector2(80, d.PresentationParameters.BackBufferHeight-30), Color.White);
        }
    }
}
