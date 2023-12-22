using Microsoft.Extensions.DependencyInjection;
using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;
using N5NOW.UserPermissions.IntegrationTests.Helpers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace N5NOW.UserPermissions.IntegrationTests.Shared
{
    public abstract class BaseTest : IClassFixture<ServerHelper<Program>>, IDisposable
    {
        public readonly HttpClient _client;
        public readonly UserPermissionDbContext _context;

        public BaseTest(ServerHelper<Program> factory)
        {
            _client = new HttpClient();
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.BaseAddress = new Uri("https://localhost:7000/");
            _client.DefaultRequestVersion = new Version(1, 0);

            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<UserPermissionDbContext>();
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object request)
        {
            var serializedJson = JsonSerializer.Serialize(request);

            HttpContent stringContent = new StringContent(serializedJson, Encoding.UTF8, "application/json");

            return await _client.PostAsync(url, stringContent);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _client.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync($"{url}");
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object request)
        {
            var serializedJson = JsonSerializer.Serialize(request);
            HttpContent stringContent = new StringContent(serializedJson, Encoding.UTF8, "application/json");

            return await _client.PutAsync(url, stringContent);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
