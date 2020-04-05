using System;
namespace PoorMansRefinementTypes.Lib
{
    /// <summary>
    /// These attributes indicate which exceptions can be thrown and will throw an exception if the
    /// throw exception is not in the list. 
    /// </summary>
    public class CanThrowAttribute
    {
        public string ExceptionClass { get; set; }
    }
}
