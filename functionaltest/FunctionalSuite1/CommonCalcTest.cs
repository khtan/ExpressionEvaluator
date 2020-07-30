#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;
using FluentAssertions;
using Xunit.Sdk;
using Xunit;

namespace FunctionalSuite1
{
    public class CommonCalcTest
    {
           // [MaybeNull] attribute not disabling CS8618 warning
           // Warning CS8618  Non-nullable field 'CalcFn' is uninitialized.Consider declaring the field as nullable.FunctionalSuite1 C:\cprojects\github\circleci\ExpressionEvaluator\functionaltest\FunctionalSuite1\CommonCalcTest.cs	12	Active
        [MaybeNull] public ITestOutputHelper? Output;
        [MaybeNull] public Func<string, Tuple<dynamic?, string?>>? CalcFn;
    #region testhelpers
        protected void TestAnExpression(string expr, dynamic? expected, string? errorMessage = null)
        {
            if(CalcFn is null)
            {
                CalcFn.Should().NotBeNull();
            } else {
                Tuple<dynamic?, string?> t = CalcFn(expr);
                /*
                            var expectedTuple = new Tuple<dynamic?, string?>(expected, errorMessage);
                            t.Should().Be(expectedTuple); // Bug? Be works in FunctionalCalcLibUnitTest but not here, always fail
                            // t.Should().Equals(expectedTuple); // Bug? Equals does not work on Tuple<dynamic?, string?>, always pass, regardless
                */
                Double? d = t.Item1;
                string? m = t.Item2;
                if (d != null) Output.WriteLine($"v = {d}");
                if (m != null) Output.WriteLine($"m = {m}");

                if (expected != null)
                {
                    // Bug in FluentAssertions
                    // This should work but fails to compile
                    // d.Should().BeApproximately(expected, 0.00000001);
                    // Below is workaround
                    // https://github.com/fluentassertions/fluentassertions/issues/101
                    if(  (!Double.IsPositiveInfinity((Double)d) && !Double.IsPositiveInfinity((Double)expected))
                       && ( !Double.IsNegativeInfinity((Double)d) && !Double.IsNegativeInfinity((Double)expected) )
                        ) {
                        Double expectedPrecision = Math.Abs(expected) / 1000000000000000; // 15 significant digits
                        // ?Bug in FluentAssertions BeApproximately should also compare infinities as equal
                        // NumericAssertionsExtensions.BeApproximately(d.Should(), expected, 0.00000001);
                        // NumericAssertionsExtensions.BeApproximately(d.Should(), expected, 3000);
                        NumericAssertionsExtensions.BeApproximately(d.Should(), expected, expectedPrecision);
                    }
                }
                if (errorMessage != null)
                {
                    m.Should().Be(errorMessage);
                }
            }
        }
    #endregion testhelpers
    #region tests
        public void t0000_general(string expr, dynamic expectedResult, string expectedErrorMsg){
            TestAnExpression(expr, expectedResult, expectedErrorMsg);
        }
        public void t0001_parenthesis(){
            TestAnExpression("(((3)))", 3);
        }
        public void t0002_decimal(){
            TestAnExpression(".3", 0.3);
        }
        public void t0004_precedence()
        {
            TestAnExpression("2 + 3 * 5", 17);
        }
        public void t0005_precedence()
        {
            TestAnExpression("2 * 3 + 5", 11);
        }
        public void t0006_precedence()
        {
            TestAnExpression("2 + 3 * 5 + 7 * 9 + 11", 91);
        }
        public void t0007_precedence()
        {
            TestAnExpression("2 * 3 + 5 * 7 + 9 * 11", 140);
        }
    #endregion tests
    }
}
