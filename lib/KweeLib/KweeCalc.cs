﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

namespace KweeLib
{
    public class KweeCalc
    {
        /// <summary>
        /// Checks that userInput is valid, then rationalizes it
        /// to ensure there is an outer () and single spaces between operators
        /// This makes it easier to Evaluate to process input
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>
        /// Tuple of (validatedString, errorString)
        /// If validateString is null, use errorString to report or diagnose
        /// </returns>
        public Tuple<string?,string?> rationalizeExpression(string userInput)
        {
            string? rationalExpression = null;
            string? errorMessage = null;
            if (hasValidCharacters(userInput))
            {
                if (isParenBalanced(userInput))
                {
                    rationalExpression = "( " + ensureSingleSpace(userInput) + " )";
                } else
                {
                    errorMessage = "There are unbalanced parenthesis in the expression";
                }
            } else
            {
                errorMessage = "There are invalid characters in the expression";
            }
            return Tuple.Create(rationalExpression, errorMessage);
        }
        private bool isParenBalanced(string input)
        {
            var numBalance = 0;
            var isBalanced = true;
            foreach (char c in input)
            {
                if (c == '(') numBalance++;
                if (c == ')') numBalance--;
                if (numBalance < 0) { isBalanced = false; break; }
            }
            return isBalanced;
        }
        /// <summary>
        /// Arithemtic expressions should be numeric, operators and spaces. Other characters should not be allowed.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool hasValidCharacters(string input)
        {
            var hasValid = true;
            // Valid char set are 0-9. +*()

            foreach (char c in input)
            {
                var m = Regex.Match(input, @"[^0-9 +*.)(]+");
                if (m.Success)
                {
                    hasValid = false;
                    break;
                }
            }
            return hasValid;
        }
        private string ensureSingleSpace(string input)
        {
            input = input.Replace("+", " + ")
                .Replace("*", " * ")
                .Replace("(", " ( ")
                .Replace(")", " ) "); // expand the key characters
            input = input.Trim(); // clean the beginning and end
            input = Regex.Replace(input, @"\s+", " "); // collapse extraneous spaces
            return input;
        }
        private void calcAndPush(Stack<string> opStack, Stack<Double> valStack){
            var op = opStack.Pop();
            var secondValue = valStack.Pop(); // tricky: secondValue first
            var firstValue = valStack.Pop();
            Double computedValue = 0;
            if (op == "+") computedValue = firstValue + secondValue;
            else if (op == "*") computedValue = firstValue * secondValue;
            valStack.Push(computedValue);
        }
        public Tuple<Double?,string?> Evaluate(string userInput) // validated == no unbalanced, no bad characters
        {
            double? returnValue = null;
            string? errorMessage = null;

            var exprTuple = rationalizeExpression(userInput);
            if(exprTuple.Item1 != null)
            {
                var opStack = new Stack<String>();
                var valStack = new Stack<Double>();
                var parenExpression = exprTuple.Item1;
                var listTokens = parenExpression.Split(new char[] { ' ' }); // Split cannot take empty character

                foreach (var token in listTokens)
                {
                    switch (token)
                    {
                        case "(": { break; }
                        case "+":
                            {
                                if (opStack.Count > 0 && opStack.Peek() == "*") // higher precedence
                                {
                                    calcAndPush(opStack, valStack);
                                }
                                opStack.Push(token);
                                break;
                            }
                        case "*": { opStack.Push(token); break; }
                        case ")":
                            {
                                while (opStack.Count > 0)
                                {
                                    calcAndPush(opStack, valStack);
                                }
                                break;
                            }
                        default:
                            {
                                valStack.Push(Double.Parse(token));
                                break;
                            }
                    }
                }
                Trace.Assert(opStack.Count == 0);
                Trace.Assert(valStack.Count == 1);
                returnValue = valStack.Pop();
            }
            else
            {
                errorMessage = exprTuple.Item2;
            }
            return Tuple.Create(returnValue, errorMessage);
        }// Evaluate
    }// class
}// namespace
