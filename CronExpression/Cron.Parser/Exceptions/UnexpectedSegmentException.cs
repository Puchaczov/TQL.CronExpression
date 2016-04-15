using Cron.Parser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnexpectedSegmentException : Exception
    {

        public UnexpectedSegmentException(Segment segment)
        {
            this.Segment = segment;
        }
        public Segment Segment
        {
            get;
        }
    }
}
