using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using Nest;

namespace N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {
        private string IndexName { get; set; }
        private readonly IElasticClient _client;

        public ElasticSearchService(IElasticClient client)
        {
            _client = client;
        }

        public async Task<T> AddOrUpdate(T document)
        {
            var indexResponse =
            await _client.IndexAsync(document, idx => idx.Index(IndexName));

            if (!indexResponse.IsValid)
            {
                throw new Exception(indexResponse.DebugInformation);
            }

            return document;
        }

        public async Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents)
        {
            var indexResponse = await _client.BulkAsync(b => b
                .Index(IndexName)
                .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert()));

            return indexResponse;
        }
    }
}
