namespace Cron.Parser.Tokens
{
    public class LToken : Token
    {
        public LToken(TextSpan span)
            : base("L", Enums.TokenType.L, span)
        { }

        public override Token Clone() => new LToken(Span.Clone());
    }
}
