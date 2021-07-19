using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public double Distance { get; set; }

        public Trail.DifficultyType Difficulty { get; set; }

        [Required] public int NationalParkId { get; set; }

        public NationalParkDto NationalParkDto { get; set; }
    }
}