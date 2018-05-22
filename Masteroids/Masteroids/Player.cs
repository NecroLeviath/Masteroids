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
		private float invulnerabilityTimer, blinkTimer;
		private Vector2 distance, bulletPos;
        private MouseState mouseStateCurrent, mouseStatePrevious;
        private KeyboardState keyboardState, pastKeyboardState;
        private GamePadState gamePadStateCurrent, gamePadStatePrevious;
        private SpriteEffects entityFx;
        public PlayerIndex PlayerValue;
        public bool ClassicMode; //Asteroid playmode yes or no?
        public int HP;
        public bool GamePadMode;

        List<Bullet> bulletList = new List<Bullet>();
        EntityManager entityMgr;
		PlayerHandler playerHandler;


		public Player(Texture2D texture, Vector2 position, PlayerIndex playerValue, EntityManager entityManager, PlayerHandler playerHandler, Viewport viewport)
            : base(texture, position, viewport)
        {
            PlayerValue = playerValue; ////Avgör spelare. -> återfinns på loadcontent Game1
            tex = texture;
            entityMgr = entityManager;
			this.playerHandler = playerHandler;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            startPosition = new Vector2(200, 200);
            IsAlive = true;
            shouldWrap = true;
            velocity = Vector2.Zero;
			Radius = tex.Height / 2;
            ClassicMode = true;
			HP = 1;
			invulnerabilityTimer = 2;
            GamePadMode = false;
		}

        public override void Update(GameTime gameTime)
        {
            pastKeyboardState = keyboardState;  //Dev: Not being used
            keyboardState = Keyboard.GetState();

            mouseStatePrevious = mouseStateCurrent;  //Dev: Not being used
            mouseStateCurrent = Mouse.GetState();

            gamePadStatePrevious = gamePadStateCurrent;  //Dev: Not being used
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
				IsAlive = false;

			if (invulnerabilityTimer > 0)
			{
				invulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (blinkTimer <= 0)
					blinkTimer = invulnerabilityTimer / 10;
			}
			if (blinkTimer > 0)
				blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

			GamePadCapabilities capabilities =              
                GamePad.GetCapabilities(PlayerValue);
            if (capabilities.IsConnected)
            {
                GamePadMode = true;

                GamePadState gamePadState = GamePad.GetState(PlayerValue);
                if (capabilities.HasLeftXThumbStick)
                {
                    if(!ClassicMode)
                        AsterInputGamePad();          //DEV: !ClassicMode för MasterInputGamePad
                    else
                        MasterInputGamePad();
                }
            }
            else if(PlayerValue == PlayerIndex.One)
            {
                if (!ClassicMode)                       //DEV: !ClassicMode för MasterInput
                    AsterInput();
                else
                    MasterInput();
            }
            //Keyboard kontroller till Asteroids
            //if(!ClassicMode)
            //MasterInput(); //Keyboard + mus kontroller till Masteroids. Denna metod måste läggas som kommentar för att inte störa andras rotation.


            ScreenWrap();
        }

        private void CreateBullet() //Skapar och skjuter skott i rätt riktning
        {
            direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            bulletInterval = 0.2f;

            if (bulletTimer >= bulletInterval)
            {
				Bullet bullet = new Bullet(Assets.BulletTex, pos, 10f, 10, direction, viewport, this);
				entityMgr.Add(bullet);
                bulletTimer = 0;
            }
        }

        private void MasterInput()   //Masteroids kontroller för Keyboard och Mus
        {
            rotation = (float)Math.Atan2(distance.Y, distance.X) + (float)Math.PI / 2;
            speed = 0.2f;

            if (keyboardState.IsKeyDown(Keys.A))
                velocity.X -= speed;
            if (keyboardState.IsKeyDown(Keys.D))
                velocity.X += speed;
            if (keyboardState.IsKeyDown(Keys.W))
                velocity.Y -= speed;
            if (keyboardState.IsKeyDown(Keys.S))
                velocity.Y += speed;

            if (/*mouseStatePrevious.LeftButton == ButtonState.Released &&*/ 
                mouseStateCurrent.LeftButton == ButtonState.Pressed)
                CreateBullet();
        }

        private void MasterInputGamePad() //Masteroids kontroller för Gamepad
        {
            speed = 0.07f;

            velocity.X += gamePadStateCurrent.ThumbSticks.Left.X * speed*3;
            velocity.Y -= gamePadStateCurrent.ThumbSticks.Left.Y * speed*3;

            if (gamePadStateCurrent.ThumbSticks.Right != Vector2.Zero)
                rotation = (float)Math.Atan2(-gamePadStateCurrent.ThumbSticks.Right.Y, gamePadStateCurrent.ThumbSticks.Right.X) + MathHelper.PiOver2;

            if (gamePadStateCurrent.Triggers.Right > 0/* && gamePadStatePrevious.Triggers.Right == 0*/)
                CreateBullet();
        }

        private void AsterInput()   //Asteroids kontroller för keyboard
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            rotationVelocity = 4f;
            speed = 0.08f;

            if (keyboardState.IsKeyDown(Keys.A))            //Rotera med tangentbord Asteroids
                rotation -= MathHelper.ToRadians(rotationVelocity);
            if (keyboardState.IsKeyDown(Keys.D))
                rotation += MathHelper.ToRadians(rotationVelocity);
            
            if (keyboardState.IsKeyDown(Keys.W))            //Frammåt och Bakåt med tangentbord Asteroids
                velocity += direction * speed;

            if (keyboardState.IsKeyDown(Keys.Space)
                /*&& pastKeyboardState.IsKeyUp(Keys.Space)*/)   //Skjuta
                CreateBullet();
        }

        private void AsterInputGamePad() //Asteroids kontroller för Gamepad
        {
            rotationVelocity = 0.08f;
            speed = 0.07f;

            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - 
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));

            if (gamePadStateCurrent.ThumbSticks.Left.X < -0.1f) //Rotera med styrspak
                rotation -= rotationVelocity;
            if (gamePadStateCurrent.ThumbSticks.Left.X > 0.1f)
                rotation += rotationVelocity;
            
            if (gamePadStateCurrent.Buttons.A == ButtonState.Pressed)
                velocity += direction * speed;
            //if (gamePadStateCurrent.Buttons.B == ButtonState.Pressed)
            //    velocity -= direction * speed;

            if (gamePadStateCurrent.DPad.Up == ButtonState.Pressed)
                velocity += direction * speed;
            if (gamePadStateCurrent.DPad.Left == ButtonState.Pressed)
                rotation -= rotationVelocity;
            if (gamePadStateCurrent.DPad.Right == ButtonState.Pressed)
                rotation += rotationVelocity;

            if (gamePadStateCurrent.Triggers.Left > 0)
                velocity += direction * speed;

            if (gamePadStateCurrent.Triggers.Right > 0)
                CreateBullet();

            /*&& gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released*/
            //    CreateBullet();
            //if (gamePadStateCurrent.Buttons.X == ButtonState.Pressed
            //&& gamePadStatePrevious.Buttons.X == ButtonState.Released)
            //if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed)
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			if (blinkTimer <= 0)
			{
				spriteBatch.Draw(tex, pos, sourceRectangle, Color.White, rotation,
					new Vector2(tex.Width / 2, tex.Height - 20), scale, entityFx, 0);
				base.Draw(spriteBatch);
			}
        }

        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
				spriteBatch.Draw(tex, pos + wrapOffset, sourceRectangle, Color.White,
					rotation, new Vector2(tex.Width / 2, tex.Height - 20), scale, entityFx, 0);
        }

		public override void HandleCollision(GameObject other)
		{
			if (invulnerabilityTimer <= 0)
			{
				if (other is Bullet)
					HP -= (other as Bullet).Damage;
				if (other is Asteroid)
					HP -= (other as Asteroid).Damage;
				if (other is Enemy)
					HP = 0;
			}
        }

		public void IncrementScore(int value)
		{
			playerHandler.Score += value;
		}
	}
}
