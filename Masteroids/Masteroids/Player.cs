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
    class Player : GameObject //Andreas
    {
        private float scale = 0.5f, rotationVelocity = 3f;
        private float bulletTimer, bulletInterval;
        private Vector2 distance, bulletPos;
        private MouseState mouseStateCurrent, mouseStatePrevious;
        private KeyboardState keyboardState, pastKeyboardState;
        private GamePadState gamePadStateCurrent, gamePadStatePrevious;
        private SpriteEffects entityFx;
        public PlayerIndex playerValue;
        public Rectangle sourceRect;
        public bool Dead, AsteroidMode; //Asteroid playmode yes or no?

        List<Bullet> bulletList = new List<Bullet>();
        EntityManager entityMgr;

        public Player(Texture2D texture, Vector2 position, PlayerIndex playerValue, EntityManager entityMgr, Viewport viewport)
            : base(position, viewport)
        {
            this.playerValue = playerValue; ////Avgör spelare. -> återfinns på loadcontent Game1
            this.texture = texture;
            this.entityMgr = entityMgr;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            startPosition = new Vector2(200, 200);
            Dead = false;
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

            bulletPos = new Vector2(position.X, position.Y);
            AsteroidMode = false;

            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity;
            
            rotation = 0.1f;
            speed = 0.3f;

            GamePadCapabilities capabilities =              
                GamePad.GetCapabilities(playerValue);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(playerValue);
                if (capabilities.HasLeftXThumbStick)
                {
                    //MInputGamePad();          //Gamepad kontroller för Masteroid

                    AInputGamePad();          //GamepadKontroller för Asteroid
                }
            }

            //AInput(); //Keyboard kontroller till Asteroids
            MInput(); //Keyboard + mus kontroller till Masteroids. Denna metod måste läggas som kommentar för att inte störa andras rotation.

            ScreenWrap();
        }

        private void CreateBullet() //Skapar och skjuter skott i rätt riktning
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            bulletInterval = 0.2f;

            if (bulletTimer >= bulletInterval)
            {
                entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);
                bulletTimer = 0;
            }
        }

        private void MInput()   //Masteroids kontroller för Keyboard och Mus
        {
            distance.X = mouseStateCurrent.X - position.X;
            distance.Y = mouseStateCurrent.Y - position.Y;
            rotation = (float)Math.Atan2(distance.Y, distance.X) + (float)Math.PI / 2;

            if (keyboardState.IsKeyDown(Keys.A))
                velocity.X -= speed;
            if (keyboardState.IsKeyDown(Keys.D))
                velocity.X += speed;
            if (keyboardState.IsKeyDown(Keys.W))
                velocity.Y -= speed;
            if (keyboardState.IsKeyDown(Keys.S))
                velocity.Y += speed;

            if (mouseStatePrevious.LeftButton == ButtonState.Released
                && mouseStateCurrent.LeftButton == ButtonState.Pressed)
                CreateBullet();
        }

        private void MInputGamePad() //Masteroids kontroller för Gamepad
        {
            velocity.X += gamePadStateCurrent.ThumbSticks.Left.X * speed;
            velocity.Y -= gamePadStateCurrent.ThumbSticks.Left.Y * speed;

            //rotation = gamePadState.ThumbSticks.Right.Y;

            if (gamePadStateCurrent.ThumbSticks.Right.X != 0)
                rotation = gamePadStateCurrent.ThumbSticks.Right.X;

            if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && 
                gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released)
                CreateBullet();
        }

        private void AInput()   //Asteroids kontroller för keyboard
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));

            if (keyboardState.IsKeyDown(Keys.A))            //Rotera med tangentbord Asteroids
                rotation -= MathHelper.ToRadians(rotationVelocity);
            if (keyboardState.IsKeyDown(Keys.D))
                rotation += MathHelper.ToRadians(rotationVelocity);
            
            if (keyboardState.IsKeyDown(Keys.W))            //Frammåt och Bakåt med tangentbord Asteroids
                velocity += direction * speed;

            if (keyboardState.IsKeyDown(Keys.Space) && 
                pastKeyboardState.IsKeyUp(Keys.Space))   //Skjuta
                CreateBullet();
        }

        private void AInputGamePad() //Asteroids kontroller för Gamepad
        {
            rotationVelocity = 0.08f;

            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));

            if (gamePadStateCurrent.ThumbSticks.Left.X < -0.1f) //Rotera med styrspak
                rotation -= rotationVelocity;
            if (gamePadStateCurrent.ThumbSticks.Left.X > 0.1f)
                rotation += rotationVelocity;
            
            if (gamePadStateCurrent.Buttons.A == ButtonState.Pressed)  //Frammåt och Bakåt med Knappar
                velocity += direction * speed;

            if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && 
                gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released)
                CreateBullet();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {
                spriteBatch.Draw(texture, position, sourceRect, Color.White, rotation, 
                    new Vector2(texture.Width / 2, texture.Height - 20), scale, entityFx, 0);
                base.Draw(spriteBatch);
            }
        }

        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + wrapOffset, sourceRect, Color.White, 
                rotation, new Vector2(texture.Width / 2, texture.Height - 20), scale, entityFx, 0);
        }
    }
}
