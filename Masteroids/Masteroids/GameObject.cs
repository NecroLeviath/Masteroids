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
        protected Texture2D tex;
        public Vector2 Position { get { return pos; } }
        protected Vector2 pos, velocity, rotationCenter, direction, startPosition, wrapOffset;
        protected float speed, rotation, acceleration, deacceleration;
        protected Rectangle sourceRectangle;
        protected Rectangle hitbox;
        public Rectangle GetHitbox() { return hitbox; }
        protected Viewport viewport;
        protected bool shouldWrap;
        public bool IsAlive { get; protected set; }
        public float Radius;

        public GameObject(Texture2D texture, Vector2 position, Viewport viewport)
        {
			tex = texture;
            pos = position;
            this.viewport = viewport;
            IsAlive = true;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (shouldWrap && wrapOffset != Vector2.Zero)
                WrapDraw(spriteBatch);
        }

        public virtual void HandleCollision(GameObject other) { } // DEV: This will probably be an abstract method

        protected virtual void WrapDraw(SpriteBatch spriteBatch) { }

        protected void ScreenWrap() // Simon
        {
            if (shouldWrap)
            {
                wrapOffset = Vector2.Zero;

                if (pos.X < viewport.X) // Checks if the entitys position is off the screen...
                    pos.X += viewport.Width; // ... if it is, moves the entity to the other side of the screen
                else if (pos.X - (sourceRectangle.Width / 2) < viewport.X) // Otherwise, if the entity is only a bit off the screen...
                    wrapOffset.X += viewport.Width; // ... set an offset so that the entity can appear on the other side of the screen

                if (pos.X > viewport.X + viewport.Width)
                    pos.X -= viewport.Width;
                else if (pos.X + (sourceRectangle.Width / 2) > viewport.X + viewport.Width)
                    wrapOffset.X -= viewport.Width;

                if (pos.Y < viewport.Y)
                    pos.Y += viewport.Height;
                else if (pos.Y - (sourceRectangle.Height / 2) < viewport.Y)
                    wrapOffset.Y += viewport.Height;

                if (pos.Y > viewport.Y + viewport.Height)
                    pos.Y -= viewport.Height;
                else if (pos.Y + (sourceRectangle.Height / 2) > viewport.Y + viewport.Height)
                    wrapOffset.Y -= viewport.Height;
            }
        }
    }
}
