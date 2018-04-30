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
        Vector2 bosspos;
        SpriteFont font;
        Viewport viewport;

        EntityManager entityMgr;
        AsteroidSpawner asteroidSpawner;

        Vector2 bossFontPos;
        SpriteFont bossFont;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
			: base(game, graphicsDevice, content)
        {
            viewport = graphicsDevice.Viewport;
            entityMgr = new EntityManager(viewport);
            asteroidSpawner = new AsteroidSpawner(entityMgr, viewport);
            bossFont = content.Load<SpriteFont>("BossLife");
            bossFontPos = new Vector2(1000, 20);
            bosspos = new Vector2(250, 50);
            Boss boss = new Boss(Art.BossTex, bosspos, 4, 1, viewport, entityMgr);
			entityMgr.Add(boss);

			//Player 1, Kontroll och Tangentbord
			PlayerIndex[] players = new PlayerIndex[]
			{
				PlayerIndex.One,
				PlayerIndex.Two,
				PlayerIndex.Three,
				PlayerIndex.Four
			};
			int numberOfPlayers = 1;
			for (int i = 0; i < numberOfPlayers; i++)
			{
				Player player = new Player(Art.PlayerTex, new Vector2(viewport.Width / 2, viewport.Height / 2), players[i], entityMgr, viewport);
				entityMgr.Add(player);
			}

            #region Debug
            Centipede centipede = new Centipede(Art.CentipedeTex, new Vector2(200), 4, 3, 9, viewport, entityMgr);
            entityMgr.Add(centipede);
            #endregion

            font = content.Load<SpriteFont>(@"Fonts/font");
        }

        public override void Update(GameTime gameTime)
        {
            asteroidSpawner.Update(gameTime);
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
			entityMgr.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            entityMgr.Draw(spriteBatch);
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

			int bossHP = 0;
			for (int i = 0; i < entityMgr.Bosses.Count; i++)	// Summarizes the HP of the boss.
				bossHP += entityMgr.Bosses[i].HP;
			spriteBatch.DrawString(bossFont, "Life: " + bossHP, bossFontPos, Color.White);  // DEV: Might be changed to a health bar instead of a number
		}
    }
}
