using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids // Simon
{
    class Enemy : GameObject
    {
        public Enemy(Texture2D texture, Vector2 position, float speed, Viewport viewport)
            : base(position, viewport) { } //måsvingar

        public override void Update(GameTime gameTime) { } //måsvingar
    }
}
