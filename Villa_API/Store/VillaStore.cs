using Villa_API.Dto;

namespace Villa_API.Store
{
    public static class VillaStore
    {
            public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1, Nombre="Vista a la Piscina"},
            new VillaDto{Id=2, Nombre="Vista a la Playa"}
        };
        
    }
}
