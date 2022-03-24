using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    public class CustomModel
    {
        // ServiceRequest
        public int ServiceRequestId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public double ServiceHours { get; set; }
        public decimal TotalCost { get; set; }
        public string Comments { get; set; }
        public bool HasPets { get; set; }
        public int? Status { get; set; }
        public int? ServiceProviderId { get; set; }

        //ServiceRequestAddress
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }

        //ServiceRequestExtra
        public string ServiceExtraId { get; set; }

        //User
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SPFirstName { get; set; }
        public string SPLastName { get; set; }
        public string UserProfilePicture { get; set; }

        //Favourite and Blocked
        public bool IsBlocked { get; set; }
        public int Id { get; set; }

        //Ratings
        public int RatingId { get; set; }
        public decimal Ratings { get; set; }
        public DateTime RatingDate { get; set; }
        public decimal OnTimeArrival { get; set; }
        public decimal Friendly { get; set; }
        public decimal QualityOfService { get; set; }
    }
}
