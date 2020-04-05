using System;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class Person
    {
        public string Name { get; set; }

        protected int age;
        public virtual int Age { get; set;  }

        public decimal Income { get; set; }


        private bool canGetMortgage; 


        public static bool IsEligableForMortgage(bool a, object me = null)
        {
            return a && ((Person)me).Age >= 18; 
        }

        [Ensures(ValidationMethod = nameof(IsEligableForMortgage), Throw = false)]
        public virtual bool CanGetMortgage
        {
            get
            {
                return canGetMortgage;
            }
            set
            {
                canGetMortgage = SuperProxy.Validate(value, this);
            }
        }
    }
}
