namespace Cron.Parser.Tokens
{
    public class StarToken : Token
    {
        public StarToken(TextSpan span)
            : base("*", Enums.TokenType.Star, span)
        { }

        public override Token Clone() => new StarToken(Span.Clone());
    }
}
