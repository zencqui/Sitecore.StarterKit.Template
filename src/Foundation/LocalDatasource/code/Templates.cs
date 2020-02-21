using Sitecore.Data;

namespace HelixExample.Foundation.LocalDatasource
{
    public class Templates
    {
        public struct RenderingOptions
        {
            public static ID ID = new ID("{D1592226-3898-4CE2-B190-090FD5F84A4C}");

            public struct Fields
            {
                public static readonly ID SupportsLocalDatasource = new ID("{BBF9CDA9-5436-4F9B-B0F3-E6EBE1F43725}");
            }
        }

        public struct Index
        {
            public struct Fields
            {
                public static readonly string LocalDatasourceContent_IndexFieldName = "local_datasource_content";
            }
        }
    }
}