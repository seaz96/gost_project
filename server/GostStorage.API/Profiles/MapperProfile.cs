using AutoMapper;
using GostStorage.API.Entities;
using GostStorage.API.Models.Docs;

namespace GostStorage.API.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddNewDocDtoModel, FieldEntity>();
        CreateMap<UpdateFieldDtoModel, FieldEntity>();
    }
}