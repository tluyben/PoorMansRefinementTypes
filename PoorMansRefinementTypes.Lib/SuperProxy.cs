using System;
using System.Diagnostics;
using System.Reflection;

namespace PoorMansRefinementTypes.Lib
{
    public class SuperProxy
    {

        public static MemberInfo GetCallingMethodInfo()
        {
            var frames = new StackTrace(); 
            var method = frames.GetFrame(2).GetMethod();

            MemberInfo realMember = (MemberInfo)method;

            // it's a property
            if (method.Name.StartsWith("set_", StringComparison.InvariantCultureIgnoreCase))
            {
                realMember = method.DeclaringType.GetProperty(method.Name.Substring(4)); 
            }

            return realMember; 
        }

        public static T HandleEnsureAttribute<T>(MemberInfo caller, T a)
        {
            foreach (EnsuresAttribute ensure in caller.GetCustomAttributes<EnsuresAttribute>())
            {
                var validator = caller.DeclaringType.GetMethod(ensure.ValidationMethod, BindingFlags.Static|BindingFlags.Public);
                if (validator == null)
                {
                    throw new MissingMethodException($"Cannot find validation method {ensure.ValidationMethod} for attribute {ensure.GetType().Name} in for member {caller.Name} in class {caller.DeclaringType.Name}.");
                }

                var res = (bool)validator.Invoke(null, new object[] { a });

                if (!res)
                {
                    if (ensure.Throw)
                    {
                        throw new InvalidValueException($"Value {a} is not a valid value for member {caller.Name} according to {ensure.GetType().Name} in class {caller.DeclaringType.Name}"); 
                    }
                    else
                    {
                        if (ensure.GetDefault != null)
                        {
                            var defaultGenerator = caller.DeclaringType.GetMethod(ensure.GetDefault, BindingFlags.Static | BindingFlags.Public);
                            if (defaultGenerator == null)
                            {
                                throw new MissingMethodException($"Cannot find default generator method {ensure.GetDefault} for attribute {ensure.GetType().Name} in for member {caller.Name} in class {caller.DeclaringType.Name}.");
                            }
                            return (T)defaultGenerator.Invoke(null, new object[] { a }); 
                        }
                        else
                        {
                            return default(T);
                        }
                        
                    }
                }
            }

            return a; 
        }

        public static int Validate(int a)
        {
            return HandleEnsureAttribute<int>(GetCallingMethodInfo(), a); 
        }

        public static string Validate(string s)
        {
            return HandleEnsureAttribute<string>(GetCallingMethodInfo(), s);
        }

        public static double Validate(double d)
        {
            return HandleEnsureAttribute<double>(GetCallingMethodInfo(), d);
        }
    }
}
