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


        //ServiceRequestAddress
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }

        //ServiceRequestExtra
        public List<int> ServiceExtraId { get; set; }
    }
}
