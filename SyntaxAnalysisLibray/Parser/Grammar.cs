using System;
using System.Collections.Generic;
using System.Text;
using Terminal = SyntaxAnalysisLibray.Lexer.TokenType;

namespace SyntaxAnalysisLibray.Parser
{
    static class Grammar
    {
        public static Dictionary<NonTerminal, List<List<object>>> Rules { get; } = new Dictionary<NonTerminal, List<List<object>>>();

        static Grammar()
        {
            Rules.Add(NonTerminal.Root, new List<List<object>>
            {
                new List<object>{NonTerminal.Element, Terminal.EOF}
            });
            Rules.Add(NonTerminal.Element, new List<List<object>>
            {
                new List<object>{Terminal.Number},
                new List<object>{NonTerminal.Pattern},
                new List<object>{NonTerminal.Expression},
                new List<object>{Terminal.Symbol}
            });
            Rules.Add(NonTerminal.Pattern, new List<List<object>>
            {
                new List<object>{Terminal.Symbol, Terminal.Underscores, Terminal.Symbol},
                new List<object>{Terminal.Symbol, Terminal.Underscores}
            });
            Rules.Add(NonTerminal.Expression, new List<List<object>>
            {
                new List<object>{Terminal.Symbol, Terminal.LeftBracket, NonTerminal.Operands, Terminal.RightBracket}
            });
            Rules.Add(NonTerminal.Operands, new List<List<object>>
            {
                new List<object>{NonTerminal.Element, NonTerminal.Continuation}
            });
            Rules.Add(NonTerminal.Continuation, new List<List<object>>
            {
                new List<object>{Terminal.Comma, NonTerminal.Operands},
                new List<object>{}
            });
        }
    }
}
