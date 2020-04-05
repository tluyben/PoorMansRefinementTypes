using System;
namespace PoorMansRefinementTypes.Tests
{
    public interface IPersonTalk
    {
        string SayAsChild(int age, string str);
        string SayAsAdult(int age);
    }
}
