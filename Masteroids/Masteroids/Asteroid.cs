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

		public Asteroid(Texture2D texture, Vector2 speed, Vector2 position, Viewport viewport)
			: base(texture, position, viewport)
        {
            tex = texture;
            pos = position;
            velocity = speed;
            shouldWrap = true;
            texOffset = new Vector2(tex.Width / 2, tex.Height / 2);
            sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			Radius = tex.Width / 2;
            damage = 1; //ANDREAS SOM FIFFLAT TILL 1 damage till ASTEROIDERNA
            HP = 1;
			size = 3;
        }

		public Asteroid(Texture2D texture, Vector2 position, Vector2 direction, float speed, int size, Viewport viewport)
			: base(texture, position, viewport)
		{
			this.direction = direction;
			this.speed = speed;
			this.size = size;
			texOffset = new Vector2(tex.Width / 2, tex.Height / 2);
			sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			Radius = tex.Width / 2;
			damage = 1;
			HP = 1;
			shouldWrap = true;
		}

        public override void Update(GameTime gameTime)
        {
            pos = pos + velocity;
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
            if(HP == 1)
            spriteBatch.Draw(tex, pos - texOffset + wrapOffset, Color.White);
        }
		
		public override void HandleCollision(GameObject other)
		{
			if (other is Bullet)
			{
				HP -= (other as Bullet).Damage;
				IsAlive = false;
				//entityMgr.Asteroids.Count -= 1;
			}
			else if (other is Player)
			{
				IsAlive = false;
			}
		}

		public void Split()
		{

		}
	}
}
