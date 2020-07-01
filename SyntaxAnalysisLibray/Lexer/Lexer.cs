using CommonsLibrary;
using CommonsLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntaxAnalysisLibray.Lexer
{
    public static class Lexer
    {
        public static List<Token> Tokenize(string str)
        {
            var tokens = new List<Token>();
            for (var i = 0; i < str.Length; i++)
            {
                var result = GetToken(str.Substring(i));
                if (result.Success)
                {
                    var token = (Token)result.Value;
                    tokens.Add(token);
                    i += token.Content.Length - 1;
                }
                else
                {
                    throw new NoSuitableTokenException(str.Substring(i));
                }
            }
            tokens.Add(new Token(TokenType.EOF, ""));
            return tokens;
        }

        private static Result GetToken(string str)
        {
            foreach (var (tokenType, tokenDefinition) in Grammar.TokenDefinitions)
            {
                var match = (new Regex(tokenDefinition)).Match(str);
                if (match.Success)
                {
                    return new Result(true, new Token(tokenType, match.Value));
                }
            }
            return new Result(false);
        }
    }
}
