#nullable enable
using System;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using FunctionalCalcLib;
using System.IO;

namespace KweeConsole
{
    public enum Mode { ROBUST, FAST }
    /// <summary>
    /// Minimal console, without help etc
    /// Provides interactive way to use/test calculator
    ///    Inputs are echoed with i:
    ///    Errors are echoed with e:
    ///    Values are echoed without any prefix indicators
    /// In a shell : enter Control-C to indicate end of input
    /// In a file redirection, the EOF serves as end of input
    /// </summary>
    [Command(ExtendedHelpText = @"
Mode selection:
   robust ( default ) : full checking of input, only allows well-formed expressions to be evaluated
   fast : competitive programming where inputs are strictly well-formed
Examples:
   Interactive run
      > KweeConsole.exe
      > 1 + 2
      > 3
      > 2 + hello
      > error1: There are invalid characters in the expression
      >   C-c C-c^C

   Regression run
      > KweeConsole.exe -i test1.input -r test1.ref
      > 19
      > 40
      > error3: 40 != 41
      > 19
      > 110
      > Runtime: 00:00:00.08

   Performance runs
      > KweeConsole.exe -s -i explist.dat -m robust
      Runtime: 00:00:07.86
      > KweeConsole.exe -s -i explist.dat -m fast
      Runtime: 00:00:01.37"
    )] // appears at the end of help message
    class Program
    {
        // Properties for commandline option handling
        [Option(Description = "(optional): Input file to run")]
        public string? InputFilePath { get; }
        [Option(Description = "(optional): Reference file to compare")]
        public string? RefFilePath { get; }
        [Option(Description = "(optional) Silent by turning off all outputs")]
        public bool Silent { get; }
        [Option(Description = "(optional) Run mode , ie {fast, robust}")]
        public Mode? ModeSetting { get; }
        // Internal properties
        public StreamReader? inputReader;
        public StreamReader? refReader;
        public Mode mode;
        public Func<string, Tuple<dynamic?, string?>>? CalcImpl;
        // Main
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);
        // helpers
        private string? Setup(){
            string? errorMessage = null;
            try{
                if(InputFilePath != null)
                {
                    inputReader = new System.IO.StreamReader(InputFilePath);
                    System.Console.SetIn(inputReader);
                }
                if (RefFilePath != null) refReader = new System.IO.StreamReader(RefFilePath);
                mode = ModeSetting ?? Mode.ROBUST;
                if (mode == Mode.ROBUST) CalcImpl = Calc.CalcImplRobustKwee;
                else CalcImpl = Calc.CalcImplFastKwee;
            }catch(Exception ex){
                errorMessage = ex.Message;
            }
            return errorMessage;
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
        private TimeSpan TimeReadExecute(bool isSilent)
        { // A lot of lines to accomplish this, so keep in its own function to avoid distractions
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            if(isSilent){
                ReadExecuteSilent(); // measure this
            } else {
                ReadExecuteVerbose();
            }
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
        private void ReadExecuteSilent(){
            string codeToEval;
            while ((codeToEval = Console.ReadLine()) != null)
            {
                var t = CalcImpl(codeToEval);
            }//while
        }
        private void ReadExecuteVerbose(){
            string codeToEval;
            while ((codeToEval = Console.ReadLine()) != null)
            {
                //  Console.WriteLine($"i: {codeToEval}");
                // use the functional interface for Kwee calculator
                    var t = CalcImpl(codeToEval);
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
            }//while
        }
        private void OnExecute()
        {
            var errorMessage = Setup();
            if(errorMessage == null){
                var elapsed = TimeReadExecute(Silent);
                PrintElapsedTime(elapsed);
                Teardown();
            } else {
                Console.WriteLine($"error5: {errorMessage}");
            }
        }
    }
}
