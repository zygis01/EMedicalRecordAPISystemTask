using System.ComponentModel.DataAnnotations;

namespace EMedicalRecordAPISystemTask.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extensions = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extensions.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return "This photo extension is not allowed!";
        }
    }
}
