﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids
{
    class Centipede : Enemy
    {
        Centipede parent;
        EntityManager entityMgr;
        bool isHead;
        public List<Vector2> Beacons { get; private set; }
        float beaconTimer, beaconInterval = 0.2f;
		int hp = 3; // DEV: Should be moved to constuctor

        public Centipede(Texture2D texture, Vector2 position, float speed, Viewport viewport, EntityManager entityMgr)
            : base(texture, position, speed, viewport)
        {
            this.texture = texture; // DEV: Should be moved to GameObject
            this.speed = speed;     // DEV: Should be moved to GameObject
            this.entityMgr = entityMgr;
            isHead = true;
			Radius = texture.Height / 2;
            Beacons = new List<Vector2>();
        }

        public Centipede(Texture2D texture, Vector2 position, float speed, Viewport viewport, Centipede parent, EntityManager entityMgr)
            : base(texture, position, speed, viewport)
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
                isHead = true;
			if (hp < 0)
				IsAlive = false;

            position += velocity;

            beaconTimer += delta;
            if (beaconTimer >= beaconInterval)
            {
                Beacons.Add(position);
                beaconTimer = 0;
            }

            if (isHead)
            {
                direction = Vector2.Normalize(entityMgr.Players[0].Position - position);    // DEV: TEMP
                velocity = direction * speed;
            }
            else if (!isHead && parent.Beacons.Count > 0)
            {
                var distanceToBeacon = (parent.Beacons[0] - position);
                if (distanceToBeacon.LengthSquared() < speed * speed) // 0.1f is a safety distance
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
			Vector2 drawPos = position - new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, drawPos, Color.White);
        }

        public void RemoveFirstBeacon()
        {
            Beacons.RemoveAt(0);
        }

		public override void HandleCollision(GameObject other)
		{
			if (other is Bullet)
				hp -= (other as Bullet).Damage;
		}
	}
}
