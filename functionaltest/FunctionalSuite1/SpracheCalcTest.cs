#nullable enable
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
    #region tests
        [Fact]
        public void s0003_incompleteDecimalFail(){
            TestAnExpression("3.", null, "Parsing failure: Unexpected end of input reached; expected numeric character (Line 1, Column 3); recently consumed: 3.");
        }
    #endregion tests
    }
}
