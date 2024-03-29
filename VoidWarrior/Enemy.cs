﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoidWarrior.Ui.Progress;
using VoidWarrior.Ui.Menu;

namespace VoidWarrior
{
    // TODO: Write shooting mekanic
    class Enemy : IDynamic
    {
        private Sprite sprite;
        private Vector2 startPosition;
        private Path path;
        private int health;
        private Bar healthBar;
        private float delay;
        private int value;

        public Enemy(Sprite sprite, Texture2D healthBarTexture, int health, int value, Path path, float delay = 0)
        {
            this.sprite = sprite;
            startPosition = sprite.Position;
            this.path = path;
            this.delay = delay;
            this.health = health;
            this.value = value;
            healthBar = new Bar(healthBarTexture, X + sprite.Width / 2 - 80 / 2, Y + sprite.Height + 10, 80, 6, health, health, Color.Red);
        }

        /// <summary>
        /// Move Enemy along its path and update it's healthbar
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            path.Update(gameTime);
            sprite.Position = path.Position + startPosition;
            healthBar.Value = health;
            healthBar.X = X + sprite.Width / 2 - 80 / 2;
            healthBar.Y = Y + sprite.Height + 10;
        }

        /// <summary>
        /// Draw enemy to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
        }

        public float X
        {
            get { return path.X + startPosition.X; }
            set { startPosition.X = value - path.X; }
        }

        public float Y
        {
            get { return path.Y + startPosition.Y; }
            set { startPosition.Y = value - path.Y; }
        }

        public float StartX
        {
            get { return startPosition.X; }
            set { startPosition.X = value; }
        }

        public float StartY
        {
            get { return startPosition.Y; }
            set { startPosition.Y = value; }
        }

        public float Width
        {
            get { return sprite.Width; }
        }

        public float Height
        {
            get { return sprite.Height; }
        }

        public Rectangle Bounds
        {
            get
            {
                return sprite.Bounds;
            }
        }
        public float Delay { get { return delay; } }

        public int Health {
            get { return health; }
            set { health = value; }
        }

        public int Value
        {
            get { return value; }
        }

        public void Reset()
        {
            health = (int)healthBar.MaxValue;
            path.Reset();
        }
    }
}
