using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Models
{
    public class OutputCls
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
