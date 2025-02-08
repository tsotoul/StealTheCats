using Microsoft.Extensions.Options;
using StealTheCatsApi.Configuration;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Repositories.Interfaces;

namespace StealTheCatsApi.Repositories
{
    public class CatsApiRepository : ICatsApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public CatsApiRepository(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _appSettings = options.Value;
        }

        public async Task<IEnumerable<ApiCatDto>> GetCatsAsync(int numberOfCatsToFetch)
        {
            try
            {
                string url = $"{_appSettings.CatsApiUrl}?has_breeds=true&limit={numberOfCatsToFetch}";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add(_appSettings.ApiKey, _appSettings.ApiSecret);

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<ApiCatDto>>() ?? throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when trying to fetch the cats from the Api: " + e.Message);
            }
        }

        public async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            try
            {
                var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                return imageBytes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error downloading the image", ex);
            }
        }
    }
}
