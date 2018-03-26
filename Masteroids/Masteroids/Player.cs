using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroid
{
    public class Player : GameObject
    {
        Texture2D texture;
        public Vector2 origin;
        float rotation = 0.1f;
        SpriteEffects entityFx;
        public PlayerIndex playerValue;
        float scale = 0.5f;
        public Rectangle playerRec;
        public bool Dead;
        public Color[] textureData;
        //velocity += forward* acceleration_amount * delta_time;

        public float linearVelocity = 0.04f; //Frammåt
        public float rotationVelocity = 3f; //Hastighet den roterar

        public Player(Texture2D texture, Vector2 position, PlayerIndex playerValue)
        {
            this.playerValue = playerValue; ////Avgör spelare. -> återfinns på loadcontent Game1
            this.texture = texture;
            this.position = position;
            startPosition = new Vector2(200, 200);
            Dead = false;
            playerRec = new Rectangle(0, 0, texture.Width, texture.Height);
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        public override void Update(GameTime gameTime)
        {
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            position += velocity;

            GamePadCapabilities capabilities =
                GamePad.GetCapabilities(playerValue);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(playerValue);
                if (capabilities.HasLeftXThumbStick)
                {
                    //Rotera med styrspak
                    if (gamePadState.ThumbSticks.Left.X < -0.1f) //0.1f står för hur mycket spaken ska luta för att svänga.
                        rotation -= 0.07f;
                    if (gamePadState.ThumbSticks.Left.X > 0.1f)
                        rotation += 0.07f;

                    //Frammåt och Bakåt med Knappar
                    if (gamePadState.Buttons.A == ButtonState.Pressed)
                        velocity += direction * linearVelocity;
                    if (gamePadState.Buttons.B == ButtonState.Pressed) //Bakåt men är inte cannon så placerad inom //
                        velocity -= direction * linearVelocity;



                    ////Frammåt och Bakåt med Styrspak
                    //if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                    //    position += direction * linearVelocity;
                    //if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                    //    position -= direction * linearVelocity/3;
                }
            }


            //Rotera med tangentbord
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                rotation -= MathHelper.ToRadians(rotationVelocity);

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                rotation += MathHelper.ToRadians(rotationVelocity);

            //Frammåt och Bakåt med tangentbord
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                velocity += direction * linearVelocity;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                velocity -= direction * linearVelocity / 3;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {
                spriteBatch.Draw(texture, new Vector2(position.X, position.Y), playerRec, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height - 20), scale, entityFx, 0);
            }
        }
    }
}
