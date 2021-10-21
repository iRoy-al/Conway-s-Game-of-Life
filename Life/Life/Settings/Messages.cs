using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    static class Messages
    {
        /// <summary>
        /// Writes error messages in red in the console
        /// </summary>
        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes Success messages in green in the console
        /// </summary>
        public static void Success(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
