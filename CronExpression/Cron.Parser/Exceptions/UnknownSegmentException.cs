using System;

namespace Cron.Parser.Exceptions
{
    public class UnknownSegmentException : Exception
    {
        private int position;

        public int Position
        {
            get
            {
                return position;
            }
        }

        public UnknownSegmentException(int position)
        {
            this.position = position;
        }

        public override string Message
        {
            get
            {
                return string.Format("Unknown segment while processing expression. Exception occured in position {0}", Position);
            }
        }
    }
}