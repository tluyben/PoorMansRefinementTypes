using NUnit.Framework;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class ProxyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ChildTest1()
        {
            var personTalk = (IPersonTalk)new PersonTalkProxy();

            var s = personTalk.SayAsChild(15, "bunnies!");

            Assert.True(s.Contains("child"));
            Assert.True(s.Contains("15"));

        }

        [Test]
        public void ChildTest2()
        {
            var personTalk = (IPersonTalk)new PersonTalkProxy();

            var s = personTalk.SayAsChild(19, "bunnies!");

            Assert.True(s.Contains("child"));
            Assert.True(s.Contains("-1"));

        }
    }
}
