using System;
namespace PoorMansRefinementTypes.Lib
{
    public class InvalidProxyException : Exception
    {
        public InvalidProxyException(string s) : base(s)
        {
        }
    }
}
