using Autofac;

namespace AspNetCore.Autofac
{
    /// <summary>
    /// 依赖注入Container
    /// </summary>
    public class AutofacContainer
    {
        private static ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Current
        /// </summary>
        public static ILifetimeScope Current => _lifetimeScope;

        internal static void Register(ILifetimeScope autofacRoot)
        {
           _lifetimeScope = autofacRoot;
        }
    }
}
