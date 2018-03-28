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
        int stopX = 1780;
        float bulletTimer, bulletIntervall = 0.5f;
        List<Bullet> bulletList = new List<Bullet>();
        public Color[] textureData;
        EntityManager entityMgr;


        public Boss(Vector2 velocity, EntityManager entityMgr)
        {
            bosstex = Art.BossTex;
            this.velocity = velocity;
            this.entityMgr = entityMgr;
            velocity = new Vector2(0, 0);
            pos = new Vector2(250, 50);
            Left();
            textureData = new Color[bosstex.Width * bosstex.Height];
            bosstex.GetData(textureData);

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
                entityMgr.CreateBullet(bulletpos, 10f, 10, new Vector2(0,1));
                bulletTimer = 0;
            }

            //for (int i = 0; i < bulletList.Count; i++)
            //{
            //    Bullet bullet = bulletList[i];
            //    bullet.Update(gameTime);

            //    if (bullet.IsDead())
            //    {
            //        bulletList.Remove(bullet);
            //        i--;
            //    }
            //}          

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bosstex, pos, Color.White);
            //foreach (Bullet b in bulletList)
            //{
            //    b.Draw(spriteBatch);
            //}
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
