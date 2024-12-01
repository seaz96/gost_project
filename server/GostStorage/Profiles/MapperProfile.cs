using AutoMapper;
using GostStorage.Entities;
using GostStorage.Models.Docs;

namespace GostStorage.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddNewDocDtoModel, FieldEntity>();
        CreateMap<UpdateFieldDtoModel, FieldEntity>();
        CreateMap<FieldEntity, GostsFtsDocument>();
        CreateMap<FtsSearchEntity, ShortInfoDocumentModel>()
            .ForMember(
                dest => dest.RelevanceMark,
                options => options.MapFrom(x => Math.Round(x.Score * 5)));
    }
}