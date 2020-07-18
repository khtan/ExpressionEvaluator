#nullable enable
using System;
using Xunit;
using FunctionalCalcLib;
using Xunit.Abstractions;
using FluentAssertions;

namespace FunctionalCalcLib.test
{
    public class FunctionalCalcLibUnitTest
    {
        private readonly ITestOutputHelper Output;
        public FunctionalCalcLibUnitTest(ITestOutputHelper output)
        {
            Output = output;
        }
        [Fact]
        public void t0001_CalcImplSprache()
        {
            var tuple = Calc.CalcImplSprache("3 * 7");
            ((Double)tuple.Item1).Should().Be(21);
            tuple.Item2.Should().BeNull();
        }
        [Fact]
        public void t0002_CalcImplKwee()
        {
            var tuple = Calc.CalcImplKwee("11 * 13");
            ((Double)tuple.Item1).Should().Be(143);
            tuple.Item2.Should().BeNull();
        }
        [Fact]
        public void t0003_TestAgainstBothImpl()
        {
            var expr = "17 * 19";
            var tupleKwee = Calc.CalcImplKwee(expr);
            var tupleSprache = Calc.CalcImplSprache(expr);
            tupleKwee.Should().Be(tupleSprache);
        }
    }
}
