namespace HospitalData.DTOs
{
    public class InventoryDto
    {
        public int MedicationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}