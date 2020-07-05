using SyntaxAnalysisLibray.Lexer;
using SyntaxAnalysisLibray.Parser;
using System;

namespace ifmo.compilers
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "pseudocode.txt";
            var code = IOManager.ReadFile(fileName);
            var splited = code.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var tokenizedCode = Lexer.Tokenize(code);
            var parsedCode = Parser.Parse(tokenizedCode);
        }
    }
}
