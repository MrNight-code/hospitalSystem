using System.ComponentModel.DataAnnotations;

namespace HospitalData.DTOs
{
    public class CreateMedicationDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad inicial no puede ser negativa")]
        public int InitialQuantity { get; set; } = 0;
    }
}