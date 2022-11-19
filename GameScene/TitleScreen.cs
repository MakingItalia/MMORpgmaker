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

            bt = new UIXButton(d, font, new Vector2(50, 100));
            bt.Text = "Button";
            container.Controls.Add(bt);

            tx_user = new UIXTextBox(d, font, new Vector2(100, 150));
            container.Controls.Add(tx_user);

            background = Content.Load<Texture2D>("Background");
        }



        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            container.Update(gameTime, ms, kb);
            


        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            
            //spriteBatch.Draw(background, new Rectangle(0, 0, d.PresentationParameters.BackBufferWidth, d.PresentationParameters.BackBufferHeight), Color.White);
            container.Draw(spriteBatch);
            

            spriteBatch.End();
        }

    }
}
