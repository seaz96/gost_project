using AutoMapper;
using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;

namespace GostStorage.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddOrUpdateDocumentRequest, PrimaryField>();
        CreateMap<AddOrUpdateDocumentRequest, Field>();

        CreateMap<PrimaryField, Field>().ReverseMap();
        CreateMap<ActualField, Field>().ReverseMap();
        
        CreateMap<Field, SearchDocument>();
        CreateMap<SearchEntity, GeneralDocumentInfoModel>()
            .ForMember(
                dest => dest.RelevanceMark,
                options => options.MapFrom(x => Math.Round(x.Score * 5)));

        CreateMap<Document, ReferenceDocumentResponse>();
        CreateMap<FullDocument, FullDocumentResponse>();
        CreateMap<FullDocument, GeneralDocumentInfoModel>()
            .ForMember(
                dest => dest.CodeOks,
                src => src.MapFrom(x => x.Primary.CodeOks))
            .ForMember(
                dest => dest.Designation,
                src => src.MapFrom(x => x.Primary.Designation))
            .ForMember(
                dest => dest.FullName,
                src => src.MapFrom(x => x.Primary.FullName))
            .ForMember(
                dest => dest.RelevanceMark,
                src => src.MapFrom(x => 5));
        CreateMap<Field, FieldResponse>();
        CreateMap<ActualField, FieldResponse>();
        CreateMap<PrimaryField, FieldResponse>();
        
    }
}