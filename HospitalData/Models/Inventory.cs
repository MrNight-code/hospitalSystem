using System;
using System.Collections.Generic;

namespace HospitalData.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int MedicationId { get; set; }

    public int Quantity { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Medication Medication { get; set; } = null!;
}
