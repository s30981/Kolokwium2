public class WashingMachineDto
{
    public double MaxWeight { get; set; }
    public string SerialNumber { get; set; }
}

public class WashingProgramDto
{
    public string ProgramName { get; set; }
    public double Price { get; set; }
}

public class CreateWashingMachineRequest
{
    public WashingMachineDto WashingMachine { get; set; }
    public List<WashingProgramDto> AvailablePrograms { get; set; }
}