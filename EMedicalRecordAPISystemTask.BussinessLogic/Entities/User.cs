﻿using System.ComponentModel.DataAnnotations;

namespace EMedicalRecordAPISystemTask.BussinessLogic.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "User";
        public HumanInfo HumanInfo { get; set; }
    }
}
