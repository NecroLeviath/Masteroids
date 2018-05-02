    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Boss : BaseBoss 
    {
        Vector2 bulletpos, bulletpos1, bulletpos2; 
        public Rectangle hitBox;
        public int stopX = 1780;   
        float bulletTimer, bulletIntervall = 0.5f;
        EntityManager entityMgr;

        public Boss(Texture2D texture, Vector2 position, float speed, int hitPoints, Viewport viewport, EntityManager entityMgr)
			: base(texture, position, speed, hitPoints, viewport)
        {
            tex = texture;
            this.entityMgr = entityMgr;
            velocity = new Vector2(0, 0);
            pos = position;
            Left();
            HP = hitPoints; 
			Radius = tex.Width / 2;
            HP = 100;
            if (HP < 0)
                HP = 0;
        }

        public override void Update(GameTime gameTime)
        {
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);

            if (pos.X < tex.Width / 2)
            {
                Right();
            }
            else if (pos.X > viewport.Width - (tex.Width / 2))
            {
                Left();
            }
            if(HP < 1)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }
            pos += velocity;
            bulletpos = new Vector2(pos.X, pos.Y + 80);
            bulletpos1 = new Vector2(pos.X - tex.Width / 2 + 20, pos.Y + 60);
            bulletpos2 = new Vector2(pos.X + tex.Width / 2 - 20, pos.Y + 60);


            if (bulletTimer >= bulletIntervall && HP > 50) //Ha en bullethastighet som är relativt till bossen.
            {
                CreateBullet(bulletpos, 10f, new Vector2(0, 1));
                bulletTimer = 0;
            }
            if (bulletTimer >= bulletIntervall && HP <= 50 && HP > 20)
            {
                bulletIntervall = 0.4f;
                CreateBullet(bulletpos, 10f, new Vector2(0, 1));
				CreateBullet(bulletpos1, 10f, new Vector2(-1, 1));
				CreateBullet(bulletpos2, 10f, new Vector2(1, 1));
                bulletTimer = 0;
            }
            if (bulletTimer >= bulletIntervall && HP <= 20 && HP > 0)
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
			var drawPos = pos - new Vector2(tex.Width / 2, tex.Height / 2);
            if(HP >= 30)
				spriteBatch.Draw(tex, drawPos, Color.White);
            if (HP < 30 && HP >= 1)
            {
                spriteBatch.Draw(tex, drawPos, Color.Red);
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
            if (other is Bullet && HP >= 1)
                HP -=5;
		}
    }
}
