using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    public class Asteroid : GameObject
    {
        Texture2D tex;

        public Asteroid(Texture2D tex, Vector2 speed, Vector2 pos) : base()
        {
            this.tex = tex;
            position = pos;
            velocity = speed;
        }

        public override void Update(GameTime gameTime)
        {
            position = position + velocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, position, Color.White);
        }
    }
}
