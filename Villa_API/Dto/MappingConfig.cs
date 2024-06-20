using AutoMapper;
using Villa_API.Models;

namespace Villa_API.Dto
{
   public class MappingConfig : Profile
   {
      public MappingConfig()
      {
         // se crean los mapeos de los objetos al dto y su inversa
         CreateMap<Villa, VillaDto>().ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name)).ReverseMap();
         CreateMap<Villa, VillaCreateDto>().ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name)).ReverseMap();
         CreateMap<Villa, VillaUpdateDto>().ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name)).ReverseMap();
      }
   }
}
