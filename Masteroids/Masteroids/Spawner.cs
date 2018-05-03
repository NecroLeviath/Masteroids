using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    abstract class Spawner
    {
        protected EntityManager entityMgr;
        protected Viewport viewport;

        public Spawner(EntityManager entityManager, Viewport viewport)
        {
            entityMgr = entityManager;
            this.viewport = viewport;
        }

        public abstract void Update(GameTime gameTime);
    }
}
