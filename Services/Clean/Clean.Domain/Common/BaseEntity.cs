using System.ComponentModel.DataAnnotations.Schema;

namespace Clean.Domain.Common;

public class BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}