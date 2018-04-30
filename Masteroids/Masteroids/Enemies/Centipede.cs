using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Centipede : BaseBoss
    {
        Centipede parent;
        EntityManager entityMgr;
        bool isHead;
        public List<Vector2> Beacons { get; private set; }
		float beaconTimer, moveTimer;
		float beaconInterval = 0.2f, moveInterval = 3f;
		float maxTurnRate = 2f;
		Random rand;
		Vector2 goal;

        public Centipede(Texture2D texture, Vector2 position, float speed, int hitPoints, int numberOfSegments, Viewport viewport, EntityManager entityMgr)
            : base(texture, position, speed, hitPoints, viewport)
        {
            this.entityMgr = entityMgr;
            isHead = true;
			Radius = texture.Height / 2;
            Beacons = new List<Vector2>();
			rand = new Random();
			FindGoal(moveInterval);
			Centipede previous = this;
			for (int i = 0; i < numberOfSegments; i++)
			{
				Centipede next = new Centipede(Art.CentipedeTex, this.position, 4, 3, viewport, previous, entityMgr);
				entityMgr.Add(next);
				previous = next;
			}
        }

        public Centipede(Texture2D texture, Vector2 position, float speed, int hitPoints, Viewport viewport, Centipede parent, EntityManager entityMgr)
            : base(texture, position, speed, hitPoints, viewport)
        {
            this.texture = texture; // DEV: Should be moved to GameObject
            this.speed = speed;     // DEV: Should be moved to GameObject
            this.parent = parent;
            this.entityMgr = entityMgr;
            isHead = false;
			Radius = texture.Height / 2;
			Beacons = new List<Vector2>();
		}

        public override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isHead && !parent.IsAlive)
			{
				isHead = true;
				rand = new Random();
				FindGoal(moveInterval);
			}
			if (HP < 0)
			{
				HP = 0;
				IsAlive = false;
			}

			position += velocity;

            beaconTimer += delta;
            if (beaconTimer >= beaconInterval)
            {
                Beacons.Add(position);
                beaconTimer = 0;
            }

            if (isHead)
            {
				FindGoal(delta);
				ClampAngle(delta);
				velocity = direction * speed;
            }
            else if (!isHead && parent.Beacons.Count > 0)
            {
                Vector2 distanceToBeacon = (parent.Beacons[0] - position);
                if (distanceToBeacon.LengthSquared() < speed * speed) // LengthSquared and speed^2 for efficiency
                {
                    parent.RemoveFirstBeacon();
                    direction = parent.Beacons.Count > 0 ? Vector2.Normalize(parent.Beacons[0] - position) : direction;
                    velocity = direction * speed;
                }
                else
                {
                    direction = Vector2.Normalize(distanceToBeacon);
                    velocity = direction * speed;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			var drawPos = position - new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, drawPos, Color.White);
        }

        public void RemoveFirstBeacon()
        {
            Beacons.RemoveAt(0);
        }

		public override void HandleCollision(GameObject other)
		{
			if (other is Bullet)
				HP -= (other as Bullet).Damage;
		}

		private void FindGoal(float delta)
		{
			moveTimer += delta;
			if (moveTimer >= moveInterval || Vector2.Distance(position, goal) < 10)
			{
				float x = rand.Next(200, viewport.Width - 200);
				float y = rand.Next(200, viewport.Height - 200);
				goal = new Vector2(x, y);

				moveTimer = 0;
			}
		}

		private void ClampAngle(float delta)
		{
			var currentAngle = (float)Math.Atan2(direction.Y, direction.X);
			var desiredDirection = Vector2.Normalize(goal - position);
			var desiredDirectionAngle = (float)Math.Atan2(desiredDirection.Y, desiredDirection.X);
			currentAngle = MathHelper.Clamp(MathHelper.WrapAngle(desiredDirectionAngle - currentAngle), -maxTurnRate, maxTurnRate) * delta + currentAngle;
			direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
		}
	}
}
