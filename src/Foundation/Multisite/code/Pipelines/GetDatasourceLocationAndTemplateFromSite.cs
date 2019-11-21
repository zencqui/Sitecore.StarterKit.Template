using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelixExample.Foundation.DependencyInjection;
using HelixExample.Foundation.Multisite.Providers;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetRenderingDatasource;

namespace HelixExample.Foundation.Multisite.Pipelines
{
    [Service]
    public class GetDatasourceLocationAndTemplateFromSite
    {
        private readonly IDatasourceProvider _provider;

        public GetDatasourceLocationAndTemplateFromSite(IDatasourceProvider provider)
        {
            _provider = provider;
        }

        public void Process(GetRenderingDatasourceArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            var datasource = args.RenderingItem[Templates.RenderingOptions.Fields.DatasourceLocation];
            if (!DatasourceConfigurationService.IsSiteDatasourceLocation(datasource))
            {
                return;
            }

            this.ResolveDatasource(args);
            this.ResolveDatasourceTemplate(args);
        }

        private void ResolveDatasourceTemplate(GetRenderingDatasourceArgs args)
        {
            var contextItem = args.ContentDatabase.GetItem(args.ContextItemPath);
            var datasourceLocation = args.RenderingItem[Templates.RenderingOptions.Fields.DatasourceLocation];
            var name = DatasourceConfigurationService.GetSiteDatasourceConfigurationName(datasourceLocation);

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            args.Prototype = _provider.GetDatasourceTemplate(contextItem, name);
        }

        private void ResolveDatasource(GetRenderingDatasourceArgs args)
        {
            var contextItem = args.ContentDatabase.GetItem(args.ContextItemPath);
            var source = args.RenderingItem[Templates.RenderingOptions.Fields.DatasourceLocation];
            var name = DatasourceConfigurationService.GetSiteDatasourceConfigurationName(source);
            
            if(string.IsNullOrEmpty(name))
                return;

            var datasourceLocations = _provider.GetDatasourceLocations(contextItem, name);
            args.DatasourceRoots.AddRange(datasourceLocations);
        }
    }
}