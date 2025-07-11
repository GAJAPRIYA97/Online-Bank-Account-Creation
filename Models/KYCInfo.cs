using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBankApp.Models
{
    [Table("KYCInfo")] // This tells EF to map to the correct table name
    public class KYCInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string CaptchaEntered { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.Now;
    }
}
