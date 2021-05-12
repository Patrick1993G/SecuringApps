using Microsoft.AspNetCore.Identity;
using System;

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
