using monoGame.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Commands
{
    public class CommandInput
    {
        private Action _action;

        public CommandInput(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke();
        }
    }
}
