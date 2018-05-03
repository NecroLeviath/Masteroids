﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Masteroids.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Masteroids.States
{
    public class MenuState : State
    {
        private List<Component> _components;
		EntityManager entityMgr;
		BaseBoss boss;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager)
			: base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>("button");
            SpriteFont buttonFont = _content.Load<SpriteFont>(@"Fonts/Font");
            Sound.Load(content);
            //MediaPlayer.Play(Sound.Music);
            Sound.MusicInstance.Play();
			entityMgr = entityManager;
            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;

            Button newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 200),
                Text = "New Game"
            };

            Button highScoreButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 250),
                Text = "Highscore"
            };

            Button quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 300),
                Text = "Quit Game"
            };
            newGameButton.Click += NewGameButton_click;
            highScoreButton.Click += HighScoreButton_click;
            quitGameButton.Click += quitGameButton_click;
            _components = new List<Component>()
            {
                newGameButton,
                highScoreButton,
                quitGameButton,
            };


        }

        private void NewGameButton_click(object sender, EventArgs e)
		{
            boss = new Boss(Art.BossTex, new Vector2(-100, 100), 1, 3, _graphicsDevice.Viewport, entityMgr);
            //boss = new Centipede(Art.CentipedeSheet, new Vector2(200), 240, 3, 99, _graphicsDevice.Viewport, entityMgr);
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1, boss));
			//_game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1));
			//här startar spelet
		}

        private void HighScoreButton_click(object sender, EventArgs e)
        {
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
        private void quitGameButton_click(object sender, EventArgs e)
        {
            _game.Exit();

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            //spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}
