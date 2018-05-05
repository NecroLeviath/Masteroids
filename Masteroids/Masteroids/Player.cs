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
    public class Player : GameObject //Andreas
    {
        private float scale = 0.5f, rotationVelocity, maxSpeed = 6f;
        private float bulletTimer, bulletInterval;
        private Vector2 distance, bulletPos;
        private MouseState mouseStateCurrent, mouseStatePrevious;
        private KeyboardState keyboardState, pastKeyboardState;
        private GamePadState gamePadStateCurrent, gamePadStatePrevious;
        private SpriteEffects entityFx;
        public PlayerIndex PlayerValue;
        public bool AsteroidMode; //Asteroid playmode yes or no?
        public int HP;

        List<Bullet> bulletList = new List<Bullet>();
        EntityManager entityMgr;

        public Player(Texture2D texture, Vector2 position, PlayerIndex playerValue, EntityManager entityMgr, Viewport viewport)
            : base(texture, position, viewport)
        {
            PlayerValue = playerValue; ////Avgör spelare. -> återfinns på loadcontent Game1
            tex = texture;
            this.entityMgr = entityMgr;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            startPosition = new Vector2(200, 200);
            IsAlive = true;
            shouldWrap = true;
            velocity = Vector2.Zero;
			Radius = tex.Height / 2;
            AsteroidMode = false;
            HP = 30;
        }

        public override void Update(GameTime gameTime)
        {
            pastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();

            gamePadStatePrevious = gamePadStateCurrent;
            gamePadStateCurrent = GamePad.GetState(PlayerValue);

            distance.X = mouseStateCurrent.X - pos.X;
            distance.Y = mouseStateCurrent.Y - pos.Y;

            bulletPos = new Vector2(pos.X, pos.Y);

            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (velocity.Length() > maxSpeed)
            {
                direction = velocity;
                direction.Normalize();
                velocity = direction * maxSpeed;
            }
            pos += velocity;

            if (HP <= 0)
            {
                HP = 0;
                IsAlive = false;
            }

            GamePadCapabilities capabilities =              
                GamePad.GetCapabilities(PlayerValue);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerValue);
                if (capabilities.HasLeftXThumbStick)
                {
                    MasterInputGamePad();          //Gamepad kontroller för Masteroid

                    //AsterInputGamePad();          //GamepadKontroller för Asteroid
                }
            }

            //AsterInput(); //Keyboard kontroller till Asteroids
            MasterInput(); //Keyboard + mus kontroller till Masteroids. Denna metod måste läggas som kommentar för att inte störa andras rotation.
            

            ScreenWrap();
        }

        private void CreateBullet() //Skapar och skjuter skott i rätt riktning
        {
            direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            bulletInterval = 0.2f;

            if (bulletTimer >= bulletInterval)
            {
				Bullet bullet = new Bullet(Art.BulletTex, pos, 10f, 10, direction, viewport, this);
				entityMgr.Add(bullet);
                bulletTimer = 0;
            }
        }

        private void MasterInput()   //Masteroids kontroller för Keyboard och Mus
        {
            
            rotation = (float)Math.Atan2(distance.Y, distance.X) + (float)Math.PI / 2;
            speed = 0.3f;

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

        private void MasterInputGamePad() //Masteroids kontroller för Gamepad
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

        private void AsterInput()   //Asteroids kontroller för keyboard
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            rotationVelocity = 3.3f;
            speed = 0.07f;

            if (keyboardState.IsKeyDown(Keys.A))            //Rotera med tangentbord Asteroids
                rotation -= MathHelper.ToRadians(rotationVelocity);
            if (keyboardState.IsKeyDown(Keys.D))
                rotation += MathHelper.ToRadians(rotationVelocity);
            
            if (keyboardState.IsKeyDown(Keys.W))            //Frammåt och Bakåt med tangentbord Asteroids
                velocity += direction * speed/2;

            if (keyboardState.IsKeyDown(Keys.Space) && 
                pastKeyboardState.IsKeyUp(Keys.Space))   //Skjuta
                CreateBullet();
        }

        private void AsterInputGamePad() //Asteroids kontroller för Gamepad
        {
            rotationVelocity = 0.08f;
            speed = 0.04f;

            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));

            if (gamePadStateCurrent.ThumbSticks.Left.X < -0.1f) //Rotera med styrspak
                rotation -= rotationVelocity;
            if (gamePadStateCurrent.ThumbSticks.Left.X > 0.1f)
                rotation += rotationVelocity;
            
            if (gamePadStateCurrent.Buttons.A == ButtonState.Pressed)  //Frammåt och Bakåt med Knappar
                velocity += direction * speed/2;
            if (gamePadStateCurrent.Buttons.B == ButtonState.Pressed)  //Frammåt och Bakåt med Knappar
                velocity -= direction * speed/3;

            //if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && 
            //    gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released)
            //    CreateBullet();

            if (gamePadStateCurrent.Buttons.X == ButtonState.Pressed &&
                gamePadStatePrevious.Buttons.X == ButtonState.Released)
                CreateBullet();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.Draw(tex, pos, sourceRectangle, Color.White, rotation, 
                    new Vector2(tex.Width / 2, tex.Height - 20), scale, entityFx, 0);
                base.Draw(spriteBatch);
            }
            else if(!IsAlive)
            {

            }
        }

        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos + wrapOffset, sourceRectangle, Color.White, 
                rotation, new Vector2(tex.Width / 2, tex.Height - 20), scale, entityFx, 0);
        }

		public override void HandleCollision(GameObject other)
		{
            if (other is Bullet)
                HP -= (other as Bullet).Damage;
            if (other is Asteroid)
                HP -= (other as Asteroid).Damage;
        }
	}
}
