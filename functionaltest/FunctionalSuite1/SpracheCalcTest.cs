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
        public ITestOutputHelper Output;
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
        [InlineData("()", "Parsing failure: unexpected ')'; expected - or ( or Constant (Line 1, Column 2); recently consumed: (")]
        [InlineData("2 + 4 ()", "Parsing failure: unexpected '('; expected end of input (Line 1, Column 7); recently consumed: 2 + 4 ")]
        public void s0008_emptyFail(string expr, string expectedMessage){
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
        [Fact] public void f0007_precedence(){ t0007_precedence();}
    #endregion forwarded tests
    }
}
