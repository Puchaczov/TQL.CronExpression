using System;
using Cron.Utils.Filters;

namespace Cron
{
    public class ReplaceChar : FilterBase<string>
    {
        private char sourceChar;
        private char destinationChar;

        public ReplaceChar(char source, char destination)
        {
            this.sourceChar = source;
            this.destinationChar = destination;
        }

        protected override string Process(string input) => input.Replace(sourceChar, destinationChar);
    }
}