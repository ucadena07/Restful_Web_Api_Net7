using System.ComponentModel.DataAnnotations;

namespace MagicVillaApi.Models.Dtos
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}
