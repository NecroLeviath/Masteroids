using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Enemy : GameObject
    {
        public Enemy(Vector2 position, Viewport viewport)
            : base(position, viewport) { }

        public override void Update(GameTime gameTime) { }
    }
}
