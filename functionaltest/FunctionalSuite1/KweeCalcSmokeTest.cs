#nullable enable
// Remove comment to add '/' and '-' operators
#define MOREOPERATORS

using System.Threading;
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
            this.CalcFn = Calc.CalcImplRobustKwee;
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
        [InlineData("0 * (3-11)", -0, null)] // compare against -0
        [InlineData("0 * (5-11)", 0, null)] // compare against 0
        [InlineData("0 * (7-11)", +0, null)] // compare against +0
        [InlineData("0 * (7-3)", 0, null)]
        public void s0099_subtract(string expr, dynamic expectedResult, string? errorMsg){
            TestAnExpression(expr, expectedResult, errorMsg);
        }
        [Theory]
        [InlineData("(((6 - 12) * (19 * 18) * 4) * ((5 * 16) * (4 * 18) * 19) * (((1 - 13) - 18 + 3) * ((7 + 6) - (8 - 5) + 6) * (8 + 14 + 5) * (12 * 19)) - (18 - 12) + (13 * (7 + (8 + 11) + (17 * 18)) * (10 + 12) * (16 - 10)))", 2388888007389546, null)]
        [InlineData("(((6 - 12) * (19 * 18) * 4) * ((5 * 16) * (4 * 18) * 19) * (((1 - 13) - 18 + 3) * ((7 + 6) - (8 - 5) + 6) * (8 + 14 + 5) * (12 * 19)) - (18 - 12) + (13 * (7 + (8 + 11) + (17 * 18)) * (10 + 12) * (16 - 10)))", 2.388888007389546E+15, null)]
        // Below: rounding from ..46E+15 to ..5E15 results in difference of 4, so test comparison fails if checking to 15 significant figures
        // [InlineData("(((6 - 12) * (19 * 18) * 4) * ((5 * 16) * (4 * 18) * 19) * (((1 - 13) - 18 + 3) * ((7 + 6) - (8 - 5) + 6) * (8 + 14 + 5) * (12 * 19)) - (18 - 12) + (13 * (7 + (8 + 11) + (17 * 18)) * (10 + 12) * (16 - 10)))", (Double)2.38888800738955E+15, null)]
        [InlineData("(13 * 15 * 19) * (15 * (15 - 7 * (18 + 0)) - (16 * 2) * (3 + 7)) * ((5 - 1) - (2 - 0) * (3 * 18)) * ((12 * 18 - 7) * 16 * ((2 + 6) * (18 * 13) - (16 * 15)) * ((18 - 12) * (3 + 13) * (18 - 2))) + (3 + 6 - 3)", 6.411500811819418E+18, null)]
        [InlineData("(13 * 15 * 19) * (15 * (15 - 7 * (18 + 0)) - (16 * 2) * (3 + 7)) * ((5 - 1) - (2 - 0) * (3 * 18)) * ((12 * 18 - 7) * 16 * ((2 + 6) * (18 * 13) - (16 * 15)) * ((18 - 12) * (3 + 13) * (18 - 2))) + (3 + 6 - 3)", 6.41150081181942E+18, null)]
        [InlineData("(1-8209) * 109440 * ((1-2659393) - (18 - 12) + (13 * (7 + (8 + 11) + (17 * 18)) * (10 + 12) * (16 - 10) )", null, "There are unbalanced parenthesis in the expression")]
        [InlineData("(1-8208) * 109440 * ))1-2659393( - (18 - 12) + (13 * (7 + (8 + 11) + (17 * 18)) * (10 + 12) * (16 - 10) )", null, "There are unbalanced parenthesis in the expression")]
        public void s0098_largeValuesWithSubtraction(string expr, dynamic expectedResult, string? errorMsg){
            TestAnExpression(expr, expectedResult, errorMsg);
        }
#endif
        #endregion special tests
    }
}
