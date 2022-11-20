using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MMORpgmaker.Controls
{
    interface IControls
    {
        void Initialize();

        void Draw(SpriteBatch sprite);

        void Update(GameTime gameTime, KeyboardState kb, MouseState ms);
    }
}
