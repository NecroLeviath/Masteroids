using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class EntityManager
    {
        Viewport viewport;
        bool isUpdating;
        List<GameObject> entities = new List<GameObject>();
        List<GameObject> addedEntities = new List<GameObject>();
        List<GameObject> bullets = new List<GameObject>();
        public List<GameObject> Asteroids = new List<GameObject>();

        public EntityManager(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            isUpdating = true;
            foreach (GameObject o in entities)
                o.Update(gameTime);
            isUpdating = false;

            foreach (GameObject e in addedEntities)
                AddEntity(e);
            addedEntities.Clear();

            // Removes dead entities
            entities = entities.Where(x => !x.IsAlive).ToList();
            bullets = bullets.Where(x => !x.IsAlive).ToList();
            Asteroids = Asteroids.Where(x => !x.IsAlive).ToList();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject o in entities)
                o.Draw(spriteBatch);
        }

        public void Add(GameObject entity) // This will replace CreateBullet and CreateAsteroid in other classes
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
        }

        public void CreateBullet(Vector2 pos, float speed, int damage, Vector2 direction)
        {
            GameObject o = new Bullet(pos, speed, damage, direction, viewport);
            entities.Add(o);
            bullets.Add(o);
        }

        public void CreateAsteroid(Vector2 pos, Vector2 speed)
        {
            GameObject o = new Asteroid(Art.AsteroidTex, speed, pos, viewport);
            entities.Add(o);
            Asteroids.Add(o);
        }

        private bool IsColliding(GameObject entityA, GameObject entityB)
        {
            var radius = entityA.Radius + entityB.Radius;
            return entityA.IsAlive && entityB.IsAlive &&
                Vector2.DistanceSquared(entityA.Position, entityB.Position) < radius * radius;
        }

        private void HandleCollisions()
        {
        }
    }
}
