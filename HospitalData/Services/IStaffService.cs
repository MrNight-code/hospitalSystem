using HospitalData.DTOs;
using HospitalData.Models; 

namespace HospitalData.Services
{
    public interface IStaffService
    {
        // Regla 1: Debe haber una forma de obtener la lista de todos los doctores.
        Task<List<Doctor>> ObtenerDoctoresAsync();

        // Regla 2: Debe haber una forma de crear un nuevo doctor usando el DTO.
        Task CrearDoctorAsync(CreateDoctorDto nuevoDoctor);

        // Más adelante añadiremos aquí métodos para pacientes, inventario, etc.
        Task<List<Patient>> ObtenerPacientesAsync();
        Task CrearPacienteAsync(CreatePatientDto nuevoPaciente);

        Task<List<InventoryDto>> ObtenerInventarioAsync();
        Task CrearMedicamentoAsync(CreateMedicationDto nuevoMedicamento);
        Task ActualizarStockAsync(UpdateStockDto stockUpdate);
        Task<List<AppointmentDetailDto>> ObtenerCitasAsync();
    }
}