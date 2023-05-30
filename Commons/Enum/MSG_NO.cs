﻿using System;
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
        public static string USERNAME_IS_USED = "E004";
        /// <summary>
        /// Giới tính không tồn tại
        /// </summary>
        public static string GENDER_NOT_EXIST = "E005";
        /// <summary>
        /// Xuất chiếu đã tồn tại
        /// </summary>
        public static string SHOWTIME_IS_EXIST = "E006";
        /// <summary>
        /// Film dài hơn so với thời gian chiếu
        /// </summary>
        public static string TIME_FILM_LONGER_THAN_SHOW = "E007";
        /// <summary>
        /// Độ dài của tên đăng nhập ít nhất 8 kí tự
        /// </summary>
        public static string LENGTH_USERNAME_NOT_OK = "E008";
        /// <summary>
        /// Độ dài của tên đăng nhập ít nhất 6 kí tự
        /// </summary>
        public static string LENGTH_USERNAME_NOT_OK_6 = "E009";
        /// <summary>
        /// Tài khoản của bạn không có quyền thực hiện chức năng này
        /// </summary>
        public static string ACCOUNT_NOT_HAVE_PERMISSION = "E010";
        /// <summary>
        /// Suất chiếu phải nằm trong từ 8h => 25h hôm qua
        /// </summary>
        public static string TIME_SHOW_ERROR = "E010";
    }
}

