using System;
using System.Collections.Generic;
using System.Text;

namespace Weather.ConsoleApp
{
    public interface IConsoleWrapper
    {
        void WriteLine(string text);
        string ReadLine();
    }
}
