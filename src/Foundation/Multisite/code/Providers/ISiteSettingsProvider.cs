using Sitecore.Data.Items;

namespace HelixExample.Foundation.Multisite.Providers
{
    public interface ISiteSettingsProvider
    {
        Item GetSetting(Item contextItem, string settingsName, string setting);
    }
}