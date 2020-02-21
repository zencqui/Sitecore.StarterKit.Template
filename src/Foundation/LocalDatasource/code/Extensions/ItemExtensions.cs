using System;
using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace HelixExample.Foundation.LocalDatasource.Extensions
{
    public static class ItemExtensions
    {
        public static bool HasLayout(this Item item)
        {
            return item?.Visualization?.Layout != null;
        }

        public static Item[] GetLocalDatasourceDependencies(this Item item)
        {
            if (!item.HasLocalDatasourceFolder())
            {
                return new Item[] { };
            }

            var itemLinks = Globals.LinkDatabase.GetReferences(item)
                .Where(r => 
                    (r.SourceFieldID == Sitecore.FieldIDs.LayoutField || 
                     r.SourceFieldID == FieldIDs.FinalLayoutField && 
                     r.TargetDatabaseName == item.Database.Name));

            return itemLinks.Select(
                l => l.GetTargetItem())
                .Where(i => i != null && i.IsLocalDatasourceItem(item)).Distinct().ToArray();
        }

        public static bool IsLocalDatasourceItem(this Item datasourceItem, Item ofItem)
        {
            if (datasourceItem == null)
            {
                throw new ArgumentNullException(nameof(datasourceItem));
            }

            var datasourceFolder = ofItem.GetLocalDatasourceFolder();
            return datasourceFolder != null && datasourceItem.Axes.IsDescendantOf(datasourceFolder);
           
        }

        public static bool IsLocalDatasourceItem(this Item datasourceItem)
        {
            if(datasourceItem == null)
                throw new ArgumentNullException(nameof(datasourceItem));

            if (MainUtil.IsID(Settings.LocalDatasourceFolderTemplate))
            {
                return datasourceItem.Parent?.TemplateID
                           .Equals(ID.Parse(Settings.LocalDatasourceFolderTemplate)) ?? false;
            }

            return datasourceItem.Parent?.TemplateName.Equals(Settings.LocalDatasourceFolderTemplate,
                       StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        public static Item GetParentLocalDatasourceFolder(this Item datasourceItem)
        {
            if(datasourceItem == null)
                throw new ArgumentNullException(nameof(datasourceItem));

            var template = datasourceItem.Database.GetTemplate(Settings.LocalDatasourceFolderTemplate);
            if (template == null)
            {
                Log.Warn($"Cannot find the local datasource folder template '{Settings.LocalDatasourceFolderTemplate}'", datasourceItem);
                return null;
            }

            return datasourceItem.Axes.GetAncestors().LastOrDefault(i => i.DescendsFrom(template.ID));
        }

        public static Item GetLocalDatasourceFolder(this Item item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));

            return item.Children[Settings.LocalDatasourceFolderName];
        }

        public static bool HasLocalDatasourceFolder(this Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.Children[Settings.LocalDatasourceFolderName] != null;
        }
    }
}