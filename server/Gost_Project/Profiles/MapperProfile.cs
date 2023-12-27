using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;

namespace Gost_Project.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddNewDocDtoModel, FieldEntity>();
        CreateMap<UpdateFieldDtoModel, FieldEntity>();
    }
}