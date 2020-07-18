using System;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace SpracheLib.test
{
    public class SpracheLibUnitTest
    {
        private readonly SimpleCalculator Calc;
        private readonly ITestOutputHelper Output;

        public SpracheLibUnitTest(ITestOutputHelper output)
        {
            Calc = new SimpleCalculator();
            this.Output = output;
        }
        [Fact]
        public void t0000_parseExpression()
        {
            Assert.Equal(3.14159d, Math.Round(Calc.ParseExpression("4*(1/1-1/3+1/5-1/7+1/9-1/11+1/13-1/15+1/17-1/19+10/401)").Compile()(), 5));
            Assert.Equal(2.97215d, Math.Round(Calc.ParseExpression("2*(2/1*2/3*4/3*4/5*6/5*6/7*8/7*8/9)").Compile()(), 5));
            Assert.Equal(Math.E.ToString(), Calc.ParseExpression(string.Format(CultureInfo.InvariantCulture, "{0}", Math.E)).Compile()().ToString());
        }
        [Fact]
        public void t0001_bracketExpression()
        {
            var exp = "(1+2)*(3+4)";
            var result = Calc.ParseExpression(exp).Compile()();
            Output.WriteLine($"result={result}");
        }
        [Fact]
        public void t0002_doublebracketExpression()
        {
            var exp = "((2))";
            var result = Calc.ParseExpression(exp).Compile()();
            Output.WriteLine($"result={result}");
        }
    }
}
