using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiniBankApp.Models
{
    [Table("CustomerAccounts")] //  This tells EF to look for your table

    public class CustomerAccount
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}