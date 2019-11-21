using System;
using HelixExample.Foundation.DependencyInjection;
using HelixExample.Foundation.Multisite.Providers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Sites;

namespace HelixExample.Foundation.Multisite
{
    [Service]
    public class SiteContext
    {
        private readonly ISiteDefinitionsProvider _siteDefinitionsProvider;

        public SiteContext(ISiteDefinitionsProvider siteDefinitionsProvider)
        {
            _siteDefinitionsProvider = siteDefinitionsProvider;
        }

        public virtual SiteDefinition GetSiteDefinition(Item item)
        {
            Assert.ArgumentNotNull(item, nameof(item));

            return _siteDefinitionsProvider.GetContextSiteDefinition(item);
        }

        public static implicit operator SiteContext(Sitecore.Sites.SiteContext v)
        {
            throw new NotImplementedException();
        }
    }
}