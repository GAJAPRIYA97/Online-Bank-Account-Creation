using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBankApp.Models
{
    [Table("IndividualProfiles")] //  This tells EF to look for your table
    public class IndividualProfile
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(50)]
        public string MotherMaidenName { get; set; }

        [Required]
        public string MaritalStatus { get; set; }

        [Required]
        public string AnnualIncome { get; set; }
    }
}
