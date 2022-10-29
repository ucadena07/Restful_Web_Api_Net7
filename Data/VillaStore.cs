using MagicVillaApi.Models.Dtos;
using System.Xml.Linq;

namespace MagicVillaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>()
            {
                new VillaDto() { Id = 1, Name = "Villa 1"},
                new VillaDto() { Id = 2, Name = "Villa 2"}
            };
    }
}
