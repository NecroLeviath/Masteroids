using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroid
{
    class EntityManager
    {
        List<GameObject> entities = new List<GameObject>();
        List<GameObject> bullets = new List<GameObject>();

        public EntityManager()
        {
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject o in entities)
            {
                o.Update(gameTime);
            }

            entities = entities.Where(x => !x.IsAlive).ToList();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject o in entities)
            {
                o.Draw(spriteBatch);
            }
        }

        public void CreateBullet(Vector2 pos, float speed, int damage)
        {
            GameObject o = new Bullet(pos, speed, damage);
            entities.Add(o);
            bullets.Add(o);
        }
    }
}
