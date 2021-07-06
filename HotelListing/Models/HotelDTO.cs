using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Hotel name cannot exceed 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }
    }


    public class UpdateHotelDTO : CreateHotelDTO
    {

    }


}
