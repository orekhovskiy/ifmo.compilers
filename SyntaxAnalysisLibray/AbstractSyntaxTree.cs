using SyntaxAnalysisLibray.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxAnalysisLibray
{
    public class AbstractSyntaxTree
    {

        public AbstractSyntaxTree(List<Token> elements)
        {
            Value = elements[0];
            ChildNodes = elements.Skip(1).ToList();
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
