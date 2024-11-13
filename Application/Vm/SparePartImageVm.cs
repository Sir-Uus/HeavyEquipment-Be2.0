using System;

namespace Application.Vm;

public class SparePartImageDetailsVm
{
    public int Id { get; set; }
    public int SparePartId { get; set; }
    public string FileName { get; set; }
    public string ContenType { get; set; }
    public string Image { get; set; }
}

public class SparePartImagesVm
{
    public int ImageId { get; set; }
    public string FileName { get; set; }
}
