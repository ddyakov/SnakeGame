namespace Utilities
{
    using System;

    public static class ConsoleProperties
    {
        public static void InitialConsoleSettings()
        {
            Console.CursorVisible = false;

            Console.WindowHeight = 15;

            Console.WindowWidth = 60;

            Console.BufferHeight = Console.WindowHeight;

            Console.BufferWidth = Console.WindowWidth;
        }
    }
}
