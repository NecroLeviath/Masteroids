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
        //public static Song Music { get; private set; }
        public static SoundEffect Music { get; private set; }
        public static SoundEffectInstance MusicInstance { get; private set; }

        //public static SoundEffect SEffect;
        // DEV: Or for sound effects with variation
        //private static Random rnd = new Random();
        //private static SoundEffect[] sEffects;
        //public static SoundEffect SEffect { get { return sEffects[rnd.Next(sEffects.Length)]; } }

        public static void Load(ContentManager content)
        {
            //Music = content.Load<Song>("Sound/MasteroidsTheme"); // DEV: This is just a debug song and it should be replaced
            Music = content.Load<SoundEffect>("Sound/MasteroidsTheme"); // DEV: This is just a debug song and it should be replaced
            MusicInstance = Music.CreateInstance();
            MusicInstance.IsLooped = true;
            //SEffect = content.Load<SoundEffect>("");
            // DEV: Or for sound effects with variation
            // sEffects = Enumerable.Range(1, #).Select(x => content.Load<SoundEffect>("" + x)).ToArray();
        }
    }
}
