using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using GostStorage.Domain.Entities;
using GostStorage.Domain.Models;
using GostStorage.Domain.Navigations;
using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Helpers;
using GostStorage.Services.Services.Abstract;

namespace GostStorage.Infrastructure.Repositories;

public class SearchRepository(IDocsService docsService) : ISearchRepository
{
    private readonly ElasticsearchClient _client = new(ElasticsearchSettings.GetSettings());
    private readonly IDocsService _docsService = docsService;
    
    public async Task<SearchResponse<FieldEntity>> SearchValidFieldsAsync<T>(SearchParametersModel parameters, int limit, int offset)
    {
        var response = await _client.SearchAsync<FieldEntity>(s => s
            .Index("fields")
            .From(0)
            .Size(10000)
            .TrackScores()
            .Query(q =>
                {
                    q.Bool(b => b.Filter(f => f.Term(t => t.Field("status").Value("valid"))));
                    if (parameters.Name is not null)
                    {
                        q.Bool(b => b.Should(s =>
                                                {
                                                    if (parameters.Name is not null)
                                                        s.Match(m =>
                                                            m.Field(new Field("fullName")).Query(parameters.Name));
                                                }, s =>
                                                {
                                                    if (parameters.Name is not null)
                                                        s.Match(m =>
                                                            m.Field(new Field("content")).Query(parameters.Name));
                                                }));
                    }
                    if (parameters.ActivityField is not null)
                        q.Match(m => m.Field(new Field("activityField")).Query(parameters.ActivityField));
                    if (parameters.Author is not null)
                        q.Match(m => m.Field(new Field("author")).Query(parameters.Author));
                    if (parameters.AcceptedFirstTimeOrReplaced is not null)
                        q.Match(m =>
                            m.Field(new Field("acceptedFirstTimeOrReplaced"))
                                .Query(parameters.AcceptedFirstTimeOrReplaced));
                    if (parameters.KeyWords is not null)
                        q.Match(m => m.Field(new Field("keyWords")).Query(parameters.KeyWords));
                    if (parameters.ApplicationArea is not null)
                        q.Match(m => m.Field(new Field("applicationArea")).Query(parameters.ApplicationArea));
                    if (parameters.Changes is not null)
                        q.Match(m => m.Field(new Field("changes")).Query(parameters.Changes));
                    if (parameters.Amendments is not null)
                        q.Match(m => m.Field(new Field("amendments")).Query(parameters.Amendments));
                    if (parameters.GetType().GetProperties()
                        .Where(pi => pi.PropertyType == typeof(string))
                        .Select(pi => pi.GetValue(parameters))
                        .All(value => value is null))
                        q.MatchAll(ma => ma.QueryName("MatchAll"));
                }
                
            ));

        return response;
    }

    public async Task<SearchResponse<FieldEntity>> SearchCanceledFieldsAsync<T>(SearchParametersModel parameters, int limit, int offset)
    {
        var response = await _client.SearchAsync<FieldEntity>(s => s
            .Index("fields")
            .From(0)
            .Size(10000)
            .TrackScores()
            .Query(q =>
                {
                    q.Bool(b => b.MustNot(mn => mn.Bool(bb => bb.Filter(f => f.Term(t => t.Field("status").Value("valid"))))));
                    if (parameters.Name is not null)
                    {
                        q.Bool(b => b.Should(s =>
                                                {
                                                    if (parameters.Name is not null)
                                                        s.Match(m =>
                                                            m.Field(new Field("fullName")).Query(parameters.Name));
                                                }, s =>
                                                {
                                                    if (parameters.Name is not null)
                                                        s.Match(m =>
                                                            m.Field(new Field("content")).Query(parameters.Name));
                                                }));
                    }
                    if (parameters.ActivityField is not null)
                        q.Match(m => m.Field(new Field("activityField")).Query(parameters.ActivityField));
                    if (parameters.Author is not null)
                        q.Match(m => m.Field(new Field("author")).Query(parameters.Author));
                    if (parameters.AcceptedFirstTimeOrReplaced is not null)
                        q.Match(m =>
                            m.Field(new Field("acceptedFirstTimeOrReplaced"))
                                .Query(parameters.AcceptedFirstTimeOrReplaced));
                    if (parameters.KeyWords is not null)
                        q.Match(m => m.Field(new Field("keyWords")).Query(parameters.KeyWords));
                    if (parameters.ApplicationArea is not null)
                        q.Match(m => m.Field(new Field("applicationArea")).Query(parameters.ApplicationArea));
                    if (parameters.Changes is not null)
                        q.Match(m => m.Field(new Field("changes")).Query(parameters.Changes));
                    if (parameters.Amendments is not null)
                        q.Match(m => m.Field(new Field("amendments")).Query(parameters.Amendments));
                    if (parameters.GetType().GetProperties()
                        .Where(pi => pi.PropertyType == typeof(string))
                        .Select(pi => pi.GetValue(parameters))
                        .All(value => value is null))
                        q.MatchAll(ma => ma.QueryName("MatchAll"));
                }
                
            ));

        return response;
    }

    public async Task IndexAllAsync()
    {
        
        var docs = _docsService.GetDocumentsAsync(new SearchParametersModel(), true, 100000, 0).Result;
        docs = docs.OrderBy(x => x.DocId).ToList();
        foreach (var doc in docs)
        {
            var p = doc.Primary;
            var a = doc.Actual;
            var response2 =
                await _client.IndexAsync(p, x => x.Document(p));
            var response3 = await _client.IndexAsync(a, x => x.Document(a));
        }
    }
}