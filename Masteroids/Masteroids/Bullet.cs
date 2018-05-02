using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    public class Bullet : GameObject
    {
        private int damage, age;
        float bulletTimer;
        protected Vector2 center;
        protected Vector2 origin;
        List<Bullet> bulletList = new List<Bullet>();
        public Rectangle bulletRec;
        public Color[] textureData;
        public Vector2 Direction;
        public GameObject Owner { get; private set; }

        public int Damage
        {
            get { return damage; }
        }

        public bool IsDead()
        {
            return age > 100;
        }
        
        public Bullet(Texture2D texture, Vector2 position, float speed, int damage, Vector2 direction, Viewport viewport, GameObject owner)
            : base(texture, position, viewport)
        {
            pos = position;
            tex = texture;
            this.damage = damage;
            this.speed = speed;
            Owner = owner;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            center = new Vector2(this.pos.X + tex.Width / 2, this.pos.Y + tex.Height / 2);
            textureData = new Color[tex.Width * tex.Height];
            tex.GetData(textureData);

            Direction = direction;
			Radius = tex.Width / 2;
        }

        public void Kill()
        {
            this.age = 200;
        }

        public override void Update(GameTime gameTime)
        {
            bulletRec = new Rectangle((int)pos.X, (int)pos.Y, 5, 5);
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            age++;
            //center.Y += speed;


            pos += Direction * speed;

            if (pos.Y >= 200)
            {
                Kill();
            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, bulletRec, Color.White, 0,
                 origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(tex, pos, Color.White);
        }

		public override void HandleCollision(GameObject other)
		{
			IsAlive = false;
		}
	}
}
