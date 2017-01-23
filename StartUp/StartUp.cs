namespace StartUp
{
    using GameEngine;
    using System;
    using Utilities;

    public class StartUp
    {
        public static void Main()
        {
            ConsoleProperties.InitialConsoleSettings();

            Engine engine = new Engine();
            engine.Start();        
        }
    }
}
