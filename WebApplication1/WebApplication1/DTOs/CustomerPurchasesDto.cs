namespace WebApplication1.DTOs;

public class CustomerPurchasesDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public List<PurchaseDto> Purchases { get; set; }
}

public class PurchaseDto
{
    public DateTime Date { get; set; }
    public int? Rating { get; set; }
    public decimal Price { get; set; }
    public WashingMachineDto WashingMachine { get; set; }
    public ProgramDto Program { get; set; }
}

public class WashingMachineDto
{
    public string Serial { get; set; }
    public double MaxWeight { get; set; }
}

public class ProgramDto
{
    public string Name { get; set; }
    public int Duration { get; set; }
}
