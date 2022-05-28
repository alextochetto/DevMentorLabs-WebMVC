using Core.Contract;
using Core.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreLocator
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUser, UserService>();
        }
    }
}
