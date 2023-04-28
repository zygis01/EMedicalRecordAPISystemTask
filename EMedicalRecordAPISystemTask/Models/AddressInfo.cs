using System.ComponentModel.DataAnnotations;

namespace EMedicalRecordAPISystemTask.Models
{
    public class AddressInfo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "City cannot be empty!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Street cannot be empty!")]
        public string Street { get; set; }
        [Required(ErrorMessage = "House number cannot be empty!")]
        public int HouseNum { get; set; }
        public int AppartmentNum { get; set; } = 0;
        public HumanInfo HumanInfo { get; set; }
    }
}
