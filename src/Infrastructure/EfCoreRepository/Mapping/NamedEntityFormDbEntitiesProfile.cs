using AutoMapper;
using EfCoreRepository.Models;
using NlpService.Core.Models;

namespace EfCoreRepository.Mapping;

public class NamedEntityFormDbEntitiesProfile : Profile
{
    public NamedEntityFormDbEntitiesProfile()
    {
        CreateMap<NamedEntityForm, NamedEntityFormDbEntity>(MemberList.Destination)
            .ForMember(d => d.Id, options => options.MapFrom(s => s.Id))
            .ForMember(d => d.NamedEntity, options => options.MapFrom(s => s.NamedEntity))
            .ForMember(d => d.Value, options => options.MapFrom(s => s.Value))
            .ForMember(d => d.NewsIds, options => options.MapFrom(s => s.NewsIds));

        CreateMap<NamedEntityFormDbEntity, NamedEntityForm>(MemberList.Destination)
            .ForMember(d => d.Id, options => options.MapFrom(s => s.Id))
            .ForMember(d => d.NamedEntity, options => options.MapFrom(s => s.NamedEntity))
            .ForMember(d => d.Value, options => options.MapFrom(s => s.Value))
            .ForMember(d => d.NewsIds, options => options.MapFrom(s => s.NewsIds));

        CreateMap<NamedEntity, NamedEntityDbEntity>(MemberList.Destination)
            .ForMember(d => d.Id, options => options.MapFrom(s => s.Id))
            .ForMember(d => d.Value, options => options.MapFrom(s => s.Value))
            .ForMember(d => d.NamedEntityForms, options => options.MapFrom(s => s.NamedEntityForms));

        CreateMap<NamedEntityDbEntity, NamedEntity>(MemberList.Destination)
            .ForMember(d => d.Id, options => options.MapFrom(s => s.Id))
            .ForMember(d => d.Value, options => options.MapFrom(s => s.Value))
            .ForMember(d => d.NamedEntityForms, options => options.MapFrom(s => s.NamedEntityForms));

        CreateMap<NewsIdDbEntity, Guid>(MemberList.Destination)
            .ConvertUsing<NewsIdDbEntityToGuidConverter>();

        CreateMap<Guid, NewsIdDbEntity>(MemberList.Destination)
            .ConvertUsing<GuidToNewsIdDbEntityConverter>();
    }


}

public class NewsIdDbEntityToGuidConverter : ITypeConverter<NewsIdDbEntity, Guid>
{
    Guid ITypeConverter<NewsIdDbEntity, Guid>.Convert(NewsIdDbEntity source, Guid destination, ResolutionContext context)
    {
        return source.NewsId;
    }
}

public class GuidToNewsIdDbEntityConverter : ITypeConverter<Guid, NewsIdDbEntity>
{
    NewsIdDbEntity ITypeConverter<Guid, NewsIdDbEntity>.Convert(Guid source, NewsIdDbEntity destination, ResolutionContext context)
    {
        return new NewsIdDbEntity { NewsId = source };
    }
}