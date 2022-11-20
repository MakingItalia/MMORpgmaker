using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Controls;
using MMORpgmaker_Client.Controls;
using MMORpgmaker_Client.Enums;
using UIXControls;

namespace MMORpgmaker_Client
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        UIXContainer container = new UIXContainer();

        //Game State
        GameState gamestate = new GameState(GameState.gameState.TitleScreen);

        UIXButton bt;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            container.Initialize(GraphicsDevice, this);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("Segoe");
            Symb = Content.Load<SpriteFont>("symb");

            background = Content.Load<Texture2D>("Background");


            titleScreen = new GameScene.TitleScreen(GraphicsDevice,Content, font, Symb);

            skin = new SkinSystem(GraphicsDevice, "Eau"); //Skin Folder

            //---------------  Adding Controls for testing ------- \\
            //---- BUTTON

            m = new Message(skin, GraphicsDevice, font, "Unkow Error!");
            msg = new Msgbox(new Vector2(250, 300), skin, font, this);
            msg.refereced = m;
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (gamestate._GameState == GameState.gameState.TitleScreen)
            {
                // container.Update(gameTime, ms, kb);
                titleScreen.Update(gameTime, kb, ms);
                msg.Update(gameTime, kb, ms);
            }

            m.Update(gameTime, kb, ms);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            if (gamestate._GameState == GameState.gameState.TitleScreen)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
                
                msg.Draw(_spriteBatch);

                m.Draw(_spriteBatch);
                _spriteBatch.End();
            }

  


            base.Draw(gameTime);
        }
    }
}