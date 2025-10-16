using System.ComponentModel.DataAnnotations;

namespace HospitalData.DTOs
{
    public class UpdateStockDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un medicamento válido")]
        public int MedicationId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad a añadir debe ser positiva")]
        public int QuantityToAdd { get; set; }
    }
}