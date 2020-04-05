using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace PoorMansRefinementTypes.Lib
{
    public class SuperProxy
    {
        // NOT THREAD SAFE (obviously) 
        private Dictionary<string, object> parameters = new Dictionary<string, object>();


        private static MemberInfo GetCallingMethodInfo(int si = 0)
        {
            var frames = new StackTrace();
            var method = frames.GetFrame(2+si).GetMethod();

            MemberInfo realMember = (MemberInfo)method;

            // it's a property
            if (method.Name.StartsWith("set_", StringComparison.InvariantCultureIgnoreCase))
            {
                realMember = method.DeclaringType.GetProperty(method.Name.Substring(4)); 
            }

            return realMember; 
        }

        protected static T HandleEnsureAttribute<T>(MemberInfo caller, T a, object callingObject = null, string name = null)
        {
            foreach (EnsuresAttribute ensure in caller.GetCustomAttributes<EnsuresAttribute>())
            {

                if (ensure.ParameterName!=null && !(callingObject is SuperProxy))
                {
                    throw new InvalidProxyException($"Trying to assign a method parameter with name {ensure.ParameterName} from a class that does not inherit from {nameof(SuperProxy)}"); 
                }
                
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
                    var param = ensure.ParameterName != null ? $", parameter {ensure.ParameterName}," : "";

                    if (ensure.Throw)
                    {
                        throw new InvalidValueException($"Value {a} is not a valid value for member {caller.Name}{param} according to {ensure.GetType().Name} in class {caller.DeclaringType.Name}"); 
                    }
                    else
                    {
                        if (ensure.GetDefault != null)
                        {
                            var defaultGenerator = caller.DeclaringType.GetMethod(ensure.GetDefault, BindingFlags.Static | BindingFlags.Public);
                            if (defaultGenerator == null)
                            {
                                throw new MissingMethodException($"Cannot find default generator method {ensure.GetDefault} for attribute {ensure.GetType().Name} in for member {caller.Name}{param} in class {caller.DeclaringType.Name}.");
                            }

                            if (callingObject != null)
                            {
                                if (defaultGenerator.GetParameters().Length > 1 && args.Length==1)
                                {
                                    args = new object[] { a, callingObject };
                                }
                            }

                            a = (T)defaultGenerator.Invoke(null, args); 
                        }
                        else
                        {
                            a = default(T);
                        }
                        
                    }
                }

                // collect the parameters
                if (ensure.ParameterName != null)
                {
                    ((SuperProxy)callingObject).parameters[ensure.ParameterName] = a;
                }

                // we cannot continue as new, default  value might impact further runs, so
                // for now we quit on the first issue
                if (!res)
                {
                    break; 
                }
            }

           
            return a; 
        }

        public void SetUnguarded(object o)
        {
            foreach(var p in o.GetType().GetProperties())
            {
                parameters[p.Name] = p.GetValue(this); 
            }
        }

        public R CallMethod<R>(object nonProxyObject, params object[] args)
        {
            return (R)CallMethod(nonProxyObject, args); 
        }


        protected object CallMethod(object nonProxyObject, params object[] args)
        {
            var m = GetCallingMethodInfo(1);

            if (!(m is MethodBase))
            {
                // this cannot happen 
            }

            var method = (MethodBase)m; 

            // get the *non* proxy version; this is very inefficient and you usually want to pass the actual object along anyway.
            if (nonProxyObject == null)
            {
                var lookingFor = m.DeclaringType.Name.Replace("Proxy", "");
                Type t = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.DefinedTypes).SelectMany(a => a).First(a => a.Name == lookingFor); 
                nonProxyObject = Activator.CreateInstance(t); 
            }

            // match up the parameters
            var ps = method.GetParameters(); 
            var finalArgs = new object[ps.Length];

            var count = 0; 
            foreach(var p in ps)
            {
                finalArgs[count] = parameters.ContainsKey(p.Name) ? parameters[p.Name] : args[count];
                count++; 
            }

            // now we can invoke;
            return nonProxyObject.GetType().GetMethod(m.Name).Invoke(nonProxyObject, finalArgs); 
        }

        public static int Validate(int a, object o = null, string name = null)
        {
            return HandleEnsureAttribute<int>(GetCallingMethodInfo(), a, o, name); 
        }

        public static string Validate(string s, object o = null, string name = null)
        {
            return HandleEnsureAttribute<string>(GetCallingMethodInfo(), s, o, name);
        }

        public static double Validate(double d, object o = null, string name = null)
        {
            return HandleEnsureAttribute<double>(GetCallingMethodInfo(), d, o, name);
        }

        public static float Validate(float f, object o = null, string name = null)
        {
            return HandleEnsureAttribute<float>(GetCallingMethodInfo(), f, o, name);
        }

        public static DateTime Validate(DateTime dt, object o = null, string name = null)
        {
            return HandleEnsureAttribute<DateTime>(GetCallingMethodInfo(), dt, o, name);
        }

        public static bool Validate(bool b, object o = null, string name = null)
        {
            return HandleEnsureAttribute<bool>(GetCallingMethodInfo(), b, o, name);
        }
    }
}
