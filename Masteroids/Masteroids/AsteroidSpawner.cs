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
                int speedX = rnd.Next(-2, 2);   //Asteroider kan spawna utan att kunna röra sig.
                int speedY = rnd.Next(-2, 2);
                Vector2 pos = new Vector2(positionX, positionY);
                Vector2 speed = new Vector2(speedX, speedY);
				Asteroid asteroid = new Asteroid(Art.AsteroidTex, speed, pos, viewport);
				entityMgr.Add(asteroid);
            }
            
        }
    }
}
