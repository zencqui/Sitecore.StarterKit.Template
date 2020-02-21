using Sitecore.Data;

namespace HelixExample.Foundation.Multisite
{
    public class Templates
    {
        public struct Site
        {
            public static ID ID = new ID("{D3403FBE-068F-4F45-A762-7236EABBC573}");
        }

        public struct DatasourceConfiguration
        {
            public static ID ID = new ID("{AC93B000-052A-4958-8C43-2F87DBDB5ADF}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{F4B5F6B0-6B10-4E3E-83B7-D0522141154A}");
                public static readonly ID DatasourceTemplate = new ID("{C6D026D0-F108-4370-96AC-76FD910C9775}");
            }
        }

        public struct SiteSettings
        {
            public static ID ID = new ID("{E98CFBDD-011A-4FF4-942C-856CA9B7F6C0}");
        }

        public struct RenderingOptions
        {
            public static ID ID = new ID("{D1592226-3898-4CE2-B190-090FD5F84A4C}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{B5B27AF1-25EF-405C-87CE-369B3A004016}");
                public static readonly ID DatasourceTemplate = new ID("{1A7C85E5-DC0B-490D-9187-BB1DBCB4C72F}");
            }
        }
    }
}