using System.Collections;
using System.Text;
using GostStorage.Models.Docs;
using GostStorage.Repositories.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace GostStorage.Repositories.Concrete;

public class FtsRepository(HttpClient httpClient, string ftsApiUrl) : ISearchRepository
{
    public async Task<List<FtsSearchEntity>?> SearchAsync(FtsSearchQuery query)
    {
        var queryParams = CreateQuery(query);
        
        Log.Logger.Information($"Begin search request to indexes: {queryParams}");
        return await SendGetRequestAsync<List<FtsSearchEntity>>($"{ftsApiUrl}/search?{queryParams}").ConfigureAwait(false);
    }

    //todo(azanov.n): убрать хардкод макс количества документов, когда фронт пофиксится
    public async Task<List<FtsSearchEntity>?> SearchAllAsync(int limit, int offset)
    {
        Log.Logger.Information($"Begin search-all request to indexes: {limit} offset: {offset}");
        return await SendGetRequestAsync<List<FtsSearchEntity>>($"{ftsApiUrl}/search-all?take=1000&skip={offset}")
            .ConfigureAwait(false);
    }

    public async Task IndexAllDocumentsAsync(List<FtsIndexModel> documents)
    {
        await Task.WhenAll(documents.Select(IndexDocumentAsync)).ConfigureAwait(false);
    }

    public async Task IndexDocumentAsync(FtsIndexModel document)
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
    
    private async Task<TResult?> SendGetRequestAsync<TResult>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request).ConfigureAwait(false);
        
        return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }

    private string CreateQuery(FtsSearchQuery request)
    {
        var query = new StringBuilder();
        query.Append($"take=10000&skip={request.Offset}");

        if (request.Text is not null)
            query.Append($"&text={request.Text}");

        var filters = request.SearchFilters;

        if (filters is null) 
            return query.ToString();
        
        var properties = filters.GetType().GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(filters, null) != null)
            .ToDictionary(x => x.Name, x => x.GetValue(filters, null));

        var propertyNames = properties
            .Where(x => !(x.Value is string) && x.Value is IEnumerable)
            .Select(x => x.Key)
            .ToList();

        foreach (var key in propertyNames)
        {
            var valueType = properties[key].GetType();
            var valueElemType = valueType.IsGenericType
                ? valueType.GetGenericArguments()[0]
                : valueType.GetElementType();
            if (valueElemType.IsPrimitive || valueElemType == typeof(string))
            {
                var enumerable = properties[key] as IEnumerable;
                properties[key] = string.Join(',', enumerable.Cast<object>());
            }

            query.Append(string.Join("&", properties
                .Select(x => string.Concat(
                    "SearchFilters.",
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString())))));
        }

        return query.ToString();
    }
}