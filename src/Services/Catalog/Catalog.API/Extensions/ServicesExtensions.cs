using Catalog.API.Data;
using Catalog.API.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddAppServicesExtensions(this IServiceCollection services)
        {
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
