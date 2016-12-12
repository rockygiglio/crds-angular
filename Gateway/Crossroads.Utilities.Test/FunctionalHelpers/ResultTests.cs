using Crossroads.Utilities.FunctionalHelpers;
using NUnit.Framework;

namespace Crossroads.Utilities.Test.FunctionalHelpers
{
    public class ResultTests
    {

        [Test]
        public void ShouldMapOverAnOk()
        {
            var result = new Ok<int>(34);
            var newRes = result.Map(i => i + 1);
            Assert.AreEqual(35, newRes.Value);
        }

        [Test]
        public void ShouldNotMapOverAnErr()
        {
            var result = new Err<int>("Value not Found");
            var newRes = result.Map(i => i + 1);
            Assert.IsInstanceOf<Err<int>>(newRes);
            Assert.IsNotNull(newRes.ErrorMessage);
            Assert.AreEqual(default(int), newRes.Value);            
        }

    }
}
