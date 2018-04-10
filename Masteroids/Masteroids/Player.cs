using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Player : GameObject
    {
        private float rotation = 0.1f, speed = 0.3f, scale = 0.5f;
        private Vector2 distance, bulletPos;
        private MouseState mouseStateCurrent, mouseStatePrevious;
        private KeyboardState keyboardState, pastKeyboardState;
        private GamePadState gamePadStateCurrent, gamePadStatePrevious;
        private SpriteEffects entityFx;
        public PlayerIndex playerValue;
        public Rectangle playerRec;
        public Color[] textureData;
        public bool Dead;
        public float linearVelocity = 0.02f; //Frammåt
        public float rotationVelocity = 3f; //Hastighet den roterar

        List<Bullet> bulletList = new List<Bullet>();
        EntityManager entityMgr;

        public Player(Texture2D texture, Vector2 position, PlayerIndex playerValue, EntityManager entityMgr, Viewport viewport)
            : base(position, viewport)
        {
            this.playerValue = playerValue; ////Avgör spelare. -> återfinns på loadcontent Game1
            this.texture = texture;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            startPosition = new Vector2(200, 200);
            Dead = false;
            playerRec = new Rectangle(0, 0, texture.Width, texture.Height);
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            this.entityMgr = entityMgr;
            shouldWrap = true;
        }

        public override void Update(GameTime gameTime)
        {
            pastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();
            gamePadStatePrevious = gamePadStateCurrent;
            gamePadStateCurrent = GamePad.GetState(playerValue);

            position += velocity;
            bulletPos = new Vector2(position.X, position.Y);

            GamePadCapabilities capabilities =              
                GamePad.GetCapabilities(playerValue);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(playerValue);
                if (capabilities.HasLeftXThumbStick)
                {
                    //AInputGamePad();        //Kontroller för Asteroid
                    MInputGamePad();      //Kontroller för Masteroid
                }
            }

            //MInput();
            AInput();

            ScreenWrap();
        }

        private void CreateBullet()
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);
        }

        private void MInput()
        {
            distance.X = mouseStateCurrent.X - position.X;
            distance.Y = mouseStateCurrent.Y - position.Y;
            rotation = (float)Math.Atan2(distance.Y, distance.X) + (float)Math.PI / 2;
            position += velocity;

            if (keyboardState.IsKeyDown(Keys.A))
                velocity.X -= 0.1f;
            if (keyboardState.IsKeyDown(Keys.D))
                velocity.X += 0.1f;
            if (keyboardState.IsKeyDown(Keys.W))
                velocity.Y -= 0.1f;
            if (keyboardState.IsKeyDown(Keys.S))
                velocity.Y += 0.1f;

            if (mouseStatePrevious.LeftButton == ButtonState.Released
                && mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                CreateBullet();
            }
        }

        private void MInputGamePad()
        {
            velocity.X += gamePadStateCurrent.ThumbSticks.Left.X * speed;
            velocity.Y -= gamePadStateCurrent.ThumbSticks.Left.Y * speed;

            //rotation = gamePadState.ThumbSticks.Right.Y;

            if (gamePadStateCurrent.ThumbSticks.Right.X != 0)
                rotation = gamePadStateCurrent.ThumbSticks.Right.X;

            if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released)
            {
                CreateBullet();
            }
        }

        private void AInput()
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));

            if (keyboardState.IsKeyDown(Keys.A))            //Rotera med tangentbord Asteroids
                rotation -= MathHelper.ToRadians(rotationVelocity);
            if (keyboardState.IsKeyDown(Keys.D))
                rotation += MathHelper.ToRadians(rotationVelocity);
            
            if (keyboardState.IsKeyDown(Keys.W))            //Frammåt och Bakåt med tangentbord Asteroids
                velocity += direction * linearVelocity;
            if (keyboardState.IsKeyDown(Keys.S))
                velocity -= direction * linearVelocity / 3;

            if (keyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))   //Skjuta
                CreateBullet();
        }

        private void AInputGamePad()
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            position += velocity;

            if (gamePadStateCurrent.ThumbSticks.Left.X < -0.1f) //Rotera med styrspak
                rotation -= 0.07f;
            if (gamePadStateCurrent.ThumbSticks.Left.X > 0.1f)
                rotation += 0.07f;
            
            if (gamePadStateCurrent.Buttons.A == ButtonState.Pressed)  //Frammåt och Bakåt med Knappar
                velocity += direction * linearVelocity;
            if (gamePadStateCurrent.Buttons.B == ButtonState.Pressed)  //Bakåt men är inte cannon så placerad inom //
                velocity -= direction * linearVelocity;

            if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released)
            {
                entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {
                spriteBatch.Draw(texture, position, playerRec, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height - 20), scale, entityFx, 0);
                base.Draw(spriteBatch);
            }
        }

        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + wrapOffset, playerRec, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height - 20), scale, entityFx, 0);
        }

        private void Scrapped()     //Gammal kod som kan va gött att spara ifall man behöver
        {
            //GamePadCapabilities capabilities =
            //    GamePad.GetCapabilities(playerValue);
            //if (capabilities.IsConnected)
            //{
            //    GamePadState gamePadState = GamePad.GetState(playerValue);
            //    if (capabilities.HasLeftXThumbStick)
            //    {
            //        ////Rotera med styrspak
            //        //if (gamePadState.ThumbSticks.Left.X < -0.1f) //0.1f står för hur mycket spaken ska luta för att svänga.
            //        //    rotation -= 0.07f;
            //        //if (gamePadState.ThumbSticks.Left.X > 0.1f)
            //        //    rotation += 0.07f;

            //        //Frammåt och Bakåt med Knappar
            //        if (gamePadState.Buttons.A == ButtonState.Pressed)
            //            velocity += direction * linearVelocity;
            //        if (gamePadState.Buttons.B == ButtonState.Pressed) //Bakåt men är inte cannon så placerad inom //
            //            velocity -= direction * linearVelocity;

            //        velocity.X += gamePadState.ThumbSticks.Left.X * speed;
            //        velocity.Y -= gamePadState.ThumbSticks.Left.Y * speed;

            //        //rotation = gamePadState.ThumbSticks.Right.Y;
            //        if (gamePadState.ThumbSticks.Right.X < -0.1)
            //        {
            //            rotation = gamePadState.ThumbSticks.Right.X;
            //        }

            //        ////Frammåt och Bakåt med Styrspak
            //        //if (gamePadState.ThumbSticks.Left.Y > 0.5f)
            //        //    position += direction * linearVelocity;
            //        //if (gamePadState.ThumbSticks.Left.Y < -0.5f)
            //        //    position -= direction * linearVelocity/3;

            //        if (gamePadState.Buttons.Y == ButtonState.Pressed)
            //        {
            //            entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);
            //        }
            //    }
            //}
        }
    }
}
