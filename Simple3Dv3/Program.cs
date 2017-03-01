using System;

namespace GameOn
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameOn game = new GameOn())
            {
                game.Run();
            }
        }
    }
}

