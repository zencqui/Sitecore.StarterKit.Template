using System;
using HelixExample.Foundation.LocalDatasource.Extensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetRenderingDatasource;
using Sitecore.SecurityModel;

namespace HelixExample.Foundation.LocalDatasource.Pipelines
{
    public class GetLocalDatasourceLocation
    {
        public void process(GetRenderingDatasourceArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            CheckboxField datasource = args.RenderingItem
                .Fields[Templates.RenderingOptions.Fields.SupportsLocalDatasource];

            if(datasource == null || !datasource.Checked)
                return;

            var contextItem = args.ContentDatabase.GetItem(args.ContextItemPath);
            if(contextItem == null)
                return;

            var localDatasourceFolder = this.GetConfiguredLocalDatasourceFolder(contextItem, args.Prototype);
            if (localDatasourceFolder == null)
            {
                Log.Warn($"Cannot find the local datasource folder template '{Settings.LocalDatasourceFolderTemplate}'", this);
                return;
            }

            //Add the datasource folder to the top of the list to make it appear first in the dialog
            args.DatasourceRoots.Insert(0, localDatasourceFolder);

        }

        private Item GetConfiguredLocalDatasourceFolder(Item contextItem, Item datasourceTemplate)
        {
            using (new BulkUpdateContext())
            {
                var localDatasourceFolder = this.GetOrCreateLocalDatasourceFolder(contextItem);
                if (localDatasourceFolder == null)
                    return null;

                this.AddDatasourceTemplateToLocalDatasourceInsertOptions(localDatasourceFolder, datasourceTemplate);
                return localDatasourceFolder;

            }
        }

        private void AddDatasourceTemplateToLocalDatasourceInsertOptions(Item localDatasourceFolder, Item datasourceTemplate)
        {
            if (datasourceTemplate == null)
            {
                return;
            }

            var insertOptions = localDatasourceFolder[FieldIDs.Branches];
            // is the datasouce template already on the insert options?
            if(insertOptions.IndexOf(datasourceTemplate.ID.ToString(), StringComparison.InvariantCultureIgnoreCase) > -1)
                return;

            // otherwise it ads it
            using (new EditContext(localDatasourceFolder, SecurityCheck.Disable))
            {
                localDatasourceFolder[FieldIDs.Branches] =
                    insertOptions + (string.IsNullOrWhiteSpace(insertOptions) ? "" : "|") + datasourceTemplate.ID;
            }
        }

        private Item GetOrCreateLocalDatasourceFolder(Item contextItem)
        {
            return contextItem.GetLocalDatasourceFolder() ?? this.CreateLocalDatasourceFolder(contextItem);
        }

        private Item CreateLocalDatasourceFolder(Item contextItem)
        {
            var template = contextItem.Database.GetTemplate(Settings.LocalDatasourceFolderTemplate);
            if (template == null)
            {
                Log.Warn($"Cannot find the local datasource folder template '{Settings.LocalDatasourceFolderTemplate}'", this);
                return null;
            }

            using (new SecurityDisabler())
            {
                var datasourceFolder = contextItem.Add(Settings.LocalDatasourceFolderName, template);
                SetLocalDatasourceFolderSortOrder(datasourceFolder);
                return datasourceFolder;
            }

        }

        private static void SetLocalDatasourceFolderSortOrder(Item localDatasourceFolder)
        {
            using (new EditContext(localDatasourceFolder))
            {
                localDatasourceFolder.Appearance.Sortorder = -1000;
            }
        }
    }
}