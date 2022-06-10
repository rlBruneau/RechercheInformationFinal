using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Cameras;
using monoGame.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.TileMaps
{
    public class Tile
    {
        public Rectangle Value { get; private set; }
        public bool IsSolid { get; set; }
        public float AppliedFriction { get; set; }
        public Tile(Rectangle tileRectangle, bool isSolid = false)
        {
            Value = tileRectangle;
            IsSolid = isSolid;
        }
    }

    public class TileMap
    {
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public Rectangle SolidCollisionRectangle { get; set; }
        public List<Tile> Tiles { get; set; }
        private Texture2D Texture { get; set; }
        public int[,] Map { get; set; }
        private int Scale { get; set; } = 1;
        public TileMap(Texture2D texture, int tileWidth, int tileHeight, Rectangle solidColisionRectangle, int scale)
        {
            Scale = scale;
            Texture = texture;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            SolidCollisionRectangle = solidColisionRectangle;
        }

        public Tile GetTile(int posX, int posY)
        {
            if((posX/Scale)/TileWidth < Map.GetLength(1) && (posY/Scale)/TileHeight < Map.GetLength(0) && posX >= 0 && posY >= 0)
            {
                return Tiles[Map[(posY/Scale) / TileHeight, (posX/Scale) / TileWidth]];
            }
            return null;
        }

        public void AddTilesRange(int x, int y, int nbTiles, int offset, bool isSolid = false, bool isVertical = false)
        {
            if (Tiles == null)
            {
                Tiles = new List<Tile>();
            }

            for(int i = 0; i < nbTiles; i++)
            {
                Rectangle tileRect = new Rectangle(x, y, TileWidth, TileHeight);
                Tiles.Add(new Tile(tileRect, isSolid));

                x = !isVertical ? x + TileWidth + ((i + 1) * offset) : x;
                y = isVertical ? y + TileHeight + ((i + 1) * offset) : y;
                
                
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            int x = 0;
            int y = 0;
            for (int Y=0; Y < Map.GetLength(0); Y++)
            {
                for(int X=0; X<Map.GetLength(1); X++)
                {
                    if (camera == null || (x >= Math.Abs(camera.Offset.X) - (TileWidth * Scale) && y >= Math.Abs(camera.Offset.Y) - (TileHeight * Scale) && x <= Math.Abs(camera.Offset.X) + monoGameProjectManager.WindowWidth && y <= Math.Abs(camera.Offset.Y) + monoGameProjectManager.WindowHeight) && Map[Y,X] >= 0)
                    {
                        Rectangle tilePos = new Rectangle((int)(x + (camera != null ? camera.Offset.X : 0)), (int)(y + (camera != null ? camera.Offset.Y : 0)), TileWidth * Scale, TileHeight * Scale);
                        spriteBatch.Draw(Texture, tilePos, Tiles[Map[Y, X]].Value, Color.White);

                        if (ParamsManager.gameMode == GameMode.DEBUG && GetTile(x, y).IsSolid)
                        {
                            Rectangle collision = GetSolidCollisionRectPos(x, y);
                            Rectangle drawCollision = new Rectangle(collision.X + (int)camera.Offset.X, collision.Y + (int)camera.Offset.Y, collision.Width, collision.Height);
                            spriteBatch.Draw(SpriteService.Instance.Textures[Textures.RedBackground], drawCollision, Color.White);
                        }
                    }
                    x += TileWidth * Scale;
                }
                x = 0;
                y += TileHeight * Scale;
            }
        }

        public Rectangle GetSolidCollisionRectPos(int x, int y)
        {
            return new Rectangle(x + (((TileWidth * Scale) - (SolidCollisionRectangle.Width * Scale)) / 2), y + (((TileHeight * Scale) - (SolidCollisionRectangle.Height * Scale)) / 2), SolidCollisionRectangle.Width * Scale, SolidCollisionRectangle.Height * Scale);
        }
    }
}
