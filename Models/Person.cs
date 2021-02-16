using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace MVCwithLogin1.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string PerAd { get; set; }
        public string Photo { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
