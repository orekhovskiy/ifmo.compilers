using SyntaxAnalysisLibray.Lexer;
using System;

namespace ifmo.compilers
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "pseudocode.txt";
            var code = IOManager.ReadFile(fileName);
            var substr = code.Substring(110);
            var tocenizedCode = Lexer.Tokenize(code);
        }
    }
}
