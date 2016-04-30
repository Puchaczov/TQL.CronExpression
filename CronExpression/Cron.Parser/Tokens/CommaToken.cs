namespace Cron.Parser.Tokens
{
    public class CommaToken : Token
    {
        public CommaToken(TextSpan span)
            : base(",", Enums.TokenType.Comma, span)
        { }

        public override Token Clone() => new CommaToken(this.Span.Clone());
    }
}
