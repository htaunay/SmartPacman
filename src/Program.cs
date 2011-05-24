using System;

namespace SmartPacMan
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SmartPacMan game = new SmartPacMan())
            {
                game.Run();
            }
        }
    }
#endif
}

