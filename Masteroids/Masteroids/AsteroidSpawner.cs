using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class AsteroidSpawner
    {
        Viewport viewport;
        EntityManager entityMgr;
        private Random rnd;

        public AsteroidSpawner(EntityManager entityMgr, Viewport viewport)
        {
            this.entityMgr = entityMgr;
            this.viewport = viewport;
            rnd = new Random();
        }
        public void Update(GameTime gameTime)
        {
            //how many asteroids that spawns and how their movement is.
            if (entityMgr.Asteroids.Count < 10)
            {
                int positionX = rnd.Next(0, viewport.Width - Art.AsteroidTex.Width);
                int positionY = rnd.Next(0, viewport.Height + Art.AsteroidTex.Height);
                int speedX = 0;
                int speedY = 0;
                while (speedX == 0)
                    speedX = rnd.Next(-2, 2);
                while (speedY == 0)
                    speedY = rnd.Next(-2, 2);
                Vector2 position = new Vector2(positionX, positionY);
                Vector2 speed = new Vector2(speedX, speedY);
                entityMgr.CreateAsteroid(position, speed);
            }
        }
    }
}
