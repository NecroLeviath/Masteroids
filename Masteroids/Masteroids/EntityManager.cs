using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids // Simon
{
    public class EntityManager
    {
        Viewport viewport;
        bool isUpdating;
        List<GameObject> entities = new List<GameObject>();
        List<GameObject> addedEntities = new List<GameObject>();
        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();
		public List<BaseBoss> Bosses = new List<BaseBoss>();
        public List<Player> Players = new List<Player>();
        public List<Asteroid> Asteroids = new List<Asteroid>();

        public EntityManager(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            isUpdating = true;
            foreach (GameObject o in entities)
                o.Update(gameTime);
            HandleCollisions();
            isUpdating = false;

            foreach (GameObject e in addedEntities)
                AddEntity(e);
            addedEntities.Clear();

            // Removes dead entities
            entities = entities.Where(x => x.IsAlive).ToList();
            bullets = bullets.Where(x => x.IsAlive).ToList();
            enemies = enemies.Where(x => x.IsAlive).ToList();
			Bosses = Bosses.Where(x => x.IsAlive).ToList();
			Players = Players.Where(x => x.IsAlive).ToList();
			Asteroids = Asteroids.Where(x => x.IsAlive).ToList();
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject o in entities)
                o.Draw(spriteBatch);
        }

        public void Add(GameObject entity) // DEV: This will replace CreateBullet and CreateAsteroid in other classes
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private void AddEntity(GameObject entity)
        {
            entities.Add(entity);
			if (entity is Bullet)
				bullets.Add(entity as Bullet);
			else if (entity is Asteroid)
				Asteroids.Add(entity as Asteroid);
			else if (entity is BaseBoss)
				Bosses.Add(entity as BaseBoss);
			else if (entity is Enemy)
				enemies.Add(entity as Enemy);
			else if (entity is Player)
				Players.Add(entity as Player);
        }

        private bool IsColliding(GameObject entityA, GameObject entityB)
        {
            var radius = entityA.Radius + entityB.Radius;
            return entityA.IsAlive && entityB.IsAlive &&
                Vector2.DistanceSquared(entityA.Position, entityB.Position) < radius * radius;
        }

        private void HandleCollisions()
        {
			for (int i = 0; i < bullets.Count; i++)
			{
				Bullet bullet = bullets[i];
				if (bullet.IsAlive)
				{
					if (bullet.Owner is Player)
					{
						for (int j = 0; j < Asteroids.Count; j++)
						{
							Asteroid asteroid = Asteroids[j];
							if (asteroid.IsAlive && IsColliding(bullet, asteroid))
							{
								bullet.HandleCollision(asteroid);
								asteroid.HandleCollision(bullet);
							}
						}
						for (int j = 0; j < enemies.Count; j++)
						{
							Enemy enemy = enemies[j];
							if (enemy.IsAlive && IsColliding(bullet, enemy))
							{
								bullet.HandleCollision(enemy);
								enemy.HandleCollision(bullet);
							}
						}
						for (int j = 0; j < Bosses.Count; j++)
						{
							BaseBoss boss = Bosses[j];
							if (boss.IsAlive && IsColliding(bullet, boss))
							{
								bullet.HandleCollision(boss);
								boss.HandleCollision(bullet);
							}
						}
					}
					else
					{
						for (int j = 0; j < Players.Count; j++)
						{
							Player player = Players[j];
							if (player.IsAlive && IsColliding(bullet, player))
							{
								bullet.HandleCollision(player);
								player.HandleCollision(bullet);
							}
						}
					}
				}
			}
			for (int i = 0; i < Players.Count; i++)
			{
				Player player = Players[i];
				if (player.IsAlive)
				{
					for (int j = 0; j < enemies.Count; j++)
					{
						Enemy enemy = enemies[j];
						if (enemy.IsAlive && IsColliding(player, enemy))
						{
							player.HandleCollision(enemy);
							enemy.HandleCollision(player);
						}
					}
					for (int j = 0; j < Bosses.Count; j++)
					{
						BaseBoss boss = Bosses[j];
						if (boss.IsAlive && IsColliding(player, boss))
						{
							player.HandleCollision(boss);
							boss.HandleCollision(player);
						}
					}
					for (int j = 0; j < Asteroids.Count; j++)
					{
						Asteroid asteroid = Asteroids[j];
						if (asteroid.IsAlive && IsColliding(player, asteroid))
						{
							player.HandleCollision(asteroid);
							asteroid.HandleCollision(player);
						}
					}
				}
			}
        }
    }
}
