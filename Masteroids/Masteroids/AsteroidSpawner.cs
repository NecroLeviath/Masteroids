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
        private Viewport viewport;
        EntityManager entityMgr;
        private Random rnd;
        private int positionX, positionY, position, move, speedX, speedY;

        public AsteroidSpawner(EntityManager entityMgr, Viewport viewport)
        {
            this.entityMgr = entityMgr;
            this.viewport = viewport;
            rnd = new Random();
        }
        public void Update(GameTime gameTime)
        {
            if (entityMgr.Asteroids.Count < 15)
            {
                Location();
                Vector2 pos = new Vector2(positionX, positionY);
                Vector2 speed = new Vector2(speedX, speedY);
				Asteroid asteroid = new Asteroid(Art.AsteroidTex, speed, pos, viewport);
				entityMgr.Add(asteroid);
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
    }
}
