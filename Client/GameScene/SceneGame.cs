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
    public class SceneGame
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

        public int acc_id = 0;
        public int char_num = 0;
        CharPaket chara;

        public SceneGame(GraphicsDevice dev, ContentManager c, SkinSystem sk, SpriteFont f, SpriteFont s, Game1 g, GameClient cli)
        {
            d = dev;
            content = c;
            skin = sk;
            font = f;
            symb = s;
            game = g;
            client = cli;

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

        }



        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {

        }
        

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
