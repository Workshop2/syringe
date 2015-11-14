﻿using System.ComponentModel.DataAnnotations;

namespace Syringe.Web.Models
{
    public class ParseResponseItem
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Regex { get; set; }
    }
}