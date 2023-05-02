namespace EMedicalRecordAPISystemTask.Interfaces
{
    public interface IJwtService
    {
        public string GetJwtTokenUser(string username);
        public string GetJwtTokenAdmin(string username, string role);
    }
}
