namespace Cron.Parser.Tokens
{
    public class RangeToken : Token
    {
        public RangeToken(TextSpan span)
            : base("-", Enums.TokenType.Range, span)
        { }

        public override Token Clone() => new RangeToken(Span.Clone());
    }
}
