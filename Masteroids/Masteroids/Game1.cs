using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Masteroids {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player1;
        Player player2;
        Vector2 playerPos, position;
        bool enteredGame = false;
        int screenWidth = 1920, screenHeight = 1080;
        SpriteFont font;


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D playerShip = Content.Load<Texture2D>("shipTex");

            //Player 1, Kontroll och Tangentbord
            player1 = new Player(playerShip, new Vector2(screenWidth / 2, screenHeight / 2), PlayerIndex.One);

            player2 = new Player(playerShip, new Vector2(200, 200), PlayerIndex.Two);

            font = Content.Load<SpriteFont>("font");

        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GamePadCapabilities capabilities =
                GamePad.GetCapabilities(PlayerIndex.Two);
            //GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.Two);
                //if (capabilities.HasLeftXThumbStick)
                //Player 2, Tangentboard endast
                if (gamePadState.Buttons.Start == ButtonState.Pressed)
                {
                    enteredGame = true;
                }
            }

            player1.Update(gameTime);
            if (enteredGame)
            {
                player2.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            player1.Draw(spriteBatch);

            if (enteredGame)
            {
                player2.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(font, "Press start to Enter", new Vector2(1700, 980), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
