using SyntaxAnalysisLibray.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifmo.compilers
{
    class AbstractSyntaxTree
    {
        public AbstractSyntaxTree(Token value)
        {
            Value = Value;
            ChildNodes = new List<Token>();
        }

        public AbstractSyntaxTree(Token value, List<Token> childNodes)
        {
            Value = value;
            ChildNodes = childNodes;
        }

        public AbstractSyntaxTree(Token value, params Token[] childNodes)
        {
            Value = value;
            ChildNodes = childNodes.ToList();
        }

        public Token Value { get; set; }
        public List<Token> ChildNodes { get; set; }
    }
}
