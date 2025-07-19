using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Delivery;

public class DeliveryInfoCreateModel
{
    public string RecipientName { get; set; }
    //public long CityId { get; set; }
    public long PostDepartmentId { get; set; }
    public long PaymentTypeId { get; set; }
    public string PhoneNumber { get; set; }
}
