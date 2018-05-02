using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Centipede : BaseBoss // Simon
    {
        Centipede parent;
        EntityManager entityMgr;
        bool isHead;
        public List<Vector2> Beacons { get; private set; }
		int nrSegments;
        int animationFrame = 1;
        float beaconTimer, moveTimer, animationTimer;
		float beaconInterval = 0.2f, moveInterval = 3f, animationInterval = 0.1f;
		float maxTurnRate = 2f;
		Random rnd;
		Vector2 goal;

		public Centipede(Texture2D texture, Vector2 position, float speed, int hitPoints, int numberOfSegments, Viewport viewport, EntityManager entityMgr)
			: base(texture, position, speed, hitPoints, viewport)
		{
			this.entityMgr = entityMgr;
			isHead = true;
			Radius = 30;
			Beacons = new List<Vector2>();
            sourceRectangle = new Rectangle(texture.Width / 5, 0, texture.Width / 5, texture.Height);
            rnd = new Random();
			nrSegments = numberOfSegments;
		}

        public Centipede(Texture2D texture, Vector2 position, float speed, int hitPoints, Viewport viewport, Centipede parent, EntityManager entityMgr)
            : base(texture, position, speed, hitPoints, viewport)
        {
            this.parent = parent;
            this.entityMgr = entityMgr;
            isHead = false;
			Radius = 30;
			Beacons = new List<Vector2>();
            sourceRectangle = new Rectangle(texture.Width / 5, 0, texture.Width / 5, texture.Height);
        }

		public override void Start()
		{
			FindGoal(moveInterval);
			Centipede previous = this;
			for (int i = 0; i < nrSegments; i++)
			{
				Centipede next = new Centipede(Art.CentipedeSheet, pos, speed, 3, viewport, previous, entityMgr);
				entityMgr.Add(next);
				previous = next;
			}
		}

		public override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isHead && !parent.IsAlive)
			{
				isHead = true;
				rnd = new Random();
				FindGoal(moveInterval);
			}
			if (HP < 0)
			{
				HP = 0;
				IsAlive = false;
			}

			pos += velocity * delta;

            beaconTimer += delta;
            if (beaconTimer >= beaconInterval)
            {
                Beacons.Add(pos);
                beaconTimer = 0;
            }

            if (isHead)
            {
				FindGoal(delta);
				ClampAngle(delta);
				velocity = direction * speed;
                animationTimer += delta;
                if (animationTimer >= animationInterval)
                {
                    animationFrame = ((animationFrame) % 4) + 1;
                    sourceRectangle.X = animationFrame * tex.Width / 5;
                    animationTimer = 0;
                }
            }
            else if (!isHead && parent.Beacons.Count > 0)
            {
                Vector2 distanceToBeacon = (parent.Beacons[0] - pos);
                if (distanceToBeacon.LengthSquared() < 16) // LengthSquared for efficiency. 16 is a safety value
                {
                    parent.RemoveFirstBeacon();
                    direction = parent.Beacons.Count > 0 ? Vector2.Normalize(parent.Beacons[0] - pos) : direction;
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
            rotation = (float)Math.Atan2(direction.Y, direction.X);
			rotationCenter = new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f);
            spriteBatch.Draw(tex, pos, sourceRectangle, Color.White, rotation, rotationCenter, 60f/33f, SpriteEffects.None, 0);
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
			if (moveTimer >= moveInterval || Vector2.Distance(pos, goal) < 10)
			{
				float x = rnd.Next(200, viewport.Width - 200);
				float y = rnd.Next(200, viewport.Height - 200);
				goal = new Vector2(x, y);

				moveTimer = 0;
			}
		}

		private void ClampAngle(float delta)
		{
			var currentAngle = (float)Math.Atan2(direction.Y, direction.X);
			var desiredDirection = Vector2.Normalize(goal - pos);
			var desiredDirectionAngle = (float)Math.Atan2(desiredDirection.Y, desiredDirection.X);
			currentAngle = MathHelper.Clamp(MathHelper.WrapAngle(desiredDirectionAngle - currentAngle), -maxTurnRate, maxTurnRate) * delta + currentAngle;
			direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
		}
	}
}
