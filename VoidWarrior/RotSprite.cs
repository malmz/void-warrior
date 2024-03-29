﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace VoidWarrior
{
    /// <summary>
    /// Den här classen är skriven med hjälp av matten från följande länk. 
    /// https://www.gamedev.net/resources/_/technical/game-programming/2d-rotated-rectangle-collision-r2604
    /// </summary>
    class RotSprite
    {
        private Texture2D texture;
        private Vector2 tL, tR, bL, bR, nX, nY, center, size;
        private Rectangle rectangle;
        float rotation, radius;
        private Color color;

        public RotSprite(Texture2D texture, Vector2 center, Vector2 size, Color color, float rotation = 0)
        {
            this.texture = texture;
            this.color = color;
            this.rotation = rotation * (float)Math.PI / 180;
            this.center = center;
            this.size = size;

            if (rotation == 0)
            {
                rectangle = new Rectangle((int)(center.X - size.X / 2), (int)(center.Y - size.Y / 2), (int)size.X, (int)size.Y);
            }
            else
            {
                tL = Rotatate(new Vector2(-size.X / 2, -size.Y / 2), this.rotation);
                tR = Rotatate(new Vector2(size.X / 2, -size.Y / 2), this.rotation);
                bL = Rotatate(new Vector2(-size.X / 2, size.Y / 2), this.rotation);
                bR = Rotatate(new Vector2(size.X / 2, size.Y / 2), this.rotation);

                nX = tR - tL;
                nY = bR - tR;
            }
            
        }
        
        public bool Intersects(RotSprite otherSprite)
        {
            float sTLX = Vector2.Dot(Project(tL + center, nX), nX);
            float sTRX = Vector2.Dot(Project(tR + center, nX), nX);
            List<float> sListOtherX = otherSprite.Vertecies
                .Select(x => Project(x + otherSprite.Location, nX))
                .Select(x => Vector2.Dot(x, nX)).ToList();

            float bMaxX = sListOtherX.Max();
            float bMinX = sListOtherX.Min();

            if (bMinX <= sTRX && bMaxX >= sTLX)
            {
                float sTRY = Vector2.Dot(Project(tR + center, nY), nY);
                float sBRY = Vector2.Dot(Project(bR + center, nY), nY);
                List <float> sListOtherY = otherSprite.Vertecies
                    .Select(x => Project(x + otherSprite.Location, nY))
                    .Select(x => Vector2.Dot(x, nY)).ToList();
                float bMaxY = sListOtherY.Max();
                float bMinY = sListOtherY.Min();
                if (bMinY <= sBRY && bMaxY >= sTRY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: texture, 
                destinationRectangle: new Rectangle(center.ToPoint(), size.ToPoint()), 
                sourceRectangle: null,
                color: color, 
                origin: new Vector2(texture.Width / 2, texture.Height / 2),
                rotation: rotation,
                effects: SpriteEffects.None,
                layerDepth: 0
            );
        }

        private static Vector2 Rotatate(Vector2 vector, float angle)
        {
            return new Vector2(
                (float)(vector.X * Math.Cos(angle) - vector.Y * Math.Sin(angle)),
                (float)(vector.X * Math.Sin(angle) - vector.Y * Math.Cos(angle))
            );
        }

        private Vector2 Project(Vector2 vector, Vector2 axis)
        {
            float tmp = (vector.X * axis.X + vector.Y * axis.Y) / (axis.X * axis.X + axis.Y * axis.Y);
            return new Vector2(
                 tmp * axis.X,
                 tmp * axis.Y
            );
        }

        public List<Vector2> Vertecies
        {
            get { return new List<Vector2> { tL, tR, bL, bR }; }
        }

        public List<Vector2> Normals
        {
            get { return new List<Vector2> { nX, nY }; }
        }

        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                tL = Rotatate(tL, rotation);
                tR = Rotatate(tR, rotation);
                bL = Rotatate(bL, rotation);
                bR = Rotatate(bR, rotation);
            }
        }
        public Vector2 Location {
            get { return center; }
            set { center = value; }
        }
    }
}
