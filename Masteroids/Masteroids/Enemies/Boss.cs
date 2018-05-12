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
        public Vector2 BulletPos, BulletPos1, BulletPos2; 
        public Rectangle HitBox;
        public int StopX = 1780;   
        protected float BulletTimer, BulletIntervall = 0.5f;
        EntityManager EntityMgr;

        public Boss(Texture2D texture, Vector2 position, float speed, int hitPoints, Viewport viewport, EntityManager entityMgr)
			: base(texture, position, speed, hitPoints, viewport)
        {
            tex = texture;
            this.EntityMgr = entityMgr;
            velocity = new Vector2(0, 0);
            pos = position;
            Left();
			Radius = tex.Width / 2;
        }

        public override void Update(GameTime gameTime)
        {
            BulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            HitBox = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);

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
            BulletPos = new Vector2(pos.X, pos.Y + 80);
            BulletPos1 = new Vector2(pos.X - tex.Width / 2 + 20, pos.Y + 60);
            BulletPos2 = new Vector2(pos.X + tex.Width / 2 - 20, pos.Y + 60);


            if (BulletTimer >= BulletIntervall && HP > 50) //Ha en bullethastighet som är relativt till bossen.
            {
                CreateBullet(BulletPos, 10f, new Vector2(0, 1));
                BulletTimer = 0;
            }
            if (BulletTimer >= BulletIntervall && HP <= 50 && HP > 20)
            {
                BulletIntervall = 0.4f;
                CreateBullet(BulletPos, 10f, new Vector2(0, 1));
				CreateBullet(BulletPos1, 10f, new Vector2(-1, 1));
				CreateBullet(BulletPos2, 10f, new Vector2(1, 1));
                BulletTimer = 0;
            }
            if (BulletTimer >= BulletIntervall && HP <= 20 && HP > 0)
            {
                BulletIntervall = 0.1f;
                CreateBullet(BulletPos, 5f, new Vector2(0, 1));
                CreateBullet(BulletPos, 5f, new Vector2(1, 1));
                CreateBullet(BulletPos, 5f, new Vector2(-1, 1));
                CreateBullet(BulletPos, 5f, new Vector2(2, 1));
                CreateBullet(BulletPos, 5f, new Vector2(-2, 1));
                CreateBullet(BulletPos, 5f, new Vector2(3, 1));
                CreateBullet(BulletPos, 5f, new Vector2(-3, 1));
                CreateBullet(BulletPos1, 10f, new Vector2(-1, 0));
                CreateBullet(BulletPos2, 10f, new Vector2(1, 0));
                BulletTimer = 0;
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
			Bullet bullet = new Bullet(Assets.BulletTex, position, speed, 10, direction, viewport, this);
			EntityMgr.Add(bullet);
		}

		public override void HandleCollision(GameObject other)
		{
            if (other is Bullet)
                HP -= (other as Bullet).Damage;
		}
    }
}
