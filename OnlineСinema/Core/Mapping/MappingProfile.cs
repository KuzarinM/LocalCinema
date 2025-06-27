using AutoMapper;
using OnlineСinema.Models;
using OnlineСinema.Models.Dtos.Images;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Internal.Synchonisation;

namespace OnlineСinema.Core.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Episode, EpisodeDto>()
                ;

            CreateMap<Seasone, SeasonDto>()
                ;

            CreateMap<Title, TitleFullDto>()
                .ForMember(x=>x.Tags, opt => opt.MapFrom(x=>x.Tags.Select(x=>x.Name)))
                .ForMember(x=>x.Seasons, opt => opt.MapFrom(x=>x.Seasones))
                .ForMember(x => x.IsSeen, opt => opt.MapFrom(x => x.UserSeens.Count() > 0))
                ;

            CreateMap<Image, ImageDto>()
                .ForMember(x=>x.Extention, opt => opt.MapFrom(x=>x.fileExtention))
                ;

            CreateMap<Image, ImageFullDto>()
                .ForMember(x=>x.Data, opt=>opt.MapFrom(x=>new MemoryStream(x.Data)))
                .ForMember(x=>x.MediaType, opt => opt.MapFrom(x=>$"image/{x.fileExtention.Replace(".","")}"))
                ;

            CreateMap<Title, TitleDto>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(x => x.Tags.Select(x => x.Name)))
                .ForMember(x => x.SeasonesCount, opt => opt.MapFrom(x => x.Seasones.Count()))
                .ForMember(x => x.EpisodesCount, opt => opt.MapFrom(x => x.Seasones.Select(y=>y.Episodes.Count()).Sum()))
                .ForMember(x=>x.IsSeen, opt => opt.MapFrom(x=>x.UserSeens.Count()>0))
                ;

            CreateMap<MediaModel, Title>()
                .ForMember(x=>x.Id, opt => opt.MapFrom(x=>Guid.NewGuid()))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => "placeholder"))
                .ForMember(x => x.Isfilm, opt => opt.MapFrom(x => x.IsFilm))
                .ForMember(x=>x.Seasones, opt => opt.MapFrom(x=>x.Children))
                ;

            CreateMap<MediaModel, Seasone>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                .ForMember(x => x.Episodes, opt => opt.MapFrom(x => x.Children))
                ;

            CreateMap<MediaModel, Episode>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                ;

            CreateMap<string, Tag>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x))
                ;
        }
    }
}
