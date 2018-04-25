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
        Centipede parent;
        bool isHead;

        public Centipede(Texture2D texture, Vector2 position, float speed, Viewport viewport)
            : base(texture, position, speed, viewport)
        {
            this.texture = texture;
            this.speed = speed;
            isHead = true;
        }

        public Centipede(Texture2D texture, Vector2 position, float speed, Viewport viewport, Centipede parent)
            : base(texture, position, speed, viewport)
        {
            this.texture = texture;
            this.speed = speed;
            this.parent = parent;
            isHead = false;
        }
    }
}
