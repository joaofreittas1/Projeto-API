using AutoMapper;
using MusicApi.Data.Dtos;
using MusicApi.Models;

namespace MusicApi.Profiles;

public class MusicProfile : Profile
{
    public MusicProfile() 
    {
        CreateMap<CreateMusicDto, Music>();
        CreateMap<UpdateMusicDto, Music>();
        CreateMap<Music, UpdateMusicDto>();
    }
}
