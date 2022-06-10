using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Cameras;
using monoGame.Sprites;
using monoGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Actors
{
    public abstract class ActorBase: ICollidable
    {
        protected int Frame { get; set; }
        protected bool FacingRight { get; set; } = true;
        private SpriteEffects Flip { get => FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally; }
        public float Scale { get; set; } = 1;
        public bool IsMovable { get; set; }
        protected float ScaleBaseMOdificator { get; set; } = 1;
        public bool ToDelete { get; set; } = false;
        public float TotalScale
        {
            get => Scale * ScaleBaseMOdificator;
        }
        protected double AnimationSpeedAccumulator { get; set; }
        protected float AnimationSpeed { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; protected set; }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera);
        public abstract void Update(GameTime gameTime);
        private List<ActorBase> Colliders { get; set; } = new List<ActorBase>();
        public bool IsVisible { get; set; } = true;

        protected ActorBase(Sprite sprite, Vector2 position)
        {
            IsMovable = false;
            Position = position;
            Sprite = sprite;
            AnimationSpeed = Sprite.AnimationSpeed;
            Frame = 0;
        }

        protected void UpdateFrame(GameTime gameTime)
        {
            if (AnimationSpeed > 0)
            {
                AnimationSpeedAccumulator += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (AnimationSpeed <= AnimationSpeedAccumulator)
                {
                    Frame = (Frame + 1) % Sprite.SpriteFrames.Length;
                    AnimationSpeedAccumulator -= AnimationSpeed;
                }
            }
        }

        protected virtual void DrawBase(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            if (IsVisible)
            {
                if (Frame >= Sprite.SpriteFrames.Length)
                {
                    Frame = 0;
                }
                if (camera == null)
                {
                    spriteBatch.Draw(Sprite.Texture, Position, Sprite.SpriteFrames[Frame], Color.White, 0f, new Vector2(), TotalScale, Flip, 0f);
                    if (ParamsManager.gameMode == GameMode.DEBUG)
                    {
                        foreach (Rectangle rectangle in Sprite.CollisionRectangle)
                        {
                            spriteBatch.Draw(SpriteService.Instance.Textures[Textures.RedBackground], GetCollisionRectPosition(rectangle), new Color(Color.Blue, 75));
                        }
                    }
                }
                else
                {
                    spriteBatch.Draw(Sprite.Texture, Vector2.Add(Position, camera.Offset), Sprite.SpriteFrames[Frame], Color.White, 0f, new Vector2(), TotalScale, Flip, 0f);
                    if (ParamsManager.gameMode == GameMode.DEBUG)
                    {
                        foreach (Rectangle rectangle in Sprite.CollisionRectangle)
                        {
                            Rectangle collision = GetCollisionRectPosition(rectangle);
                            Rectangle drawCollision = new Rectangle(collision.X + (int)camera.Offset.X, collision.Y + (int)camera.Offset.Y, collision.Width, collision.Height);
                            spriteBatch.Draw(SpriteService.Instance.Textures[Textures.RedBackground], drawCollision, new Color(Color.Blue, 75));
                        }
                    }
                }
            }
        }

        protected Rectangle GetCollisionRectPosition(Rectangle collisionRectangle)
        {
            return new Rectangle((int)(collisionRectangle.X + Position.X), (int)(collisionRectangle.Y + Position.Y), (int)(collisionRectangle.Width * TotalScale), (int)(collisionRectangle.Height * TotalScale));
        }
        public virtual void ManageSpriteSpeedBased() {}

        public virtual void IsColliding(ActorBase actor)
        {
            foreach(ActorBase act in Colliders)
            {
                if (Position.X + (Sprite.Width * TotalScale) >= act.Position.X && Position.X <= act.Position.X + (act.Sprite.Width * TotalScale) && Position.Y <= act.Position.Y + (act.Sprite.Height * TotalScale) && Position.Y + (Sprite.Height * TotalScale) >= act.Position.Y)
                {
                    act.Emit(this);
                    Emit(this);
                }
            }
        }
        public virtual void Subscribe(ActorBase actor)
        {
            Colliders.Add(actor);
        }
        public abstract void Emit(ActorBase actor);
    }
}
