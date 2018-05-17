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
        public List<Enemy> Enemies = new List<Enemy>();
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
            Enemies = Enemies.Where(x => x.IsAlive).ToList();
			Bosses = Bosses.Where(x => x.IsAlive).ToList();
			Players = Players.Where(x => x.IsAlive).ToList();
			Asteroids = Asteroids.Where(x => x.IsAlive).ToList();
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject o in entities)
                o.Draw(spriteBatch);
        }

        public void Add(GameObject entity)
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
				Enemies.Add(entity as Enemy);
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
							CheckAndHandleCollision(bullet, Asteroids[j]);
						for (int j = 0; j < Enemies.Count; j++)
							CheckAndHandleCollision(bullet, Enemies[j]);
						for (int j = 0; j < Bosses.Count; j++)
							CheckAndHandleCollision(bullet, Bosses[j]);
					}
					else
						for (int j = 0; j < Players.Count; j++)
							CheckAndHandleCollision(bullet, Players[j]);
				}
			}
			for (int i = 0; i < Players.Count; i++)
			{
				Player player = Players[i];
				if (player.IsAlive)
				{
					for (int j = 0; j < Enemies.Count; j++)
						CheckAndHandleCollision(player, Enemies[j]);
					for (int j = 0; j < Bosses.Count; j++)
						CheckAndHandleCollision(player, Bosses[j]);
					for (int j = 0; j < Asteroids.Count; j++)
						CheckAndHandleCollision(player, Asteroids[j]);
				}
			}
        }

		private void CheckAndHandleCollision(GameObject a, GameObject b)
		{
			if (b.IsAlive && IsColliding(a, b))
			{
				a.HandleCollision(b);
				b.HandleCollision(a);
			}
		}
    }
}
