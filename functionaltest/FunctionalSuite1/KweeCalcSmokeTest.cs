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
        [InlineData("5+14"   , 19, null)]
        [InlineData("(8+2)*4", 40, null)]
        [InlineData("7+3+9"  , 19, null)]
        [InlineData("(6 + 5) * (8 + 2)", 110, null)]
        [InlineData(" 2 + hello", null, "There are invalid characters in the expression")]
        public void s0001_smoke(string expr, dynamic? expectedResult, string? errorMsg){
            TestAnExpression(expr, expectedResult, errorMsg);
        }
    #endregion special tests
    }
}
