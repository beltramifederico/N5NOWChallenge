using Nest;

namespace N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface
{
    public interface IElasticSearchService<T> where T : class
    {
        Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents);
        Task<T> AddOrUpdate(T document);
    }
}
