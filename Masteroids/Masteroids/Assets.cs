using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids // Simon
{
    static class Assets
    {
		#region Art
		public static Texture2D BossTex;
        public static Texture2D BulletTex;
		public static Texture2D[] AsteroidTexs;
        public static Texture2D PlayerTex;
        public static Texture2D CentipedeTex;
        public static Texture2D CentipedeSheet;
        public static Texture2D EnemySheet;
        public static Texture2D MenuTitleTex;
		#endregion

		#region Sound
		//public static Song Music { get; private set; }
		public static SoundEffect Music { get; private set; }
		public static SoundEffectInstance MusicInstance { get; private set; }

		//public static SoundEffect SEffect;
		// DEV: Or for sound effects with variation
		//private static Random rnd = new Random();
		//private static SoundEffect[] sEffects;
		//public static SoundEffect SEffect { get { return sEffects[rnd.Next(sEffects.Length)]; } }
		#endregion

		public static void Initialize(ContentManager content)
        {
			#region Art
			BossTex = content.Load<Texture2D>("ballBoss");
            BulletTex = content.Load<Texture2D>("laser");
			AsteroidTexs = new Texture2D[]
			{
				content.Load<Texture2D>("astTex"),
				content.Load<Texture2D>("ast2Tex"),
				content.Load<Texture2D>("ast4Tex")
			};
            PlayerTex = content.Load<Texture2D>("shipTex");
            CentipedeTex = content.Load<Texture2D>("CentipedeTemp");
            CentipedeSheet = content.Load<Texture2D>("pacmanSheetMod");
            EnemySheet = content.Load<Texture2D>("shooterSheet");
            MenuTitleTex = content.Load<Texture2D>("MenuTitleTex");
			#endregion

			#region Sound
			//Music = content.Load<Song>("Sound/MasteroidsTheme"); // DEV: This is just a debug song and it should be replaced
			Music = content.Load<SoundEffect>("Sound/ChillnDestroy"); // DEV: This is just a debug song and it should be replaced
			MusicInstance = Music.CreateInstance();
			MusicInstance.IsLooped = true;
			//SEffect = content.Load<SoundEffect>("");
			// DEV: Or for sound effects with variation
			// sEffects = Enumerable.Range(1, #).Select(x => content.Load<SoundEffect>("" + x)).ToArray();
			#endregion
		}
	}
}
