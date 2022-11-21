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
    public interface IControl
    {
         int X { get; set; }
         int Y { get; set; }
         Vector2 Position { get; set; }

         void Initialize(object ControlType);
         void Draw(SpriteBatch sprite);
         void Update(GameTime gameTime,MouseState ms,KeyboardState kb);


    }
}
