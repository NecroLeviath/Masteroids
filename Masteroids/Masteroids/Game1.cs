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
        Texture2D bosstex, skottTex;
        Vector2 bosspos;
        EntityManager entityMgr;
        Boss boss;
        AsteroidSpawner asteroidSpawner;
        Bullet bullet;
        Player player1;
        Player player2;
        Vector2 playerPos, position;
        bool enteredGame = false;
        int screenWidth = 1920, screenHeight = 1080;
        SpriteFont font;
        Viewport defaultView;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.ToggleFullScreen();
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
            Window.IsBorderless = true;
        }
        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultView = GraphicsDevice.Viewport;
            entityMgr = new EntityManager(defaultView);
            asteroidSpawner = new AsteroidSpawner(entityMgr, defaultView);
            Art.Initialize(Content);
            bosstex = Content.Load<Texture2D>("boss");
            skottTex = Content.Load<Texture2D>("laser");
            boss = new Boss(bosspos, entityMgr);
            Texture2D playerShip = Content.Load<Texture2D>("shipTex");

            //Player 1, Kontroll och Tangentbord
            player1 = new Player(playerShip, new Vector2(screenWidth / 2, screenHeight / 2), PlayerIndex.One, entityMgr, defaultView);
            
            player2 = new Player(playerShip, new Vector2(200, 200), PlayerIndex.Two, entityMgr, defaultView);

            font = Content.Load<SpriteFont>("font");

            bosspos = new Vector2(250, 50);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            asteroidSpawner.Update(gameTime);

            //PixelCollision();
            boss.Update(gameTime);
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

            entityMgr.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            boss.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            entityMgr.Draw(spriteBatch);

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
        static bool PixelCollision(Rectangle rect1, Color[] data1,
                                   Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);
            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color colorA = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color colorB = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (colorA.A > 250 && colorB.A > 0)
                        return true;
                }
            return false;
        }
        public void PixelCollision()
        {
            if (player1.playerRec.Intersects(bullet.bulletRec))
            {
                if (PixelCollision(player1.playerRec, player1.textureData, bullet.bulletRec, bullet.textureData))
                {
                    player1.Dead = true;
                }
            }
        }
    }
}
