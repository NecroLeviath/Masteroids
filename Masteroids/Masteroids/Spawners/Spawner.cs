using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    abstract class Spawner
    {
		protected Game1 game;
        protected EntityManager entityMgr;
		protected List<PlayerHandler> playerHandlers;
        protected Viewport viewport;
        protected Random rnd;

        public Spawner(Game1 game, EntityManager entityManager, List<PlayerHandler> playerHandlers, Viewport viewport)
        {
			this.game = game;
            entityMgr = entityManager;
			this.playerHandlers = playerHandlers;
            this.viewport = viewport;
            rnd = new Random();
        }

		public Spawner(EntityManager entityManager, Viewport viewport)
		{
			entityMgr = entityManager;
			this.viewport = viewport;
			rnd = new Random();
		}

        public abstract void Update(GameTime gameTime);

		protected Vector2 RandomSide()
		{
			var temp = rnd.Next(4);
			var x = 0;
			var y = 0;

			if (temp <= 1)
				x = rnd.Next(viewport.Width);
			else if (temp >= 2)
				y = rnd.Next(viewport.Height);
			if (temp == 1)
				y = viewport.Height;
			else if (temp == 3)
				x = viewport.Width;

			var pos = new Vector2(x, y);
			return pos;
		}
    }
}
