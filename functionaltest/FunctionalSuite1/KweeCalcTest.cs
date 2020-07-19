#nullable enable
using Xunit;
using Xunit.Abstractions;
using FunctionalCalcLib;

namespace FunctionalSuite1
{
    public class KweeCalcTest : CommonCalcTest
    {
        #region class internals
        public ITestOutputHelper Output;
        public KweeCalcTest(ITestOutputHelper output) { 
            this.Output = output;
            this.CalcFn = Calc.CalcImplKwee;
        }
        #endregion class internals
        #region tests
        [Fact]
        public void s0003_incompleteDecimalPass(){
            TestAnExpression("3.", 3);
        }
        [Theory]
        [InlineData("", "There is no or empty input")]
        [InlineData(" ", "There is no or empty input")]
        [InlineData("\t", "There is no or empty input")]
        [InlineData("()", "There is no or empty input")]
        [InlineData("2 + 4 ()", "There is no or empty input")]
        public void s0008_emptyPass(string expr, string expectedMessage){
            TestAnExpression(expr, null, expectedMessage);
        }
        #endregion special tests
        #region forwarded tests
        // Below are forwarding tests to the base class.
        // A tool can be used to generate it
        // Hence single line per test
        [Theory]
        [InlineData("2 + 3", 5)]
        [InlineData("3 + 2", 5)]
        [InlineData("2.0 + 3.0", 5.0)]
        [InlineData("3.0 + 2.0", 5.0)]
        public void f0000_Add(string expr, dynamic expectedResult) { t0000_Add(expr, expectedResult); }
        [Theory]
        [InlineData("2 * 3", 6)]
        [InlineData("3 * 2", 6)]
        [InlineData("2.0 * 3.0", 6.0)]
        [InlineData("3.0 * 2.0", 6.0)]
        public void f0000_Mult(string expr, dynamic expectedResult) { t0000_Mult(expr, expectedResult); }
        [Fact] public void f0001_parenthesis() { t0001_parenthesis(); }
        [Fact] public void f0003_decimal() { t0002_decimal(); }
        [Fact] public void f0004_precedence() { t0004_precedence(); }
        [Fact] public void f0005_precedence() { t0005_precedence(); }
        [Fact] public void f0006_precedence() { t0006_precedence(); }
        [Fact] public void f0007_precedence() { t0007_precedence(); }
        [Theory]
        [InlineData("2\t+\t5", 7)]
        [InlineData("2\t*\t5", 10)]
        public void f0009_tabExpr(string expr, dynamic expectedResult){ t0009_tabExpr(expr, expectedResult); }
        #endregion forwarded tests
    }
}
