﻿using TQL.CronExpression.Parser.Nodes;

namespace TQL.CronExpression.Parser
{
    public interface INodeVisitor
    {
        void Visit(CommaNode node);
        void Visit(StarNode node);
        void Visit(SegmentNode node);
        void Visit(RootComponentNode node);
        void Visit(RangeNode node);
        void Visit(IncrementByNode node);
        void Visit(WordNode node);
        void Visit(NumberNode node);
        void Visit(QuestionMarkNode node);
        void Visit(LNode node);
        void Visit(WNode node);
        void Visit(HashNode node);
        void Visit(EndOfFileNode node);
        void Visit(NumericPrecededLNode node);
        void Visit(NumericPrecededWNode node);
        void Visit(LwNode node);
        void Visit(MissingNode node);
    }
}