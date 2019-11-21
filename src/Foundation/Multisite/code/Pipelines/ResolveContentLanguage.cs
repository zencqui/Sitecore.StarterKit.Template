using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;

namespace HelixExample.Foundation.Multisite.Pipelines
{
    public class ResolveContentLanguage : Sitecore.ExperienceEditor.Pipelines.HttpRequest.ResolveContentLanguage
    {
        private const string DefaultSiteSetting = "Preview.DefaultSite";
        public override void Process(HttpRequestArgs args)
        {
            if(Sitecore.Context.Item == null)
                return;

            var siteContext = LinkManager.GetPreviewSiteContext(Sitecore.Context.Item);

            using (new SettingsSwitcher(DefaultSiteSetting, siteContext.Name))
            {
                base.Process(args);
            }
        }
    }
}