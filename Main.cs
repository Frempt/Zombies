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
        private SpriteFont font;
        private MouseState prevMouseState;

        public enum MovePhase { MAP = 0, TOKENS = 1, COMBAT = 2, DRAWCARDS = 3, MOVEPLAYER = 4, MOVEZOMBIES = 5, DISCARD = 6 };
        private MovePhase movePhase = MovePhase.MAP;

        private List<MapTile> tiles = new List<MapTile>();
        private MapTile newTile;

        private Frempt.Grid gridMap;

        private bool tileDrawn = false;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;

            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            gridMap = new Frempt.Grid(new Point(0, 0), 5, 5, 1000, 1000);

            MapDeck.Shuffle();
            newTile = MapDeck.Draw();
            newTile.MoveTo(gridMap.GetClosestGridPoint(gridMap.GetWidth() / 2, gridMap.GetWidth() / 2));
            newTile.SetLegality(true);
            tiles.Add(newTile);
            newTile = null;

            prevMouseState = Mouse.GetState();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Frempt.TextureLibrary.LoadTextures(Content);

            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
        }

        protected MapTile GetTileAtGridPoint(int gridIndexX, int gridIndexY)
        {
            foreach (MapTile tile in tiles)
            {
                if (tile.GetRect().X == gridMap.GetGridPosX(gridIndexX) && tile.GetRect().Y == gridMap.GetGridPosY(gridIndexY))
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

            if (!gridMap.IsFirstX(gridIndexX))
            {
                MapTile left = GetTileAtGridPoint(gridIndexX - 1, gridIndexY);
                if (left != null && ((left.HasRightConnection() && !tile.HasLeftConnection()) || (!left.HasRightConnection() && tile.HasLeftConnection())))
                {
                    return false;
                }
            }

            if(!gridMap.IsLastX(gridIndexX))
            {
                MapTile right = GetTileAtGridPoint(gridIndexX + 1, gridIndexY);
                if (right != null && ((right.HasLeftConnection() && !tile.HasRightConnection()) || (right.HasRightConnection() && !tile.HasLeftConnection())))
                {
                    return false;
                }
            }

            if (!gridMap.IsFirstY(gridIndexY))
            {
                MapTile up = GetTileAtGridPoint(gridIndexX, gridIndexY - 1);
                if (up != null && ((up.HasDownConnection() && !tile.HasUpConnection()) || (!up.HasDownConnection() && tile.HasUpConnection())))
                {
                    return false;
                }
            }

            if (!gridMap.IsLastY(gridIndexY))
            {
                MapTile down = GetTileAtGridPoint(gridIndexX, gridIndexY + 1);
                if (down != null && ((down.HasUpConnection() && !tile.HasDownConnection()) || (!down.HasUpConnection() && tile.HasDownConnection())))
                {
                    return false;
                }
            }

            return true;
        }

        protected bool CanBePlaced(MapTile tile)
        {
            if (tile != null)
            {
                for (int i = 0; i < gridMap.GetPortionCountX(); i++)
                {
                    for (int j = 0; j < gridMap.GetPortionCountY(); j++)
                    {
                        if (IsLegalPlacement(i, j, tile))
                        {
                            return true;
                        }

                        newTile.Rotate();

                        if (IsLegalPlacement(i, j, tile))
                        {
                            return true;
                        }

                        newTile.Rotate();

                        if (IsLegalPlacement(i, j, tile))
                        {
                            return true;
                        }

                        newTile.Rotate();

                        if (IsLegalPlacement(i, j, tile))
                        {
                            return true;
                        }
                    }
                }
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
                        newTile = MapDeck.Draw();
                        tileDrawn = true;
                    }

                    newTile.MoveTo(gridMap.GetClosestGridPoint(Mouse.GetState().X, Mouse.GetState().Y));

                    if (Mouse.GetState().RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                    {
                        newTile.Rotate();
                    }

                    Point gridIndices = gridMap.GetClosestGridIndex(newTile.GetRect().X, newTile.GetRect().Y);
                    bool isLegal = false;
                    //if (!isLegal) isLegal = MapDeck.Peek() == null;
                    //if(!isLegal) isLegal = (CanBePlaced(MapDeck.Peek()) || newTile.GetName() == "Helipad");
                    isLegal = IsLegalPlacement(gridIndices.X, gridIndices.Y, newTile);

                    newTile.SetLegality(isLegal);

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (isLegal)
                        {
                            movePhase = MovePhase.TOKENS;
                            tileDrawn = false;
                            tiles.Add(newTile);

                            newTile = null;
                        }
                    }

                    break;

                case MovePhase.TOKENS:
                    movePhase = MovePhase.MAP;
                    break;
            }

            prevMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw objects which aren't being scaled here
            spriteBatch.Begin();

            gridMap.DrawGrid(spriteBatch, 1.5f, Color.Black, GraphicsDevice);

            spriteBatch.End();

            Matrix matrix = Matrix.CreateScale(screenWidth / 1280, screenHeight / 1024, 1.0f);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, matrix);
            foreach (MapTile tile in tiles)
            {
                tile.Draw(spriteBatch, font);
            }

            if (newTile != null)
            {
                newTile.Draw(spriteBatch, font);
            }

            spriteBatch.DrawString(font, "Phase = " + movePhase, new Vector2(screenWidth - screenWidth / 5, screenHeight / 20), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
