using Microsoft.Xna.Framework;
using monoGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace monoGame.Actors.Movables
{
    public abstract class Movable: ActorBase
    {
        protected float MaxXSpeed { get; set; } = 5;
        protected float MaxYSpeed { get; set; } = 10;
        protected float Friction { get; set; }
        protected static float GravityAcceleration { get; set; } = 2f;
        public Vector2 Speed { get; protected set; } = Vector2.Zero;
        protected Vector2 Acceleration { get; set; }
        protected float Velocity { get; set; }
        protected bool IsPhysicable { get; set; }
        protected Movable(Sprite sprite, Vector2 position, bool isPhysicable = false, float normalFriction = 0.8f) : base(sprite, position) 
        {
            IsMovable = true;
            IsPhysicable = isPhysicable;
            Friction = normalFriction;
        }

        protected void AddVector(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            if(IsPhysicable)
            {
                Acceleration = Vector2.Add(Acceleration, new Vector2(0, GravityAcceleration * delta));

                Vector2 FrictionAccel;
                if (Speed.X > 0.5)
                {
                    FrictionAccel = new Vector2(-Friction, 0);
                }
                else if(Speed.X < -0.5)
                {
                    FrictionAccel = new Vector2(Friction, 0);
                }
                else
                {
                    FrictionAccel = Vector2.Zero;
                    Speed = new Vector2(0, Speed.Y);
                }
                Acceleration = Vector2.Add(Acceleration, FrictionAccel);
            }
            Acceleration = Vector2.Add(Acceleration, Vector2.Multiply(Acceleration, delta));

            Speed = Vector2.Add(Speed, Acceleration);

            if(Math.Abs(Speed.X) > MaxXSpeed)
            {
                Speed = new Vector2(Speed.X > 0 ? MaxXSpeed : -MaxXSpeed, Speed.Y);
            }
            if (Math.Abs(Speed.Y) > MaxYSpeed)
            {
                Speed = new Vector2(Speed.X, Speed.Y > 0 ? MaxYSpeed : -MaxYSpeed);
            }


            Position = Vector2.Add(Position, Speed);
            Acceleration = Vector2.Zero;
        }

        public void TestSolidTileCollision(TileMap tileMap)
        {
            bool isColiding = false;
            int nbColliderRect = 0;


            while (!isColiding && nbColliderRect < Sprite.CollisionRectangle.Count)
            {
                Rectangle collisionRect = GetCollisionRectPosition(Sprite.CollisionRectangle[nbColliderRect]);
                Tile bottomLeftTile = tileMap.GetTile(collisionRect.X, collisionRect.Bottom);
                Tile bottomRightTile = tileMap.GetTile(collisionRect.Right, collisionRect.Bottom);
                Tile topLeftTile = tileMap.GetTile(collisionRect.X, collisionRect.Y);
                Tile topRightTile = tileMap.GetTile(collisionRect.Right, collisionRect.Y);

                if(bottomLeftTile != null && bottomRightTile != null && bottomLeftTile.Value == bottomRightTile.Value && bottomRightTile.IsSolid)
                {
                    Friction = bottomRightTile.AppliedFriction;
                }

                if (bottomLeftTile != null && bottomLeftTile.IsSolid)
                {
                    if(collisionRect.Bottom <= tileMap.GetSolidCollisionRectPos(collisionRect.X, (int)(Position.Y + (Sprite.Height * TotalScale)) / tileMap.TileHeight * tileMap.TileHeight).Y)
                    {
                        Position = new Vector2(Position.X, ((int)Position.Y / tileMap.TileHeight) * tileMap.TileHeight);
                        Speed = new Vector2(Speed.X, Speed.Y > 0 ? 0 : Speed.Y);
                    }

                    collisionRect = GetCollisionRectPosition(Sprite.CollisionRectangle[nbColliderRect]);
                    bottomLeftTile = tileMap.GetTile(collisionRect.X, collisionRect.Bottom - tileMap.TileHeight/4);
                    
                    if (bottomLeftTile != null && bottomLeftTile.IsSolid && collisionRect.X >= tileMap.GetSolidCollisionRectPos(collisionRect.X / tileMap.TileWidth * tileMap.TileWidth, (collisionRect.Bottom - (tileMap.TileHeight/4)) / tileMap.TileHeight * tileMap.TileHeight).Right)
                    {
                        Position = new Vector2((int)Position.X / tileMap.TileWidth * tileMap.TileWidth + tileMap.TileWidth, Position.Y);
                        Speed = new Vector2(0, Speed.Y);
                    }
                }

                if (bottomRightTile != null && bottomRightTile.IsSolid)
                {
                    if (collisionRect.Bottom <= tileMap.GetSolidCollisionRectPos(collisionRect.X, collisionRect.Bottom / tileMap.TileHeight * tileMap.TileHeight).Y)
                    {
                        Position = new Vector2(Position.X, ((int)Position.Y / tileMap.TileHeight) * tileMap.TileHeight);
                        Speed = new Vector2(Speed.X, Speed.Y > 0 ? 0 : Speed.Y);
                    }

                    collisionRect = GetCollisionRectPosition(Sprite.CollisionRectangle[nbColliderRect]);
                    bottomRightTile = tileMap.GetTile(collisionRect.Right, collisionRect.Bottom - tileMap.TileHeight / 4);

                    if (bottomRightTile != null && bottomRightTile.IsSolid && collisionRect.Right <= tileMap.GetSolidCollisionRectPos(collisionRect.Right / tileMap.TileWidth * tileMap.TileWidth, (collisionRect.Bottom - (tileMap.TileHeight / 4)) / tileMap.TileHeight * tileMap.TileHeight).X)
                    {
                        Position = new Vector2((int)Position.X / tileMap.TileWidth * tileMap.TileWidth, Position.Y);
                        Speed = new Vector2(0, Speed.Y);
                    }
                }

                if (topRightTile != null && topRightTile.IsSolid)
                {
                    if (collisionRect.Y <= tileMap.GetSolidCollisionRectPos(collisionRect.Right, (int)(Position.Y + (Sprite.Height * TotalScale)) / tileMap.TileHeight * tileMap.TileHeight).Bottom)
                    {
                        Speed = new Vector2(Speed.X, Speed.Y < 0 ? 0 : Speed.Y);
                    }

                    collisionRect = GetCollisionRectPosition(Sprite.CollisionRectangle[nbColliderRect]);
                    topRightTile = tileMap.GetTile(collisionRect.Right, collisionRect.Y + tileMap.TileHeight / 4);

                    if (topRightTile != null && topRightTile.IsSolid && collisionRect.Right <= tileMap.GetSolidCollisionRectPos(collisionRect.Right / tileMap.TileWidth * tileMap.TileWidth, (collisionRect.Bottom + (tileMap.TileHeight / 4)) / tileMap.TileHeight * tileMap.TileHeight).X)
                    {
                        Position = new Vector2((int)Position.X / tileMap.TileWidth * tileMap.TileWidth, Position.Y);
                        Speed = new Vector2(0, Speed.Y);
                    }
                }

                if (topLeftTile != null && topLeftTile.IsSolid)
                {
                    if (collisionRect.Y <= tileMap.GetSolidCollisionRectPos(collisionRect.X, (int)(Position.Y + (Sprite.Height * TotalScale)) / tileMap.TileHeight * tileMap.TileHeight).Bottom)
                    {
                        Speed = new Vector2(Speed.X, Speed.Y < 0 ? 0 : Speed.Y);
                    }

                    collisionRect = GetCollisionRectPosition(Sprite.CollisionRectangle[nbColliderRect]);
                    topLeftTile = tileMap.GetTile(collisionRect.X, collisionRect.Y + tileMap.TileHeight / 4);

                    if (topLeftTile != null && topLeftTile.IsSolid && collisionRect.X <= tileMap.GetSolidCollisionRectPos(collisionRect.Right / tileMap.TileWidth * tileMap.TileWidth, (collisionRect.Bottom + (tileMap.TileHeight / 4)) / tileMap.TileHeight * tileMap.TileHeight).X)
                    {
                        Position = new Vector2((int)Position.X / tileMap.TileWidth * tileMap.TileWidth + tileMap.TileWidth, Position.Y);
                        Speed = new Vector2(0, Speed.Y);
                    }
                }
                nbColliderRect++;
            }
        }
    }
}
