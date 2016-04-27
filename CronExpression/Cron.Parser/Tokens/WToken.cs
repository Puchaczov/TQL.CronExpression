namespace Cron.Parser.Tokens
{
    public class WToken : Token
    {
        public WToken(TextSpan span)
            : base("W", Enums.TokenType.W, span)
        { }

        public override Token Clone() => new WToken(Span.Clone());
    }
}
