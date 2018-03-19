using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Bullet
    {
        private int damage, age;
        Texture2D texture;
        Vector2 pos;
        float bulletTimer, speed;
        protected Vector2 center;
        protected Vector2 origin;
        List<Bullet> bulletList = new List<Bullet>();

        public int Damage
        {
            get { return damage; }
        }

        public bool IsDead()
        {
            return age > 100;
        }

        public Bullet(Texture2D texture, Vector2 pos, float speed, int damage)
        {
            this.texture = texture;
            this.damage = damage;
            this.speed = speed;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            center = new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2);
        }

        public void Kill()
        {
            this.age = 200;
        }

        public void Update(GameTime gameTime)
        {
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            age++;
            center.Y += speed;

            if (pos.Y >= 200)
            {
                Kill();
            }


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, center, null, Color.White, 0,
                 origin, 1.0f, SpriteEffects.None, 0);
        }

    }
}
