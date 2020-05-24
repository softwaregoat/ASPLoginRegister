using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP_WEB.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public List<Company> GetCompanies()
        {
            using(MyDatabaseEntities de = new MyDatabaseEntities())
            {
              return   de.Companies.ToList();
            }
        }
        public string GetCompany(int CompanyID)
        {
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                return de.Companies.Where(a=>a.CompanyID==CompanyID).FirstOrDefault().CompanyName;
            }

        }
    }
    public class UserMetadata
    {

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Minimum 6 characters required")]
        public string Password { get; set; }
    }
}