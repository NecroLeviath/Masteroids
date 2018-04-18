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




        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>("button");
            SpriteFont buttonFont = _content.Load<SpriteFont>(@"Fonts/Font");
            Sound.Load(content);
            //MediaPlayer.Play(Sound.Music);
            Sound.MusicInstance.Play();
            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;

            Button newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 200),
                Text = "New Game"
            };

            Button HighScoreButton = new Button(buttonTexture, buttonFont)
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
            HighScoreButton.Click += HighScoreButton_click;
            quitGameButton.Click += quitGameButton_click;
            _components = new List<Component>()
            {
                newGameButton,
                HighScoreButton,
                quitGameButton,
            };


        }

        private void NewGameButton_click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            //här startar spelet
        }

        private void HighScoreButton_click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));

        }
        private void quitGameButton_click(object sender, EventArgs e)
        {
            _game.Exit();

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            foreach (Masteroids.Component component in _components)
                component.Draw(gameTime, spriteBatch);

            //spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Masteroids.Component component in _components)
                component.Update(gameTime);
        }
    }
}
