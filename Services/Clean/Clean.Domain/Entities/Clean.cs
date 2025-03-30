using System.ComponentModel.DataAnnotations.Schema;

namespace Clean.Domain.Entities;
public class Clean
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
}