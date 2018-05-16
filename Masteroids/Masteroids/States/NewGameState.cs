using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Masteroids.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Masteroids.States
{
    class NewGameState : State
    {
        List<Component> _components;
        EntityManager entityMgr;
        BaseBoss boss;

        Viewport viewport;

        public NewGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content , EntityManager entityManager) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/MenuFont");


            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;
            Button ClassicGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 625),
                Text = "Classic Mode"
            };

            Button MasteroidsGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 675),
                Text = "Masteroid Mode"
            };

            ClassicGameButton.Click += ClassicGameButton_click;
            MasteroidsGameButton.Click += MasteroidsGameButton_click;
            _components = new List<Component>()
            {
                ClassicGameButton,
                MasteroidsGameButton,

            };
        }
        private void ClassicGameButton_click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1));
        }

        private void MasteroidsGameButton_click(object sender, EventArgs e)
        {
            boss = new Centipede(Art.CentipedeSheet, new Vector2(200), 240, 3, 99, _graphicsDevice.Viewport, entityMgr);
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, entityMgr, 1, boss));
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Masteroids.Component component in _components)
                component.Draw(gameTime, spriteBatch);
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
