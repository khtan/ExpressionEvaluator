#nullable enable
using System;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using FunctionalCalcLib;
using System.IO;

namespace KweeConsole
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
        // Properties for commandline option handling
        [Option(Description = "(optional): Input file to run")]
        public string? InputFilePath { get; }
        [Option(Description = "(optional): Reference file to compare")]
        public string? RefFilePath { get; }
        [Option(Description = "(optional) Performance run, turn off all outputs")]
        public bool PerformanceOn { get; }
        // Internal properties
        public StreamReader? inputReader;
        public StreamReader? refReader;
        // Main
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);
        // helpers
        private void Setup(){
            if(InputFilePath != null)
            {
                inputReader = new System.IO.StreamReader(InputFilePath);
                System.Console.SetIn(inputReader);
            }
            if (RefFilePath != null) refReader = new System.IO.StreamReader(RefFilePath);
        }
        private void Teardown(){
            if (inputReader != null) inputReader.Dispose();
            if (refReader != null) refReader.Dispose();
        }
        private void PrintElapsedTime(TimeSpan ts)
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine($"Runtime: {elapsedTime}");
        }
        private TimeSpan TimeReadExecute(bool isPerformance)
        { // A lot of lines to accomplish this, so keep in its own function to avoid distractions
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            ReadExecute(isPerformance); // measure this
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
        private void ReadExecute(bool isPerformance){
            string codeToEval;
            while ((codeToEval = Console.ReadLine()) != null)
            {
                //  Console.WriteLine($"i: {codeToEval}");
                // use the functional interface for Kwee calculator
                var t = Calc.CalcImplRobustKwee(codeToEval);
                if( isPerformance == false )
                {
                    if (t.Item1 != null) Console.WriteLine(t.Item1); // value
                    if (t.Item2 != null) Console.WriteLine($"error1: {t.Item2}"); // error message
                    if (refReader != null)
                    {
                        string? sd = refReader.ReadLine();
                        if (!String.IsNullOrEmpty(sd))
                        {
                            if (t.Item1 != null)
                            {
                                Double d = Double.Parse(sd);
                                if (d != t.Item1) Console.WriteLine($"error3: {t.Item1} != {d}");

                            }
                            if (t.Item2 != null)
                            {
                                if (sd != t.Item2) Console.WriteLine($"error4: {t.Item2} != {sd}");
                            }
                        }
                        else Console.WriteLine($"error2: refFile out of inputs");
                    }//if
                }//if
            }//while
        }
        private void OnExecute()
        {
            Setup();
            var elapsed = TimeReadExecute(PerformanceOn);
            PrintElapsedTime(elapsed);
            Teardown();
        }
    }
}
