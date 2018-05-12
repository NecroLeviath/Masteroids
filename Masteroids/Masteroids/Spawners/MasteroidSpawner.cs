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
        int nrOfEnemies;
		public int BossMaxHP { get; private set; }
        BaseBoss boss;
        float spawnTimer, spawnInterval = 1f;
        bool hasBossSpawned = false;

        public MasteroidSpawner(EntityManager entityManager, Viewport viewport, BaseBoss boss, int numberOfEnemies)
            : base(entityManager, viewport)
        {
            this.boss = boss;
            nrOfEnemies = numberOfEnemies;
        }

        public override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            spawnTimer += delta;
            if (nrOfEnemies > 0 && (entityMgr.Enemies.Count == 0 || spawnTimer >= spawnInterval))
            {
                var pos = RandomSide();
                Shooter shooter = new Shooter(Assets.EnemySheet, pos, 100, entityMgr, viewport);
                entityMgr.Add(shooter);
                nrOfEnemies--;
                spawnTimer = 0;
            }
            else if (nrOfEnemies == 0 && !hasBossSpawned && spawnTimer >= spawnInterval)
            {
                entityMgr.Add(boss);
                entityMgr.Bosses[0].Start();
				for (int i = 0; i < entityMgr.Bosses.Count; i++)
					BossMaxHP += entityMgr.Bosses[i].MaxHP;
                hasBossSpawned = true;
            }
        }
    }
}
