using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("Purchase_History")]

[PrimaryKey(nameof(AvailableProgramId),nameof(CustomerId))]
public class PurchaseHistory
{
    [ForeignKey("AvailableProgram")]
    public int AvailableProgramId { get; set; }
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int? Rating { get; set; }
    
    public AvailableProgram AvailableProgram { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}