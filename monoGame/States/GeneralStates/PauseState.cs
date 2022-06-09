using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.States.GeneralStates
{
    public class PauseState: StateBase
    {
        private static object lockObj = new object();
        private static PauseState instance = null;
        public static PauseState Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new PauseState();
                    return instance;
                }
            }
        }

        private PauseState() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            GameState.Instance.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(SpriteService.Instance.Textures[Textures.RedBackground], new Rectangle(0, 0, monoGameProjectManager.WindowWidth, monoGameProjectManager.WindowHeight), new Color(Color.White, 100));
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
