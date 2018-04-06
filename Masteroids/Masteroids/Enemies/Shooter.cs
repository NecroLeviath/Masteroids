using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Shooter : Enemy
    {
        EntityManager entityMgr;
        Player target;
        float movementTimer, pauseTimer, bulletTimer;
        float movementInterval = 1, pauseInterval = 2, bulletInterval = 1;

        public Shooter(Texture2D texture, Vector2 position, EntityManager entityManager, Viewport viewport)
            : base(texture, position, viewport)
        {
            this.texture = texture; // DEV: This should be moved to GameObject
            entityMgr = entityManager;
        }

        public override void Update(GameTime gameTime)
        {
            float delta = gameTime.ElapsedGameTime.Seconds;
            movementTimer += delta;
            pauseTimer += delta;
            bulletTimer += delta;

            ChooseTarget();
            if (target != null)
            {
                if (bulletTimer >= bulletInterval)
                {
                    Bullet bullet = new Bullet(position, 1, 1, GetPlayerDirection(), viewport);
                    entityMgr.Add(bullet);
                    bulletTimer = 0;
                }
            }
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
            return Vector2.Normalize(target.Position - position);
        }
    }
}
