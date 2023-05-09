using System;
using System.ComponentModel.DataAnnotations;

namespace FMStyle.API.Areas.Auth.Models.Login.Schemas
{
    public class Token
    {
        /// <summary>
        /// Token xác thực của phiên đăng nhập hiện tại
        /// </summary>
        [Required(ErrorMessage = "E001")]
        [StringLength(400, ErrorMessage = "E005")]
        public string JwtToken { get; set; }

        /// <summary>
        /// Token ngẫu nhiên của phiên đăng nhập hiện tại
        /// </summary>
        [Required(ErrorMessage = "E001")]
        [StringLength(400, ErrorMessage = "E005")]
        public string RefreshToken { get; set; }
    }
}
