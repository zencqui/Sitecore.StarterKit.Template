using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq.Expressions;
using Sitecore;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Foundation.Indexing.Models;
using Sitecore.Foundation.Indexing.Services;

namespace HelixExample.Foundation.LocalDatasource.Infrastructure.Indexing
{
    public class LocalDatasourceQueryPredicateProvider : ProviderBase, IQueryPredicateProvider
    {
        public Expression<Func<SearchResultItem, bool>> GetQueryPredicate(IQuery query)
        {
            var fieldNames = new[] { Templates.Index.Fields.LocalDatasourceContent_IndexFieldName };
            return GetFreeTextPredicateService.GetFreeTextPredicate(fieldNames, query);

        }

        public IEnumerable<ID> SupportedTemplates => new[] { TemplateIDs.StandardTemplate };
    }
}