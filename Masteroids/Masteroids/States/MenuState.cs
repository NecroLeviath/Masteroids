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
        private List<Component> components;
		EntityManager entityMgr;
        Viewport viewport;
        AsteroidSpawner asteroidSpawner;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content , EntityManager entityManager) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>("button");
            SpriteFont buttonFont = _content.Load<SpriteFont>("Fonts/MenuFont");
            Sound.Load(content);
            Sound.MusicInstance.Play();
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
            components = new List<Component>()
            {
                NewGameButton,
                HighScoreButton,
                QuitGameButton,
            };
        }

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
        }

        private void NewGameButton_click(object sender, EventArgs e)
		{
            _game.ChangeState(new NewGameState(_game, _graphicsDevice, _content, entityMgr,this));
        }

        private void HighScoreButton_click(object sender, EventArgs e) 
        {
            _game.ChangeState(new HighScoreState(_game, _graphicsDevice, _content, entityMgr,this));

        }
        private void QuitGameButton_click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            entityMgr.Draw(spriteBatch);
            foreach (Masteroids.Component component in components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            asteroidSpawner.Update(gameTime);
            entityMgr.Update(gameTime);
            foreach (Masteroids.Component component in components)
                component.Update(gameTime);
        }
    }
}
