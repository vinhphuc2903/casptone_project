using System;
namespace CapstoneProject.Commons.Enum
{
	public class MSG_NO
	{
        /// <summary>
        /// Số điện thoại hoặc mật khẩu không chính xác, vui lòng nhập lại.
        /// </summary>
        public static string USERNAME_OR_PASSWORD_NOT_INCORRECT = "E001";
        /// <summary>
        /// Số điện thoại đã tồn tại
        /// </summary>
        public static string PHONE_IS_USED = "E002";
        /// <summary>
        /// Email đã tồn tại
        /// </summary>
        public static string EMAIL_IS_USED = "E003";
        /// <summary>
        /// Tên đăng nhập đã tồn tại
        /// </summary>
        public static string USERNAME_IS_USED = "E003";
        /// <summary>
        /// Giới tính không tồn tại
        /// </summary>
        public static string GENDER_NOT_EXIST = "E004";
    }
}

