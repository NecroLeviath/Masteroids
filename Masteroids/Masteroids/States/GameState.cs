using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Masteroids.States
{
	public class GameState : State
	{
		SpriteFont font;
		Viewport viewport;

		EntityManager entityMgr;
		AsteroidSpawner asteroidSpawner;
        Boss boss;

		Vector2 bossFontPos;
		SpriteFont bossFont;

		// Asteroids
		public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, int numberOfPlayers)
			: base(game, graphicsDevice, content)
		{
            CommonConstructor(graphicsDevice, content, entityManager, numberOfPlayers);
            Shooter shooter = new Shooter(Art.EnemySheet, new Vector2(20), 100, entityMgr, viewport);
            entityMgr.Add(shooter);

        }

        // Masteroids
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, int numberOfPlayers, BaseBoss boss)
			: base(game, graphicsDevice, content)
		{
			CommonConstructor(graphicsDevice, content, entityManager, numberOfPlayers);
            Shooter shooter = new Shooter(Art.EnemySheet, new Vector2(20), 100, entityMgr, viewport);
            boss = new Boss(Art.BossTex, new Vector2(-100,100), 1, 3, viewport, entityMgr);
            entityMgr.Add(shooter);
			entityMgr.Add(boss);
			entityMgr.Bosses[0].Start();
		}

		private void CommonConstructor(GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, int numberOfPlayers)
		{
			viewport = graphicsDevice.Viewport;
			entityMgr = entityManager;
			asteroidSpawner = new AsteroidSpawner(entityMgr, viewport);
			bossFont = content.Load<SpriteFont>("BossLife");
			bossFontPos = new Vector2(1000, 20);
			font = content.Load<SpriteFont>(@"Fonts/font");

			PlayerIndex[] players = new PlayerIndex[]
			{
				PlayerIndex.One,
				PlayerIndex.Two,
				PlayerIndex.Three,
				PlayerIndex.Four
			};
			for (int i = 0; i < numberOfPlayers; i++)
			{
				Player player = new Player(Art.PlayerTex, new Vector2(viewport.Width / 2, viewport.Height / 2), players[i], entityMgr, viewport);
				entityMgr.Add(player);
			}
		}

		public override void Update(GameTime gameTime)
		{
			asteroidSpawner.Update(gameTime);
			entityMgr.Update(gameTime);
			#region Out-commented
			//GamePadCapabilities capabilities =
			//    GamePad.GetCapabilities(PlayerIndex.Two);
			//GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
			//if (capabilities.IsConnected)
			//{
			//    GamePadState gamePadState = GamePad.GetState(PlayerIndex.Two);
			//    //if (capabilities.HasLeftXThumbStick)
			//    //Player 2, Tangentboard endast
			//    if (gamePadState.Buttons.Start == ButtonState.Pressed)
			//    {
			//        enteredGame = true;
			//    }
			//}

			//if (enteredGame)
			//{
			//    player2.Update(gameTime);
			//}
			#endregion
		}

		public override void PostUpdate(GameTime gameTime) { }

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			entityMgr.Draw(spriteBatch);

			if (entityMgr.Bosses.Count > 0)
			{
				int bossHP = 0;
				for (int i = 0; i < entityMgr.Bosses.Count; i++)    // Summarizes the HP of the boss.
					bossHP += entityMgr.Bosses[i].HP;
				spriteBatch.DrawString(bossFont, "Life: " + bossHP, bossFontPos, Color.White);  // DEV: Might be changed to a health bar instead of a number
			}
           

			#region Out-commented
			//if (enteredGame)
			//{
			//    player2.Draw(spriteBatch);
			//}
			//else
			//{
			//    spriteBatch.DrawString(font, "Press start to Enter", new Vector2(1700, 980), Color.White);
			//}
			#endregion
		}
    }
}
