#nullable enable
using Xunit;
using Xunit.Abstractions;
using FunctionalCalcLib;

namespace FunctionalSuite1
{
    public class KweeCalcTest : CommonCalcTest
    {
    #region class internals
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
    #endregion tests
    }
}
