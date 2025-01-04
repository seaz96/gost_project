using AutoMapper;
using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;

namespace GostStorage.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddDocumentRequest, PrimaryField>();
        CreateMap<UpdateDocumentRequest, Field>();

        CreateMap<PrimaryField, Field>().ReverseMap();
        CreateMap<ActualField, Field>().ReverseMap();
        
        CreateMap<Field, SearchDocument>();
        CreateMap<SearchEntity, ShortInfoDocumentModel>()
            .ForMember(
                dest => dest.RelevanceMark,
                options => options.MapFrom(x => Math.Round(x.Score * 5)));
    }
}