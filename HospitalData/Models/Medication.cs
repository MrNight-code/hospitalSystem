using System;
using System.Collections.Generic;

namespace HospitalData.Models;

public partial class Medication
{
    public int MedicationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
