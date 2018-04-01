﻿using Microsoft.Xna.Framework;
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
            if(entityMgr.Asteroids.Count < 5)
            {
                int positionX = rnd.Next(0, viewport.Width - Art.AsteroidTex.Width);
                int positionY = viewport.Height + Art.AsteroidTex.Height;
                int speedX = rnd.Next(-2, 2);
                int speedY = rnd.Next(-3, -1);
                Vector2 position = new Vector2(positionX, positionY);
                Vector2 speed = new Vector2(speedX, speedY);
                entityMgr.CreateAsteroid(position, speed);
            }
        }
    }
}