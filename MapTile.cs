using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombies
{
    public class MapTile : Frempt.Sprite
    {
        private Texture2D illegalTexture;
        private string name;
        private bool[] connections;
        private int numberOfZombies;
        private int numberOfBullets;
        private int numberOfLifeTokens;
        private bool isLegal;

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

            isLegal = false;

            illegalTexture = Frempt.TextureLibrary.Illegal;
        }

        public void Rotate()
        {
            rotation += (90.0f*0.0174532925f);

            bool[] oldCon = new bool[4];
            for (int i = 0; i < oldCon.Length; i++)
            {
                oldCon[i] = connections[i];
            }

            connections[0] = oldCon[3];
            connections[1] = oldCon[0];
            connections[2] = oldCon[1];
            connections[3] = oldCon[2];

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

        public bool IsLegal()
        {
            return isLegal;
        }

        public void SetLegality(bool legal)
        {
            isLegal = legal;
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            //translate origin to the center of the object
            Vector2 origin = new Vector2(rect.Width / 2, rect.Height / 2);
            MoveBy(rect.Width / 2, rect.Height / 2);

            sb.Draw(texture, rect, null, Color.White, rotation, origin, effects, 0.0f);
            if (!isLegal) sb.Draw(illegalTexture, rect, null, Color.White * 0.5f, rotation, origin, effects, 0.0f);

            //translate back
            MoveBy(-rect.Width / 2, -rect.Height / 2);

            sb.DrawString(font, "Tile = " + name, new Vector2(rect.X + rect.Width / 10, rect.Y + rect.Height / 10), Color.Black);
        }
    }
}
