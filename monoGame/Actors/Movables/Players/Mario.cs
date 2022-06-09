using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Cameras;
using monoGame.Commands;
using monoGame.SoundManagers;
using monoGame.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace monoGame.Actors.Movables.Players
{
    public class Mario : Movable
    {
        private int JumpImpulse { get; set; } = -5;
        private List<ICollidable> Collidables { get; set; } = new List<ICollidable>();
        public Mario(Sprite sprite, Vector2 position, bool isPhysicable = false, float normalFriction = 0.8f) : base(sprite, position, isPhysicable, normalFriction) 
        {
            InputManager.Instance.Jump = new CommandInput(Jump);
            InputManager.Instance.MoveLeft = new CommandInput(MoveLeftPressed);
            InputManager.Instance.MoveRight = new CommandInput(MoveRightPressed);

            //ScaleBaseMOdificator = 2f;
            Velocity = 1f;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera = null)
        {
            DrawBase(gameTime, spriteBatch, camera);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateFrame(gameTime);
            AddVector(gameTime);

        }

        public override void ManageSpriteSpeedBased()
        {
            Debug.WriteLine(Speed);

            SpriteService spriteService = SpriteService.Instance;
            if(Speed.X == 0 && Speed.Y == 0)
            {
                Sprite = spriteService.Sprites[Sprites.Sprites.MarioIdle];
                AnimationSpeed = 0;
            }
            else if(Speed.X != 0 && Speed.Y == 0)
            {
                Sprite = spriteService.Sprites[Sprites.Sprites.MarioRun];
                AnimationSpeed = 100;
            }
            else
            {
                Sprite = spriteService.Sprites[Sprites.Sprites.MarioJump];
                AnimationSpeed = 0;
            }
        }

        private void Jump()
        {
            Acceleration = new Vector2(Acceleration.X, JumpImpulse);
            Friction = 0.8f;
            SoundManager.Instance.PlaySoundEffect(SoundEffects.JumpSuper);
        }

        private void MoveLeftPressed()
        {
            Acceleration = new Vector2(-Velocity, Acceleration.Y);
            FacingRight = false;
        }

        private void MoveRightPressed()
        {
            Acceleration = new Vector2(Velocity, Acceleration.Y);
            FacingRight = true;
        }

        public override bool IsColliding(ActorBase actor)
        {
            bool isColiding = false;
            int nbColliderRect = 0;

            while (!isColiding && nbColliderRect < Sprite.CollisionRectangle.Count)
            {
                nbColliderRect++;
            }

            return isColiding;
        }

        public override void Subscribe(ActorBase actor)
        {
            Collidables.Add(actor);
        }

        public override void Emit()
        {
            throw new NotImplementedException();
        }
    }
}
