using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids //Laila
{
	public class Asteroid : GameObject
	{
		Vector2 texOffset;
		EntityManager entityMgr;
		float HP;
		int size;
		private int damage;
		public int Damage //ANDREAS SOM FIFFLAT TILL 1 damage till ASTEROIDERNA
		{
			get { return damage; }
		}

		public Asteroid(Texture2D texture, Vector2 speed, Vector2 position, EntityManager entityManager, Viewport viewport)
			: base(texture, position, viewport)
		{
			velocity = speed;
			this.entityMgr = entityManager;
			shouldWrap = true;
			texOffset = new Vector2(tex.Width / 2, tex.Height / 2);
			sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			Radius = tex.Width / 2;
			damage = 1; //ANDREAS SOM FIFFLAT TILL 1 damage till ASTEROIDERNA
			HP = 1;
			size = 3;
		}

		public Asteroid(Texture2D texture, Vector2 position, Vector2 direction, float speed, int size, EntityManager entityManager, Viewport viewport)
			: base(texture, position, viewport)
		{
			this.direction = direction;
			this.speed = speed;
			this.size = size;
			entityMgr = entityManager;
			texOffset = new Vector2(tex.Width / 2, tex.Height / 2);
			sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			Radius = tex.Width / 2;
			damage = 1;
			HP = 1;
			shouldWrap = true;
			velocity = direction * speed;
		}

		public override void Update(GameTime gameTime)
		{
			pos = pos + velocity;

			if (HP <= 0)
			{
				if (size > 1)
				{
					 Split();
				}
				IsAlive = false;
			}

			ScreenWrap();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (HP == 1)
			{
				spriteBatch.Draw(tex, pos - texOffset, Color.White);
				base.Draw(spriteBatch);
			}
		}

		protected override void WrapDraw(SpriteBatch spriteBatch)
		{
			if (HP == 1)
				spriteBatch.Draw(tex, pos - texOffset + wrapOffset, Color.White);
		}

		public override void HandleCollision(GameObject other)
		{
			if (other is Bullet)
			{
				HP -= (other as Bullet).Damage;
			}
			else if (other is Player)
			{
				HP = 0;
			}
		}

		public void Split()
		{
			var newTex = tex;
			if (size == 3)
				newTex = Art.AsteroidTexs[1];
			else if (size == 2)
				newTex = Art.AsteroidTexs[0];

			direction = Vector2.Normalize(velocity);
			rotation = (float)Math.Atan2(direction.Y, direction.X);

			var newRotation = MathHelper.WrapAngle(rotation + 0.4f);
			var newDirection = new Vector2((float)Math.Cos(newRotation), (float)Math.Sin(newRotation));
			var newSpeed = velocity.Length();
			var newSize = size - 1;

			Asteroid asteroid = new Asteroid(newTex, pos, direction, newSpeed, newSize, entityMgr, viewport);
			entityMgr.Add(asteroid);

			newRotation = MathHelper.WrapAngle(rotation - 0.4f);
			newDirection = new Vector2((float)Math.Cos(newRotation), (float)Math.Sin(newRotation));

			asteroid = new Asteroid(newTex, pos, newDirection, newSpeed, newSize, entityMgr, viewport);
			entityMgr.Add(asteroid);
		}
	}
}
