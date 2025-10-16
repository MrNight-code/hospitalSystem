using HospitalData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalData.DTOs;

namespace HospitalData.Services
{
    public interface IDoctorService
    {
        Task<List<VwDoctorAgendaSummary>> GetMyAgendaAsync(int doctorId);
        Task<Appointment?> GetAppointmentDetailsAsync(int appointmentId);
        Task CompleteAppointmentAsync(int appointmentId, string diagnosisNotes);
        Task CancelAppointmentAsync(int appointmentId);
        Task<List<MedicalHistoryDto>> GetMyMedicalHistoryAsync(int doctorId);
    }
}