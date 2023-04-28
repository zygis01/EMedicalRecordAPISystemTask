namespace EMedicalRecordAPISystemTask.Options
{
    public class JwtOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Secret { get; set; }
        public int ExpirationHours { get; set; }
        public JwtOptions(IConfiguration configuration)
        {
            Secret = configuration.GetValue<string>("JWT:Secret");
        }
    }
}
