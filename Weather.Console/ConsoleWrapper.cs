using System;
using System.Collections.Generic;
using System.Text;

namespace Weather.ConsoleApp
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
