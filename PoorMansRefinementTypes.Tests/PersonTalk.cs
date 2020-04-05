using System;
namespace PoorMansRefinementTypes.Tests
{
    public class PersonTalk : IPersonTalk
    {
        public PersonTalk()
        {
        }

        public string SayAsAdult(int age)
        {
            return null;
        }

        public string SayAsChild(int age, string str)
        {
            return $"A child of {age} says {str}!"; 
        }
    }
}
