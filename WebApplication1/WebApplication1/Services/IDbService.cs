using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IDbService
{ 
    Task<CustomerPurchasesDto?> GetCustomerPurchasesAsync(int customerId);
    Task AddWashingMachineAsync(CreateWashingMachineRequest request);
}