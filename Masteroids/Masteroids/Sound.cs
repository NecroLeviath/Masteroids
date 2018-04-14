using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    static class Sound
    {
        public static Song Music { get; private set; }

        public static void Load(ContentManager content)
        {
            Music = content.Load<Song>("Sound/Castle Crashers - Jumper");
        }
    }
}
