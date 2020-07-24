#nullable enable
using System;
using Xunit;
using Xunit.Abstractions;
using FunctionalCalcLib;

namespace FunctionalSuite1
{
    public class SpracheCalcTest : CommonCalcTest
    {
    #region class internals
        public SpracheCalcTest(ITestOutputHelper output) { 
            this.Output = output;
            this.CalcFn = Calc.CalcImplSprache;
        }
    #endregion class internals
    #region special tests
        [Fact]
        public void s0003_incompleteDecimalFail(){
            TestAnExpression("3.", null, "Parsing failure: Unexpected end of input reached; expected numeric character (Line 1, Column 3); recently consumed: 3.");
        }
        [Theory]
        [InlineData("",   "Parsing failure: Unexpected end of input reached; expected - or ( or Constant (Line 1, Column 1); recently consumed: ")]
        [InlineData(" ",  "Parsing failure: Unexpected end of input reached; expected - or ( or Constant (Line 1, Column 2); recently consumed:  ")]
        [InlineData("\t", "Parsing failure: Unexpected end of input reached; expected - or ( or Constant (Line 1, Column 2); recently consumed: 	")]
        public void s0008_emptyFail(string expr, string expectedMessage){
            TestAnExpression(expr, null, expectedMessage);
        }
        [Theory]
        [InlineData("()", "Parsing failure: unexpected ')'; expected - or ( or Constant (Line 1, Column 2); recently consumed: (")]
        [InlineData("(\t)", "Parsing failure: unexpected ')'; expected - or ( or Constant (Line 1, Column 3); recently consumed: (\t")]
        [InlineData("2 + 4 ()", "Parsing failure: unexpected '('; expected end of input (Line 1, Column 7); recently consumed: 2 + 4 ")]
        [InlineData("2 ++ 4", "Parsing failure: unexpected '+'; expected end of input (Line 1, Column 3); recently consumed: 2 ")]
        [InlineData("2 *+ 4", "Parsing failure: unexpected '*'; expected end of input (Line 1, Column 3); recently consumed: 2 ")]
        [InlineData("2 +*+ 4", "Parsing failure: unexpected '+'; expected end of input (Line 1, Column 3); recently consumed: 2 ")]
        public void s0010_malformedFail(string expr, string expectedMessage){
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
        public void f0000_Add(string expr, dynamic expectedResult) { t0000_general(expr, expectedResult, null); }
        [Theory]
        [InlineData("2 * 3", 6)]
        [InlineData("3 * 2", 6)]
        [InlineData("2.0 * 3.0", 6.0)]
        [InlineData("3.0 * 2.0", 6.0)]
        public void f0000_Mult(string expr, dynamic expectedResult) { t0000_general(expr, expectedResult, null); }
        [Fact] public void f0001_parenthesis() { t0001_parenthesis(); }
        [Fact] public void f0003_decimal() { t0002_decimal(); }
        [Fact] public void f0004_precedence() { t0004_precedence(); }
        [Fact] public void f0005_precedence() { t0005_precedence(); }
        [Fact] public void f0006_precedence() { t0006_precedence(); }
        [Fact] public void f0007_precedence(){ t0007_precedence();}
        [Theory]
        [InlineData("2\t+\t5", 7)]
        [InlineData("2\t*\t5", 10)]
        public void f0009_tabExpr(string expr, dynamic expectedResult){ t0000_general(expr, expectedResult, null); }
        [Theory]
        [InlineData("2147483647 + 1", 2147483648)]
        [InlineData("9999999999 + 1", 10000000000)]
        public void f0011_largeValues(string expr, dynamic expectedResult){
            // int.MaxValue = 2147483647
            // double.MaxValue = 1.7976931348623157E+308
            t0000_general(expr, expectedResult, null);
        }
        [Theory]
        [InlineData("2.7 * 1.6", 4.32, null)]
        [InlineData("0.9999 * 0.9999", 0.99980001, null)]
        public void f0012_precision(string expr, dynamic expectedResult, string? errorMsg){
            t0000_general(expr, expectedResult, errorMsg);
        }
        [Theory]
        [InlineData("(2 + 9) * (2 + 12) * (12 + 13) + (3 * (16 * 8) * (19 * 6))", 47626, null)]
        [InlineData("3 * 3 * 3 + 2 * 2", 31, null)]
        public void f0013_longchain(string expr, dynamic expectedResult, string? errorMsg){
            t0000_general(expr, expectedResult, errorMsg);
        }
    #endregion forwarded tests
    }
}
