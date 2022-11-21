
namespace MMORpgmaker
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        int accountID = 0;

        struct character
        {
            public string name;
            public string job;
            public int hair_id;
            public int level;
            public int exp;
            public int max_hp;
            public int max_sp;
            public int hp;
            public int sp;
            public int str;
            public int agi;
            public int vit;
            public int intel;
            public int dex;
            public int luk;
        }

        character SelectedCharacter = new character();

        character CharInfo1 = new character();
        character CharInfo2 = new character();
        character CharInfo3 = new character();

        /// <summary>
        /// Posizione assegnata alla creazione di un personaggio
        /// </summary>
        public int charSelPos = 1;
    }
}