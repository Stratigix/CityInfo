using System.ComponentModel.DataAnnotations;

namespace CityInfo.ASP.Models
{
    public class PointOfInterestToUpdateDto
    {
        [Required(ErrorMessage = "The input should contain a name.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "That description is too long. Please keep it short.")]
        public string Description { get; set; }
    }
}
