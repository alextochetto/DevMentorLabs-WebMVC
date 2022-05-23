using Microsoft.Extensions.DependencyInjection;
using Web.Contract.Identity;
using Web.Service.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebLocator
    {
        public static void AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IIdentityAuthentication, IdentityAuthentication>();
        }
    }
}
