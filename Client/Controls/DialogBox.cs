using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client.Controls
{
    public class DialogBox
    {
        GraphicsDevice d;
        ContentManager c;
        SkinSystem skin;
        SpriteFont font;
        SpriteFont symb;
        Game1 game;
        Utils u = new Utils();
        Texture2D bg;
        public TextBox tx;
        private string tex;

       // public string Text { get => tx.Text; set => tx.Text = value; }
        public string Text
        {
            get { return tx.Text; }
            set 
            {
                tex = value;
                tx.Text = tex;
            }
        }

        public DialogBox(GraphicsDevice dev,ContentManager manager,SkinSystem sk,SpriteFont f,SpriteFont s,Game1 g)
        {
            d = dev;
            c = manager;
            skin = sk;
            font = f;
            symb = s;
            game = g;

           bg = u.LoadTextureFromFile("Eau/basic_interface/dialog_bg.png", d, Utils.TextureType.SystemSkin);
            tx = new TextBox(new Vector2(190, d.PresentationParameters.BackBufferHeight - 28), "", font, skin, 200);
            tx.OnTextChanged += Tx_OnTextChanged;
            tx.Is_DialogBox = true;
        }

        string oldtext;
        private void Tx_OnTextChanged(TextBox_EventArgs e)
        {
            if (oldtext != Text)
            {
                Text = e.Text;
                oldtext = Text;
            }
        }

        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            tx.Update(gameTime, kb, ms);
        }

        public void Reset()
        {
            tx.Text = "";
            Text = "";

        }


        public void Draw(SpriteBatch sprite)
        {
             sprite.Draw(bg, new Vector2(80, d.PresentationParameters.BackBufferHeight - 30), Color.White);
             tx.Draw(sprite);
        }
    }
}
