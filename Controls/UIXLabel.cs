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
    public class UIXLabel : Controls
    {
      
        private SpriteFont f;

        /// <summary>
        /// Get or set Text of label
        /// </summary>
      

        public Color TextColor { get; set; }


        public UIXLabel(GraphicsDevice dev,SpriteFont font,Vector2 position,string Text,Color textColor) : base(dev)
        {
            base.Position = position;
            base.Text = Text;
            f = font;
            TextColor = textColor;
        }

         
        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);
            sprite.DrawString(f, Text, Position, TextColor);
        }


        public override void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            base.Update(gameTime, ms, kb);
        }


    }
}
