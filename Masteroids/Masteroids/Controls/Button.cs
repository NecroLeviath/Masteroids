using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids.Controls
{
    public class Button : Component
    {
        private MouseState currentMouse;

        private SpriteFont _font;

        private bool isHovering;

        private MouseState previousMouse;

        private Texture2D _texture;

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (isHovering)
                PenColour = Color.Yellow;
            else 
                PenColour = Color.White;
    
            spriteBatch.Draw(_texture, Rectangle, colour);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
                spriteBatch.Draw(Art.MenuTitleTex, new Vector2(Rectangle.X + (Rectangle.Width / 2) - (Art.MenuTitleTex.Width / 2), 200), Color.White);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click.Invoke(this, new EventArgs());
                }
            }

        }

    }
}
