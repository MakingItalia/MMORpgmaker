using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORpgmaker_Client.Enums
{
    public class GameState
    {
        public enum gameState
        {
            TitleScreen,
            Game,
            CharSelection
        }

        public gameState _GameState = gameState.TitleScreen;

        /// <summary>
        /// Constructor of Game State
        /// </summary>
        /// <param name="gameState">Initial State for game</param>
        public GameState(gameState gameState)
        {
            _GameState = gameState;
        }

    }
}
