using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class SoundManager
    {
        private static List<SoundEffect> explosions = new List<SoundEffect>();
        private static int explosionCount = 4;

        private static SoundEffect playerShot;
        private static SoundEffect enemyShot;

        private static Random rand = new Random();

        public static void Initialize(ContentManager Content)
        {
            try
            {
                playerShot = Content.Load<SoundEffect>(@"Sound\Shot1");

                for (int x = 1; x <= explosionCount; x++)
                {

                }
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }
        }

        public static void PlayExplosion()
        {
            try
            {
                explosions[rand.Next(0, explosionCount)].Play();
            }
            catch
            {
                Debug.Write("PlayExplosion Failed");
            }
        }

        public static void PlayPlayerShot()
        {
            try
            {
                playerShot.Play();
            }
            catch
            {
                Debug.Write("PlayPlayerShot Failed");
            }
        }

        public static void PlayEnemyShot()
        {
            try
            {
                enemyShot.Play();
            }
            catch
            {
                Debug.Write("PlayEnemyShot Failed");
            }
        }
    }
}
