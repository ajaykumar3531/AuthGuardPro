using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Infrastucture
{
    public static class DependencyIn
    {
        public static IServiceCollection InfraDI(this IServiceCollection services)
        {

            return services;
        }
    }
}
