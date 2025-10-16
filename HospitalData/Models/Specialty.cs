using System;
using System.Collections.Generic;

namespace HospitalData.Models;

public partial class Specialty
{
    public int SpecialtyId { get; set; }

    public string SpecialtyName { get; set; } = null!;
}
