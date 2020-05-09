using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wall_Assign.Models
    {
        public class LoginUser
        {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string LoginPassword {get;set;}
        }
    }