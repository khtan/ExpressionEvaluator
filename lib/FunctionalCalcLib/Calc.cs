#nullable enable
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

        public static Tuple<dynamic?, string?> CalcImplKwee(string expr)
        {
            string? errorMessage = null;
            dynamic? value = null;
            var tuple = Kwee.Evaluate(expr);
            if (tuple.Item1 != null) { 
                value = tuple.Item1; 
            }
            if (tuple.Item2 != null)
            {
                errorMessage = tuple.Item2;
            }
            return new Tuple<dynamic?, string?>(value, errorMessage);
        }
    }
}
