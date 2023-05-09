using System;
using System.ComponentModel.DataAnnotations;

namespace FMStyle.API.Areas.Auth.Models.Login.Schemas
{
    public class UserInput
    {
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string GoogleId { get; set; }
        public string FacebookId { get; set; }
    }
}
