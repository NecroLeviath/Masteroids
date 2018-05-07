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
        Vector2 texOffset;
        EntityManager entityMgr;
        float HP;

        public Asteroid(Texture2D texture, Vector2 speed, Vector2 position, Viewport viewport)
			: base(texture, position, viewport)
        {
            tex = texture;
            pos = position;
            velocity = speed;
            shouldWrap = true;
            texOffset = new Vector2(tex.Width / 2, tex.Height / 2);
            sourceRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			Radius = tex.Width / 2;
            HP = 1;       
        }
        public override void Update(GameTime gameTime)
        {
            pos = pos + velocity;
            ScreenWrap();
        }
        public override void HandleCollision(GameObject other)
        {
            if (other is Bullet)
                HP -= (other as Bullet).Damage;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (HP == 1)
            {
                spriteBatch.Draw(tex, pos - texOffset, Color.White);
                base.Draw(spriteBatch);
            }
        }
        protected override void WrapDraw(SpriteBatch spriteBatch)
        {
            if(HP == 1)
            spriteBatch.Draw(tex, pos - texOffset + wrapOffset, Color.White);
        }
	}
}
