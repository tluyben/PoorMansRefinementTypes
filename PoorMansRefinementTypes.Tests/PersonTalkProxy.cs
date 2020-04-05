using System;
using PoorMansRefinementTypes.Lib;

namespace PoorMansRefinementTypes.Tests
{
    public class PersonTalkProxy : SuperProxy, IPersonTalk
    {
        public PersonTalkProxy()
        {
        }

        public static int ErrorAge(int a)
        {
            return -1;
        }

        public static bool IsOfChildAge(int a)
        {
            return a < 18;
        }


        public string SayAsAdult(int age)
        {
            return null; 
        }

        [Ensures(ParameterName = "age", ValidationMethod = nameof(IsOfChildAge), Throw = false, GetDefault = nameof(ErrorAge))]
        public string SayAsChild(int age, string str)
        {
            Validate(age, this, "age");
            return CallMethod<string>(null, age, str); 
        }
    }
}
