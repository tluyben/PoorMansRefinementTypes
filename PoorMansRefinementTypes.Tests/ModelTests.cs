using NUnit.Framework;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class ModelTests
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


        [Test]
        public void ChildTest5()
        {
            Child child = new Child();
            child.Age = 5;

            Assert.True(child.Age == 5);

            child.CanGetMortgage = true;

            Assert.False(child.CanGetMortgage); 
            

        }

        [Test]
        public void AdultTest1()
        {
            Adult adult = new Adult();
            adult.Age = 19;

            Assert.True(adult.Age == 19);

            adult.CanGetMortgage = true;

            Assert.True(adult.CanGetMortgage);


        }
    }
}
