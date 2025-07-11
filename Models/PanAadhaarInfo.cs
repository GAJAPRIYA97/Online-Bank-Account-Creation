using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBankApp.Models
{
    [Table("PanAadhaarInfo")] // Matches your SQL table name
    public class PanAadhaarInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string PAN { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        [MaxLength(12)]
        public string Aadhaar { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.Now;
    }
}
