using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dev">GraphicsDevice</param>
        /// <param name="f">Font</param>
        /// <param name="s">Symbol Font</param>
        /// <param name="g">Main Game Class</param>
        public CharSelect(GraphicsDevice dev,ContentManager c,SpriteFont f,SpriteFont s,Game1 g)
        {
            game = g;
            d = dev;
            font = f;
            symb = s;
            content = c;
                
        }


        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }


        public void Draw(SpriteBatch spriteBatch)
        {

        }


        
    }
}
