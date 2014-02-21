/*
	File:			Grid.cs
	Version:		1.0
	Last altered:	21/02/2014.
	Authors:		James MacGilp.

	Description:	- An object representing a 2D grid of a specified size and segment quantity. 
	
					- Can return the closest grid point from any given point.
					
					- Can be drawn with a specified colour and line thickness.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frempt
{
    class Grid
    {
        int[] gridX;
        int[] gridY;

        int sizeX;
        int sizeY;

        public Grid(int portionsX, int portionsY, int gridWidth, int gridHeight)
        {
            sizeX = gridWidth;
            sizeY = gridHeight;

            int spacingX = (int)gridWidth / (portionsX - 1);
            int spacingY = (int)gridHeight / (portionsY - 1);

            gridX = new int[portionsX];
            gridY = new int[portionsY];

            for (int i = 0; i < gridX.Length; i++)
            {
                gridX[i] = (spacingX * i);
            }
            for (int i = 0; i < gridY.Length; i++)
            {
                gridY[i] = (spacingY * i);
            }
        }

        public Point GetClosestGridPoint(int posX, int posY)
        {
            Point returnVal = new Point();
            float distance = 5000.0f;

            for (int i = 0; i < gridX.Length; i++)
            {
                for (int j = 0; j < gridY.Length; j++)
                {
                    Vector2 vec = new Vector2((float)gridX[i] - (float)posX, (float)gridY[j] - (float)posY);

                    if (vec.Length() < distance)
                    {
                        returnVal = new Point(gridX[i], gridY[j]);
                        distance = vec.Length();
                    }
                }
            }

            return returnVal;
        }

        public Point GetClosestGridIndex(int posX, int posY)
        {
            Point returnVal = new Point();
            float distance = 5000.0f;

            for (int i = 0; i < gridX.Length; i++)
            {
                for (int j = 0; j < gridY.Length; j++)
                {
                    Vector2 vec = new Vector2((float)gridX[i] - (float)posX, (float)gridY[j] - (float)posY);

                    if (vec.Length() < distance)
                    {
                        returnVal = new Point(i, j);
                        distance = vec.Length();
                    }
                }
            }

            return returnVal;
        }

        public int GetGridPosX(int index)
        {
            if (index >= gridX.Length) index = gridX.Length - 1;
            return gridX[index];
        }

        public int GetGridPosY(int index)
        {
            if (index >= gridY.Length) index = gridY.Length - 1;
            return gridY[index];
        }

        public int GetPortionCountX()
        {
            return gridX.Length;
        }

        public int GetPortionCountY()
        {
            return gridY.Length;
        }

        public int GetWidth()
        {
            return sizeX;
        }

        public int GetHeight()
        {
            return sizeY;
        }

        public bool IsFirstX(int index)
        {
            return index == 0;
        }

        public bool IsLastX(int index)
        {
            return index == (gridX.Length - 1);
        }

        public bool IsFirstY(int index)
        {
            return index == 0;
        }

        public bool IsLastY(int index)
        {
            return index == (gridY.Length - 1);
        }

        public void DrawGrid(SpriteBatch sb, float thickness, Color color, GraphicsDevice device)
        {
            foreach (int x in gridX)
            {
                Primitives.DrawLine(sb, color, new Point(x, 0), new Point(x, sizeY), thickness, device);
            }

            foreach (int y in gridY)
            {
                Primitives.DrawLine(sb, color, new Point(0, y), new Point(sizeX, y), thickness, device);
            }
        }
    }
}
