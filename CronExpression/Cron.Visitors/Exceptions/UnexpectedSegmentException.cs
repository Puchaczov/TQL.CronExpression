using Cron.Parser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Exceptions
{
    public class UnexpectedSegmentException : Exception
    {
        public Segment Segment
        {
            get;
            private set;
        }

        public UnexpectedSegmentException(Segment segment)
        {
            this.Segment = segment;
        }
    }
}
