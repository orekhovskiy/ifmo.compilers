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

        public static IElement Parse(List<Token> tokens)
        {
            PrepareToRead(tokens);
            var result = GetSymbol(NonTerminal.Root);
            if (result.Success)
            {
                var element = (IElement)result.Value;
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
            switch (result.Success, symbol)
            {
                case (false, _):
                    return result;
                case (_, NonTerminal.Expression):
                    return new Result(true, BuildExpression((List<IElement>)result.Value));
                case (_, NonTerminal.Pattern):
                    return new Result(true, BuildPattern((List<IElement>)result.Value));
                case (_, NonTerminal.Root):
                    return new Result(true, ((List<IElement>)result.Value)[0]);
                case (_, Terminal.Number):
                    return new Result(true, new Number(Convert.ToInt32(result.Value)));
                case (_, Terminal.Symbol):
                    return new Result(true, new Symbol((string)result.Value));
                case (_, Terminal.Underscores):
                    return new Result(true, new Symbol((string)result.Value));
                case (_, _):
                    return result;
            }
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
            var productions = Grammar.Rules[requestedSymbol];
            foreach (var production in productions)
            {
                SavePosition();
                var productionMatches = true;
                var elements = new List<IElement>();
                foreach (var symbol in production)
                {
                    var result = GetSymbol(symbol);
                    if (!result.Success)
                    {
                        productionMatches = false;
                        break;
                    }
                    switch (result.Value)
                    {
                        case "":
                            continue;
                        case List<IElement> nonTerminal:
                            elements.AddRange(nonTerminal);
                            break;
                        default:
                            elements.Add((IElement)result.Value);
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

        private static Expression BuildExpression(List<IElement> objectsList)
        {
            var head = ((Symbol)objectsList[0]).Value;
            var operands = objectsList.GetRange(1, objectsList.Count - 1);
            return new Expression(head, operands);
        }

        private static IPattern BuildPattern(List<IElement> objects)
        {
            var patternName = ((Symbol)objects[0]).Value;
            var underscoresCount = ((Symbol)objects[1]).Value.Length;
            var typeName = objects.Count == 2 ? "" : ((Symbol)objects[2]).Value;
            switch (underscoresCount, typeName)
            {
                case (1, ""):
                    return new ElementPattern(patternName);
                case (1, "symbol"):
                    return new ElementPattern(patternName);
                case (1, "integer"):
                    return new NumberPattern(patternName);
                case (3, ""):
                    return new NullableSequencePattern(patternName);
                default:
                    throw new StrangePatternFormException();
            }
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
