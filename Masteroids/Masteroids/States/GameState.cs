using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Masteroids
{
	public class GameState : State
	{
		SpriteFont font;
		Viewport viewport;

		EntityManager entityMgr;
		Spawner spawner;
		List<PlayerHandler> playerHandlers;

		//Vector2 bossFontPos;
		//Vector2 bossFontPos;
		//SpriteFont bossFont;

		// Asteroids
		public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, int numberOfPlayers)
			: base(game, graphicsDevice, content)
		{
			CommonConstructor(graphicsDevice, content, entityManager, numberOfPlayers);
            spawner = new AsteroidSpawner(game, entityMgr, playerHandlers, viewport, 2);
        }

		// Masteroids
		public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, int numberOfPlayers, BaseBoss boss)
			: base(game, graphicsDevice, content)
		{
			CommonConstructor(graphicsDevice, content, entityManager, numberOfPlayers);
            spawner = new MasteroidSpawner(game, entityMgr, playerHandlers, viewport, boss, 10);
		}

		private void CommonConstructor(GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, int numberOfPlayers)
		{
			viewport = graphicsDevice.Viewport;
			entityMgr = entityManager;
			playerHandlers = new List<PlayerHandler>();
			//bossFont = content.Load<SpriteFont>("BossLife");
			//bossFontPos = new Vector2(1000, 20);
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
				PlayerHandler playerHandler = new PlayerHandler(players[i], entityMgr, viewport);
				playerHandlers.Add(playerHandler);
				//Player player = new Player(Art.PlayerTex, new Vector2(viewport.Width / 2, viewport.Height / 2), players[i], entityMgr, viewport);
				//entityMgr.Add(player);
			}
		}

		public override void Update(GameTime gameTime)
		{
			spawner.Update(gameTime);
			entityMgr.Update(gameTime);
			foreach (PlayerHandler ph in playerHandlers)
				ph.Update(gameTime);
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
				//spriteBatch.DrawString(bossFont, "Life: " + bossHP + " / " + (spawner as MasteroidSpawner).BossMaxHP, bossFontPos, Color.White);
				Rectangle bossHPRect = new Rectangle(((viewport.Width - 300) / 2), 50, 300, 10);
				spriteBatch.Draw(Assets.CentipedeTex, bossHPRect, new Rectangle(30, 30, 1, 1), Color.DarkRed);
				bossHPRect.Width = bossHPRect.Width * bossHP / (spawner as MasteroidSpawner).BossMaxHP;
				spriteBatch.Draw(Assets.CentipedeTex, bossHPRect, new Rectangle(30, 30, 1, 1), Color.Red);
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
