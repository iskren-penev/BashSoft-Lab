namespace BashSoft.IO
{
    using System;
    using Static_data;

    public static class InputReader
    {
        private const string EndCommand = "quit";

        public static void StartReadingCommands()
        {
            OutputWriter.WriteMessage($"{SessionData.currentPath}> ");
            string input = Console.ReadLine();
            input = input.Trim();

            while (!input.Equals(EndCommand))
            {
                CommandInterpreter.InterpretCommand(input);
                OutputWriter.WriteMessage($"{SessionData.currentPath}> ");
                input = Console.ReadLine();
                input = input.Trim();
            }
        }
    }
}
