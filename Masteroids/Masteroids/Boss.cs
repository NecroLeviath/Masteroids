using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Boss
    {
        Texture2D bosstex, bulletTex;
        Vector2 velocity, pos, bulletpos;
        public Rectangle hitBox;
        int stopX = 600;
        float bulletTimer, bulletIntervall = 0.5f;
        List<Bullet> bulletList = new List<Bullet>();


        public Boss(Texture2D bosstex, Texture2D bulletTex, Vector2 velocity)
        {
            this.bosstex = bosstex;
            this.velocity = velocity;
            this.bulletTex = bulletTex;
            velocity = new Vector2(0, 0);
            pos = new Vector2(250, 50);
            Left();

        }
        public void Update(GameTime gameTime)
        {
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, bosstex.Width, bosstex.Height);

            if (pos.X < 0)
            {
                Right();
            }
            else if (pos.X > stopX)
            {
                Left();
            }
            pos += velocity;
            bulletpos = new Vector2(pos.X + 90, pos.Y + 90);

            if (bulletTimer >= bulletIntervall)
            {
                Bullet bullet = new Bullet(bulletTex, bulletpos, 10f, 10);
                bulletTimer = 0;
                bulletList.Add(bullet);
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];
                bullet.Update(gameTime);

                if (bullet.IsDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }



        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bosstex, pos, Color.White);
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }
        public bool Left()
        {
            velocity.X = -2;
            return true;
        }
        public bool Right()
        {
            velocity.X = 4;
            return true;
        }




    }
}
