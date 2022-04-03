using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    public class SPSettings
    {
        //User
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required Field!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required Field!")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Required Field!")]
        public string Email { get; set; }

        [RegularExpression(@"^[5-9]{1}[0-9]{9}$", ErrorMessage = "Please Enter a Valid 10 digit Mobile Number")]
        [Required(ErrorMessage = "Required Field!")]
        public string Mobile { get; set; }

        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UserProfilePicture { get; set; }


        //UserAddress
        public int AddressId { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }

        [RegularExpression("^[0-9]{6}$",ErrorMessage = "Enter 6 Digit Valid Postal Code!")]
        [Required]
        public string PostalCode { get; set; }
    }
}
