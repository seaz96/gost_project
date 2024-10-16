using Elastic.Clients.Elasticsearch;
using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Repositories.Interfaces;
using Serilog;

namespace GostStorage.Repositories.Concrete;

public class SearchRepository(ElasticsearchClient client, IConfiguration config) : ISearchRepository
{
    public async Task<SearchResponse<DocumentESModel>> SearchValidFieldsAsync(SearchParametersModel parameters, int limit, int offset)
    {
        var response = await client.SearchAsync<DocumentESModel>(s => s
            .Index(config.GetValue<string>("ELASTIC_INDEX")!)
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
                                                            m.Field(new Field("field.fullName")).Query(parameters.Name));
                                                }, s =>
                                                {
                                                    if (parameters.Name is not null)
                                                        s.Match(m =>
                                                            m.Field(new Field("field.designation")).Query(parameters.Name));
                                                }));
                    }
                    if (parameters.ActivityField is not null)
                        q.Match(m => m.Field(new Field("field.activityField")).Query(parameters.ActivityField));
                    if (parameters.Author is not null)
                        q.Match(m => m.Field(new Field("field.author")).Query(parameters.Author));
                    if (parameters.AcceptedFirstTimeOrReplaced is not null)
                        q.Match(m =>
                            m.Field(new Field("field.acceptedFirstTimeOrReplaced"))
                                .Query(parameters.AcceptedFirstTimeOrReplaced));
                    if (parameters.KeyWords is not null)
                        q.Match(m => m.Field(new Field("field.keyWords")).Query(parameters.KeyWords));
                    if (parameters.ApplicationArea is not null)
                        q.Match(m => m.Field(new Field("field.applicationArea")).Query(parameters.ApplicationArea));
                    if (parameters.Changes is not null)
                        q.Match(m => m.Field(new Field("field.changes")).Query(parameters.Changes));
                    if (parameters.Amendments is not null)
                        q.Match(m => m.Field(new Field("field.amendments")).Query(parameters.Amendments));
                }
                
            ));

        return response;
    }

    public async Task<SearchResponse<DocumentESModel>> SearchCanceledFieldsAsync(SearchParametersModel parameters, int limit, int offset)
    {
        var response = await client.SearchAsync<DocumentESModel>(s => s
            .Index(config.GetValue<string>("ELASTIC_INDEX")!)
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
                                                            m.Field(new Field("field.fullName")).Query(parameters.Name));
                                                }, s =>
                                                {
                                                    if (parameters.Name is not null)
                                                        s.Match(m =>
                                                            m.Field(new Field("field.designation")).Query(parameters.Name));
                                                }));
                    }
                    if (parameters.ActivityField is not null)
                        q.Match(m => m.Field(new Field("field.activityField")).Query(parameters.ActivityField));
                    if (parameters.Author is not null)
                        q.Match(m => m.Field(new Field("field.author")).Query(parameters.Author));
                    if (parameters.AcceptedFirstTimeOrReplaced is not null)
                        q.Match(m =>
                            m.Field(new Field("field.acceptedFirstTimeOrReplaced"))
                                .Query(parameters.AcceptedFirstTimeOrReplaced));
                    if (parameters.KeyWords is not null)
                        q.Match(m => m.Field(new Field("field.keyWords")).Query(parameters.KeyWords));
                    if (parameters.ApplicationArea is not null)
                        q.Match(m => m.Field(new Field("field.applicationArea")).Query(parameters.ApplicationArea));
                    if (parameters.Changes is not null)
                        q.Match(m => m.Field(new Field("field.changes")).Query(parameters.Changes));
                    if (parameters.Amendments is not null)
                        q.Match(m => m.Field(new Field("field.amendments")).Query(parameters.Amendments));
                    if (parameters.GetType().GetProperties()
                        .Where(pi => pi.PropertyType == typeof(string))
                        .Select(pi => pi.GetValue(parameters))
                        .All(value => value is null))
                        q.MatchAll(ma => ma.QueryName("MatchAll"));
                }
                
            ));

        return response;
    }

    public async Task<SearchResponse<DocumentESModel>> SearchAllAsync(int limit, int offset)
    {
        var response = await client.SearchAsync<DocumentESModel>(s => s
            .Index(config.GetValue<string>("ELASTIC_INDEX")!)
            .From(0)
            .Size(10000)
            .TrackScores()
            .Query(q => q.MatchAll(ma => ma.QueryName("MatchAll"))));

        return response;
    }

    public async Task IndexAllDocumentsAsync(List<DocumentWithFieldsModel> docs)
    {
        docs = docs.OrderBy(x => x.DocId).ToList();
        
        foreach (var d in from doc in docs let p = doc.Primary let a = doc.Actual let field = new FieldEntity
                 {
                     DocId = doc.DocId,
                     Designation = a.Designation ?? p.Designation,
                     FullName = a.FullName ?? p.FullName,
                     CodeOKS = a.CodeOKS ?? p.CodeOKS,
                     ApplicationArea = a.ApplicationArea ?? p.ApplicationArea,
                     Author = a.Author ?? p.Author,
                     AcceptedFirstTimeOrReplaced = a.AcceptedFirstTimeOrReplaced ?? p.AcceptedFirstTimeOrReplaced,
                     KeyWords = a.KeyWords ?? p.KeyWords,
                     Changes = a.Changes ?? p.Changes,
                     Amendments = a.Amendments ?? p.Amendments,
                     ActivityField = a.ActivityField ?? p.ActivityField,
                     AcceptanceYear = a.AcceptanceYear ?? p.AcceptanceYear,
                     CommissionYear = a.CommissionYear ?? p.CommissionYear,
                     Content = a.Content ?? p.Content,
                     DocumentText = a.DocumentText ?? p.DocumentText,
                     Harmonization = a.Harmonization ?? p.Harmonization,
                     AdoptionLevel = a.AdoptionLevel ?? p.AdoptionLevel,
                     Status = p.Status,
                     IsPrimary = true
                 } select new DocumentESModel{ Id = doc.DocId, Field = field, Data = "" })
        {
            Log.Logger.Information("Indexing document {docId}", d.Id);
            _ = await client.IndexAsync(d, x => x
                .Index(config.GetValue<string>("ELASTIC_INDEX")!)
                .Document(d)
                .Pipeline("attachment"));
        }
        
        Log.Logger.Information($"Indexed {docs.Count} documents");
    }

    public async Task IndexDocument(DocumentESModel document)
    {
        await client.IndexAsync(document, x => x.Document(document).Pipeline("attachment"));
    }
    
    public async Task IndexDocumentDataAsync(string data, long docId)
    {
        var doc = (await client.GetAsync<DocumentESModel>(new GetRequest("fields", new Id(docId)))).Source;
        doc.Data = data;
        await client.IndexAsync(doc, x => x
            .Index(config.GetValue<string>("ELASTIC_INDEX")!)
            .Document(doc)
            .Pipeline("attachment"));
    }

    public async Task DeleteDocumentAsync(long docId)
    {
        await client.DeleteAsync(config.GetValue<string>("ELASTIC_INDEX")!, docId);
    }
}