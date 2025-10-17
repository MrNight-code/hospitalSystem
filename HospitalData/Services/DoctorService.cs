using HospitalData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalData.DTOs;

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

            var doctor = await _context.Doctors
                                    .FirstOrDefaultAsync(d => d.UserId == loggedInUserId);

            if (doctor == null)
            {
                return new List<VwDoctorAgendaSummary>();
            }

            int correctDoctorId = doctor.DoctorId;


            return await _context.VwDoctorAgendaSummaries
                                .Where(cita => cita.DoctorId == correctDoctorId)
                                .OrderBy(cita => cita.AppointmentDate)
                                .ToListAsync();
        }
        public async Task<Appointment?> GetAppointmentDetailsAsync(int appointmentId)
        {
            return await _context.Appointments
                                .Include(a => a.Patient)
                                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task CompleteAppointmentAsync(int appointmentId, string diagnosisNotes)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                throw new Exception("Cita no encontrada.");
            }

            var historyRecord = new MedicalHistory
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Description = diagnosisNotes,
                VisitDate = DateTime.Now
            };
            _context.MedicalHistories.Add(historyRecord);

            appointment.Status = "Completada";

            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC SP_CancelAppointment @AppointmentID={appointmentId}");
        }
        public async Task<List<MedicalHistoryDto>> GetMyMedicalHistoryAsync(int loggedInUserId)

        {
            var doctor = await _context.Doctors
                           .FirstOrDefaultAsync(d => d.UserId == loggedInUserId);

            if (doctor == null)
            {
                return new List<MedicalHistoryDto>(); 
            }
            return await _context.MedicalHistories
                .Where(mh => mh.DoctorId == doctor.DoctorId) 
                .OrderByDescending(mh => mh.VisitDate) 
                .Select(mh => new MedicalHistoryDto 
                {
                    HistoryID = mh.HistoryId,
                    VisitDate = mh.VisitDate,
                    Description = mh.Description,
                    PatientFirstName = mh.Patient.FirstName, 
                    PatientLastName = mh.Patient.LastName
                })
                .ToListAsync();
        }
    }
}