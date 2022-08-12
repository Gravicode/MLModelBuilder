using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Helpers
{
    public class EventLogger:ILogger
    {
        public EventHandler<string> PrintData;

        public ConsoleColor ForegroundColor { get; set; }= ConsoleColor.Black;

      
        public bool IsReady()
        {
            return PrintData != null;
        }

        public void ReadKey()
        {
            //do nothing
        }

        public void WriteLine()
        {
            PrintData?.Invoke(null, Environment.NewLine);
        }

        public void WriteLine(string Message)
        {
            PrintData?.Invoke(null, Message);
        }

        public void WriteLine(string Format, params string[] args)
        {
            var Msg = string.Format(Format, args);
            PrintData?.Invoke(null, Msg);
        }
    }
}
