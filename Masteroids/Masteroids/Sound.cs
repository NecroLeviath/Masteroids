using Microsoft.Xna.Framework.Audio;
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
        
        //public static SoundEffect SEffect;
        // DEV: Or for sound effects with variation
        //private static Random rand = new Random();
        //private static SoundEffect[] sEffects;
        //public static SoundEffect SEffect { get { return sEffects[rand.Next(sEffects.Length)]; } }

        public static void Load(ContentManager content)
        {
            Music = content.Load<Song>("Sound/Castle Crashers - Jumper"); // DEV: This is just a debug song and it should be replaced
            //SEffect = content.Load<SoundEffect>("");
            // DEV: Or for sound effects with variation
            // sEffects = Enumerable.Range(1, #).Select(x => content.Load<SoundEffect>("" + x)).ToArray();
        }
    }
}
