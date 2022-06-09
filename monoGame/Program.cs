using System;

namespace monoGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new monoGameProjectManager())
                game.Run();
        }
    }
}
