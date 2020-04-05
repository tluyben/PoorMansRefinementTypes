using System;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class Child : Person
    {
        public static int ErrorAge(int a)
        {
            return -1; 
        }

        public static bool IsOfChildAge(int a)
        {
            return a < 18; 
        }

        public static bool IsPositive(int a)
        {
            return a >= 0; 
        }

        [Ensures(ValidationMethod = nameof(IsOfChildAge), Throw = false, GetDefault = nameof(ErrorAge))]
        [Ensures(ValidationMethod = nameof(IsPositive), Throw = true)]
        public override int Age {
            get
            {
                return age;
            }
            set
            {
                age = SuperProxy.Validate(value); 
            }
        }
    }
}
