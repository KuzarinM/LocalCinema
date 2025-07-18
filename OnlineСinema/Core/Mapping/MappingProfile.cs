using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Models;
using OnlineСinema.Models.Dtos.Images;
using OnlineСinema.Models.Dtos.Tags;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Dtos.Users;
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
                .ForMember(x=>x.LastSceenEpisode, opt => opt.MapFrom(x=>GetLastSceenData(x)))
                .ForMember(x => x.IsSeen, opt => opt.MapFrom(x
                    => x.UserSeens.Count() > 0
                    || x.Seasones.Any(y
                        => y.Episodes.Any(z
                            => z.UserSeens.Count() > 0)
                        )
                    )
                )
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
                .ForMember(x=>x.IsSeen, opt => opt.MapFrom(x
                    =>x.UserSeens.Count()>0 
                    || x.Seasones.Any(y
                        =>y.Episodes.Any(z
                            =>z.UserSeens.Count()>0)
                        )
                    )
                )
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

            CreateMap<Tag, TagDto>()
                .ForMember(x => x.Lable, opt => opt.MapFrom(x => x.Name))
                ;

            CreateMap<Episode, TitleVideoInformaionDto>()
                .ForMember(x => x.TitleId, opt => opt.MapFrom(x => x.Seasone.Title.Id))
                .ForMember(x => x.TitleName, opt => opt.MapFrom(x => x.Seasone.Title.Name))
                .ForMember(x => x.EpisodeName, opt => opt.MapFrom(x => $"{x.Seasone.Name} {x.Name}"))
                .ForMember(x => x.NextId, opt => opt.MapFrom(x => GetNextId(x)))
                .ForMember(x => x.PreveousId, opt => opt.MapFrom(x => GetPreveousId(x)))
                .ForMember(x => x.IsSceen, opt => opt.MapFrom(x => x.UserSeens.Count() > 0))
                ;

            CreateMap<Title, TitleVideoInformaionDto>()
                .ForMember(x => x.TitleId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.TitleName, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.EpisodeName, opt => opt.MapFrom(x => ""))
                .ForMember(x => x.IsSceen, opt => opt.MapFrom(x => x.UserSeens.Count() > 0))
                ;

            CreateMap<IdentityUser, UserViewDto>()
                .ForMember(x=>x.Login, opt=>opt.MapFrom(x=>x.UserName))
                .ForMember(x=>x.IsActive, opt => opt.MapFrom(x => x.EmailConfirmed))
                ;
        }

        private Guid? GetNextId(Episode episode)
            => episode.Seasone.Episodes
                    .OrderBy(x => x.Orderindex)
                    .FirstOrDefault(y => y.Orderindex > episode.Orderindex)?.Id
                    ?? episode.Seasone.Title.Seasones
                    .OrderBy(x => x.Orderindex)
                    .FirstOrDefault(y => y.Orderindex > episode.Seasone.Orderindex)?.Episodes.
                        OrderBy(x => x.Orderindex).FirstOrDefault()?.Id;

        private Guid? GetPreveousId(Episode episode)
            => episode.Seasone.Episodes
                    .OrderByDescending(x => x.Orderindex)
                    .FirstOrDefault(y => y.Orderindex < episode.Orderindex)?.Id
                    ?? episode.Seasone.Title.Seasones
                    .OrderByDescending(x => x.Orderindex)
                    .FirstOrDefault(y => y.Orderindex < episode.Seasone.Orderindex)?.Episodes.
                        OrderByDescending(x => x.Orderindex).FirstOrDefault()?.Id;

        private EpisodeSmallDto? GetLastSceenData(Title title)
        {
            if(title.Seasones.Count == 0)
            {
                return null;
            }
                 
            var data = title.Seasones
                            .SelectMany(y =>
                            y.Episodes
                                .Where(z => z.UserSeens.Count() > 0)
                                .Select(z =>
                                    new EpisodeSmallDto()
                                    {
                                        SeasoneId = y.Id,
                                        SeasoneOrderIndex = y.Orderindex,
                                        SeasoneName = y.Name,
                                        EpisodeId = z.Id,
                                        EpisodeOrderIndex = z.Orderindex,
                                        EpisodeName = z.Name,
                                    }
                                )
                            )
                        .OrderByDescending(x => x.SeasoneOrderIndex + 1000 + x.EpisodeOrderIndex)
                        .FirstOrDefault();

            return data;
        }

    }
}
