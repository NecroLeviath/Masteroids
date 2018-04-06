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

        public Shooter(Vector2 position, EntityManager entityManager, Viewport viewport)
            : base(position, viewport)
        {
            entityMgr = entityManager;
        }

        public override void Update(GameTime gameTime)
        {
            ChooseTarget();
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
    }
}
