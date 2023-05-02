using Microsoft.Extensions.Configuration;

namespace EMedicalRecordAPISystemTask.Options
{
    public class JwtOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Secret { get; set; }
        public JwtOptions(IConfiguration configuration)
        {
            ValidIssuer = configuration.GetValue<string>("JWT:ValidIssuer");
            ValidAudience = configuration.GetValue<string>("JWT:ValidAudience");
            Secret = configuration.GetValue<string>("JWT:Secret");
        }
    }
}
