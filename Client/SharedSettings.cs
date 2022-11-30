using Microsoft.Xna.Framework.Graphics;
using MMORpgmaker.Controls;
using MMORpgmaker_Client.Controls;
using MMORpgmaker_Client.GameScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client
{
    public partial class Game1
    {
        SpriteFont font;
        SpriteFont Symb;
        int count = 0;

        //Scene
        TitleScreen titleScreen;
        CharSelect charselection;
        public CharCreation charcreation;

        //Main Background
        Texture2D background;
        Texture2D _Mouse;

        Msgbox msg;

        /// <summary>
        /// Global MessageBox
        /// </summary>
        public Message m;

        SkinSystem skin;
        Dialog dlg;
    }
}
