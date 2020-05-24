using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace ASP_WEB.Models
{
    public class UserLogin
    {
        [Display(Name ="Email")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Email required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Password required")]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }

    }
}