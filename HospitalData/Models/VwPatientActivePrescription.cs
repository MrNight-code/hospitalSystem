using System;
using System.Collections.Generic;

namespace HospitalData.Models;

public partial class VwPatientActivePrescription
{
    public int PrescriptionId { get; set; }

    public int PatientId { get; set; }

    public string MedicationName { get; set; } = null!;

    public string? Dosage { get; set; }

    public int QuantityToDispense { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
