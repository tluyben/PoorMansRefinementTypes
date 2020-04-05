using System;
using System.Diagnostics;
using System.Reflection;

namespace PoorMansRefinementTypes.Lib
{
    public class SuperProxy
    {

        protected static MemberInfo GetCallingMethodInfo()
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

        protected static T HandleEnsureAttribute<T>(MemberInfo caller, T a, object callingObject = null)
        {
            foreach (EnsuresAttribute ensure in caller.GetCustomAttributes<EnsuresAttribute>())
            {
                
                var validator = caller.DeclaringType.GetMethod(ensure.ValidationMethod, BindingFlags.Static|BindingFlags.Public);
                if (validator == null)
                {
                    throw new MissingMethodException($"Cannot find validation method {ensure.ValidationMethod} for attribute {ensure.GetType().Name} in for member {caller.Name} in class {caller.DeclaringType.Name}.");
                }

                // default args
                var args = new object[] { a };

                // if we have a calling object, let's see if we should pass it; 
                if (callingObject!=null)
                {
                    if (validator.GetParameters().Length > 1)
                    {
                        args = new object[] { a, callingObject }; 
                    }
                }

                var res = (bool)validator.Invoke(null, args);

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
                            return (T)defaultGenerator.Invoke(null, args); 
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

        public static int Validate(int a, object o = null)
        {
            return HandleEnsureAttribute<int>(GetCallingMethodInfo(), a, o); 
        }

        public static string Validate(string s, object o = null)
        {
            return HandleEnsureAttribute<string>(GetCallingMethodInfo(), s, o);
        }

        public static double Validate(double d, object o = null)
        {
            return HandleEnsureAttribute<double>(GetCallingMethodInfo(), d, o);
        }

        public static float Validate(float f, object o = null)
        {
            return HandleEnsureAttribute<float>(GetCallingMethodInfo(), f, o);
        }

        public static DateTime Validate(DateTime dt, object o = null)
        {
            return HandleEnsureAttribute<DateTime>(GetCallingMethodInfo(), dt, o);
        }

        public static bool Validate(bool b, object o = null)
        {
            return HandleEnsureAttribute<bool>(GetCallingMethodInfo(), b, o);
        }
    }
}
