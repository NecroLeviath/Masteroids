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
        private List<Component> _components;
        private EntityManager entityMgr;
        private BaseBoss boss;

        public NewGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/MenuFont");
            Sound.Load(content);
            Sound.MusicInstance.Play();

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
            game.ChangeState(new GameState(game, graphicsDevice, content, entityMgr, 1));
        }

        private void MasteroidsGameButton_click(object sender, EventArgs e)
        {
            boss = new Centipede(Art.CentipedeSheet, new Vector2(200), 240, 3, 99, graphicsDevice.Viewport, entityMgr);
            game.ChangeState(new GameState(game, graphicsDevice, content, entityMgr, 1, boss));
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Masteroids.Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
