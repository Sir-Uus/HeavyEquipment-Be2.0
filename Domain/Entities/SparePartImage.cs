using System;

namespace Domain.Entities;

public class SparePartImage
{
    public int Id { get; set; }
    public int SparePartId { get; set; }
    public string FileName { get; set; }
    public string ContenType { get; set; }
    public string Image { get; set; }
    public bool IsDeleted { get; set; }
    public SparePart SparePart { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
