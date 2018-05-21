using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Masteroids
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport defaultView;
        int screenWidth = 1920, screenHeight = 1080;
        private State _currentstate; // bort med "_" johns misstag
        private State _nextState; // bort med "_" johns misstag

        public void ChangeState(State state) { //måsvinge
            _nextState = state; // bort med "_" johns misstag
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();

            Window.IsBorderless = true;
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultView = GraphicsDevice.Viewport;
            Assets.Initialize(Content);
			EntityManager entityMgr = new EntityManager(defaultView);
            _currentstate = new MenuState(this, graphics.GraphicsDevice, Content, entityMgr); // bort med "_" johns misstag
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (_nextState != null) { //måsvinge johns misstag
                _currentstate = _nextState;
                _nextState = null;
            }
            _currentstate.Update(gameTime); // bort med "_" johns misstag
            _currentstate.PostUpdate(gameTime); // bort med "_" johns misstag

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            _currentstate.Draw(gameTime, spriteBatch); // bort med "_" johns misstag
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
