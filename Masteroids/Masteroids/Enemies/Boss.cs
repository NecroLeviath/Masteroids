    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Boss : BaseBoss //public fields börjar med stor bokstav. tex TextureData.
    {
        Texture2D bosstex, bulletTex; //Hämta textures från GameObject klassn.
        Vector2 velocity, pos, bulletpos, bulletpos1, bulletpos2; //bulletPos1-2-3 istället för en bulletPos
        public Rectangle hitBox;
        int stopX = 1780;   //Lägga in åtkomst och sortera.
        float bulletTimer, bulletIntervall = 0.5f;
        List<Bullet> bulletList = new List<Bullet>();
        public Color[] textureData;
        Bullet bullet;
        EntityManager entityMgr;
        public int life;

        public Boss(Texture2D texture, Vector2 position, float speed, int hitPoints, Viewport viewport, EntityManager entityMgr)
			: base(texture, position, speed, hitPoints, viewport)
        {
            bosstex = texture;
            this.entityMgr = entityMgr;
            velocity = new Vector2(0, 0);
            pos = position;
            Left();
            textureData = new Color[bosstex.Width * bosstex.Height];
            bosstex.GetData(textureData);
            life = hitPoints; // DEV: life should be HP
			Radius = tex.Width / 2;
        }
        public override void Update(GameTime gameTime)
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
            bulletpos = new Vector2(pos.X + 80, pos.Y + 120); // + texture.Width / 2  .. 
            bulletpos1 = new Vector2(pos.X + 20, pos.Y + 65);
            bulletpos2 = new Vector2(pos.X + bosstex.Width, pos.Y + 65);


            //
            if (bulletTimer >= bulletIntervall && life > 50) //Ha en bullethastighet som är relativt till bossen.
            {
                CreateBullet(bulletpos, 10f, new Vector2(0, 1));
                bulletTimer = 0;
            }
            if (bulletTimer >= bulletIntervall && life <= 50 && life > 20)
            {
                bulletIntervall = 0.4f;
                CreateBullet(bulletpos, 10f, new Vector2(0, 1));
                CreateBullet(bulletpos1, 10f, new Vector2(1, 1));
                CreateBullet(bulletpos2, 10f, new Vector2(-1, 1));
                bulletTimer = 0;
            }
            if (bulletTimer >= bulletIntervall && life <= 20 && life > 0)
            {
                bulletIntervall = 0.1f;
                CreateBullet(bulletpos, 5f, new Vector2(0, 1));
                CreateBullet(bulletpos, 5f, new Vector2(1, 1));
                CreateBullet(bulletpos, 5f, new Vector2(-1, 1));
                CreateBullet(bulletpos, 5f, new Vector2(2, 1));
                CreateBullet(bulletpos, 5f, new Vector2(-2, 1));
                CreateBullet(bulletpos, 5f, new Vector2(3, 1));
                CreateBullet(bulletpos, 5f, new Vector2(-3, 1));
                CreateBullet(bulletpos1, 10f, new Vector2(-1, 0));
                CreateBullet(bulletpos2, 10f, new Vector2(1, 0));
                bulletTimer = 0;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(life >= 1)
            spriteBatch.Draw(bosstex, pos, Color.White);
            if (life < 1 ) //Tag Bort måsvingarna LAILA!! :)
            {
                spriteBatch.Draw(bosstex, pos, Color.Red);
            }
        }
        public bool Left()
        {
            velocity.X = -4;
            return true;
        }

        public bool Right()
        {
            velocity.X = 4;
            return true;
        }

		private void CreateBullet(Vector2 position, float speed, Vector2 direction)
		{
			Bullet bullet = new Bullet(Art.BulletTex, position, speed, 10, direction, viewport, this);
			entityMgr.Add(bullet);
		}

		public override void HandleCollision(GameObject other)
		{
			if (other is Bullet)
				HP -= (other as Bullet).Damage;
		}
	}
}
