using System;
using System.ComponentModel.DataAnnotations;

namespace FMStyle.API.Areas.Auth.Models.Login.Schemas
{
    public class UserSocial
    {
        public string Type { get; set; }
        public string Token { get; set; }

        /// <summary>
        /// Vĩ độ vị trí đăng nhập
        /// </summary>
        /// <example>16.0862274</example>
        [StringLength(30, ErrorMessage = "E005")]
        public string LatOfMap { get; set; }

        /// <summary>
        /// Kinh độ vị trí đăng nhập
        /// </summary>
        /// <example>108.1500744</example>
        [StringLength(30, ErrorMessage = "E005")]
        public string LongOfMap { get; set; }

        /// <summary>
        /// Thông tin thiết bị đăng nhập
        /// </summary>
        /// <example>Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.3 Safari/605.1.15</example>
        [StringLength(200, ErrorMessage = "E005")]
        public string Browser { get; set; }

        /// <summary>
        /// Id của thiết bị trên OneSignal
        /// </summary>
        /// <example>5bec2a2d-6147-4c9c-8e21-b1b888040b0e</example>
        [StringLength(100, ErrorMessage = "E005")]
        public string PlayerId { get; set; }

        /// <summary>
        /// Số serial của thiết bị đang đăng nhập
        /// </summary>
        /// <example>G6TZL899N70M</example>
        [StringLength(50, ErrorMessage = "E005")]
        public string Serial { get; set; }

        /// <summary>
        /// Token Captcha
        /// </summary>
        /// <example>fvzzdv123cfrfrgsfwefhtyuyiuou</example>
        [StringLength(1000, ErrorMessage = "E005")]
        public string CaptchaToken { get; set; }
    }
}
