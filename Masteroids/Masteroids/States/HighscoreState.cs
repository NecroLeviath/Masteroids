using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Masteroids.Controls;

namespace Masteroids.States
{
    class HighScoreState : State
    {
        List<Component> components;


        public HighScoreState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

            Texture2D buttonTexture = content.Load<Texture2D>("button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/MenuFont");
            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;

            Button ReturnButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 950),
                Text = "Return"
            };
            ReturnButton.Click += ReturnButton_click;
            components = new List<Component>()
            {
                ReturnButton,
            };
        }
            private void ReturnButton_click(object sender, EventArgs e)
            {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Masteroids.Component component in components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Masteroids.Component component in components)
                component.Update(gameTime);
        }
    }
}
