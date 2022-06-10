using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Actors.Movables.Players;
using monoGame.Cameras;
using monoGame.SoundManagers;
using monoGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Actors.Statics
{
    public class Coin : ActorBase, ICollidable
    {
        public Coin(Sprite sprite, Vector2 position) : base(sprite, position)
        {
            ScaleBaseMOdificator = 2;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            DrawBase(gameTime, spriteBatch, camera);
        }

        public override void Emit(ActorBase actor)
        {
            ToDelete = true;
            SoundManager.Instance.PlaySoundEffect(SoundEffects.Coin);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateFrame(gameTime);
        }
    }
}
