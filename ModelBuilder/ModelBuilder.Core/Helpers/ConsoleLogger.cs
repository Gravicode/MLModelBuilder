using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Helpers
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

        public void ReadKey()
        {
            Console.ReadKey();
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string Message)
        {
            Console.WriteLine(Message);
        }

        public void WriteLine(string Format, params string[] args)
        {
            Console.WriteLine(Format, args);
        }
    }
}
