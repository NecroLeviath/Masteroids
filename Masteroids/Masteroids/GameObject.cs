using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    public abstract class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position, velocity, acceleration, rotationCenter, direction, startPosition, wrapOffset;
        protected Rectangle sourceRectangle;
        protected Rectangle hitbox;
        public Rectangle GetHitbox() { return hitbox; }
        Viewport viewport;
        protected bool shouldWrap;
        public bool IsAlive { get; protected set; }

        public GameObject(Vector2 position, Viewport viewport)
        {
            this.position = position;
            this.viewport = viewport;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (shouldWrap)
            {
                WrapDraw(spriteBatch);
            }
        }

        protected virtual void WrapDraw(SpriteBatch spriteBatch) { }

        protected void ScreenWrap()
        {
            if (shouldWrap)
            {
                wrapOffset = Vector2.Zero;

                if (position.X < viewport.X)
                    position.X += viewport.Width;
                else if (position.X - (sourceRectangle.Width / 2) < viewport.X)
                    wrapOffset.X += viewport.Width;

                if (position.X > viewport.X + viewport.Width)
                    position.X -= viewport.Width;
                else if (position.X + (sourceRectangle.Width / 2) > viewport.X + viewport.Width)
                    wrapOffset.X -= viewport.Width;

                if (position.Y < viewport.Y)
                    position.Y += viewport.Height;
                else if (position.Y - (sourceRectangle.Height / 2) < viewport.Y)
                    wrapOffset.Y += viewport.Height;

                if (position.Y > viewport.Y + viewport.Height)
                    position.Y -= viewport.Height;
                else if (position.Y + (sourceRectangle.Height / 2) > viewport.Y + viewport.Height)
                    wrapOffset.Y -= viewport.Height;
            }
        }
    }
}
