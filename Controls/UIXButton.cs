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
    public class UIXButton : Controls
    {

        
        SpriteFont Font;

        public UIXButton(GraphicsDevice dev,SpriteFont font,Vector2 position,int width=130,int height = 30) : base(dev)
        {
            Initialize(typeof(UIXButton));
            base.X = (int)position.X;
            base.Y = (int)position.Y;
            base.Width = width;
            base.Height = height;
            Font = font;
            
        }



        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            base.Draw(sprite);

            if(!Focused)
                sprite.Draw(base.ControlGraphic, new Rectangle(X,Y,Width,Height), Color.White);
            else
                sprite.Draw(base.ControlGraphic, new Rectangle(X, Y, Width, Height), HighlightColor);

            Vector2 size = Font.MeasureString(Text);

            sprite.DrawString(Font, Text, new Vector2(X + (Width - size.X) / 2, Y + (Height - size.Y) / 2), Color.Black);
        }


        public override void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            base.Update(gameTime, ms, kb);

        }
    }
}
