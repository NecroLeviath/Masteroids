using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    public abstract class State
    {
        protected ContentManager content;
        protected Game1 game;
        protected GraphicsDevice graphicsDevice;


        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
        }

        public abstract void Update(GameTime gameTime);




    }
}
