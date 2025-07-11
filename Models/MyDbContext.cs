using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace MiniBankApp.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("BankAccount") { }

        public DbSet<AccountSelection> AccountSelections { get; set; }
        public DbSet<KYCInfo> KYCInfos { get; set; }
        public DbSet<PanAadhaarInfo> PanAadhaarInfos { get; set; }
        public DbSet<IndividualProfile> IndividualProfiles { get; set; }
        public DbSet<BusinessProof> BusinessProofs { get; set; }
        public DbSet<EntityProfile> EntityProfiles { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }

        public DbSet<BranchBankDetails> BranchBankDetails { get; set; }

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<FundTransaction> FundTransactions { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
