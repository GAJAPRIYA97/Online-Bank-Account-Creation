using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiniBankApp.Models
{
    [Table("FundTransactions")] //  This tells EF to look for your table

    public class FundTransaction
        {
            public int Id { get; set; }
            public string Scheme { get; set; }
            public decimal Amount { get; set; }
            public DateTime TransactionDate { get; set; }
        }

    
}