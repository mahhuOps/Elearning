using Core.Implements;
using Core.Interfaces;
using DatabaseService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class StartupExtension
    {
        public static void UseCore(this IServiceCollection services)
        {
            services.UseDatabaseService();
            services.AddTransient(typeof(IBaseBL<>), typeof(BaseBL<>));
        }
    }
}
