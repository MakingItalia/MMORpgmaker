using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIXControls;

namespace MMORpgmaker_Client.GameScene
{
    internal class TitleScreen
    {
        UIXContainer container = new UIXContainer();
        UIXButton bt;
        UIXTextBox tx_user;
        UIXTextBox tx_pass;
        GraphicsDevice d;
        SpriteFont font, symb;
        Texture2D background;
        ContentManager Content;
 

        public TitleScreen(GraphicsDevice dev,ContentManager _Content,SpriteFont _font,SpriteFont _symb)
        {
            d = dev;
            font = _font;
            symb = _symb;
            Content = _Content;

            //Button
            bt = new UIXButton(d, font, new Vector2(370, 485));
            bt.Text = "Login";
            bt.OnMouseDown += Bt_OnMouseDown;
            container.Controls.Add(bt);


            //TextBox
            tx_user = new UIXTextBox(d, font, new Vector2(370, 420));
            container.Controls.Add(tx_user);

            tx_pass = new UIXTextBox(d, font, new Vector2(370, 450));
            tx_pass.IsPassword = true;
            container.Controls.Add(tx_pass);


            background = Content.Load<Texture2D>("Background");
        }

        private void Bt_OnMouseDown()
        {
            
        }

        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            container.Update(gameTime, ms, kb);

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Username", new Vector2(291, 421), Color.Black);
            spriteBatch.DrawString(font, "Username", new Vector2(290, 420), Color.Red);

            spriteBatch.DrawString(font, "Password", new Vector2(291, 451), Color.Red);
            spriteBatch.DrawString(font, "Password", new Vector2(290, 450), Color.Red);

            container.Draw(spriteBatch);

         
            spriteBatch.End();
        }

    }
}
