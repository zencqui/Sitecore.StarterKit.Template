using Sitecore.Configuration;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;

namespace HelixExample.Foundation.Multisite.Commands
{
    public class OpenExperienceEditor : Sitecore.Shell.Applications.WebEdit.Commands.OpenExperienceEditor
    {
        private const string DefaultSiteSetting = "Preview.DefaultSite";

        public new void Run(ClientPipelineArgs args)
        {
            var hostname = WebUtil.GetHostName();
            var site = Sitecore.Sites.SiteContextFactory.GetSiteContext(hostname, "/");
            var siteName = site?.Name ?? Sitecore.Configuration.Settings.Preview.DefaultSite;

            using (new SettingsSwitcher(DefaultSiteSetting, siteName))
            {
                base.Run(args);
            }
        }
    }
}