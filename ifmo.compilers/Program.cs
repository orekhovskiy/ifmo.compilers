using SyntaxAnalysisLibray.Lexer;
using SyntaxAnalysisLibray.Parser;
using System;

namespace ifmo.compilers
{
    class Program
    {
        static void Main(params string[] args)
        {
            var fileName = args[0];
            var code = IOManager.ReadFile(fileName);
            var tokenizedCode = Lexer.Tokenize(code);
            var postfixCode = PrefixMaker.MakePrefix(tokenizedCode);
            var parsedCode = Parser.Parse(postfixCode);
        }
    }
}
