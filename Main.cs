using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Zombies
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        public static TimeSpan deltaTime;
        public static float screenWidth;
        public static float screenHeight;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MouseState prevMouseState;

        public enum MovePhase { MAP = 0, TOKENS = 1, COMBAT = 2, DRAWCARDS = 3, MOVEPLAYER = 4, MOVEZOMBIES = 5, DISCARD = 6 };
        private MovePhase movePhase = MovePhase.MAP;

        private List<MapTile> tiles = new List<MapTile>();
        private MapTile newTile;

        private int[] gridX;
        private int[] gridY;

        private bool tileDrawn = false;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            CalculateGrid(4, 4);

            MapDeck.Shuffle();
            newTile = MapDeck.Draw();
            newTile.MoveTo(GetClosestGridPoint((int)screenWidth / 2, (int)screenHeight / 2));
            tiles.Add(newTile);
            newTile = null;

            prevMouseState = Mouse.GetState();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Frempt.TextureLibrary.LoadTextures(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected void CalculateGrid(int portionsX, int portionsY)
        {
            int spacingX = (int)screenWidth / (portionsX - 1);
            int spacingY = (int)screenHeight / (portionsY - 1);

            gridX = new int[portionsX];
            gridY = new int[portionsY];

            for (int i = 0; i < gridX.Length; i++)
            {
                gridX[i] = (spacingX / 2) + (spacingX * i);
            }
            for (int i = 0; i < gridY.Length; i++)
            {
                gridY[i] = (spacingY / 2) + (spacingY * i);
            }
        }

        protected Point GetClosestGridPoint(int posX, int posY)
        {
            Vector2 returnVal = new Vector2(5000, 5000);

            for (int i = 0; i < gridX.Length; i++)
            {
                for (int j = 0; j < gridY.Length; j++)
                {
                    Vector2 vec = new Vector2((float)gridX[i], (float)gridY[j]);
                    if(vec.Length() < returnVal.Length())
                    {
                        returnVal = vec;
                    }
                }
            }

            Point p = new Point((int)returnVal.X, (int)returnVal.Y);
            return p;
        }

        protected Point GetClosestGridIndex(int posX, int posY)
        {
            Vector2 returnVal = new Vector2(5000, 5000);

            Point p = new Point(0, 0);

            for (int i = 0; i < gridX.Length; i++)
            {
                for (int j = 0; j < gridY.Length; j++)
                {
                    Vector2 vec = new Vector2((float)gridX[i], (float)gridY[j]);
                    if (vec.Length() < returnVal.Length())
                    {
                        returnVal = vec;
                        p = new Point(i, j);
                    }

                }
            }

            return p;
        }

        protected MapTile GetTileAtGridPoint(int gridIndexX, int gridIndexY)
        {
            foreach (MapTile tile in tiles)
            {
                if (tile.GetRect().X == gridX[gridIndexX] && tile.GetRect().Y == gridY[gridIndexY])
                {
                    return tile;
                }
            }

            return null;
        }

        protected bool IsLegalPlacement(int gridIndexX, int gridIndexY, MapTile tile)
        {
            if(GetTileAtGridPoint(gridIndexX, gridIndexY) != null)
            {
                return false;
            }

            MapTile left = GetTileAtGridPoint(gridIndexX - 1, gridIndexY);
            if (left != null && (left.HasRightConnection() && !tile.HasLeftConnection()))
            {
                return false;
            }

            MapTile right = GetTileAtGridPoint(gridIndexX + 1, gridIndexY);
            if (right != null && (right.HasLeftConnection() && !tile.HasRightConnection()))
            {
                return false;
            }

            MapTile up = GetTileAtGridPoint(gridIndexX, gridIndexY - 1);
            if (up != null && (up.HasDownConnection() && !tile.HasUpConnection()))
            {
                return false;
            }

            MapTile down = GetTileAtGridPoint(gridIndexX, gridIndexY + 1);
            if (down != null && (down.HasUpConnection() && !tile.HasDownConnection()))
            {
                return false;
            }

            return true;
        }

        protected bool CanBePlaced(MapTile tile)
        {
            if (tile != null)
            {
                MapTile lastState = tile;

                for (int i = 0; i < gridX.Length; i++)
                {
                    for (int j = 0; j < gridY.Length; j++)
                    {
                        if (IsLegalPlacement(i, j, tile))
                        {
                            newTile = lastState;
                            return true;
                        }

                        newTile.Rotate();

                        if (IsLegalPlacement(i, j, tile))
                        {
                            newTile = lastState;
                            return true;
                        }

                        newTile.Rotate();

                        if (IsLegalPlacement(i, j, tile))
                        {
                            newTile = lastState;
                            return true;
                        }

                        newTile.Rotate();

                        if (IsLegalPlacement(i, j, tile))
                        {
                            newTile = lastState;
                            return true;
                        }
                    }
                }

                newTile = lastState;
            }

            return false;
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime;

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            switch (movePhase)
            {
                case MovePhase.MAP:
                    if (!tileDrawn)
                    {
                        MapTile nextTile = MapDeck.Peek();
                        while (!CanBePlaced(nextTile) && newTile.GetName() != "Helipad")
                        {
                            newTile = MapDeck.Draw();
                        }
                    }

                    newTile.MoveTo(GetClosestGridPoint(Mouse.GetState().X, Mouse.GetState().Y));

                    if (Mouse.GetState().RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                    {
                        newTile.Rotate();
                    }

                    Point gridIndices = GetClosestGridIndex(newTile.GetRect().X, newTile.GetRect().Y);
                    bool isLegal = IsLegalPlacement(gridIndices.X, gridIndices.Y, newTile);

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (isLegal)
                        {
                            movePhase = MovePhase.TOKENS;
                            tiles.Add(newTile);

                            newTile = null;
                        }
                    }

                    break;

                case MovePhase.TOKENS:
                    break;
            }

            prevMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix matrix = Matrix.CreateScale(screenWidth / 1280);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, matrix);

            foreach (MapTile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }

            if (newTile != null)
            {
                newTile.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
