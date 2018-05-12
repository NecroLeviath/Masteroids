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

		public PlayerHandler(PlayerIndex playerIndex, EntityManager entityManager, Viewport viewport)
		{
			this.playerIndex = playerIndex;
			entityMgr = entityManager;
			this.viewport = viewport;
			spawnPos = new Vector2(viewport.Width / 2, viewport.Height / 2);
		}
	}
}
