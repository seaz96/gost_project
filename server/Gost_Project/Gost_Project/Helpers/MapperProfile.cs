using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Fields;

namespace Gost_Project.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddNewDocDto, FieldEntity>();
    }
}