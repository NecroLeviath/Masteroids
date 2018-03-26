using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroid
{
    public abstract class GameObject
    {

        protected Vector2 position, velocity, acceleration, rotationCenter, direction, startPosition;
        protected Rectangle hitbox;
        public bool IsAlive { get; protected set; }

        public Rectangle GetHitbox()
        {
            return hitbox;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
