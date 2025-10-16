using System;
using System.Collections.Generic;

namespace HospitalData.Models;

public partial class MedicalHistory
{
    public int HistoryId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public string? Description { get; set; }

    public DateTime VisitDate { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
