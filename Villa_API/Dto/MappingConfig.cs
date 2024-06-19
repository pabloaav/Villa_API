using AutoMapper;
using Villa_API.Models;

namespace Villa_API.Dto
{
   public class MappingConfig : Profile
   {
      public MappingConfig()
      {
         // se crean los mapeos de los objetos al dto y su inversa
         CreateMap<Villa, VillaDto>().ReverseMap();
         CreateMap<Villa, VillaCreateDto>().ReverseMap();
         CreateMap<Villa, VillaUpdateDto>().ReverseMap();
      }
   }
}
