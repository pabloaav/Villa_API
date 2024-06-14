using Villa_API.Dto;

namespace Villa_API.Store
{
    public static class VillaStore
    {
        //private static readonly VillaDto villaDto = new() { Id = 2, Nombre = "Vista a la Playa" };
        //private static readonly VillaDto villaDto1 = new() { Id = 1, Nombre = "Vista a la Piscina" };

        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new() {Id=1, Nombre="Vista a la Piscina"},
            new() {Id=2, Nombre="Vista a la Playa"}
        };
        
    }
}
