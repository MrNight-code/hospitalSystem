using System;
using System.Collections.Generic;

namespace HospitalData.Models;

public partial class VwDoctorAgendaSummary
{
    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string PatientFirstName { get; set; } = null!;

    public string PatientLastName { get; set; } = null!;

    public string? PatientPhone { get; set; }
}
