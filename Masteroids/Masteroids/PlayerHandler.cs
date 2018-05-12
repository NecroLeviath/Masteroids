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
		int lives;

		public PlayerHandler(PlayerIndex playerIndex, EntityManager entityManager, Viewport viewport)
		{
			this.playerIndex = playerIndex;
			entityMgr = entityManager;
			this.viewport = viewport;
			spawnPos = new Vector2(viewport.Width / 2, viewport.Height / 2);
			lives = 3;
			CreatePlayer();
		}

		public void Update(GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			if (lives > 0 && !player.IsAlive && respawnTimer <= 0)
				respawnTimer = respawnInterval;
			if (respawnTimer > 0)
			{
				respawnTimer -= delta;
				if (respawnTimer <= 0)
				{
					lives--;
					CreatePlayer();
				}
			}
		}

		private void CreatePlayer()
		{
			player = new Player(Art.PlayerTex, spawnPos, playerIndex, entityMgr, viewport);
			entityMgr.Add(player);
		}
	}
}
