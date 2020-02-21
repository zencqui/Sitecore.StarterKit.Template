using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Sitecore.Foundation.Indexing.Models
{
    public interface IQueryPredicateProvider
    {
        Expression<Func<SearchResultItem, bool>> GetQueryPredicate(IQuery query);
        IEnumerable<ID> SupportedTemplates { get; }
    }
}