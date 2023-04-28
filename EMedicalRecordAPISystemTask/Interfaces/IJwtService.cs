namespace EMedicalRecordAPISystemTask.Interfaces
{
    public interface IJwtService
    {
        public string GetJwtToken(string username);
    }
}
