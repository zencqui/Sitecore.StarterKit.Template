using System;
using HelixExample.Foundation.LocalDatasource.Services;
using Sitecore.Data.Items;
using Sitecore.Events;

namespace HelixExample.Foundation.LocalDatasource.Infrastructure.Events
{
    public class UpdateLocalDatasourceReferences
    {
        protected void OnItemCopied(object sender, EventArgs args)
        {
            var sourceItem = Event.ExtractParameter(args, 0) as Item;
            if (sourceItem == null)
                return;

            var targetItem = Event.ExtractParameter(args, 1) as Item;
            if(targetItem == null)
                return;

            new UpdateLocalDatasourceReferenceService(sourceItem, targetItem).UpdateAsync();
        }

        protected void OnItemAdded(object sender, EventArgs args)
        {
            var targetItem = Event.ExtractParameter(args, 0) as Item;
            if (targetItem?.Branch?.InnerItem.Children.Count != 1)
                return;

            var branchRoot = targetItem.Branch.InnerItem.Children[0];
            new UpdateLocalDatasourceReferenceService(branchRoot, targetItem).UpdateAsync();
        }
    }
}