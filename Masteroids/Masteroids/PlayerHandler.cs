using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
	class PlayerHandler
	{
		PlayerIndex playerIndex;
		EntityManager entityMgr;
		Viewport viewport;
		Vector2 spawnPos;
		Player player;
		float respawnTimer, respawnInterval = 3f;
		public int Lives { get; private set; }
		public int Score;

		public PlayerHandler(PlayerIndex playerIndex, EntityManager entityManager, Viewport viewport)
		{
			this.playerIndex = playerIndex;
			entityMgr = entityManager;
			this.viewport = viewport;
			spawnPos = new Vector2(viewport.Width / 2, viewport.Height / 2);
			Lives = 3;
			CreatePlayer();
		}

		public void Update(GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			if (Lives > 0 && !player.IsAlive && respawnTimer <= 0)
				respawnTimer = respawnInterval;
			if (respawnTimer > 0)
			{
				respawnTimer -= delta;
				if (respawnTimer <= 0)
				{
					Lives--;
					CreatePlayer();
				}
			}
		}

		private void CreatePlayer()
		{
			player = new Player(Assets.PlayerTex, spawnPos, playerIndex, entityMgr, this, viewport);
			entityMgr.Add(player);
		}
	}
}
