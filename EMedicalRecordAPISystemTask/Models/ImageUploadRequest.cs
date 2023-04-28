using System.ComponentModel.DataAnnotations;

namespace EMedicalRecordAPISystemTask.Models
{
    public class ImageUploadRequest
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
