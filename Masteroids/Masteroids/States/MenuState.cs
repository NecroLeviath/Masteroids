using System;
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


        Viewport viewport;

        AsteroidSpawner asteroidSpawner;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content , EntityManager entityManager) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>("button");
            SpriteFont buttonFont = _content.Load<SpriteFont>("Fonts/MenuFont");
            Assets.MusicInstance.Play();
			entityMgr = entityManager;
            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;

            viewport = graphicsDevice.Viewport;
            entityMgr = new EntityManager(viewport);
            asteroidSpawner = new AsteroidSpawner(entityMgr, viewport);

            Button NewGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 625),
                Text = "New Game"
            };

            Button HighScoreButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 675),
                Text = "Highscore"
            };

            Button QuitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 800),
                Text = "Quit Game"
            };
            NewGameButton.Click += NewGameButton_click;
            HighScoreButton.Click += HighScoreButton_click;
            QuitGameButton.Click += QuitGameButton_click;
            _components = new List<Component>()
            {
                NewGameButton,
                HighScoreButton,
                QuitGameButton,
            };
        }
        
        private void NewGameButton_click(object sender, EventArgs e)
		{
            //boss = new Boss(Art.BossTex, new Vector2(-100, 100), 1, 100, _graphicsDevice.Viewport, entityMgr);
            boss = new Centipede(Assets.CentipedeSheet, new Vector2(200), 240, 3, 99, _graphicsDevice.Viewport, entityMgr);
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1, boss));
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1));
            //här startar spelet
        }

        private void HighScoreButton_click(object sender, EventArgs e) 
        {
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1));
        }
        private void QuitGameButton_click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            entityMgr.Draw(spriteBatch);
            foreach (Masteroids.Component component in _components)
                component.Draw(gameTime, spriteBatch);
            //spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            asteroidSpawner.Update(gameTime);
            entityMgr.Update(gameTime);
            foreach (Masteroids.Component component in _components)
                component.Update(gameTime);
        }
    }
}
