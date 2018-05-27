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
	class EnterHighscoreState : State // Simon
	{
		List<PlayerHandler> playerHandlers;
		Spawner spawner;
		EntityManager entityMgr;
		char[] alphabet = new char[]
			{
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
				'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
			};
		char[][] playerNames;
		Color[] playerColors;
		string[] playerNumbers;
		string[] playerTimes;
		string[] playerScores;
		int[] playerCursors;
		int[] playerSelections;
		bool[] playerDone;
		GamePadState[] currentGamePadStates;
		GamePadState[] previousGamePadStates;
		float[] scrollTimeres;
		float scrollInterval = 0.2f;

		public EnterHighscoreState(Game1 game, List<PlayerHandler> playerHandlers, Spawner spawner, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager)
			: base(game, graphicsDevice, content)
		{
			this.playerHandlers = playerHandlers;
			this.spawner = spawner;
			entityMgr = entityManager;

			playerNames = new char[playerHandlers.Count][];
			for (int i = 0; i < playerNames.Length; i++)
				playerNames[i] = new char[] { '_', '_', '_' };
			playerColors = new Color[playerHandlers.Count];
			playerNumbers = new string[playerHandlers.Count];
			playerTimes = new string[playerHandlers.Count];
			playerScores = new string[playerHandlers.Count];
			playerCursors = new int[playerHandlers.Count];
			previousGamePadStates = new GamePadState[playerHandlers.Count];
			currentGamePadStates = new GamePadState[playerHandlers.Count];
			scrollTimeres = new float[playerHandlers.Count];
			playerSelections = new int[playerHandlers.Count];
			playerDone = new bool[playerHandlers.Count];

			for (int i = 0; i < playerHandlers.Count; i++)
			{
				var ph = playerHandlers[i];
				playerColors[i] = ph.Color;
				playerNumbers[i] = "Player " + (i + 1);
				playerTimes[i] = ph.GetTimeString();
				playerScores[i] = ph.GetScoreString();
			}
		}
		
		public override void Update(GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			for (int i = 0; i < playerHandlers.Count && playerDone.Any(x => !x); i++)
			{
				if (!playerDone[i] && GamePad.GetCapabilities(playerHandlers[i].PlayerIndex).IsConnected)
				{
					previousGamePadStates[i] = currentGamePadStates[i];
					currentGamePadStates[i] = GamePad.GetState(playerHandlers[i].PlayerIndex);

					if (currentGamePadStates[i].ThumbSticks.Left.Y > 0.8 ||
						currentGamePadStates[i].ThumbSticks.Left.Y < -0.8)
						scrollTimeres[i] -= delta;
					else
						scrollTimeres[i] = 0;

					if (currentGamePadStates[i].ThumbSticks.Left.Y > 0.8 &&
						(previousGamePadStates[i].ThumbSticks.Left.Y <= 0.8 ||
						scrollTimeres[i] <= 0))
					{
						playerSelections[i] = (playerSelections[i] + 1) % alphabet.Length;
						scrollTimeres[i] = scrollInterval;
					}
					else  if (currentGamePadStates[i].ThumbSticks.Left.Y < -0.8 &&
						(previousGamePadStates[i].ThumbSticks.Left.Y >= -0.8 ||
						scrollTimeres[i] <= 0))
					{
						playerSelections[i] = (playerSelections[i] + alphabet.Length - 1) % alphabet.Length;
						scrollTimeres[i] = scrollInterval;
					}
					if (playerCursors[i] < 3)
						playerNames[i][playerCursors[i]] = alphabet[playerSelections[i]];
					if (currentGamePadStates[i].Buttons.A == ButtonState.Pressed &&
						previousGamePadStates[i].Buttons.A == ButtonState.Released)
					{
						if (playerCursors[i] < 2)
							playerCursors[i]++;
						else if (spawner is MasteroidSpawner)
						{
							int.TryParse(playerScores[i], out int score);
							HighScoreState.SetMasteroidScore(new string(playerNames[i]), score);
							playerDone[i] = true;
						}
						else if (spawner is AsteroidSpawner)
						{
							int.TryParse(playerScores[i], out int score);
							HighScoreState.SetAsteroidScore(new string(playerNames[i]), score);
							playerDone[i] = true;
						}

					}
					if (currentGamePadStates[i].Buttons.B == ButtonState.Pressed &&
						previousGamePadStates[i].Buttons.B == ButtonState.Released &&
						playerCursors[i] != 0)
					{
						playerCursors[i]--;
						//playerSelections[i] = playerNames[i][playerCursors[i]];
					}
				}
			}
			if (playerDone.All(x => x))
				game.ChangeState(new MenuState(game, game.GraphicsDevice, game.Content, new EntityManager(game.GraphicsDevice.Viewport)));
		}

		public override void PostUpdate(GameTime gameTime)
		{
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			var width = Assets.ButtonFont.MeasureString("0000000000").X;
			var x = (game.GraphicsDevice.Viewport.Width - (width * playerHandlers.Count)) / 2;
			var y = game.GraphicsDevice.Viewport.Height / 2;
			for (int i = 0; i < playerHandlers.Count; i++)
			{
				var pos = new Vector2(x + i * width, y);
				spriteBatch.DrawString(Assets.ButtonFont, playerNumbers[i], pos, playerColors[i]);
				pos.Y += 30;
				spriteBatch.DrawString(Assets.ButtonFont, playerScores[i], pos, playerColors[i]);
				pos.Y += 30;
				spriteBatch.DrawString(Assets.ButtonFont, playerTimes[i], pos, playerColors[i]);
				pos.Y += 30;
				spriteBatch.DrawString(Assets.ButtonFont, new string(playerNames[i]), pos, playerColors[i]);
			}
		}
	}
}
