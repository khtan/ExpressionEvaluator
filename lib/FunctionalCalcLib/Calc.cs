#nullable enable
using System;
using SpracheLib;
using KweeLib;
using System.Threading.Tasks;


namespace FunctionalCalcLib
{
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
