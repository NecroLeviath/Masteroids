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
        private State currentstate;
        private State nextState;

        public void ChangeState(State state)
        {
            nextState = state;
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
            HighScoreState.GetHighscore();
            HighScoreState.SetAsteoidScore(10);
            int r;
            int.TryParse("007365", out r);
            HighScoreState.SetMasteroidScore(r);

            currentstate = new MenuState(this, graphics.GraphicsDevice, Content, entityMgr);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (nextState != null)
            {
                currentstate = nextState;
                nextState = null;
            }
            currentstate.Update(gameTime);
            currentstate.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            currentstate.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
