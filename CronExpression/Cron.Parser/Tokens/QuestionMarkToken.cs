namespace Cron.Parser.Tokens
{
    public class QuestionMarkToken : Token
    {
        public QuestionMarkToken(TextSpan span)
            : base("?", Enums.TokenType.QuestionMark, span)
        { }

        public override Token Clone() => new QuestionMarkToken(Span.Clone());
    }
}
