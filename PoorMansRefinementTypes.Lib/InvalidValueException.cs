using System;
namespace PoorMansRefinementTypes.Lib
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException(string s) : base(s)
        {
        }
    }
}
