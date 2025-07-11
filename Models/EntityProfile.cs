using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBankApp.Models
{
    [Table("EntityProfiles")]

    public class EntityProfile
    {
        public int Id { get; set; }

        [Required]
        public string EntityName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime IncorporationDate { get; set; }

        [Required]
        public string IncorporationPlace { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CommencementDate { get; set; }
    }
}
