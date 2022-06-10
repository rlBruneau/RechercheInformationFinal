using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Cameras;
using monoGame.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Actors.Statics
{
    public class EndGame : ActorBase, ICollidable
    {
        public EndGame(Sprite sprite, Vector2 position) : base(sprite, position)
        {
            IsVisible = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            DrawBase(gameTime, spriteBatch, camera);
        }

        public override void Emit(ActorBase actor)
        {
            StateManager.Instance.RestartGame();
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
