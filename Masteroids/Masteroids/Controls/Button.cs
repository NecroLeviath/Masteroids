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
        #region
        private MouseState _currentMouse;   // CR: _ före variabelnamn finns ej i riktlinjerna
											// CR: Hade tagit mindre plats om utan mellanrum mellan alla fields
		private SpriteFont _font;

        private bool _isHovering;

        private MouseState _previousMouse;

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
			// CR: Onödigt mellanrum
        }
		#endregion
		// CR: Regionen hade kunnat vara markerad "Fields" eller liknande
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)	// CR: Hade kunnat byta plats på Draw och Update för att matcha andra klasser
        {
            var colour = Color.White;	// CR: Då colour endast används på ett ställe så hade det inte behövt vara en variabel
            if (_isHovering)
                PenColour = Color.Yellow;
            else 
                PenColour = Color.White;
    
            spriteBatch.Draw(_texture, Rectangle, colour);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
                spriteBatch.Draw(Assets.MenuTitleTex, new Vector2(Rectangle.X + (Rectangle.Width / 2)- (Assets.MenuTitleTex.Width/2), 200), Color.White);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click.Invoke(this, new EventArgs());	// CR: Enradig if-sats behöver ej måsvingar
                }
            }
			// CR: Onödigt mellanrum
        }
		// CR: Onödigt mellanrum
	}
}
