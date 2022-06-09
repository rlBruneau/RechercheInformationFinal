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
        public TileMap(Texture2D texture, int tileWidth, int tileHeight, Rectangle solidColisionRectangle)
        {
            Texture = texture;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            SolidCollisionRectangle = solidColisionRectangle;
        }

        public Tile GetTile(int posX, int posY)
        {
            if(posX/TileWidth < Map.GetLength(1) && posY/TileHeight < Map.GetLength(0) && posX >= 0 && posY >= 0)
            {
                return Tiles[Map[posY / TileHeight, posX / TileWidth]];
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
                x = !isVertical ? x + TileWidth + (i * offset) : x;
                y = isVertical ? y + TileHeight + (i * offset) : y;
                
                Tiles.Add(new Tile(tileRect, isSolid));
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
                    if (camera == null || (x >= Math.Abs(camera.Offset.X) - TileWidth && y >= Math.Abs(camera.Offset.Y) - TileHeight && x <= Math.Abs(camera.Offset.X) + monoGameProjectManager.WindowWidth && y <= Math.Abs(camera.Offset.Y) + monoGameProjectManager.WindowHeight) && Map[Y,X] >= 0)
                    {
                        Rectangle tilePos = new Rectangle((int)(x + (camera != null ? camera.Offset.X : 0)), (int)(y + (camera != null ? camera.Offset.Y : 0)), TileWidth, TileHeight);
                        spriteBatch.Draw(Texture, tilePos, Tiles[Map[Y, X]].Value, Color.White);

                        if (ParamsManager.gameMode == GameMode.DEBUG && GetTile(x, y).IsSolid)
                        {
                            Rectangle collision = GetSolidCollisionRectPos(x, y);
                            Rectangle drawCollision = new Rectangle(collision.X + (int)camera.Offset.X, collision.Y + (int)camera.Offset.Y, collision.Width, collision.Height);
                            spriteBatch.Draw(SpriteService.Instance.Textures[Textures.RedBackground], drawCollision, Color.White);
                        }
                    }
                    x += TileWidth;
                }
                x = 0;
                y += TileHeight;
            }
        }

        public Rectangle GetSolidCollisionRectPos(int x, int y)
        {
            return new Rectangle(x + ((TileWidth - SolidCollisionRectangle.Width) / 2), y + ((TileHeight - SolidCollisionRectangle.Height) / 2), SolidCollisionRectangle.Width, SolidCollisionRectangle.Height);
        }
    }
}
