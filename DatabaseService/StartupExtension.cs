using DatabaseService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseService
{
    public static class StartupExtension
    {
        public static void UseDatabaseService(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseService, DatabaseService.Implements.DatabaseService>();
        }
    }
}
