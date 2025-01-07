using System.Collections;
using System.Text;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using GostStorage.Repositories.Abstract;
using Newtonsoft.Json;
using Serilog;

namespace GostStorage.Repositories.Concrete;

public class SearchRepository(HttpClient httpClient, string ftsApiUrl) : ISearchRepository
{
    public async Task<List<SearchEntity>> SearchAsync(SearchQuery query)
    {
        var queryParams = CreateQuery(query);
        
        Log.Logger.Information($"Begin search request to indexes: {queryParams}");
        return await SendGetRequestAsync<List<SearchEntity>>($"{ftsApiUrl}/search?{queryParams}").ConfigureAwait(false) 
               ?? throw new InvalidOperationException();
    }
    
    public async Task<int> CountAsync(SearchQuery query)
    {
        var queryParams = CreateQuery(query);
        
        Log.Logger.Information($"Begin search request to indexes: {queryParams}");
        return await SendGetRequestAsync<int>($"{ftsApiUrl}/count?{queryParams}").ConfigureAwait(false);
    }

    public async Task IndexAllDocumentsAsync(List<SearchIndexModel> documents)
    {
        await Task.WhenAll(documents.Select(IndexDocumentAsync)).ConfigureAwait(false);
    }

    public async Task IndexDocumentAsync(SearchIndexModel document)
    {
         var request = new HttpRequestMessage(HttpMethod.Post, $"{ftsApiUrl}/index")
         {
             Content = new StringContent(
                 JsonConvert.SerializeObject(document),
                 Encoding.UTF8, 
                 "application/json")
         };
        
         Log.Logger.Information($"Indexing document {JsonConvert.SerializeObject(document)}");
         
         var response = await httpClient.SendAsync(request).ConfigureAwait(false);
         response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteDocumentAsync(long docId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{ftsApiUrl}/delete/{docId}");
        await httpClient.SendAsync(request).ConfigureAwait(false);
    }

    public async Task ChangeDocumentStatusAsync(long id, DocumentStatus status)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{ftsApiUrl}/update-status")
        {
            Content = new StringContent(
                JsonConvert.SerializeObject(new { Id = id, Status = status }),
                Encoding.UTF8, 
                "application/json")
        };
        
        await httpClient.SendAsync(request).ConfigureAwait(false);
    }

    private async Task<TResult?> SendGetRequestAsync<TResult>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request).ConfigureAwait(false);
        
        return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }

    private string CreateQuery(SearchQuery request)
    {
        var query = new StringBuilder();
        query.Append($"limit={request.Limit}&offset={request.Offset}");

        if (request.Text is not null)
            query.Append($"&text={request.Text}");
        
        var filters = request.SearchFilters;

        if (filters is null) 
            return query.ToString();
        
        if (filters.Status is not null) query.Append($"&SearchFilters.Status={filters.Status.ToString()}");
        if (filters.CodeOks is not null) query.Append($"&SearchFilters.CodeOks={filters.CodeOks}");
        if (filters.AcceptanceYear is not null) query.Append($"&SearchFilters.AcceptanceYear={filters.AcceptanceYear.ToString()}");
        if (filters.CommissionYear is not null) query.Append($"&SearchFilters.CommissionYear={filters.CommissionYear.ToString()}");
        if (filters.Author is not null) query.Append($"&SearchFilters.Author={filters.Author}");
        if (filters.AcceptedFirstTimeOrReplaced is not null) query.Append($"&SearchFilters.AcceptedFirstTimeOrReplaced={filters.AcceptedFirstTimeOrReplaced}");
        if (filters.KeyWords is not null) query.Append($"&SearchFilters.KeyWords={filters.KeyWords}");
        if (filters.AdoptionLevel is not null) query.Append($"&SearchFilters.AdoptionLevel={filters.AdoptionLevel.ToString()}");
        if (filters.Harmonization is not null) query.Append($"&SearchFilters.Harmonization={filters.Harmonization.ToString()}");
        if (filters.Amendments is not null) query.Append($"&SearchFilters.Amendments={filters.Amendments}");
        if (filters.Changes is not null) query.Append($"&SearchFilters.Changes={filters.Changes}");

        return query.ToString();
    }
}