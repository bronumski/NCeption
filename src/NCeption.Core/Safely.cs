using System;
using System.Linq.Expressions;
using Common.Logging;

namespace NCeption
{
    public static class Safely
    {
        private static ILog logger = LogManager.GetLogger(typeof(Safely));

        public static void Dispose(IDisposable disposable)
        {
             Call(disposable, d => d.Dispose());
        }

        public static void Call<T>(T obj, Expression<Action<T>> action)
        {
            try
            {
                action.Compile()(obj);
            }
            catch (Exception ex)
            {
                logger.Warn(string.Format("Failed to call '{0}'", action.Body), ex);
            }
        }

        public static void Call(Expression<Action> action)
        {
            try
            {
                action.Compile()();
            }
            catch (Exception ex)
            {
                logger.Warn(string.Format("Failed to call '{0}'", action.Body), ex);
            }
        }
    }
}