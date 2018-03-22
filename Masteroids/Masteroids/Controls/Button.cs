
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
    class Button
    {
        private MouseState _currentmouseState;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;


        public event EventHandler Click;
        public bool clicked { get; private set; }
        public Color Pencolor { get; set; }
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
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (_isHovering)
                colour = Color.Gray;
            spriteBatch.Draw(_texture, Rectangle, colour);
            if(!String.IsNullOrEmpty(Text))
            {

            }



        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentmouseState;
            _currentmouseState = Mouse.GetState();



        }







    }
}
