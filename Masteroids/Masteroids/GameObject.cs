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
        public Vector2 Position { get { return position; } }
        protected Vector2 position, velocity, rotationCenter, direction, startPosition, wrapOffset;
        protected float speed, rotation, acceleration, deacceleration;
        protected Rectangle sourceRectangle;
        protected Rectangle hitbox;
        public Rectangle GetHitbox() { return hitbox; }
        protected Viewport viewport;
        protected bool shouldWrap;
        public bool IsAlive { get; protected set; }
        public float Radius;

        public GameObject(Vector2 position, Viewport viewport)
        {
            this.position = position;
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

                if (position.X < viewport.X) // Checks if the entitys position is off the screen...
                    position.X += viewport.Width; // ... if it is, moves the entity to the other side of the screen
                else if (position.X - (sourceRectangle.Width / 2) < viewport.X) // Otherwise, if the entity is only a bit off the screen...
                    wrapOffset.X += viewport.Width; // ... set an offset so that the entity can appear on the other side of the screen

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
