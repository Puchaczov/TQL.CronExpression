using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    /// <summary>
    /// This class allows to store association between source text and parsed tree.
    /// Every single token must contains TextSpan to allow determine which part of source it concers
    /// </summary>
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
