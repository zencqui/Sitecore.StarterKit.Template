using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelixExample.Foundation.DependencyInjection;
using Sitecore.Data.Items;

namespace HelixExample.Foundation.Multisite.Providers
{
    [Service(typeof(ISiteSettingsProvider))]
    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private readonly SiteContext _siteContext;

        public static string SettingsRootName => Sitecore.Configuration.Settings.GetSetting("Multisite.SettingsRootName", "settings");

        public SiteSettingsProvider(SiteContext siteContext)
        {
            _siteContext = siteContext;
        }

        public Item GetSetting(Item contextItem, string settingsName, string setting)
        {
            var settingsRootItem = this.GetSettingsRoot(contextItem, settingsName);
            var settingItem = settingsRootItem?.Children.FirstOrDefault(
                x => x.Key.Equals(setting.ToLower()));

            return settingItem;
        }

        private Item GetSettingsRoot(Item contextItem, string settingsName)
        {
            var currentDefinition = this._siteContext.GetSiteDefinition(contextItem);

            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var settingsFolder = definitionItem.Children[SettingsRootName];
            var settingsRootItem = settingsFolder?.Children.FirstOrDefault(
                x => x.DescendsFrom(
                    Templates.SiteSettings.ID) && x.Key.Equals(settingsName.ToLower()));

            return settingsRootItem;
        }
    }
}