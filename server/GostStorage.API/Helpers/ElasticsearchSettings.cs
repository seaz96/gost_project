using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace GostStorage.API.Helpers;

public static class ElasticsearchSettings
{
    public static ElasticsearchClientSettings GetSettings()
    {
        var password = Environment.GetEnvironmentVariable("ELASTIC_PASSWORD");
        
        return new ElasticsearchClientSettings(new Uri("http://elasticsearch:9200/"))
            .Authentication(new BasicAuthentication("elastic", password))
            .DefaultIndex("fields");
    }
}