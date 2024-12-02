using App.Application.Interfaces;
using App.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace App.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            ///     variables
            ///     


            ////=======================================================================
            ///
            ///  INSTACIAS DE SERVICES
            /// 
            ///

            services.AddScoped<ICognitoService, CognitoService>();

           



        }
    }
}
