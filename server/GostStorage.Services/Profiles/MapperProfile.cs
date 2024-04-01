using AutoMapper;
using GostStorage.Domain.Entities;
using GostStorage.Services.Models.Docs;

namespace GostStorage.Services.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddNewDocDtoModel, FieldEntity>();
        CreateMap<UpdateFieldDtoModel, FieldEntity>();
    }
}