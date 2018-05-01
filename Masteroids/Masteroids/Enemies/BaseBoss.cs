using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
	public abstract class BaseBoss : Enemy // Simon
	{
		public int HP { get; protected set; }

		public BaseBoss(Texture2D texture, Vector2 position, float speed, int hitPoints, Viewport viewport)
			: base(texture, position, speed, viewport)
		{
			this.texture = texture; // DEV: Should be moved to GameObject
			this.speed = speed;     // DEV: Should be moved to GameObject
			HP = hitPoints;
		}

		public virtual void Start() { }
	}
}
