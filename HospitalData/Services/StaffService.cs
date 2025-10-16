using HospitalData.DTOs;
using HospitalData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para el método .Join
using System.Threading.Tasks;

namespace HospitalData.Services
{
    public class StaffService : IStaffService
    {
        private readonly HospitalDbContext _context;

        public StaffService(HospitalDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS DE GESTIÓN DE DOCTORES ---
        public async Task<List<Doctor>> ObtenerDoctoresAsync()
        {
            return await _context.Doctors
                                 .Include(d => d.Specialty)
                                 .ToListAsync();
        }

        public async Task CrearDoctorAsync(CreateDoctorDto nuevoDoctor)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC SP_CreateNewEntity
                    @FirstName = {nuevoDoctor.FirstName},
                    @LastName = {nuevoDoctor.LastName},
                    @Email = {nuevoDoctor.Email},
                    @Phone = {nuevoDoctor.Phone},
                    @EntityType = 'Medico',
                    @SpecialtyID = {nuevoDoctor.SpecialtyID}
            ");
        }

        // --- MÉTODOS DE GESTIÓN DE PACIENTES ---
        public async Task<List<Patient>> ObtenerPacientesAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task CrearPacienteAsync(CreatePatientDto nuevoPaciente)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC SP_CreateNewEntity
                    @FirstName = {nuevoPaciente.FirstName},
                    @LastName = {nuevoPaciente.LastName},
                    @Email = {nuevoPaciente.Email},
                    @Phone = {nuevoPaciente.Phone},
                    @EntityType = 'Paciente'
            ");
        }

        // --- NUEVOS MÉTODOS DE GESTIÓN DE INVENTARIO ---

        public async Task<List<InventoryDto>> ObtenerInventarioAsync()
        {
            // Usamos LINQ para hacer un "JOIN" en C# entre las tablas Medications e Inventory.
            // Esto nos permite combinar la información de ambas en un solo DTO.
            return await _context.Medications
                .Join(_context.Inventories,
                      med => med.MedicationId,
                      inv => inv.MedicationId,
                      (med, inv) => new InventoryDto
                      {
                          MedicationId = med.MedicationId,
                          Name = med.Name,
                          Description = med.Description,
                          Quantity = inv.Quantity,
                          LastUpdated = inv.LastUpdated ?? DateTime.MinValue
                      })
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task CrearMedicamentoAsync(CreateMedicationDto nuevoMedicamento)
        {
            // Paso 1: Crear la nueva entidad de Medicamento.
            var medication = new Medication
            {
                Name = nuevoMedicamento.Name,
                Description = nuevoMedicamento.Description
            };

            // Paso 2: Crear la nueva entidad de Inventario y asociarla con el medicamento.
            // EF Core gestionará automáticamente la clave externa (MedicationID).
            var inventoryItem = new Inventory
            {
                Medication = medication,
                Quantity = nuevoMedicamento.InitialQuantity,
                LastUpdated = DateTime.Now
            };

            // Paso 3: Añadir ambas entidades al contexto.
            _context.Medications.Add(medication);
            _context.Inventories.Add(inventoryItem);

            // Paso 4: Guardar los cambios. EF Core ejecutará esto como una transacción,
            // asegurando que ambas inserciones se realicen correctamente.
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarStockAsync(UpdateStockDto stockUpdate)
        {
            // Buscamos el artículo de inventario por su MedicationID.
            var inventoryItem = await _context.Inventories
                .FirstOrDefaultAsync(i => i.MedicationId == stockUpdate.MedicationId);

            if (inventoryItem != null)
            {
                // Si lo encontramos, actualizamos la cantidad y la fecha.
                inventoryItem.Quantity += stockUpdate.QuantityToAdd;
                inventoryItem.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            else
            {
                // Si no lo encontramos, lanzamos un error para notificar a la interfaz.
                throw new Exception("El medicamento no fue encontrado en el inventario.");
            }
        }

        public async Task<List<AppointmentDetailDto>> ObtenerCitasAsync()
        {
            return await _context.StaffAppointmentManagementView
                                .OrderByDescending(a => a.AppointmentDate)
                                .ToListAsync();
        }
    }
}