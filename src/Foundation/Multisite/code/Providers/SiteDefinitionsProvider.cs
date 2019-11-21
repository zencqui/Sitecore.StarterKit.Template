using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using HelixExample.Foundation.DependencyInjection;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace HelixExample.Foundation.Multisite.Providers
{
    [Service(typeof(ISiteDefinitionsProvider))]
    public class SiteDefinitionsProvider : ISiteDefinitionsProvider
    {
        private IEnumerable<SiteDefinition> siteDefinitions;
        private readonly IEnumerable<SiteInfo> _sites;

        public SiteDefinitionsProvider(Sitecore.Abstractions.BaseSiteContextFactory siteContextFactory)
        {
            _sites = siteContextFactory.GetSites();
        }

        public IEnumerable<SiteDefinition> SiteDefinitions =>
            this.siteDefinitions ?? (this.siteDefinitions = this._sites
                .Where(this.IsValidSite)
                .Select(this.Create)
                .OrderBy(s => s.Item.Appearance.Sortorder).ToArray());

        public SiteDefinition GetContextSiteDefinition(Item item)
        {
            return this.GetSiteByHierarchy(item) ?? this.SiteDefinitions.FirstOrDefault(x => x.IsCurrent);
        }

        private SiteDefinition GetSiteByHierarchy(Item item)
        {
            var siteItem = this.GetSiteItemByHierarchy(item);
            return siteItem == null ? null : this.SiteDefinitions.FirstOrDefault(x => x.Item.ID == siteItem.ID);
        }

        private Item GetSiteItemByHierarchy(Item item)
        {
            return item.Axes.GetAncestors().FirstOrDefault(IsSite);
        }

        private bool IsValidSite(SiteInfo site)
        {
            return this.GetSiteRootItem(site) != null;
        }

        private SiteDefinition Create(SiteInfo site)
        {
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }

            var siteItem = this.GetSiteRootItem(site);
            return new SiteDefinition
            {
                Item = siteItem,
                Name = site.Name,
                HostName = GetHostName(site),
                IsCurrent = this.IsCurrent(site),
                Site = site
            };
        }

        private bool IsCurrent(SiteInfo site)
        {
            return site != null && Context.Site != null && Context.Site.Name.Equals(site.Name, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetHostName(SiteInfo site)
        {
            if (!string.IsNullOrEmpty(site.TargetHostName))
            {
                return site.TargetHostName;
            }
            if (Uri.CheckHostName(site.HostName) != UriHostNameType.Unknown)
            {
                return site.HostName;
            }

            throw new ConfigurationErrorsException($"Cannot determine hostname for site '{site}'");
        }

        private Item GetSiteRootItem(SiteInfo site)
        {
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (string.IsNullOrEmpty(site.Database))
            {
                return null;
            }
            var database = Database.GetDatabase(site.Database);
            var item = database?.GetItem(site.RootPath);
            if (item == null || !IsSite(item))
            {
                return null;
            }
            return item;
        }

        private static bool IsSite(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return item.DescendsFrom(Templates.Site.ID);
        }
    }
}