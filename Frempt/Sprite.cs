﻿/*
	File:			Sprite.cs
	Version:		1.3
	Last altered:	18/02/2014.
	Authors:		James MacGilp.

	Description:	- Encapsulation of a basic textured sprite object. 
	
					- Adds a SpriteEffects object, which allows the image to be flipped. Some simple commented out code in MoveBy(int, int) is called, can flip the sprite to reflect movement direction.
					
					- This class should be extended by more specific classes.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frempt
{
    public class Sprite
    {
        protected Rectangle rect;
        protected Texture2D texture;
        protected Vector2 velocity;
        protected SpriteEffects effects;
        protected float rotation;

        public Sprite()
        {
        }

        public Sprite(Texture2D tex)
        {
            texture = tex;

            rect = new Rectangle(0, 0, tex.Width, tex.Height);

            velocity = new Vector2(0.0f, 0.0f);
        }

        public bool Collision(Rectangle other)
        {
            if (rect.Left < other.Right && rect.Right > other.Left && rect.Top < other.Bottom && rect.Bottom > other.Top)
            {
                return true;
            }
            return false;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Rectangle GetRect()
        {
            return rect;
        }

        public void SetRect(Rectangle newRect)
        {
            rect = newRect;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public void SetVelocity(Vector2 newVelocity)
        {
            velocity = newVelocity;
        }

        public SpriteEffects GetSpriteEffects()
        {
            return effects;
        }

        public float GetRotation()
        {
            return rotation;
        }

        public void MoveBy(int xMove, int yMove)
        {
            Rectangle tempRect = new Rectangle(rect.Left + xMove, rect.Top + yMove, rect.Width, rect.Height);
            rect = tempRect;

            /*if (xMove > 0)
            {
                effects = SpriteEffects.None;
            }
            if (xMove < 0)
            {
                effects = SpriteEffects.FlipHorizontally;
            }*/
        }

        public void MoveTo(int x, int y)
        {
            Rectangle tempRect = new Rectangle(x, y, rect.Width, rect.Height);
            rect = tempRect;
        }

        public void MoveTo(Point pos)
        {
            Rectangle tempRect = new Rectangle(pos.X, pos.Y, rect.Width, rect.Height);
            rect = tempRect;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, null, Color.White, rotation, Vector2.Zero, effects, 0.0f);
        }
    }
}
