using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/* UIX SDK BY Thejuster
 * see my other project on pierotofy.it and Github
 * */


namespace UIXControls
{


    public class UIXContainer
    {
        Game g;
        
        GraphicsDevice dev;
        public SpriteFont DefaultFont;
        public SpriteFont SymbolFont;

        public UIXList<Controls> Controls = new UIXList<Controls>();

        public void Initialize(GraphicsDevice device,Game game)
        {
            dev = device;
            g = game;
            
        }


        public void Draw(SpriteBatch sprite)
        {
           
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Draw(sprite);
            }
        }


        public void Update(GameTime gameTime, MouseState m, KeyboardState k)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Update(gameTime, m, k);
            }
        }



      

    }
}
