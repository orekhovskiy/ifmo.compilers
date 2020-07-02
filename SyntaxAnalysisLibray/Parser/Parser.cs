using CommonsLibrary;
using CommonsLibrary.Exceptions;
using SyntaxAnalysisLibray.Lexer;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal = SyntaxAnalysisLibray.Lexer.TokenType;

namespace SyntaxAnalysisLibray.Parser
{
    public static class Parser
    {
        private static List<Token> s_tokens;
        private static Token s_token;
        private static int s_tokensIterator;
        private static Stack<int> s_tokensIteratorStack;

        public static object Parse(List<Token> tokens)
        {
            PrepareToRead(tokens);
            var result = GetSymbol(NonTerminal.Root);
            if (result.Success)
            {
                var element = result.Value;
                return element;
            }
            else
            {
                throw new NoSuitableParseTreeException();
            }
        }

        private static Result GetSymbol(object symbol)
        {
            Result result = default;
            switch (symbol)
            {
                case Terminal terminal:
                    NextToken();
                    result = GetTerminal(terminal);
                    break;
                case NonTerminal nonTerminal:
                    result = GetNonTerminal(nonTerminal);
                    break;
            }
            return result;
        }

        private static Result GetTerminal(Terminal requestedSymbol)
        {
            if (requestedSymbol == s_token.Type)
            {
                return new Result(true, s_token.Content);
            }
            else
            {
                return new Result(false);
            }
        }

        private static Result GetNonTerminal(NonTerminal requestedSymbol)
        {
            if (requestedSymbol == NonTerminal.Subexpression)
            {

            }
            var productions = Grammar.Rules[requestedSymbol];
            foreach (var production in productions)
            {
                SavePosition();
                var productionMatches = true;
                var elements = new List<string>();
                foreach (var symbol in production)
                {
                    var result = GetSymbol(symbol);
                    if (!result.Success)
                    {
                        productionMatches = false;
                        break;
                    }
                }
                if (productionMatches)
                {
                    return new Result(true, elements);
                }
                RestorePosition();
            }
            return new Result(false);
        }

        private static void PrepareToRead(List<Token> tokens)
        {
            s_tokens = tokens;
            s_tokensIterator = 0;
            s_tokensIteratorStack = new Stack<int>();
        }

        private static void SavePosition()
        {
            s_tokensIteratorStack.Push(s_tokensIterator);
        }

        private static void RestorePosition()
        {
            s_tokensIterator = s_tokensIteratorStack.Pop();
            s_token = s_tokens[s_tokensIterator];
        }

        private static void NextToken()
        {
            s_token = s_tokens[s_tokensIterator];
            s_tokensIterator++;
        }
    }
}
