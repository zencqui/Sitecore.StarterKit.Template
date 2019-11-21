using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace HelixExample.Foundation.Multisite.Providers
{
    public interface IDatasourceProvider
    {
        Item[] GetDatasourceLocations(Item contextItem, string name);

        Item GetDatasourceTemplate(Item contextItem, string name);
    }
}