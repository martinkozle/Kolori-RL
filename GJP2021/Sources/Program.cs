using System;

namespace GJP2021.Sources
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new Kolori();
            game.Run();
        }
    }
}