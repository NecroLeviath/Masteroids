using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids //Laila
{
    public class Asteroid : GameObject
    {
        Texture2D tex;
        Vector2 texOffset;

        public Asteroid(Texture2D texture, Vector2 speed, Vector2 position, Viewport viewport)
			: base(texture, position, viewport)
        {
            tex = texture;
            pos = position;
            velocity = speed;
            shouldWrap = true;
            texOffset = new Vector2(tex.Width / 2, tex.Height / 2);
            sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
        }

        public override void Update(GameTime gameTime)
        {
            pos = pos + velocity;
            ScreenWrap();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos - texOffset, Color.White);
            base.Draw(spriteBatch);
        }
        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos - texOffset + wrapOffset, Color.White);
        }
    }
}
