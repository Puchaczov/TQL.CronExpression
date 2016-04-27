namespace Cron.Parser.Tokens
{
    public class WhiteSpaceToken : Token
    {
        public WhiteSpaceToken(TextSpan span)
            : base(" ", Enums.TokenType.WhiteSpace, span)
        { }

        public override Token Clone() => new WhiteSpaceToken(Span.Clone());
    }
}
