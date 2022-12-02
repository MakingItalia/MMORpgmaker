using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Controls;
using MMORpgmaker.Helper;
using MMORpgmaker_Client.Controls;
using MMORpgmaker_Client.Enums;
using System;
using System.Net.Sockets;
using UIXControls;

namespace MMORpgmaker_Client
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        UIXContainer container = new UIXContainer();
        string ip = "127.0.0.1";
        bool serverstatus = false;
        int port = 6400;
        public Utils util = new Utils();
        NetworkStream ns;
        public Camera2d cam = new Camera2d();
        int plx = 100, ply = 100;
        public GameClient client = new GameClient("127.0.0.1", 6400);
        public int account_id = 0;
        public int mousex, mousey;

        //Game State
        public GameState gamestate = new GameState(GameState.gameState.TitleScreen);

       

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;          
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            IsMouseVisible = true;
            _graphics.ApplyChanges();
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

            skin = new SkinSystem(GraphicsDevice, "Eau"); //Skin Folder

            //Scenes
            titleScreen = new GameScene.TitleScreen(GraphicsDevice, Content, font, Symb);
            charselection = new GameScene.CharSelect(GraphicsDevice, Content,skin, font, Symb, this,client);
            charcreation = new GameScene.CharCreation(GraphicsDevice, Content, skin, font, Symb, this, client);

            _Mouse = util.LoadFromFileStream(Environment.CurrentDirectory + "/Content/SystemSkin/Mouse/Cursor.png", GraphicsDevice);

            //---------------  Adding Controls for testing ------- \\
            //---- BUTTON

            m = new Message(skin, GraphicsDevice, font, "Unkow Error!");
            msg = new Msgbox(new Vector2(250, 300), skin, font,client, this);
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

            if(gamestate._GameState == GameState.gameState.CharSelection)
            {
                charselection.Update(gameTime,kb,ms);
                
            }

            if (gamestate._GameState == GameState.gameState.CharCreation)
            {
                charcreation.Update(gameTime, kb, ms);

            }

            mousex = ms.X;
            mousey = ms.Y;

            m.Update(gameTime, kb, ms);
            base.Update(gameTime);
        }


        public void SwitchScene(GameState gameState)
        {
            if(gamestate._GameState == GameState.gameState.CharSelection)
            {
                charselection.LoadContent();
            }

            if(gamestate._GameState == GameState.gameState.CharCreation)
            {
                charcreation.LoadContent();
            }


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //TitleScreen Draw
            if (gamestate._GameState == GameState.gameState.TitleScreen)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
                
                msg.Draw(_spriteBatch);

                m.Draw(_spriteBatch);

                _spriteBatch.Draw(_Mouse, new Vector2(mousex, mousey), Color.White);
                _spriteBatch.End();
            }

  
            //CharSelection Draw
            if(gamestate._GameState == GameState.gameState.CharSelection)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                charselection.Draw(_spriteBatch);

                m.Draw(_spriteBatch);

                _spriteBatch.Draw(_Mouse, new Vector2(mousex, mousey), Color.White);
                _spriteBatch.End();
            }


            if(gamestate._GameState == GameState.gameState.CharCreation)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                charcreation.Draw(_spriteBatch);

                m.Draw(_spriteBatch);

                _spriteBatch.Draw(_Mouse, new Vector2(mousex, mousey), Color.White);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}