using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.ASP.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "The input should contain a name.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "That description is too long. Please keep it short.")]
        public string Description { get; set; }
    }
}
