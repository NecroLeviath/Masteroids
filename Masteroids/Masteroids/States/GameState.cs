﻿using System;
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
        Boss boss;
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
            //bosspos = new Vector2(250, 50);
            boss = new Boss(bosspos, entityMgr);

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
            Centipede previous = new Centipede(Art.CentipedeTex, new Vector2(200), 4, viewport, entityMgr);
            entityMgr.Add(previous);
            for (int i = 0; i < 10; i++)
            {
                Centipede next = new Centipede(Art.CentipedeTex, new Vector2(200), 4, viewport, previous, entityMgr);
                entityMgr.Add(next);
                previous = next;
            }
            #endregion

            font = content.Load<SpriteFont>(@"Fonts/font");
        }

        public override void Update(GameTime gameTime)
        {
            asteroidSpawner.Update(gameTime);
            boss.Update(gameTime);
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
            boss.Draw(spriteBatch);
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
			spriteBatch.DrawString(bossFont, "Life: ", bossFontPos, Color.White);
        }

    }
}
