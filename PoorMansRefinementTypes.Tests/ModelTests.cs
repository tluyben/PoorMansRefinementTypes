using NUnit.Framework;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ChildTest1()
        {
            Child child = new Child();

            child.Age = 14;

            Assert.True(child.Age == 14); 
        }

        [Test]
        public void ChildTest2()
        {
            Child child = new Child();

            child.Age = 19;

            Assert.True(child.Age == -1);
        }

        [Test]
        public void ChildTest3()
        {
            Child child = new Child();

            try
            {
                child.Age = -2;
                Assert.Fail();
            }
            catch (InvalidValueException e)
            {
            
            }
            
        }

        [Test]
        public void ChildTest4()
        {
            Child child = new Child();

            try
            {
                child.Age = 5;
                
            }
            catch (InvalidValueException e)
            {
                Assert.Fail();
            }

        }
    }
}
