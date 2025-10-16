namespace HospitalData.DTOs
{
    public class AuthenticatedUser
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}