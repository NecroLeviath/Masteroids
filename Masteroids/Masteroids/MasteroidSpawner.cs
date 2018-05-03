using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Masteroids
{
    class MasteroidSpawner : Spawner
    {
        private BaseBoss boss;

        public MasteroidSpawner(EntityManager entityManager, Viewport viewport, BaseBoss boss)
            : base(entityManager, viewport)
        {
            this.boss = boss;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
