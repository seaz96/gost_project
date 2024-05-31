using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace GostStorage.Infrastructure.Helpers;

public static class ElasticsearchSettings
{
    public static ElasticsearchClientSettings GetSettings()
    {
        return new ElasticsearchClientSettings(new Uri("http://localhost:9200/"))
            .Authentication(new ApiKey("XzhCSXlJOEJja09OTXRrQmpUSGQ6WGsySnZ5T0lUdk9qbUh0UHBaS0x4QQ=="))
            .DefaultIndex("index");
    }
}