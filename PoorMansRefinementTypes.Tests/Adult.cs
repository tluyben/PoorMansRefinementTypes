using System;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class Adult : Person
    {
        public static int ErrorAge(int a)
        {
            return -1;
        }

        public static bool IsOfAdultAge(int a)
        {
            return a >= 18;
        }

        [Ensures(ValidationMethod = nameof(IsOfAdultAge), Throw = false, GetDefault = nameof(ErrorAge))]
        public override int Age { get; set; }

    }
}
