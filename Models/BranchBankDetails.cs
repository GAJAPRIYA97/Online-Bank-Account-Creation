using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniBankApp.Models
{
    public class BranchBankDetails
    {
        public int Id { get; set; }  // Primary Key
        public string PreferredBranch { get; set; }
        public string HasOtherBankAccount { get; set; }
        public string IFSCCode { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
    }

}