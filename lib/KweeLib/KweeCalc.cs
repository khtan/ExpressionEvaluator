#nullable enable
// Remove comment to add '/' and '-' operators
// #define MOREOPERATORS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KweeLib
{
    /// <summary>
    /// This is a reimplementation of the "Shunting Yard" algorithm, very clearly described in
    /// Sedgewicks Algorithms 4th edition, Chapter 1 Section 1.3 on 'Arithmetic expression evaluation' :
    /// ...
    /// An expression consists of parentheses, operators, and operands (numbers).
    /// Proceeding from left to right and taking these entities one at a time,
    /// we manipulate the stacks according to four possible cases, as follows:
    ///    1. Push operands onto the operand stack.
    ///    2. Push operators onto the operator stack.
    ///    3. Ignore left parentheses.
    ///    4. On encountering a right parenthesis, pop an operator,
    ///       pop the requisite number of operands,
    ///       and push onto the operand stack the result of applying that operator to those operands.
    ///
    /// After the final right parenthesis has been processed, there is one value on the stack,
    /// which is the value of the expression.
    /// ...
    /// Additional helpers are refactored so that the original code logic is not obscured.
    ///
    /// </summary>
    public class KweeCalc
    {
    #region helpers
        /// <summary>
        /// Checks that userInput is valid, then rationalizes it
        /// to ensure there is an outer () and single spaces between operators
        /// This makes it easier for Evaluate to process input
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
            if(!HasEmptyConstructs(userInput))
            {
                if(!HasMalformedConstructs(userInput))
                {
                    if (hasValidCharacters(userInput))
                    {
                        if (isParenBalanced(userInput))
                        {
                            rationalExpression = "( " + ensureSingleSpace(userInput) + " )";
                        }
                        else
                        {
                            errorMessage = "There are unbalanced parenthesis in the expression";
                        }
                    }
                    else
                    {
                        errorMessage = "There are invalid characters in the expression";
                    }
                }
                else
                {
                    errorMessage = "There are malformed constructs in the expression";
                }
            }
            else
            {
                errorMessage = "There is no or empty input";
            }
            return Tuple.Create(rationalExpression, errorMessage);
        }
        private bool doesTargetHaveHigherPrecedence(string targetOp, string currentOp)
        {
            bool returnVal = false;
            if (currentOp == "+" || currentOp == "-")
            {
                if(targetOp == "*" || targetOp == "/")
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }
        private bool HasMalformedConstructs(string input){
            bool returnVal = false;
            // empty braces
            var m = Regex.Match(input, @"\(\s*\)");
            if (m.Success){ returnVal = true; }
            // 
#if MOREOPERATORS
            m = Regex.Match(input, @"[+*/-][+*/-]");
#else
            m = Regex.Match(input, @"[+*][+*]");
#endif
            if (m.Success){ returnVal = true; }
            return returnVal;
        }
        private bool HasEmptyConstructs(string input) // null, empty, empty braces
        {
            bool returnVal = false;
            if (String.IsNullOrEmpty(input)) { returnVal = true; }
            else if (String.IsNullOrWhiteSpace(input)) { returnVal = true; }
            return returnVal;
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
            // Valid char set are 0-9. \t+*()

            foreach (char c in input)
            {
#if MOREOPERATORS
                var m = Regex.Match(input, @"[^0-9 \t/\-+*.)(]+"); // have - and / symbols
#else
                var m = Regex.Match(input, @"[^0-9 \t+*.)(]+"); // does not have - and / symbols
#endif
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
            input = input.Replace("+", " + ") // operators
                .Replace("*", " * ")
                .Replace("(", " ( ")
                .Replace(")", " ) "); // expand the key characters
#if MOREOPERATORS
            input = input.Replace("-", " - ").
            Replace("/", " / ");
#endif
            input = input.Trim(); // clean the beginning and end
            input = Regex.Replace(input, @"\s+", " "); // collapse extraneous spaces
            return input;
        }
        public string? binaryCalcAndPush(Stack<string> opStack, Stack<Double> valStack){
            string? retString = null;
            try{
                var op = opStack.Pop();
                var secondValue = valStack.Pop(); // tricky: secondValue first
                var firstValue = valStack.Pop();
                Double computedValue = 0;
                if (op == "+") { computedValue = firstValue + secondValue; }
                else if (op == "*") { 
                    computedValue = firstValue * secondValue; 
                }
#if MOREOPERATORS
                else if (op == "-") { computedValue = firstValue - secondValue; }
                else if (op == "/") { computedValue = firstValue / secondValue; }
#endif
                valStack.Push(computedValue);
            } catch(Exception e){
                retString = e.Message;
            }
            return retString;
        }
    #endregion helpers
    #region Evaluate
        public Tuple<Double?,string?> Evaluate(string userInput)
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
                        case "(": {
                            opStack.Push(token);
                            break;
                        }
                        case "+":
                            {
                                if (opStack.Count > 0 && opStack.Peek() != "(" && doesTargetHaveHigherPrecedence(opStack.Peek(), token)){
                                        string? opMsg = binaryCalcAndPush(opStack, valStack);
                                        if(opMsg != null){
                                            errorMessage = opMsg;
                                            break;
                                        }
                                }
                                opStack.Push(token);
                                break;
                            }

                        case "*": { opStack.Push(token); break; }
                        case ")":
                            {
                                while (opStack.Count > 0 && opStack.Peek() != "(")
                                {
                                    string? opMsg = binaryCalcAndPush(opStack, valStack);
                                    if(opMsg != null){
                                        errorMessage = opMsg;
                                        break;
                                    }
                                }
                                if(opStack.Peek() == "(") opStack.Pop();
                                break;
                            }
                        default:
                            {
                                valStack.Push(Double.Parse(token));
                                break;
                            }
                    }
                }
                if(errorMessage == null){
                    // Trace.Assert(opStack.Count == 0);
                    // Trace.Assert(valStack.Count == 1);
                    returnValue = valStack.Pop();
                }
            }
            else
            {
                errorMessage = exprTuple.Item2;
            }
            return Tuple.Create(returnValue, errorMessage);
        }// Evaluate
    #endregion Evaluate
    }// class
}// namespace
 
