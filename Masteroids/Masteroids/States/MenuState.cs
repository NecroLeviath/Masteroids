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

namespace Masteroids
{
    public class MenuState : State
    {
        private List<Component> components;
        EntityManager entityMgr;
        Viewport viewport;
        AsteroidSpawner asteroidSpawner;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager)
            : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/MenuFont");
            if (Assets.MusicInstance.State != Microsoft.Xna.Framework.Audio.SoundState.Playing)
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
                Text = "Quit"
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

        private void NewGameButton_click(object sender, EventArgs e)
        {
            game.ChangeState(new NewGameState(game, graphicsDevice, content, entityMgr, this));
        }

        private void HighScoreButton_click(object sender, EventArgs e)
        {
            game.ChangeState(new HighScoreState(game, graphicsDevice, content, entityMgr, this));

        private void QuitGameButton_click(object sender, EventArgs e)
        {
            game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            entityMgr.Draw(spriteBatch);
            foreach (Masteroids.Component component in components)
                component.Draw(gameTime, spriteBatch);
            HighScoreState.ShowDraw(spriteBatch);
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
