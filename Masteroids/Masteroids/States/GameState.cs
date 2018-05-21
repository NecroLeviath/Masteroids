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
		Viewport viewport;
		EntityManager entityMgr;
		Spawner spawner;
		List<PlayerHandler> playerHandlers;

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

			PlayerIndex[] players = new PlayerIndex[]
			{
				PlayerIndex.One,
				PlayerIndex.Two,
				PlayerIndex.Three,
				PlayerIndex.Four
			};
			Vector2[] phDrawPos = new Vector2[]
			{
				new Vector2(0, 0),
				new Vector2(300, 0),
				new Vector2(600, 0),
				new Vector2(900, 0)
			};
			for (int i = 0; i < numberOfPlayers; i++)
			{
				PlayerHandler playerHandler = new PlayerHandler(players[i], phDrawPos[i], Assets.ButtonFont, entityMgr, viewport);
				playerHandlers.Add(playerHandler);
			}
		}

		public override void Update(GameTime gameTime)
		{
			spawner.Update(gameTime);
			entityMgr.Update(gameTime);
			foreach (PlayerHandler ph in playerHandlers)
				ph.Update(gameTime);
		}

		public override void PostUpdate(GameTime gameTime) { }

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			entityMgr.Draw(spriteBatch);
			foreach (PlayerHandler ph in playerHandlers)
				ph.Draw(spriteBatch);

			if (entityMgr.Bosses.Count > 0)
			{
				int bossHP = 0;
				for (int i = 0; i < entityMgr.Bosses.Count; i++) // Summarizes the HP of the boss.
					bossHP += entityMgr.Bosses[i].HP;
				Rectangle bossHPRect = new Rectangle(((viewport.Width - 300) / 2), 50, 300, 10);
				spriteBatch.Draw(Assets.CentipedeTex, bossHPRect, new Rectangle(30, 30, 1, 1), Color.DarkRed);
				bossHPRect.Width = bossHPRect.Width * bossHP / (spawner as MasteroidSpawner).BossMaxHP;
				spriteBatch.Draw(Assets.CentipedeTex, bossHPRect, new Rectangle(30, 30, 1, 1), Color.Red);
			}
		}
	}
}
