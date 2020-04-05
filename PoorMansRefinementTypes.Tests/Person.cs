using System;
namespace PoorMansRefinementTypes.Tests
{
    public class Person
    {
        public string Name { get; set; }

        protected int age;
        public virtual int Age { get; set;  }
    }
}
