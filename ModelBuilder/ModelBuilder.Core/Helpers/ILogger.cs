using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Helpers
{
    public interface ILogger
    {
        void WriteLine();
        void WriteLine(string Message);
        void WriteLine(string Format,params string[] args);
        void ReadKey();
        ConsoleColor ForegroundColor { get; set; }
    }
}
