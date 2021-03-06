﻿#nullable enable
using System;
using SpracheLib;
using KweeLib;
using System.Threading.Tasks;


namespace FunctionalCalcLib
{
    /// <summary>
    /// The fundamental functional interface is Func<string, Tuple<dynamic?, string?>>, ie
    /// the input is an expression ( string )
    /// and the output is a Tuple. 
    /// The first value of the Tuple is a nullable dynamic, representing the value of the expression, if it evaluates without error.
    /// The null represents a bad evaluation.
    /// The second value of the Tuple is a nullable string, representing the error message if any.
    /// The null represents no errors.
    /// 
    /// This library provides two implementations of the expression evaluator.
    /// The KweeCalc is written by yours truly.
    /// The SimpleCalculator is a test vehicle, pulled from 
    /// </summary>
    public static class Calc
    {
        private static readonly SimpleCalculator Simple = new SimpleCalculator();
        private static readonly KweeCalc Kwee = new KweeCalc();
        public static Tuple<dynamic?,string?> CalcImplSprache(string expr)
        {
            string? errorMessage = null;
            dynamic? value = null;
            try {
                value = Simple.ParseExpression(expr).Compile()();
            } catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            return new Tuple<dynamic?, string?>(value, errorMessage);
            // Note : Tuple.Create does not handle dynamic? well
            // Error is connot implicitly converty type Tuple<double,string> to Tuple(object,string)
            // return Tuple.Create(value, errorMessage);
        }
        /// <summary>
        /// Use this version of Calc for performance and competitive programs, where inputs are already well formed
        /// It is faster at the expense of no checking
        /// </summary>
        public static Tuple<dynamic?, string?> CalcImplFastKwee(string expr)
        {
            return Kwee.Evaluate(expr);
        }
        /// <summary>
        /// Use this version of Calc for robust, industrial usage where users can throw bad data.
        /// It is much slower due to the extra checks
        /// </summary>
        public static Tuple<dynamic?, string?> CalcImplRobustKwee(string expr)
        {
            return Kwee.EvaluateWithChecks(expr);
        }
    }
}
