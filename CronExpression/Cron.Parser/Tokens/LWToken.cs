namespace Cron.Parser.Tokens
{
    public class LWToken : Token
    {
        public LWToken(TextSpan span)
            : base("LW", Enums.TokenType.LW, span)
        { }

        public override Token Clone() => new LWToken(Span.Clone());
    }
}
