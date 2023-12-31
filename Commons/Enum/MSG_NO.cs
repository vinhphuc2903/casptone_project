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
        public static string TIME_SHOW_ERROR = "E011";
        /// <summary>
        /// Chỗ ngồi đã được chọn hoặc thanh toán, vui lòng chọn lại chỗ ngồi
        /// </summary>
        public static string TICKET_NOT_DEFINE = "E012";
        /// <summary>
        /// Xuất chiếu không tồn tại, vui lòng kiểm tra lại
        /// </summary>
        public static string SHOWTIME_NOT_EXITED = "E013";
        /// <summary>
        /// Đồ ăn đang tạm ngưng bán hoặc đã hết
        /// </summary>
        public static string FOOD_IS_PAUSE = "E014";
        /// <summary>
        /// Vui lòng đăng nhập để thực hiện chức năng này
        /// </summary>
        public static string LOGIN_IS_FALSE = "E015";
        /// <summary>
        /// Vui lòng chọn vé để tiếp tục
        /// </summary>
        public static string CHOSE_TICKET = "E016";
        /// <summary>
        /// Đã quá hạn thời gian thanh toán, vui lòng tạo đơn hàng mới
        /// </summary>
        public static string OVERDUE_PAYMENT_TIME = "E017";
        /// <summary>
        /// Tên đã tồn tại
        /// </summary>
        public static string NAME_IS_EXITED = "E018";
        /// <summary>
        /// Phòng chiếu không tồn tại
        /// </summary>
        public static string ROOM_IS_NOT_EXITED = "E019";
        // <summary>
        /// Vui lòng tạo tối đa 7 ngày
        /// </summary>
        public static string DATE_CREATE_IS_7_DAY = "E020";
        // <summary>
        /// Ngày chiếu từ phải lớn ngày ngày chiếu đến
        /// </summary>
        public static string DATE_TO_MUST_BE_LAGER_DATE_FROM = "E021";
        /// <summary>
        /// Phim chiếu không tồn tại
        /// </summary>
        public static string FILM_IS_NOT_EXITED = "E022";
        /// <summary>
        /// Ngày bắt đầu chiếu phải lớn hơn ngày tạm hoãn
        /// </summary>
        public static string DATE_START_NOT_OK = "E023";
        /// <summary>
        /// Ngày kết gia hạn phải lớn hơn ngày kết thúc
        /// </summary>
        public static string DATE_EXTEND_NOT_OK = "E024";
        /// <summary>
        /// Vui lòng chọn ngày gia hạn để tiếp tục chiếu phim
        /// </summary>
        public static string DATE_EXTEND_NOT_OK_V1 = "E025";
        /// <summary>
        /// Ngày phát hàng phải lớn hơn ngày bắt đầu
        /// </summary>
        public static string DATE_RELESE_NOT_OK = "E026";
        // <summary>
        /// Nhân viên không tồn tại
        /// </summary>
        public static string EMPLOYEE_NOT_EXITED = "E026";
    }
}

