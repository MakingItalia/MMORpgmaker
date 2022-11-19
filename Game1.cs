using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UIXControls;

namespace MMORpgmaker_Client
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        UIXContainer container = new UIXContainer();
        SpriteFont font;
        SpriteFont Symb;
        int count = 0;

        UIXButton bt;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            //---------------  Adding Controls for testing ------- \\
            //---- BUTTON

            bt = new UIXButton(GraphicsDevice, font, new Vector2(50, 100));
            bt.Text = "Button Value: " + count;
            bt.OnMouseDown += Bt_OnMouseDown;
            container.Controls.Add(bt);
        }


        private void Bt_OnMouseDown()
        {
            count++;
            bt.Text = "Button Value: " + count;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            container.Update(gameTime, ms, kb);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            container.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}