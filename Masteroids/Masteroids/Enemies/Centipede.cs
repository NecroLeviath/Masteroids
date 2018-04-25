using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Centipede : Enemy
    {
        public Centipede(Texture2D texture, Vector2 position, float speed, Viewport viewport)
            : base(texture, position, speed, viewport)
        {

        }
    }
}
