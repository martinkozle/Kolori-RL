using System.Diagnostics;
using System;
using System.Linq;

namespace GJP2021.Sources
{
    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Debug.WriteLine($"Args: {string.Join(" ", args)}");
            var rl = args.Contains("--rl");
            var port = args.Contains("--port")
                ? int.Parse(args[args.ToList().IndexOf("--port") + 1])
                : 5556;
            using var game = new Kolori(rl, port);
            game.Run();
        }
    }
}
