using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids // Simon
{
    public abstract class Enemy : GameObject
    {
		public int HP { get; protected set; }
		protected int scoreValue;

        public Enemy(Texture2D texture, Vector2 position, float speed, Viewport viewport)
            : base(texture, position, viewport)
		{
			this.speed = speed;
		}

        public override void Update(GameTime gameTime) { }

		protected void BulletCollision(Bullet bullet)
		{
			HP -= bullet.Damage;
			if (HP <= 0)
				(bullet.Owner as Player).IncrementScore(scoreValue);
		}
    }
}
