using System.Diagnostics;

namespace Cron.Core.Tokens
{
    /// <summary>
    /// This class allows to store association between source text and parsed tree.
    /// Every single token must contains TextSpan to allow determine which part of source it concers
    /// </summary>
    [DebuggerDisplay("Start: {Start}, Length: {Length}, End: {End}")]
    public struct TextSpan
    {

        public TextSpan(int start, int lenght)
        {
            this.Start = start;
            this.Length = lenght;
        }

        /// <summary>
        /// Returns end of span string.
        /// </summary>
        public int End => Start + Length;

        /// <summary>
        /// Lenght of span text.
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// Point somewhere in source code.
        /// </summary>
        public int Start { get; }

        public static bool operator !=(TextSpan left, TextSpan right) => !(left == right);

        public static bool operator ==(TextSpan left, TextSpan right)
        {
            if (object.Equals(left, null))
            {
                return object.Equals(right, null);
            }
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is TextSpan)
            {
                var span = (TextSpan)obj;
                return span.Start == this.Start && span.Length == this.Length;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode() => Start.GetHashCode() ^ Length.GetHashCode();
    }

    /// <summary>
    /// This class contains usefull extensions to handle TextSpans
    /// </summary>
    public static class TextSpanHelper
    {
        public static TextSpan Clone(this TextSpan textSpan) => new TextSpan(textSpan.Start, textSpan.Length);
    }
}
