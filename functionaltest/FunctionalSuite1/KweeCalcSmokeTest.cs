#nullable enable

using Xunit;
using Xunit.Abstractions;
using FunctionalCalcLib;

namespace FunctionalSuite1
{
    public class KweeCalcSmokeTest : CommonCalcTest
    {
    #region class internals
        public ITestOutputHelper Output;
        public KweeCalcSmokeTest(ITestOutputHelper output) {
            this.Output = output;
            this.CalcFn = Calc.CalcImplKwee;
        }
    #endregion class internals
    #region smoke tests
        [Theory]
        [InlineData("5+14", 19)]
        [InlineData("(8+2)*4", 40)]
        [InlineData("7+3+9", 19 )]
        [InlineData("(6 + 5) * (8 + 2)", 110)]
        public void s0001_smoke(string expr, dynamic expectedResult){
            TestAnExpression(expr, expectedResult);
        }
    #endregion special tests
    }
}
