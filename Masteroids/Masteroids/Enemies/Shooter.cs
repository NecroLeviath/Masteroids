using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids // Simon
{
    class Shooter : Enemy
    {
        EntityManager entityMgr;
        Player target;
        float movementTimer, pauseTimer, bulletTimer;
        float movementInterval = 1, pauseInterval = 2, bulletInterval = 1;

        public Shooter(Texture2D texture, Vector2 position, float speed, EntityManager entityManager, Viewport viewport)
            : base(texture, position, speed, viewport)
        {
            acceleration = 10f;
            deacceleration = 0.9f;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            rotationCenter = new Vector2(texture.Width / 8, texture.Height / 4);
            shouldWrap = true;
            entityMgr = entityManager;
            var rnd = new Random();
            sourceRectangle = new Rectangle(rnd.Next(4) * texture.Width / 4, rnd.Next(2) * texture.Height / 2, texture.Width / 4, texture.Height / 2);
            Radius = texture.Width / 2;
        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ChooseTarget();
            if (target != null)
            {
                Vector2 playerDir = GetPlayerDirection();
                if (velocity != Vector2.Zero)
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X) + (float)Math.PI / 2;

                pauseTimer += delta;
                if (pauseTimer >= pauseInterval)
                {
                    velocity += playerDir * acceleration;

                    movementTimer += delta;
                    if (movementTimer >= movementInterval)
                    {
                        pauseTimer = 0;
                        movementTimer = 0;
                    }
                }
                else
                {
                    if (velocity.LengthSquared() >= speed)
                        velocity *= deacceleration;
                }

                bulletTimer += delta;
                if (bulletTimer >= bulletInterval)
                {
                    Bullet bullet = new Bullet(Art.BulletTex, pos, 5, 1, playerDir, viewport, this);
                    entityMgr.Add(bullet);
                    bulletTimer = 0;
                }
            }
            pos += velocity * delta;

            ScreenWrap();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, sourceRectangle, Color.White, rotation, rotationCenter, 1, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }

        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos + wrapOffset, sourceRectangle, Color.White, rotation, rotationCenter, 1, SpriteEffects.None, 0);
        }

        private void ChooseTarget()
        {
            if (entityMgr.Players.Count > 0 && (target == null || !target.IsAlive))
            {
                var rand = new Random();
                var i = rand.Next(entityMgr.Players.Count);
                target = entityMgr.Players[i];
            }
        }

        private Vector2 GetPlayerDirection()
        {
            return Vector2.Normalize(target.Position - pos);
        }

        public override void HandleCollision(GameObject other)
        {
            IsAlive = false;
        }
    }
}
