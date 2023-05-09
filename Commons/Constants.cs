using System;
namespace CapstoneProject.Commons
{
    public class Constants
    {
        public static string CONNECTION_STRING = "User ID=fmplus_release;Password=mpSepw#SMw_6+fTQ;Server=postgresql.fmplus.com.vn;Port=1993;Database=fmplus_fmplus;";

        public static int MAX_OF_FILE_SIZE = 10;

        public static string JWT_SECRET_KEY = "23wsQUsX8rZNgN6xcvNYW4UagZWDAWgruueUzS4nGy9EKh";

        public static string JWT_ISSUER = "https://fmplus.com.vn/";

        public static string JWT_AUD = "FM Plus";

        public static int EXPIRES_MINUTE = 8 * 60;

        public static int API_EXPIRES_MINUTE = 43200;

        public static string AVATAR = "https://media.fmplus.com.vn/defaults/user.png";

        public static string COUVER = "https://media.fmplus.com.vn/defaults/cover.jpg";

        public static string ADMIN_AVATAR = "https://media.fmplus.com.vn/defaults/admin-avatar.jpg";

        public static string SMTP_MAIL = "smtp";
        public static int OTP_EXPIRES_MINUTE = 5;
        // [25/04/2021] DungNT ADD [START]
        public static string MAIL_TEMPLATE_1 = "01"; // Mail gửi OTP - Đăng ký mới

        public static string MAIL_TEMPLATE_2 = "02"; // Mail gửi OTP - Quên mật khẩu
        // [25/04/2021] DungNT ADD [END]
        public static string UPDATE_PERMISSION = "FNC14003";
        public static string APPROVE_PERMISSION = "FNC14009";
        public static double OFFICIAL_EMPLOYEE_BONUS_RATE = 1;
        public static double PROBATIONARY_EMPLOYEE_BONUS_RATE = 0.85;
    }
}
