using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class AsteroidSpawner : Spawner
    {
        private int positionX, positionY, move, speedX, speedY;
        float spawnTimer, spawnInterval;
        int nrOfEnemies;


        public AsteroidSpawner(Game1 game, EntityManager entityManager, List<PlayerHandler> playerHandlers, Viewport viewport, int numberOfEnemies)
            : base(game, entityManager, playerHandlers, viewport) { nrOfEnemies = numberOfEnemies; }

		public AsteroidSpawner(EntityManager entityManager, Viewport viewport)
			: base(entityManager, viewport) { }

        public override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnInterval = 10;
            spawnTimer += delta;

            if (playerHandlers != null && playerHandlers.All(x => x.Lives < 0))
				game.ChangeState(new EnterHighscoreState(game, playerHandlers, this, game.GraphicsDevice, game.Content, entityMgr));
			
			//how many asteroids that spawns and how their movement is.
			if (entityMgr.Asteroids.Count < 15)
            {
				Vector2 pos = RandomSide();
				Vector2 dir = RandomDirection();
				float speed = RandomSpeed();
				Asteroid asteroid = new Asteroid(Assets.AsteroidTexs[2], pos, dir, speed, 3, entityMgr, viewport);

				entityMgr.Add(asteroid);
            }
            if (entityMgr.Enemies.Count < 0 || spawnTimer >= spawnInterval)
            {
                var pos = RandomSide();
                Shooter shooter = new Shooter(Assets.EnemySheet, pos, 100, entityMgr, viewport);
                entityMgr.Add(shooter);
                //nrOfEnemies--;
                spawnTimer -= 10;
            }
        }

        public void Location()
        {
            move = rnd.Next(1, 4);
            if(move == 1)
            {
                positionX = rnd.Next(0, 1920);
                positionY = 0;
                speedX = rnd.Next(-2, 2);
                speedY = rnd.Next(1, 2);
            }
            if (move == 2)
            {
                positionX = 0;
                positionY = rnd.Next(0, 1080);
                speedX = rnd.Next(1, 2);
                speedY = rnd.Next(-2, 3);
            }
            if (move == 3)
            {
                positionX = rnd.Next(0, 1920);
                positionY = 1080;
                speedX = rnd.Next(-2, 2);
                speedY = rnd.Next(-2, -1);
            }
            if(positionX == 0 && positionY == 0)
            {
                speedX = rnd.Next(-2, 2); 
                speedY = rnd.Next(-2, 2);
            }
        }

        private Vector2 RandomDirection()
        {
            var rotation = (float)rnd.NextDouble() * MathHelper.TwoPi;
            var x = (float)Math.Cos(rotation);
            var y = (float)Math.Sin(rotation);
            var direction = new Vector2(x, y);
            return direction;
        }

        private float RandomSpeed()
        {
            var speed = (float)rnd.NextDouble() + 1;
            return speed;
        }
    }
}
