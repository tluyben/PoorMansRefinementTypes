using System;
namespace PoorMansRefinementTypes.Lib
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class EnsuresAttribute : Attribute
    {
        public string ParameterName { get; set; } = null; 
        public string ValidationMethod { get; set; }
        public bool Throw { get; set; } = true;
        public string GetDefault { get; set; } = null; 
    }
}
