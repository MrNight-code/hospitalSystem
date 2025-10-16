using HospitalData.DTOs;
using HospitalData.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalData.Services
{
    /// <summary>
    /// Implementación del servicio para las funcionalidades del Portal Médico.
    /// Consume las Vistas y Stored Procedures existentes de la base de datos.
    /// </summary>
    public class DoctorService : IDoctorService
    {
        private readonly HospitalDbContext _context;

        public DoctorService(HospitalDbContext context)
        {
            _context = context;
        }

        // --- IMPLEMENTACIÓN: GESTIÓN DE AGENDA ---

        public async Task<List<VwDoctorAgendaSummary>> ObtenerAgendaAsync(int doctorId)
        {
            // Consulta directa a la vista VwDoctorAgendaSummary filtrada por DoctorId
            // Esta vista ya está mapeada en el DbContext como VwDoctorAgendaSummaries
            return await _context.VwDoctorAgendaSummaries
                .Where(v => v.DoctorId == doctorId)
                .OrderBy(v => v.AppointmentDate)
                .ToListAsync();
        }

        // --- IMPLEMENTACIÓN: GESTIÓN DE PACIENTES ---

        public async Task<List<Patient>> BuscarPacientesAsync(int? pacienteId, string? nombre, string? apellido)
        {
            // Construimos una query dinámica según los criterios proporcionados
            var query = _context.Patients.AsQueryable();

            if (pacienteId.HasValue)
            {
                query = query.Where(p => p.PatientId == pacienteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(p => p.FirstName.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(apellido))
            {
                query = query.Where(p => p.LastName.Contains(apellido));
            }

            return await query
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<Patient?> ObtenerPacienteAsync(int pacienteId)
        {
            // Obtiene un paciente específico por su ID
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == pacienteId);
        }

        // --- IMPLEMENTACIÓN: GESTIÓN DE HISTORIAL MÉDICO ---

        public async Task<List<MedicalHistory>> ObtenerHistorialMedicoAsync(int pacienteId)
        {
            // Obtiene todo el historial médico de un paciente
            // Incluye la información del doctor que realizó cada entrada
            return await _context.MedicalHistories
                .Include(h => h.Doctor)
                .Where(h => h.PatientId == pacienteId)
                .OrderByDescending(h => h.VisitDate)
                .ToListAsync();
        }

        public async Task RegistrarHistorialAsync(int pacienteId, int doctorId, string diagnostico, string tratamiento, string? notas)
        {
            // OPCIÓN 1: Si existe un Stored Procedure específico para esto, úsalo
            // Ejemplo: await _context.Database.ExecuteSqlInterpolatedAsync($@"
            //     EXEC SP_InsertMedicalHistory
            //         @PatientID = {pacienteId},
            //         @DoctorID = {doctorId},
            //         @Diagnosis = {diagnostico},
            //         @Treatment = {tratamiento},
            //         @Notes = {notas}
            // ");

            // OPCIÓN 2: Inserción directa usando EF Core (si no hay SP o prefieres esta forma)
            var nuevaEntrada = new MedicalHistory
            {
                PatientId = pacienteId,
                DoctorId = doctorId,
                Description = $"Diagnóstico: {diagnostico}",
                VisitDate = DateTime.Now,
                Diagnosis = diagnostico,
                Treatment = tratamiento,
                Notes = notas
            };

            _context.MedicalHistories.Add(nuevaEntrada);
            await _context.SaveChangesAsync();

            // NOTA: Si hay triggers en la tabla MedicalHistory, se ejecutarán automáticamente
        }

        // --- IMPLEMENTACIÓN: GESTIÓN DE PRESCRIPCIONES ---

        public async Task<List<VwPatientActivePrescription>> ObtenerPrescripcionesActivasAsync(int doctorId)
        {
            // Consulta la vista VwPatientActivePrescription
            // IMPORTANTE: Esta vista debe incluir una columna DoctorId o debemos hacer JOIN con Prescriptions

            // Si la vista ya incluye DoctorId (recomendado):
            // return await _context.VwPatientActivePrescriptions
            //     .Where(v => v.DoctorId == doctorId)
            //     .OrderByDescending(v => v.StartDate)
            //     .ToListAsync();

            // Si la vista NO incluye DoctorId, hacemos JOIN con la tabla Prescriptions:
            var prescripcionesActivas = await (
                from vw in _context.VwPatientActivePrescriptions
                join pr in _context.Prescriptions on vw.PrescriptionId equals pr.PrescriptionId
                join ap in _context.Appointments on pr.AppointmentId equals ap.AppointmentId
                where ap.DoctorId == doctorId
                orderby vw.StartDate descending
                select vw
            ).ToListAsync();

            return prescripcionesActivas;
        }

        public async Task<Prescription?> ObtenerPrescripcionAsync(int prescripcionId)
        {
            // Obtiene una prescripción específica con sus relaciones
            return await _context.Prescriptions
                .Include(p => p.Medication)
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.PrescriptionId == prescripcionId);
        }
    }
}
