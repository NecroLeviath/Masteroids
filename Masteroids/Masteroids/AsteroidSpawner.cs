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
        private Viewport viewport;
        EntityManager entityMgr;
        private Random rnd;
        private int positionX, positionY, position, move, speedX, speedY;
        public AsteroidSpawner(EntityManager entityManager, Viewport viewport)
            : base(entityManager, viewport) { }

        public override void Update(GameTime gameTime)
        {
            //how many asteroids that spawns and how their movement is.
            if (entityMgr.Asteroids.Count < 15)
            {
                Location();
                //positionX = rnd.Next(0, viewport.Width + Art.AsteroidTex.Width);
                //positionY = rnd.Next(0, viewport.Height + Art.AsteroidTex.Height);
                //speedX = rnd.Next(-2, 2);   //Asteroider kan spawna utan att kunna röra sig.
                //speedY = rnd.Next(-2, 2);
                Vector2 pos = new Vector2(positionX, positionY);
                Vector2 speed = new Vector2(speedX, speedY);
				Asteroid asteroid = new Asteroid(Art.AsteroidTexs[2], speed, pos, entityMgr, viewport);
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
