#nullable enable
using System;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace FunctionalSuite1
{
    public class CommonCalcTest
    {
        public ITestOutputHelper Output;
        public Func<string, Tuple<dynamic?, string?>> CalcFn;
    #region testhelpers
        protected void TestAnExpression(string expr, dynamic? expected, string? errorMessage = null)
        {
            Tuple<dynamic?, string?> t = CalcFn(expr);
            Output.WriteLine($"t={t}");
/*
            var expectedTuple = new Tuple<dynamic?, string?>(expected, errorMessage);
            t.Should().Be(expectedTuple); // Bug? Be works in FunctionalCalcLibUnitTest but not here, always fail
            // t.Should().Equals(expectedTuple); // Bug? Equals does not work on Tuple<dynamic?, string?>, always pass, regardless
*/
            Double? d = t.Item1;
            string? m = t.Item2;
            if (expected != null)
            {
                d.Should().Be(expected);
            }
            if(errorMessage != null)
            {
                m.Should().Be(errorMessage);
            }
        }
        #endregion testhelpers
        #region tests
        [Theory]
        [InlineData("2 + 3", 5)]
        [InlineData("3 + 2", 5)]
        [InlineData("2.0 + 3.0", 5.0)]
        [InlineData("3.0 + 2.0", 5.0)]
        public void t0000_Add(string expr, dynamic expectedResult)
        {
            TestAnExpression(expr, expectedResult);
        }
        [Theory]
        [InlineData("2 * 3", 6)]
        [InlineData("3 * 2", 6)]
        [InlineData("2.0 * 3.0", 6.0)]
        [InlineData("3.0 * 2.0", 6.0)]
        public void t0000_Mult(string expr, dynamic expectedResult)
        {
            TestAnExpression(expr, expectedResult);
        }
        [Fact]
        public void t0001_parenthesis(){
            TestAnExpression("(((3)))", 3);
        }
        [Fact]
        public void t0002_decimal(){
            TestAnExpression(".3", 0.3);
        }
        [Fact]
        public void t004_precedence()
        {
            TestAnExpression("2 + 3 * 5", 17);
        }
        [Fact]
        public void t005_precedence()
        {
            TestAnExpression("2 * 3 + 5", 11);
        }
        [Fact]
        public void t006_precedence()
        {
            TestAnExpression("2 + 3 * 5 + 7 * 9 + 11", 91);
        }
        [Fact]
        public void t007_precedence()
        {
            TestAnExpression("2 * 3 + 5 * 7 + 9 * 11", 140);
        }
        #endregion tests
    }
}
