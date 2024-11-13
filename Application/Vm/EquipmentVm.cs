namespace Application.Vm;

public class EquipmentVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string YearOfManufacture { get; set; }
    public string Specification { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }
    public decimal RentalPrice { get; set; }
    public bool IsDeleted { get; set; }
    public int Unit { get; set; }
    public List<FeedbackVm> PerformanceFeedbacks { get; set; }
    public List<MaintenanceRecordVm> MaintenancedRecords { get; set; }
    public List<RentalHistoryVm> RentalHistories { get; set; }
    public List<SparePartVm> SpareParts { get; set; }
    public ImagesVm Images { get; set; }
}
