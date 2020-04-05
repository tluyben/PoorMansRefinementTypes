using System;
namespace PoorMansRefinementTypes.Lib
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReturnsAttribute : Attribute
    {
        public string ValidationMethod { get; set; }

    }
}
