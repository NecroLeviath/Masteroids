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
        public static Texture2D PlayerTex;
        public static Texture2D CentipedeTex;

        public static void Initialize(ContentManager content)
        {
            BossTex = content.Load<Texture2D>("ballBoss");
            BulletTex = content.Load<Texture2D>("laser");
            AsteroidTex = content.Load<Texture2D>("ast4Tex");
            PlayerTex = content.Load<Texture2D>("shipTex");
            CentipedeTex = content.Load<Texture2D>("CentipedeTemp");
        }
    }
}
