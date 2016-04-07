using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    /// <summary>
    /// This class allows to store association between source text and parsed tree.
    /// Every single token must contains TextSpan to allow determine which part of source it concers
    /// </summary>
    [DebuggerDisplay("Start: {Start}, Length: {Length}, End: {End}")]
    public struct TextSpan
    {
        /// <summary>
        /// Point somewhere in source code.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Lenght of span text.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Returns end of span string.
        /// </summary>
        public int End => Start + Length;

        public TextSpan(int start, int lenght)
        {
            this.Start = start;
            this.Length = lenght;
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj is TextSpan)
            {
                var span = (TextSpan)obj;
                return span.Start == this.Start && span.Length == this.Length;
            }
            return base.Equals(obj);
        }

        public static bool operator==(TextSpan left, TextSpan right)
        {
            if(object.Equals(left, null))
            {
                return object.Equals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(TextSpan left, TextSpan right) => !(left == right);

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ Length.GetHashCode();
        }
    }


    /// <summary>
    /// This class contains usefull extensions to handle TextSpans
    /// </summary>
    public static class TextSpanHelper
    {
        public static TextSpan Clone(this TextSpan textSpan)
        {
            return new TextSpan(textSpan.Start, textSpan.Length);
        }
    }
}
