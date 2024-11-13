namespace Application.Vm
{
    public class ImagesVm
    {
        public int ImageId { get; set; }
        public string FileName { get; set; }
    }

    public class ImageDetailsVm
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string FileName { get; set; }
        public string ContenType { get; set; }
        public string Image { get; set; }
    }
}
