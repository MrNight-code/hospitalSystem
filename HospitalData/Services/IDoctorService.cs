using HospitalData.DTOs;
using HospitalData.Models;

namespace HospitalData.Services
{
    /// <summary>
    /// Interfaz que define los métodos de servicio para las funcionalidades del Portal Médico.
    /// Este servicio será consumido por los componentes Blazor del rol "Doctor".
    /// </summary>
    public interface IDoctorService
    {
        // --- GESTIÓN DE AGENDA ---

        /// <summary>
        /// Obtiene la agenda de citas de un doctor específico desde la vista VwDoctorAgendaSummary.
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <returns>Lista de resúmenes de citas</returns>
        Task<List<VwDoctorAgendaSummary>> ObtenerAgendaAsync(int doctorId);

        // --- GESTIÓN DE PACIENTES ---

        /// <summary>
        /// Busca pacientes según criterios opcionales (ID, nombre, apellido).
        /// </summary>
        /// <param name="pacienteId">ID del paciente (opcional)</param>
        /// <param name="nombre">Nombre del paciente (opcional)</param>
        /// <param name="apellido">Apellido del paciente (opcional)</param>
        /// <returns>Lista de pacientes que coinciden con los criterios</returns>
        Task<List<Patient>> BuscarPacientesAsync(int? pacienteId, string? nombre, string? apellido);

        /// <summary>
        /// Obtiene la información completa de un paciente específico.
        /// </summary>
        /// <param name="pacienteId">ID del paciente</param>
        /// <returns>Datos del paciente o null si no existe</returns>
        Task<Patient?> ObtenerPacienteAsync(int pacienteId);

        // --- GESTIÓN DE HISTORIAL MÉDICO ---

        /// <summary>
        /// Obtiene todo el historial médico de un paciente ordenado por fecha (más reciente primero).
        /// </summary>
        /// <param name="pacienteId">ID del paciente</param>
        /// <returns>Lista de entradas del historial médico</returns>
        Task<List<MedicalHistory>> ObtenerHistorialMedicoAsync(int pacienteId);

        /// <summary>
        /// Registra una nueva entrada en el historial médico de un paciente.
        /// Puede utilizar el SP existente si está disponible, o inserción directa.
        /// </summary>
        /// <param name="pacienteId">ID del paciente</param>
        /// <param name="doctorId">ID del doctor que registra</param>
        /// <param name="diagnostico">Diagnóstico médico</param>
        /// <param name="tratamiento">Tratamiento prescrito</param>
        /// <param name="notas">Notas adicionales (opcional)</param>
        Task RegistrarHistorialAsync(int pacienteId, int doctorId, string diagnostico, string tratamiento, string? notas);

        // --- GESTIÓN DE PRESCRIPCIONES ---

        /// <summary>
        /// Obtiene las prescripciones activas de los pacientes atendidos por un doctor específico.
        /// Usa la vista VwPatientActivePrescription.
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <returns>Lista de prescripciones activas</returns>
        Task<List<VwPatientActivePrescription>> ObtenerPrescripcionesActivasAsync(int doctorId);

        /// <summary>
        /// Obtiene el detalle completo de una prescripción específica.
        /// </summary>
        /// <param name="prescripcionId">ID de la prescripción</param>
        /// <returns>Datos completos de la prescripción o null si no existe</returns>
        Task<Prescription?> ObtenerPrescripcionAsync(int prescripcionId);
    }
}
