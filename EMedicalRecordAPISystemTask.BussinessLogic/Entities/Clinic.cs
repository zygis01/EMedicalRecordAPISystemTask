using System.ComponentModel.DataAnnotations;

namespace EMedicalRecordAPISystemTask.Models
{
    public class Clinic
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
    }
}
