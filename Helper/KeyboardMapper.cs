using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MMORpgmaker.Helper
{
    public class KeyboardMapper
    {
        int key_pause = 0;
        string Text = "";
        KeyboardState kb;

       KeyboardState oldState;
       KeyboardState newState;

         public bool enter = false;
        public KeyboardMapper(string currentText)
        {
            Text = currentText;
        }

   
        public string Update(KeyboardState kb)
        {

            newState = kb;

            if (newState.IsKeyUp(Keys.Back) && oldState.IsKeyDown(Keys.Back)) 
            {
                if(Text.Length>0)
                Text = Text.Substring(0, Text.Length - 1);       
            }


            if (newState.IsKeyUp(Keys.Q) && oldState.IsKeyDown(Keys.Q)) { Text = Text + "q"; }
            if (newState.IsKeyUp(Keys.W) && oldState.IsKeyDown(Keys.W)) { Text = Text + "w"; }
            if (newState.IsKeyUp(Keys.E) && oldState.IsKeyDown(Keys.E)) { Text = Text + "e"; }
            if (newState.IsKeyUp(Keys.R) && oldState.IsKeyDown(Keys.R)) { Text = Text + "r"; }
            if (newState.IsKeyUp(Keys.T) && oldState.IsKeyDown(Keys.T)) { Text = Text + "t"; }
            if (newState.IsKeyUp(Keys.Y) && oldState.IsKeyDown(Keys.Y)) { Text = Text + "y"; }
            if (newState.IsKeyUp(Keys.U) && oldState.IsKeyDown(Keys.U)) { Text = Text + "u"; }
            if (newState.IsKeyUp(Keys.I) && oldState.IsKeyDown(Keys.I)) { Text = Text + "i"; }
            if (newState.IsKeyUp(Keys.O) && oldState.IsKeyDown(Keys.O)) { Text = Text + "o"; }
            if (newState.IsKeyUp(Keys.P) && oldState.IsKeyDown(Keys.P)) { Text = Text + "p"; }
            if (newState.IsKeyUp(Keys.A) && oldState.IsKeyDown(Keys.A)) { Text = Text + "a"; }
            if (newState.IsKeyUp(Keys.S) && oldState.IsKeyDown(Keys.S)) { Text = Text + "s"; }
            if (newState.IsKeyUp(Keys.D) && oldState.IsKeyDown(Keys.D)) { Text = Text + "d"; }
            if (newState.IsKeyUp(Keys.F) && oldState.IsKeyDown(Keys.F)) { Text = Text + "f"; }
            if (newState.IsKeyUp(Keys.G) && oldState.IsKeyDown(Keys.G)) { Text = Text + "g"; }
            if (newState.IsKeyUp(Keys.H) && oldState.IsKeyDown(Keys.H)) { Text = Text + "h"; }
            if (newState.IsKeyUp(Keys.J) && oldState.IsKeyDown(Keys.J)) { Text = Text + "j"; }
            if (newState.IsKeyUp(Keys.K) && oldState.IsKeyDown(Keys.K)) { Text = Text + "k"; }
            if (newState.IsKeyUp(Keys.L) && oldState.IsKeyDown(Keys.L)) { Text = Text + "l"; }
            if (newState.IsKeyUp(Keys.Z) && oldState.IsKeyDown(Keys.Z)) { Text = Text + "z"; }
            if (newState.IsKeyUp(Keys.X) && oldState.IsKeyDown(Keys.X)) { Text = Text + "x"; }
            if (newState.IsKeyUp(Keys.C) && oldState.IsKeyDown(Keys.C)) { Text = Text + "c"; }
            if (newState.IsKeyUp(Keys.V) && oldState.IsKeyDown(Keys.V)) { Text = Text + "v"; }
            if (newState.IsKeyUp(Keys.B) && oldState.IsKeyDown(Keys.B)) { Text = Text + "b"; }
            if (newState.IsKeyUp(Keys.N) && oldState.IsKeyDown(Keys.N)) { Text = Text + "n"; }
            if (newState.IsKeyUp(Keys.M) && oldState.IsKeyDown(Keys.M)) { Text = Text + "m"; }
            if (newState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space)) { Text = Text + " "; }
            if (newState.IsKeyUp(Keys.D1) && oldState.IsKeyDown(Keys.D1)) { Text = Text + "1"; }
            if (newState.IsKeyUp(Keys.D2) && oldState.IsKeyDown(Keys.D2)) { Text = Text + "2"; }
            if (newState.IsKeyUp(Keys.D3) && oldState.IsKeyDown(Keys.D3)) { Text = Text + "3"; }
            if (newState.IsKeyUp(Keys.D4) && oldState.IsKeyDown(Keys.D4)) { Text = Text + "4"; }
            if (newState.IsKeyUp(Keys.D5) && oldState.IsKeyDown(Keys.D5)) { Text = Text + "5"; }
            if (newState.IsKeyUp(Keys.D6) && oldState.IsKeyDown(Keys.D6)) { Text = Text + "6"; }
            if (newState.IsKeyUp(Keys.D7) && oldState.IsKeyDown(Keys.D7)) { Text = Text + "7"; }
            if (newState.IsKeyUp(Keys.D8) && oldState.IsKeyDown(Keys.D8)) { Text = Text + "8"; }
            if (newState.IsKeyUp(Keys.D9) && oldState.IsKeyDown(Keys.D9)) { Text = Text + "9"; }
            if (newState.IsKeyUp(Keys.D0) && oldState.IsKeyDown(Keys.D0)) { Text = Text + "0"; }
            if (newState.IsKeyUp(Keys.Enter) && oldState.IsKeyDown(Keys.Enter)) { enter = true; }
            oldState = newState;
            return Text;
        }
    }
}
