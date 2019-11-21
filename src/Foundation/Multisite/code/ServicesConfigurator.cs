using HelixExample.Foundation.Multisite.Placeholders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;

namespace HelixExample.Foundation.Multisite
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor
                .Singleton<BasePlaceholderCacheManager, SiteSpecificPlaceholderCacheManager>());
        }
    }
}