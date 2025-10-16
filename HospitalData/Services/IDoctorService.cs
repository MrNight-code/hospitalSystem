using HospitalData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalData.Services
{
    public interface IDoctorService
    {
        // Define una regla: cualquier DoctorService debe poder obtener
        // la agenda de un doctor específico, devolviendo una lista de VwDoctorAgendaSummary.
        Task<List<VwDoctorAgendaSummary>> GetMyAgendaAsync(int doctorId);
    }
}