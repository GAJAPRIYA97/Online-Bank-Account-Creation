
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace MiniBankApp.Models
    {
        [Table("CompanyProfiles")]

        public class CompanyProfile
        {
            public int Id { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            public string NatureOfBusiness { get; set; }

            [Required]
            public string SourceOfFunds { get; set; }

            [Required]
            [Range(0, double.MaxValue, ErrorMessage = "Turnover must be a positive number")]
            public decimal AnnualTurnover { get; set; }
        }
    }

