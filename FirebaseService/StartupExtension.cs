using FirebaseService.Interfaces;
using FirebaseService.Implements;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseService
{
    public static class StartupExtension
    {
        public static void UseFirebaseService(this IServiceCollection services) {

            services.AddTransient<IFirebaseStorageService, FirebaseStorageService>();
            services.AddTransient<IFirebaseAuthService, FirebaseAuthService>();
        }
    }
}
