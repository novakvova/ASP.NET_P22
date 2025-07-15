using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Delivery;

[Table("tblPostDepartments")]
public class PostDepartmentEntity : BaseEntity<long>
{
    public string Name { get; set; } = string.Empty;

    public ICollection<DeliveryInfoEntity>? DeliveryInfos { get; set; }
}

