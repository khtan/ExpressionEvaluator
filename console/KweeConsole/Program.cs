using System;
using FunctionalCalcLib;

namespace KweeConsole
{
    /// <summary>
    /// Interactive way to use/test calculator
    /// Inputs are echoed with i:
    /// Errors are echoed with e:
    /// Values are echoed without any prefix indicators
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var codeToEval = Console.ReadLine();
            Console.WriteLine($"i: {codeToEval}");
            do
            {
                // use the functional interface for Kwee calculator
                var t = Calc.CalcImplKwee(codeToEval);
                if(t.Item1 != null) { // value
                    Console.WriteLine(t.Item1);
                }
                if(t.Item2 != null) // error message
                {
                    Console.WriteLine($"error: {t.Item2}");
                }
                codeToEval = Console.ReadLine();
                Console.WriteLine($"i: {codeToEval}");
            } while (codeToEval != null);
        }
    }
}
