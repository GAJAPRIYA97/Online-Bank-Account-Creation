using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBankApp.Models
{
    [Table("BusinessProofs")]
    public class BusinessProof
    {
        public int Id { get; set; }
        public string ForexRequired { get; set; }
        public string GSTNumber { get; set; }
        public string UDYAMNumber { get; set; }

        // You can optionally add file path properties if you want to store file names in DB
        public string GSTFilePath { get; set; }
        public string UDYAMFilePath { get; set; }
        public string AdditionalFilePath { get; set; }
    }

}