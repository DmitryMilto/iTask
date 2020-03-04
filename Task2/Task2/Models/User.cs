using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class User : IdentityUser
    {

        public string Login { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DataRelease { get; set; }
        public bool Status { get; set; }
        public bool Check { get; set; }
        public bool Block { get; set; }
    }
}
