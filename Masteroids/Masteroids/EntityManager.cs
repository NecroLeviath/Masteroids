﻿using Microsoft.Xna.Framework;
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
        List<GameObject> entities = new List<GameObject>();
        List<GameObject> bullets = new List<GameObject>();
        public List<GameObject> Asteroids = new List<GameObject>();

        public EntityManager(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject o in entities)
            {
                o.Update(gameTime);
            }

            entities = entities.Where(x => !x.IsAlive).ToList();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject o in entities)
            {
                o.Draw(spriteBatch);
            }
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
    }
}
