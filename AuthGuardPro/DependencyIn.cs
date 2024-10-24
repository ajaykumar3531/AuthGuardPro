using AuthGuardPro_Core;
using AuthGuardPro_Infrastucture;

namespace AuthGuardPro
{
    public static class DependencyIn
    {
        public static IServiceCollection ApplicationDI(this IServiceCollection services)
        {
            services.CoreDI();
            services.ApplicationDI();
            services.InfraDI();
            return services;
        }
    }
}
