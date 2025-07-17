using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Delivery;

[Table("tblPostDepartments")]
public class PostDepartmentEntity : BaseEntity<long>
{
    public string Name { get; set; } = string.Empty;

    [ForeignKey("City")]
    public long CityId { get; set; }
    public CityEntity? City { get; set; }

    public ICollection<DeliveryInfoEntity>? DeliveryInfos { get; set; }
}

