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
        AsteroidSpawner asteroidSpawner;
        BaseBoss boss;

        Viewport viewport;

        public EntityManager EntityMgr { get => entityMgr; set => entityMgr = value; }

        public NewGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content , EntityManager entityManager) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/MenuFont");

            viewport = graphicsDevice.Viewport;
            entityMgr = new EntityManager(viewport);
            asteroidSpawner = new AsteroidSpawner(entityMgr, viewport);

            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;

            Button MasteroidsGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 625),
                Text = "Masteroids Mode"
            };
            Button ClassicGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 675),
                Text = "Classic Mode"
            };
            Button ReturnButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 800),
                Text = "Return"
            };

            ClassicGameButton.Click += ClassicGameButton_click;
            MasteroidsGameButton.Click += MasteroidsGameButton_click;
            ReturnButton.Click += ReturnButton_click;
            _components = new List<Component>()
            {
                ClassicGameButton,
                MasteroidsGameButton,
                ReturnButton,

            };
        }
        private void MasteroidsGameButton_click(object sender, EventArgs e)
        {
            boss = new Centipede(Art.CentipedeSheet, new Vector2(200), 240, 3, 99, _graphicsDevice.Viewport, EntityMgr);
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, EntityMgr, 1, boss));
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content, EntityMgr, 1));
        }

        private void ClassicGameButton_click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, EntityMgr, 1));
        }
        private void ReturnButton_click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, EntityMgr));
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            entityMgr.Draw(spriteBatch);
            foreach (Masteroids.Component component in _components)
                component.Draw(gameTime, spriteBatch);
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
