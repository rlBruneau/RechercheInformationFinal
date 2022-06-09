using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.States.GeneralStates
{
    public class MenuState : StateBase
    {
        private static object lockObject = new object();
        private static MenuState instance = null;
        public static MenuState Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                        instance = new MenuState();
                    return instance;
                }
            }
        }

        private MenuState() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
