using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    public class Input
    {
        PlayerIndex playerValue;
        Vector2 position, velocity;
        float rotation, speed;
        float linearVelocity;
        Player player;

        public void Update(GameTime gameTime)
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

                    velocity.X += gamePadState.ThumbSticks.Left.X * speed;
                    velocity.Y -= gamePadState.ThumbSticks.Left.Y * speed;
                    rotation = gamePadState.ThumbSticks.Right.X + gamePadState.ThumbSticks.Right.Y;
                    //rotation = gamePadState.ThumbSticks.Right.Y;


                    ////Frammåt och Bakåt med Styrspak
                    //if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                    //    position += direction * linearVelocity;
                    //if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                    //    position -= direction * linearVelocity/3;

                    //if (mouseStatePrevious.LeftButton == ButtonState.Released
                    //        && mouseStateCurrent.LeftButton == ButtonState.Pressed)
                    //{
                    //    entityMgr.CreateBullet(new Vector2(position.X, position.Y), 10f, 10, direction);
                    //}
                }
            }
        }
    }
}
