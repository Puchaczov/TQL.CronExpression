using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Exceptions
{
    public class UnknownSegmentException : Exception
    {
        private readonly int v;

        public UnknownSegmentException(int v)
        {
            this.v = v;
        }
    }
}
