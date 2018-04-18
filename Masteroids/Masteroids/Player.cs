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
        private float rotation = 0.1f, speed = 0.3f, scale = 0.5f, linearVelocity = 0.02f, rotationVelocity = 3f;   // CR: Rotation och speed finns redan i GameObject. Värdena på dessa kanske ska sättas i konstuktorn. Vad är linearVelocity?
        private Vector2 distance, bulletPos;    // CR: distance är ett lite otydligt namn. bulletPos är samma sak som position i koden, men den används inte, så istället hade endasts position kunnat användas
        private MouseState mouseStateCurrent, mouseStatePrevious;       //
        private KeyboardState keyboardState, pastKeyboardState;         // CR: Namnen på dessa kanske bör vara lika varandra
        private GamePadState gamePadStateCurrent, gamePadStatePrevious; //
        private float bulletTimer, bulletInterval;
        private SpriteEffects entityFx; // CR: SpriteFx kanske är ett mer passande namn
        public PlayerIndex playerValue;                         // CR: Stor första bokstav för publika variabler och denna kanske inte bör vara publik
        public Rectangle playerRec;                             // CR: Det finns en sourceRectangle i GameObject som har samma funktion som denna, så denna kan tas bort
        public Color[] textureData;                             // CR: Denna är nog ej längre relevant, då vi inte använder pixel perfekt kollision
        public bool Dead, AMode; //Asteroid playmode yes or no? // CR: IsAlive variablel finns redan i GameObject, så Dead är onödig. AMode är ett otydligt namn och den behöver nog ej vara publik

        List<Bullet> bulletList = new List<Bullet>();   // CR: Denna behövs inte då bullets lagras i EntityManager
        EntityManager entityMgr;

        public Player(Texture2D texture, Vector2 position, PlayerIndex playerValue, EntityManager entityMgr, Viewport viewport)
            : base(position, viewport)
        {
            this.playerValue = playerValue; ////Avgör spelare. -> återfinns på loadcontent Game1
            this.texture = texture; // CR: Detta kommer nog att sättas i GameObject senare
            this.entityMgr = entityMgr;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            playerRec = new Rectangle(0, 0, texture.Width, texture.Height);
            startPosition = new Vector2(200, 200);  // CR: Vet inte riktigt vad denna är till för, då man bara hade kunnat sätta position istället
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            Dead = false;
            shouldWrap = true;
        }

        public override void Update(GameTime gameTime)
        {
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;    // CR: Hade kunnat finnas ett mellanrum under denna raden för att separera alla states från allt annat
            pastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();
            gamePadStatePrevious = gamePadStateCurrent;
            gamePadStateCurrent = GamePad.GetState(playerValue);    // CR: Samma som ovan
            position += velocity;
            bulletPos = new Vector2(position.X, position.Y);
            AMode = false;

            GamePadCapabilities capabilities =              
                GamePad.GetCapabilities(playerValue);   // CR: Vet inte något om GamePad, men behöver denna kallas på varje Update?
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(playerValue);
                if (capabilities.HasLeftXThumbStick)
                {
                    //if(AMode == true)
                    //{
                    //AInputGamePad();          //GamepadKontroller för Asteroid
                    //}

                    //if (AMode == false)
                    {
                        MInputGamePad();          //Gamepad kontroller för Masteroid
                    }
                }
            }

            //if (AMode == false)
            //{
            /* AInput();*/ //Keyboard kontroller till Asteroids
                           //}

            //if (AMode == false)
            //{
            MInput(); //Keyboard + mus kontroller till Masteroids. Denna metod måste läggas som kommentar för att inte störa andras rotation.
            //}

            ScreenWrap();
        }

        private void CreateBullet() //Skapar och skjuter skott i rätt riktning
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));   // CR: direction är en variabel i GameObject
            bulletInterval = 0.2f;  // CR: bulletInterval hade kunnat sättas en gång i konstruktorn, istället för varje gång du skjuter

            if (bulletTimer >= bulletInterval)
            {
                entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);    // CR: Denna metod bör ersättas med EntityManager.AddEntity(). Du behöver heller inte göra en ny vektor, utan du kan bara skriva in position
                bulletTimer = 0;
            }
        }

        private void MInput()   //Masteroids kontroller för Keyboard och Mus // CR: Metodnamnet kanske bör vara tydligare
        {
            distance.X = mouseStateCurrent.X - position.X;  // CR: Om denna metod är det ända stället distance används på, så hade det kunnat vara en lokal variabel istället
            distance.Y = mouseStateCurrent.Y - position.Y;
            rotation = (float)Math.Atan2(distance.Y, distance.X) + (float)Math.PI / 2;
            position += velocity;   // CR: Detta händer redan i Update

            if (keyboardState.IsKeyDown(Keys.A))
                velocity.X -= 0.1f; // CR: Dessa bör nog vara speed istället för ett hårdkodat värde
            if (keyboardState.IsKeyDown(Keys.D))
                velocity.X += 0.1f;
            if (keyboardState.IsKeyDown(Keys.W))
                velocity.Y -= 0.1f;
            if (keyboardState.IsKeyDown(Keys.S))
                velocity.Y += 0.1f;

            if (mouseStatePrevious.LeftButton == ButtonState.Released
                && mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                CreateBullet(); // CR: Det är endast en rad i denna if-sats, så måsvingarna är onödiga
            }
        }

        private void MInputGamePad() //Masteroids kontroller för Gamepad // CR: Metodnamnet kanske bör vara tydligare
        {
            velocity.X += gamePadStateCurrent.ThumbSticks.Left.X * speed;
            velocity.Y -= gamePadStateCurrent.ThumbSticks.Left.Y * speed;

            //rotation = gamePadState.ThumbSticks.Right.Y;

            if (gamePadStateCurrent.ThumbSticks.Right.X != 0)
                rotation = gamePadStateCurrent.ThumbSticks.Right.X; // CR: Borde inte rotationVelocity vara involverat här?

            if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released) // CR: Den förra långa if-statsen delades upp i två rader, så denna kanske också bör göra det
            {
                CreateBullet(); // CR: Det är endast en rad i denna if-sats, så måsvingarna är onödiga
            }
        }

        private void AInput()   //Asteroids kontroller för keyboard // CR: Metodnamnet kanske bör vara tydligare
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) -  // CR: direction är en variabel i GameObject
                rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            linearVelocity = 0.04f; // CR: Om man bytar kontoll-läge under spelets gång så kommer det inte att reset:as när du bytar ifrån detta, så de andra spellägena kanske också ska sätta linearVelocity.

            if (keyboardState.IsKeyDown(Keys.A))            //Rotera med tangentbord Asteroids
                rotation -= MathHelper.ToRadians(rotationVelocity);
            if (keyboardState.IsKeyDown(Keys.D))
                rotation += MathHelper.ToRadians(rotationVelocity);
            
            if (keyboardState.IsKeyDown(Keys.W))            //Frammåt och Bakåt med tangentbord Asteroids
                velocity += direction * linearVelocity;

            if (keyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))   //Skjuta
                CreateBullet();
        }

        private void AInputGamePad() //Asteroids kontroller för Gamepad // CR: Metodnamnet kanske bör vara tydligare
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));   // CR: direction är en variabel i GameObject. Förra gången så delades denna rad up i två rader, denna kanske också bör det
            position += velocity;   // CR: Detta händer redan i Update

            if (gamePadStateCurrent.ThumbSticks.Left.X < -0.1f) //Rotera med styrspak
                rotation -= 0.07f;  // CR: Borde dessa inte vara rotationVelocity?
            if (gamePadStateCurrent.ThumbSticks.Left.X > 0.1f)
                rotation += 0.07f;
            
            if (gamePadStateCurrent.Buttons.A == ButtonState.Pressed)  //Frammåt och Bakåt med Knappar
                velocity += direction * linearVelocity;

            if (gamePadStateCurrent.Buttons.RightShoulder == ButtonState.Pressed && gamePadStatePrevious.Buttons.RightShoulder == ButtonState.Released) // CR: Denna bör kanske delas upp i två rader
            {
                entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);    // CR: Denna if-sats är bara en rad och bör inte ha måsvingar. entityMgr.CreateBullet() bör ersättas med CreateBullet()
            }
        }

        public override void Draw(SpriteBatch spriteBatch)  // CR: Kan tycka att denna bör vara direkt under Update, men det kan diskuteras
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
    }
}
