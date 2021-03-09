using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringApps_WebApplication.Data
{
    public class ApplicationUser : IdentityUser
    {

        //[Required]
        [PersonalData]
        public String Address { get; set; }

        public bool IsModerator { get; set; }

    }
}
