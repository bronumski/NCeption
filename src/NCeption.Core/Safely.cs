using System;
using System.Linq.Expressions;

namespace NCeption
{
    public static class Safely
    {
        public static void Dispose(IDisposable objectToDispose)
        {
             Call(objectToDispose, disposable => disposable.Dispose());
        }

        public static void Shutdown(IStartableService serviceToShutdown)
        {
            Call(serviceToShutdown, startable => startable.Stop());
        }

        public static void Call<T>(T obj, Expression<Action<T>> action)
        {
            try
            {
                action.Compile()(obj);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to call '{0}' on object of type '{1}'\n{2}", action.Body, obj.GetType(), ex);
            }
        }

        public static void Call(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to call action\n{0}", ex);
            }
        }
    }
}