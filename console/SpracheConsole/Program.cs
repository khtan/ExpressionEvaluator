using System;
using FunctionalCalcLib;

namespace SpracheConsole
{
    /// <summary>
    /// Minimal console, without help etc
    /// Provides interactive way to use/test calculator
    ///    Inputs are echoed with i:
    ///    Errors are echoed with e:
    ///    Values are echoed without any prefix indicators
    /// In a shell : enter Control-C to indicate end of input
    /// In a file redirection, the EOF serves as end of input
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string codeToEval;
            while ((codeToEval = Console.ReadLine()) != null){
                Console.WriteLine($"i: {codeToEval}");
                // use the functional interface for Sprache calculator
                var t = Calc.CalcImplSprache(codeToEval);
                if(t.Item1 != null) { // value
                    Console.WriteLine(t.Item1);
                }
                if(t.Item2 != null) // error message
                {
                    Console.WriteLine($"error: {t.Item2}");
                }
            }
        }
    }
}
