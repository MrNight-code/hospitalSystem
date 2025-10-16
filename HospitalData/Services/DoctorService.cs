using HospitalData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalData.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly HospitalDbContext _context;

        public DoctorService(HospitalDbContext context)
        {
            _context = context;
        }

    public async Task<List<VwDoctorAgendaSummary>> GetMyAgendaAsync(int loggedInUserId)
    {
        // --- INICIO DEL CAMBIO ---

        // Paso 1: Usar el UserID del login para encontrar al doctor correspondiente en la tabla Doctors.
        var doctor = await _context.Doctors
                                .FirstOrDefaultAsync(d => d.UserId == loggedInUserId);

        // Paso 2: Si no encontramos un doctor (por si acaso), devolvemos una lista vacía.
        if (doctor == null)
        {
            return new List<VwDoctorAgendaSummary>(); // No hay doctor, no hay agenda.
        }

        // Paso 3: Ahora que tenemos el DoctorID correcto, lo usamos para filtrar la vista.
        int correctDoctorId = doctor.DoctorId;

        // --- FIN DEL CAMBIO ---

        return await _context.VwDoctorAgendaSummaries
                            .Where(cita => cita.DoctorId == correctDoctorId) // <-- Usamos el ID correcto
                            .OrderBy(cita => cita.AppointmentDate)
                            .ToListAsync();
    }
    }
}