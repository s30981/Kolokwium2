using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services;
public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<CustomerPurchasesDto?> GetCustomerPurchasesAsync(int customerId)
    {
            var customer = await _context.Customers
                .Where(c => c.Id == customerId)
                .Include(c => c.Purchases)
                .ThenInclude(p => p.WashingMachine)
                .Include(c => c.Purchases)
                .ThenInclude(p => p.Program)
                .FirstOrDefaultAsync();

            if (customer == null) return null;

            return new CustomerPurchasesDto
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Purchases = customer.Purchases.Select(p => new PurchaseDto
                {
                    Date = p.Date,
                    Rating = p.Rating,
                    Price = p.Price,
                    WashingMachine = new WashingMachineDto
                    {
                        SerialNumber = p.WashingMachine.Serial,
                        MaxWeight = p.WashingMachine.MaxWeight
                    },
                    Program = new ProgramDto
                    {
                        Name = p.Program.Name,
                        Duration = p.Program.Duration
                    }
                }).ToList()
            };
        }
   public async Task AddWashingMachineAsync(CreateWashingMachineRequest request)
    {
        if (request.WashingMachine.MaxWeight < 8)
            throw new BadRequestException("MaxWeight must be at least 8.");

        var serialNumber = request.WashingMachine.SerialNumber;

        var existing = await _context.WashingMachines.AnyAsync(wm => wm.SerialNumber == serialNumber);
        if (existing)
            throw new ConflictException("Washing machine with this serial number already exists.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var validProgramNames = await _context.AvailablePrograms
                .Select(p => p.Program.Name)
                .ToListAsync();

            var invalidPrograms = request.AvailablePrograms
                .Where(p => !validProgramNames.Contains(p.ProgramName))
                .ToList();

            if (invalidPrograms.Any())
            {
                var names = string.Join(", ", invalidPrograms.Select(p => p.ProgramName));
                throw new NotFoundException($"Invalid program(s): {names}");
            }

            if (request.AvailablePrograms.Any(p => p.Price > 25))
            {
                var tooExpensive = request.AvailablePrograms
                    .Where(p => p.Price > 25)
                    .Select(p => p.ProgramName);

                throw new BadRequestException($"Programs exceeding price limit: {string.Join(", ", tooExpensive)}");
            }

            var machine = new WashingMachine
            {
                SerialNumber = serialNumber,
                MaxWeight = request.WashingMachine.MaxWeight
            };

            _context.WashingMachines.Add(machine);
            await _context.SaveChangesAsync();

            foreach (var prog in request.AvailablePrograms)
            {
                var programEntity = await _context.AvailablePrograms.FirstAsync(p => p.Name == prog.ProgramName);

                _context.WashingMachines.Add(new WashingMachine
                {
                    WashingMachineId = machine.WashingMachineId,
                    ProgramId = programEntity.Id,
                    Price = prog.Price
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}




