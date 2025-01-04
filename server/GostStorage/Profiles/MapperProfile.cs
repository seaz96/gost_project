using AutoMapper;
using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;

namespace GostStorage.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddDocumentRequest, Field>();
        CreateMap<UpdateDocumentRequest, Field>();
        CreateMap<Field, SearchDocument>();
        CreateMap<SearchEntity, ShortInfoDocumentModel>()
            .ForMember(
                dest => dest.RelevanceMark,
                options => options.MapFrom(x => Math.Round(x.Score * 5)));
    }
}