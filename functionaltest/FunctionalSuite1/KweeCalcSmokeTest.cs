#nullable enable
// Remove comment to add '/' and '-' operators
// #define MOREOPERATORS

using Xunit;
using Xunit.Abstractions;
using FunctionalCalcLib;

namespace FunctionalSuite1
{
    public class KweeCalcSmokeTest : CommonCalcTest
    {
    #region class internals
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
#if MOREOPERATORS
        [Theory]
        [InlineData("4/2", 2, null)]
        [InlineData("4/0", double.PositiveInfinity, null)]
        public void s0099_divide(string expr, dynamic expectedResult, string? errorMsg){
            TestAnExpression(expr, expectedResult, errorMsg);
        }
        [Theory]
        [InlineData("4 - 2", 2, null)]
        [InlineData("2 - 4", -2, null)]
        [InlineData("7-2-1", 4, null)]
        [InlineData("37-17-7-3", 10, null)]
        [InlineData("(((37))-((17))-((7))-((3)))", 10, null)]
        [InlineData("(7*4) -6 -(2 + 11)", 9, null)]
        public void s0099_subtract(string expr, dynamic expectedResult, string? errorMsg){
            TestAnExpression(expr, expectedResult, errorMsg);
        }
#endif
    #endregion special tests
    }
}
