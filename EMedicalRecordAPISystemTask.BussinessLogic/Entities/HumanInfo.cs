using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EMedicalRecordAPISystemTask.BussinessLogic.Entities
{
    public class HumanInfo
    {
        [ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public double PersonalCode { get; set; }
        [Required]
        public double PhoneNum { get; set; }
        [Required]
        public string eMail { get; set; }
        public byte[] ProfilePic { get; set; }
        public User User { get; set; }
        public AddressInfo AddressInfo { get; set; }
    }
}
