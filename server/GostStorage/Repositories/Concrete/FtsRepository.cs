using System.Collections;
using GostStorage.Models.Docs;
using GostStorage.Repositories.Interfaces;
using Newtonsoft.Json;

namespace GostStorage.Repositories.Concrete;

public class FtsRepository(HttpClient httpClient, string ftsApiUrl) : ISearchRepository
{
    public async Task<List<FtsSearchEntity>?> SearchAsync(FtsSearchQuery query)
    {
        var queryParams = CreateQuery(query);
        return await SendGetRequestAsync<List<FtsSearchEntity>>($"{ftsApiUrl}/search?{queryParams}").ConfigureAwait(false);
    }

    public async Task<List<FtsSearchEntity>?> SearchAllAsync(int limit, int offset)
    {
        return await SendGetRequestAsync<List<FtsSearchEntity>>($"{ftsApiUrl}/search-all?limit={limit}&offset={offset}")
            .ConfigureAwait(false);
    }

    public async Task IndexAllDocumentsAsync(List<FtsIndexModel> documents)
    {
        await Task.WhenAll(documents.Select(IndexDocument)).ConfigureAwait(false);
    }

    public async Task IndexDocument(FtsIndexModel document)
    {
         var request = new HttpRequestMessage(HttpMethod.Post, $"{ftsApiUrl}/index")
         {
             Content = new StringContent(JsonConvert.SerializeObject(document))
         };
        
         await httpClient.SendAsync(request).ConfigureAwait(false);
    }
    
    public async Task DeleteDocumentAsync(long docId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{ftsApiUrl}/delete/{docId}");
        await httpClient.SendAsync(request).ConfigureAwait(false);
    }
    
    private async Task<TResult?> SendGetRequestAsync<TResult>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        
        var response = await httpClient.SendAsync(request).ConfigureAwait(false);
        
        return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }
    
    private string CreateQuery(object request, string separator = ",")
    {
        ArgumentNullException.ThrowIfNull(request);

        var properties = request.GetType().GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(request, null) != null)
            .ToDictionary(x => x.Name, x => x.GetValue(request, null));

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
            if (valueElemType.IsPrimitive || valueElemType == typeof (string))
            {
                var enumerable = properties[key] as IEnumerable;
                properties[key] = string.Join(separator, enumerable.Cast<object>());
            }
        }

        return string.Join("&", properties
            .Select(x => string.Concat(
                Uri.EscapeDataString(x.Key), "=",
                Uri.EscapeDataString(x.Value.ToString()))));
    }
}