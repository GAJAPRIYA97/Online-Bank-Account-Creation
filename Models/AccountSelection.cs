using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiniBankApp.Models
{
    [Table("AccountSelections")]
    public class AccountSelection
    {
        public int Id { get; set; }
        public string AccountType { get; set; }
        public DateTime SubmittedOn { get; set; }
    }
}