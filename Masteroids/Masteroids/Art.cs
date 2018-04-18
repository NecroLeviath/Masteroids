using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids // Simon
{
    static class Art
    {
        public static Texture2D BossTex;
        public static Texture2D BulletTex;
        public static Texture2D AsteroidTex;

        public static void Initialize(ContentManager content)
        {
            BossTex = content.Load<Texture2D>("fireboss");
            BulletTex = content.Load<Texture2D>("skott");
            AsteroidTex = content.Load<Texture2D>("asteroid2");
        }
    }
}
