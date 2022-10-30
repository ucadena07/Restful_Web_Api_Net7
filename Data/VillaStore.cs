using MagicVillaApi.Models.Dtos;
using System.Xml.Linq;

namespace MagicVillaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>()
            {
                new VillaDto() { Id = 1, Name = "Villa 1",Occupancy =2,Sqft=1000},
                new VillaDto() { Id = 2, Name = "Villa 2",Occupancy = 4, Sqft = 2000}
            };
    }
}
