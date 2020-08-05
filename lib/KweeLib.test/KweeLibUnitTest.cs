using System;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace KweeLib.test
{
    public class KweeLibUnitTest
    {
        private readonly ITestOutputHelper Output;
        private readonly KweeCalc Calc;
        public KweeLibUnitTest(ITestOutputHelper output)
        {
            Output = output;
            Calc = new KweeCalc();
        }
        [Fact]
        public void t0000_noSpaceBetweenParens()
        {
            var s = Calc.ensureSingleSpace("(((3)))");
            s.Should().Be("( ( ( 3 ) ) )"); // quirk : Should().Equal() is not what is seems
        }
        [Fact]
        public void t0001_squishedExpression()
        {
            var s = Calc.ensureSingleSpace("2.5*7.88");
            s.Should().Be("2.5 * 7.88");

        }
        [Fact]
        public void t0002_unbalancedParens()
        {
            var tuple = Calc.rationalizeExpression(") 3 + 2 (");
            tuple.Item1.Should().BeNull();
            tuple.Item2.Should().Be("There are unbalanced parenthesis in the expression");
        }
        [Fact]
        public void t0003_invalidCharacter()
        {
            var tuple = Calc.rationalizeExpression("3 x 2");
            tuple.Item1.Should().BeNull();
            tuple.Item2.Should().Be("There are invalid characters in the expression");
        }
    }
}
