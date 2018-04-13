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
        Vector2 velocity, pos, bulletpos, bulletpos1, bulletpos2;
        public Rectangle hitBox;
        int stopX = 1780;
        float bulletTimer, bulletIntervall;
        List<Bullet> bulletList = new List<Bullet>();
        public Color[] textureData;
        Bullet bullet;
        EntityManager entityMgr;
        public int life;

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
            life = 100;
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
            if(life < 1)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }
            pos += velocity;
            bulletpos = new Vector2(pos.X + 90, pos.Y + 90);
            bulletpos1 = new Vector2(pos.X + 50, pos.Y + 50);
            bulletpos2 = new Vector2(pos.X + 140, pos.Y + 50);

            //
            if (bulletTimer >= bulletIntervall && life > 50)
            {
                bulletIntervall = 0.5f;
                entityMgr.CreateBullet(bulletpos, 10f, 10, new Vector2(0, 1));
            }
            if (bulletTimer >= bulletIntervall && life <= 50 && life > 0)
            {
                bulletIntervall = 0.1f;
                entityMgr.CreateBullet(bulletpos, 10f, 10, new Vector2(0, 1));
                entityMgr.CreateBullet(bulletpos1, 10f, 10, new Vector2(1, 1));
                entityMgr.CreateBullet(bulletpos2, 10f, 10, new Vector2(-1, 1));
                bulletTimer = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(life >= 1)
            spriteBatch.Draw(bosstex, pos, Color.White);
            if (life < 1 )
            {
                spriteBatch.Draw(bosstex, pos, Color.Red);
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
