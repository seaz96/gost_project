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
    }
}