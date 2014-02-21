﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombies
{
    public class MapTile : Frempt.Sprite
    {
        private string name;
        private bool[] connections;
        private int numberOfZombies;
        private int numberOfBullets;
        private int numberOfLifeTokens;

        public MapTile(Texture2D tex, string name)
        {
            texture = tex;

            rect = new Rectangle(0, 0, tex.Width, tex.Height);

            velocity = new Vector2(0.0f, 0.0f);

            this.name = name;
        }

        public void Setup(bool[] connections, int zombies, int bullets, int lives)
        {
            this.connections = connections;

            numberOfBullets = bullets;
            numberOfZombies = zombies;
            numberOfLifeTokens = lives;
        }

        public void Rotate()
        {
            rotation += (90.0f*0.0174532925f);

            bool[] oldCon = connections;
            connections[0] = oldCon[1];
            connections[1] = oldCon[2];
            connections[2] = oldCon[3];
            connections[3] = oldCon[0];

            if (rotation > Math.PI*2)
            {
                rotation -= ((float)Math.PI*2);
            }
        }

        public int GetNumberOfBullets()
        {
            return numberOfBullets;
        }

        public int GetNumberOfZombies()
        {
            return numberOfZombies;
        }

        public int GetNumberOfLives()
        {
            return numberOfLifeTokens;
        }

        public bool HasLeftConnection()
        {
            return connections[3];
        }

        public bool HasUpConnection()
        {
            return connections[0];
        }

        public bool HasRightConnection()
        {
            return connections[1];
        }

        public bool HasDownConnection()
        {
            return connections[2];
        }

        public string GetName()
        {
            return name;
        }

        new public void Draw(SpriteBatch sb)
        {
            //translate origin to the center of the object
            Vector2 origin = new Vector2(rect.Width / 2, rect.Height / 2);
            MoveBy(-rect.Width / 2, -rect.Height / 2);

            sb.Draw(texture, rect, null, Color.White, rotation, origin, effects, 0.0f);


            //translate back
            MoveBy(rect.Width / 2, rect.Height / 2);
        }
    }
}
