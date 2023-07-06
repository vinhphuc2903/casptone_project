using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CapstoneProject.Commons
{
    /// <summary>
    /// Các xử lý liên quan đến mã hóa và giải mã dữ liệu
    /// <para>Author: VinhPhuc</para>
    /// <para>Created: 15/02/2023</para>
    /// </summary>
    public class Security
    {
        /// <summary>
        /// Mã hóa MD5 của 1 chuỗi có thêm chuối khóa đầu và cuối.
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 15/02/2023</para>
        /// </summary>
        /// <param name="str">Chuỗi cần mã hóa.</param>
        /// <param name="firstStr">Chuỗi bảo mật đầu</param>
        /// <param name="lastStr">Chuỗi bảo mật cuối</param>
        /// <returns>Chuỗi sau khi đã được mã hóa.</returns>
        public static string GetMD5(string str, string firstStr = "", string lastStr = "")
        {
            str = firstStr + str + firstStr;
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(str);
            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("x2");
            }
            return str_md5;
        }

        /// <summary>
        /// Mã hóa MD5 của 1 chuỗi không có thêm chuối khóa đầu và cuối.
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 15/02/2023</para>
        /// </summary>
        /// <param name="str">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi đã được mã hóa</returns>
        public static string GetSimpleMD5(string str)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(str);
            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("x2");
            }
            return str_md5;
        }

        /// <summary>
        /// Mã hóa base64 của 1 chuỗi
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 15/02/2023</para>
        /// </summary>
        /// <param name="plainText">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi mã hóa</returns>
        public static string Base64Encode(string plainText)
        {
            try
            {
                if (String.IsNullOrEmpty(plainText))
                {
                    return "";
                }
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                string base64Str = Convert.ToBase64String(plainTextBytes);
                int endPos = 0;
                for (endPos = base64Str.Length; endPos > 0; endPos--)
                {
                    if (base64Str[endPos - 1] != '=')
                    {
                        break;
                    }
                }
                int numberPaddingChars = base64Str.Length - endPos;
                base64Str = base64Str.Replace("+", "-");
                base64Str = base64Str.Replace("/", "_");
                base64Str = base64Str.Substring(0, endPos);
                base64Str = $"{base64Str}{numberPaddingChars}";
                return base64Str;
            }
            catch
            {
                return plainText;
            }
        }

        /// <summary>
        /// Chuyển mã base64 về chuỗi trước khi mã hóa.
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 15/02/2023</para>
        /// </summary>
        /// <param name="base64EncodedData">Chuỗi mã hóa</param>
        /// <returns>Chuỗi sau khi giải mã</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                if (String.IsNullOrEmpty(base64EncodedData))
                {
                    return "";
                }
                base64EncodedData = base64EncodedData.Replace("-", "+");
                base64EncodedData = base64EncodedData.Replace("_", "/");
                int numberPaddingChars = 0;
                try
                {
                    numberPaddingChars = Convert.ToInt32(base64EncodedData.Substring(base64EncodedData.Length - 1, 1));
                }
                catch { }
                base64EncodedData = base64EncodedData.Substring(0, base64EncodedData.Length - 1);
                for (int i = 0; i < numberPaddingChars; i++)
                {
                    base64EncodedData += "=";
                }
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return base64EncodedData;
            }
        }

        /// <summary>
        /// Kiểm tra captcha token có hợp lệ hay ko
        /// <para>Author: DungNT</para>
        /// <para>Created: 16/05/2023</para>
        /// </summary>
        /// <param name="checkCaptcha">Chuỗi mã hóa</param>
        /// <returns>Trang thái kiểm tra: (true: hợp lệ)-(false: không hợp lệ)</returns>
        public static bool checkCaptcha(string token)
        {
            // handle captcha token

            //
            return !String.IsNullOrEmpty(token);
        }

        public static string Base64EncodeNormal(string plainText)
        {
            try
            {
                if (String.IsNullOrEmpty(plainText))
                {
                    return "";
                }
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                string base64Str = Convert.ToBase64String(plainTextBytes);
                return base64Str;
            }
            catch
            {
                return plainText;
            }
        }

        public static string Base64DecodeNormal(string base64EncodedData)
        {
            try
            {
                if (String.IsNullOrEmpty(base64EncodedData))
                {
                    return "";
                }
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return base64EncodedData;
            }
        }

        public static string HmacSha256(string plainText, string scretKey)
        {
            try
            {
                if (String.IsNullOrEmpty(plainText))
                {
                    return "";
                }
                byte[] key = Encoding.UTF8.GetBytes(scretKey);
                HMACSHA256 myhmacsha256 = new HMACSHA256(key);
                byte[] byteArray = Encoding.UTF8.GetBytes(plainText);
                MemoryStream stream = new MemoryStream(byteArray);
                string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
                return result;
            }
            catch
            {
                return plainText;
            }
        }
    }
}
