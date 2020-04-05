﻿using System;
namespace PoorMansRefinementTypes.Lib
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
    public class EnsuresAttribute : Attribute
    {
        public string ValidationMethod { get; set; }
        public bool Throw { get; set; } = true;
        public string GetDefault { get; set; } = null; 
    }
}
