using System.Linq;
using System.Text;
using HelixExample.Foundation.LocalDatasource.Extensions;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace HelixExample.Foundation.LocalDatasource.Infrastructure.Indexing
{
    public class LocalDatasourceContentField :  IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);
            if (item == null)
            {
                return null;
            }

            if (!this.ShouldIndexItem(item))
            {
                return null;
            }

            var datasrouces = item.GetLocalDatasourceDependencies();
            var result = new StringBuilder();
            foreach (var datasrouce in datasrouces)
            {
                datasrouce.Fields.ReadAll();
                foreach (var field in datasrouce.Fields.Where(this.ShouldIndexField))
                {
                    result.AppendLine(field.Value);
                }
            }

            return result.ToString();
        }

        private bool ShouldIndexField(Sitecore.Data.Fields.Field field)
        {
            return !field.Name.StartsWith("__") && this.IsTextField(field) && !string.IsNullOrEmpty(field.Value);
        }

        private bool IsTextField(Field field)
        {
            return IndexOperationsHelper.IsTextField((SitecoreItemDataField) field);
        }

        private bool ShouldIndexItem(Item item)
        {
            return item.HasLayout() && !item.Paths.LongID.Contains(ItemIDs.TemplateRoot.ToString());
        }


    }
}