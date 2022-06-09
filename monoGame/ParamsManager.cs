using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame
{
    public enum GameMode
    {
        DEBUG,
        NORMAL
    }
    public static class ParamsManager
    {
        public static GameMode gameMode = GameMode.NORMAL;
    }
}
