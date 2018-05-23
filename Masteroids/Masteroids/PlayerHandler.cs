using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
	public class PlayerHandler
	{
		PlayerIndex playerIndex;
		Vector2 drawPos;
		SpriteFont font;
		EntityManager entityMgr;
		Viewport viewport;
		Vector2 spawnPos;
		Player player;
		float respawnTimer, respawnInterval = 3f;
		float seconds, minutes;
		public int Lives { get; private set; }
		public int Score;
		public Color Color { get; private set; }

		public PlayerHandler(PlayerIndex playerIndex, Vector2 statsDrawPos, SpriteFont font, Color color, EntityManager entityManager, Viewport viewport)
		{
			this.playerIndex = playerIndex;
			drawPos = statsDrawPos;
			this.font = font;
			Color = color;
			entityMgr = entityManager;
			this.viewport = viewport;
			spawnPos = new Vector2(viewport.Width / 2, viewport.Height / 2);
			Lives = 3;
			CreatePlayer();
		}

		public void Update(GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (Lives >= 0)
			{
				seconds += delta;
				if (seconds > 59)
				{
					seconds = 0;
					minutes++;
				}
			}
			if (Lives >= 0 && !player.IsAlive && respawnTimer <= 0)
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

		public void Draw(SpriteBatch spriteBatch)
		{
			var scoreText = GetScoreString();
			var scoreTextDimensions = font.MeasureString(scoreText);
			var timeText = GetTimeString();

			spriteBatch.DrawString(font, scoreText, drawPos, Color);
			spriteBatch.DrawString(font, timeText, drawPos + new Vector2(0, scoreTextDimensions.Y), Color);
			for (int i = 0; i < Lives; i++)
			{
				var texSize = 30;
				var drawRect = new Rectangle((int)(drawPos.X + scoreTextDimensions.X) - (i + 1) * (texSize + 5), (int)(drawPos.Y + scoreTextDimensions.Y), texSize, texSize);
				spriteBatch.Draw(Assets.PlayerTex, drawRect, null, Color, 0, Vector2.Zero, SpriteEffects.None, 0);
			}
		}

		private string GetScoreString()
		{
			var text = "Score: ";
			for (int i = 0; i < 9 - Score.ToString().Length; i++)
				text += '0';
			text += Score;
			return text;
		}

		private string GetTimeString()
		{
			var text = "";
			for (int i = 0; i < 2 - ((int)minutes).ToString().Length; i++)
				text += 0;
			text += (int)minutes + ":";
			for (int i = 0; i < 2 - ((int)seconds).ToString().Length; i++)
				text += 0;
			text += (int)seconds;
			return text;
		}

		private void CreatePlayer()
		{
			player = new Player(Assets.PlayerTex, spawnPos, playerIndex, entityMgr, this, viewport);
			entityMgr.Add(player);
		}
	}
}
