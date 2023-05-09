using System;

namespace CapstoneProject.Commons.Enum
{
    /// <summary>
    /// Các giá trị hằng số xác định kết quả trả về từ server.
    /// <para>Author: VinhPhuc.</para>
    /// <para>Created at: 13/05/2023.</para>
    /// </summary>
    public class CodeResponse
    {
        /// <summary>
        /// Thành công
        /// </summary>
        public static int OK = 200;
        /// <summary>
        /// Lỗi hệ thống
        /// </summary>
        public static int SERVER_ERROR = 500;
        /// <summary>
        /// Không tìm thấy
        /// </summary>
        public static int NOT_FOUND = 404;
        /// <summary>
        /// Chưa login
        /// </summary>
        public static int NOT_LOGIN = 401;
        /// <summary>
        /// Không có quyền truy cập
        /// </summary>
        public static int NOT_ACCESS = 403;
        /// <summary>
        /// Lỗi dữ liệu không đúng yêu cầu
        /// </summary>
        public static int NOT_VALIDATE = 201;
        /// <summary>
        /// Có lỗi phát sinh
        /// </summary>
        public static int HAVE_ERROR = 202;
    }
}
