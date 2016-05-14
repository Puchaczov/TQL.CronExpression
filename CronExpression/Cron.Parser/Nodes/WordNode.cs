﻿using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;
using Cron.Parser.Enums;
using Cron.Parser.Extensions;

namespace Cron.Parser.Nodes
{
    public class WordNode : LeafNode
    {
        private readonly Token token;

        public WordNode(Token token)
            : base(token)
        {
            this.token = token;
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => Token.Value;
    }
}
