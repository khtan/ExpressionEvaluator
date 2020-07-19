#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace FunctionalSuite1
{
    public class CommonCalcTest
    {
           // [MaybeNull] attribute not disabling CS8618 warning
           // Warning CS8618  Non-nullable field 'CalcFn' is uninitialized.Consider declaring the field as nullable.FunctionalSuite1 C:\cprojects\github\circleci\ExpressionEvaluator\functionaltest\FunctionalSuite1\CommonCalcTest.cs	12	Active
           [MaybeNull] public Func<string, Tuple<dynamic?, string?>> CalcFn; 
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
                if (expected != null)
                {
                    d.Should().Be(expected);
                }
                if (errorMessage != null)
                {
                    m.Should().Be(errorMessage);
                }
            }
        }
    #endregion testhelpers
    #region tests
        public void t0000_Add(string expr, dynamic expectedResult)
        {
            TestAnExpression(expr, expectedResult);
        }
        public void t0000_Mult(string expr, dynamic expectedResult)
        {
            TestAnExpression(expr, expectedResult);
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
        public void t0009_tabExpr(string expr, dynamic expectedResult){
            TestAnExpression(expr, expectedResult);
        }
        public void t0011_largeValues(string expr, dynamic expectedResult){
            TestAnExpression(expr, expectedResult);
        }
    #endregion tests
    }
}
