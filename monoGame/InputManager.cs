using Microsoft.Xna.Framework.Input;
using monoGame.Commands;
using monoGame.States;
using monoGame.States.GeneralStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame
{
    public class InputManager
    {
        private static object lockObj = new object();
        private static InputManager instance = null;
        public static InputManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if(instance == null)
                    {
                        instance = new InputManager();
                    }
                    return instance;
                }
            }
        }
        private InputManager() { }

        private KeyboardState CurrentKeyboardState { get; set; }
        private KeyboardState LastKeyboardState { get; set; }
        public CommandInput MoveLeft { get; set; }
        public CommandInput MoveRight { get; set; }
        public CommandInput Jump { get; set; }
        public CommandInput StopMove { get; set; }

        public void KeyboardInput()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyDown(Keys.A))
            {
                MoveLeft.Execute();
            }
            if (CurrentKeyboardState.IsKeyDown(Keys.D))
            {
                MoveRight.Execute();
            }
            if (CurrentKeyboardState.IsKeyDown(Keys.W) && LastKeyboardState.IsKeyUp(Keys.W))
            {
                Jump.Execute();
            }
            if (CurrentKeyboardState.IsKeyDown(Keys.P) && LastKeyboardState.IsKeyUp(Keys.P))
            {
                StateManager.Instance.SetCurrentState(StateManager.Instance.CurrentState == GameState.Instance ? StateEnum.PAUSE : StateEnum.GAME);
            }
            if (CurrentKeyboardState.IsKeyDown(Keys.D1) && LastKeyboardState.IsKeyUp(Keys.D1))
            {
                ParamsManager.gameMode = ParamsManager.gameMode == GameMode.DEBUG ? GameMode.NORMAL : GameMode.DEBUG;
            }
        }
    }
}
