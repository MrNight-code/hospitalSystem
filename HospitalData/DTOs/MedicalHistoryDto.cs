namespace HospitalData.DTOs
{
    public class MedicalHistoryDto
    {
        public int HistoryID { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Description { get; set; }
        public string PatientFirstName { get; set; } = string.Empty;
        public string PatientLastName { get; set; } = string.Empty;
    }
}