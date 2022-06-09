using Microsoft.Xna.Framework;
using monoGame.States.GeneralStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.States
{
    public class StateManager
    {
        private static object lockObj = new object();
        private static StateManager instance = null;
        public static StateManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new StateManager();
                    return instance;
                }
            }
        }
        private StateManager()
        {
            SetCurrentState(StateEnum.MENU);
        }
        public StateBase CurrentState { get; private set; }

        public StateBase SetCurrentState(StateEnum state)
        {
            switch (state)
            {
                case StateEnum.GAME:
                    CurrentState = GameState.Instance;
                    break;
                case StateEnum.MENU:
                    CurrentState = MenuState.Instance;
                    break;
                case StateEnum.PAUSE:
                    CurrentState = PauseState.Instance;
                    break;
            }

            return CurrentState;
        }
    }
}
