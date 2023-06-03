using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace CapstoneProject.Commons
{
    /// <summary>
    /// Các xử lý chung thường xuyên sử dụng trong hệ thống.
    /// <para>Author: QuyPN</para>
    /// <para>Created: 15/02/2020</para>
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Sinh chuỗi token ngẫu nhiên theo id account đăng nhập, độ dài mặc định 100 ký tự.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi không trùng nhau sẽ cộng thêm vào token</param>
        /// <param name="length">Dộ dài của token, mặc định 100 ký tự</param>
        /// <returns> Chuỗi token </returns>
        public static string RenderToken(string str, int length = 100)
        {
            try
            {
                string token = "";
                Random ran = new Random();
                string tmp = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
                for (int i = 0; i < length; i++)
                {
                    token += tmp.Substring(ran.Next(0, 63), 1);
                }
                token += str;
                return token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Tạo ra chuỗi code để scan mã số khách hàng
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="customerId">Mã số khách hàng</param>
        /// <returns>Chuỗi scan khi thanh toán</returns>
        public static string CreateScanCode(string customerId)
        {
            try
            {
                Random ran = new Random();
                int mul = ran.Next(50, 99);
                string ranNum = "0" + ran.Next(0, 99);
                long val = mul * Convert.ToInt64(customerId);
                string val_str = "000000000000" + val.ToString();
                return mul.ToString() + val_str.Substring(val_str.Length - 12, 12) + ranNum.Substring(ranNum.Length - 2, 2);
            }
            catch (Exception e)
            {
                //throw e;
                return customerId;
            }
        }

        /// <summary>
        /// Giải mã chuỗi scan ra mã khách hàng
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="scanCode">Mã số khi scan</param>
        /// <returns>Chuỗi scan khi thanh toán</returns>
        public static string DecodeScanCode(string scanCode)
        {
            try
            {
                int mul = Convert.ToUInt16(scanCode.Substring(0, 2));
                long val = Convert.ToInt64(scanCode.Substring(2, 12));
                return (val / mul).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string PrivatePhone(string phone)
        {
            try
            {
                return phone.Substring(0, phone.Length - 4) + "xxxx";
            }
            catch
            {
                return phone;
            }
        }

        public static string PaymentCode(string prefix)
        {
            try
            {
                if (prefix == null)
                {
                    prefix = "";
                }
                string tick = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000).ToString();
                return prefix + tick.Substring(tick.Length - 16 + prefix.Length, 16 - prefix.Length);
            }
            catch
            {
                return "";
            }
        }

        public static string NextBarcode(string currentBarcode)
        {
            try
            {
                string characterCode1 = currentBarcode.Substring(0, 1);
                string characterCode2 = currentBarcode.Substring(0, 1);
                int integerCode = Convert.ToInt32(currentBarcode.Substring(2, 8));
                if (integerCode == 99999999)
                {
                    integerCode = 1;
                    string tmp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string[] tmps = tmp.Split("");
                    int index1 = Array.IndexOf(tmps, characterCode1);
                    int index2 = Array.IndexOf(tmps, characterCode2);
                    if (index2 == tmps.Length - 1)
                    {
                        index1 += 1;
                        index2 = 0;
                    }
                    else
                    {
                        index2 += 1;
                    }
                    characterCode1 = tmps[index1];
                    characterCode2 = tmps[index2];
                }
                string integerCodeStr = "00000000" + (integerCode + 1);
                integerCodeStr = integerCodeStr.Substring(integerCodeStr.Length - 8, 8);
                return characterCode1 + characterCode2 + integerCodeStr;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Chuyển từ tiếng việt có dấu thành tiếng việt không dấu.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="s">Chuỗi tiếng việt cần chuyển</param>
        /// <param name="isReplaceSpecialChracter">Có thay thế các ký tự đặc biệt hay không</param>
        /// <returns>Kết quả sau khi chuyển</returns>
        public static string ConvertToUnSign(string s, bool isReplaceSpecialChracter = false)
        {
            if (String.IsNullOrEmpty(s))
            {
                return "";
            }
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            temp = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            if (isReplaceSpecialChracter)
            {
                temp = temp.Replace(" ", "_");
                temp = temp.Replace(":", "");
                temp = temp.Replace("\\", "");
                temp = temp.Replace("/", "");
                temp = temp.Replace("\"", "");
                temp = temp.Replace("*", "");
                temp = temp.Replace("?", "");
                temp = temp.Replace(">", "");
                temp = temp.Replace("<", "");
                temp = temp.Replace("||", "");
            }
            return temp;
        }

        /// <summary>
        /// Lấy dữ liệu từ cookies theo khóa, nếu không có dữ liệu thì trả về theo dữ liệu mặc định truyền vào hoặc rỗng
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="key">Khóa cần lấy dữ liệu trong cookie</param>
        /// <param name="returnDefault">Kết quả trả về mặc định nếu không có dữ liệu trong cookie, mặc định là chuỗi rỗng</param>
        /// <returns>Giá trị lưu trữ trong cookie</returns>
        public static string GetCookie(string key, string returnDefault = "")
        {
            try
            {
                string httpCookie = StartupState.Instance.Current.Request.Cookies[key];
                if (httpCookie == null)
                {
                    return returnDefault;
                }
                return Security.Base64Decode(HttpUtility.UrlDecode(httpCookie));
            }
            catch
            {
                return returnDefault;
            }
        }

        /// <summary>
        /// Xóa file theo danh sách url đã cung cấp.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="fileUrls">Danh sách url file sẽ xóa</param>
        /// <returns>True nếu xóa thành công tất cả các file. Exception nếu có lỗi.</returns>
        public static bool DeleteFiles(List<string> fileUrls)
        {
            try
            {
                foreach (string fileUrl in fileUrls)
                {
                    Helpers.DeleteFile(fileUrl);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Xóa file theo danh sách url đã cung cấp.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="fileUrls">Danh sách url file sẽ xóa</param>
        /// <returns>True nếu xóa thành công tất cả các file. Exception nếu có lỗi.</returns>
        public static bool DeleteFiles(string[] fileUrls)
        {
            try
            {
                foreach (string fileUrl in fileUrls)
                {
                    Helpers.DeleteFile(fileUrl);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Xóa 1 file url đã cung cấp.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="fileUrl">Đường dẫn đến file cần xóa</param>
        /// <returns>True nếu xóa thành công file. Exception nếu có lỗi.</returns>
        public static bool DeleteFile(string fileUrl)
        {
            try
            {
                string path = "";
                if (!String.IsNullOrEmpty(fileUrl))
                {
                    if (!fileUrl.StartsWith("~"))
                    {
                        path = StartupState.Instance.WebHostEnvironment.WebRootPath + fileUrl.Substring(1, fileUrl.Length);
                    }
                    else
                    {
                        path = StartupState.Instance.WebHostEnvironment.WebRootPath + fileUrl;
                    }
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lưu file gửi từ client vào thư mục của server.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="file">File được gửi lên</param>
        /// <param name="folder">Đường dẫn đến thư mục muốn lưu (mặc định là /public/images/upload/)</param>
        /// <param name="fileName">Tên file muốn lưu (nếu không truyền vào sẽ lấy theo ngày giờ upload)</param>
        /// <param name="typeFiles">Các kiểu file được chấp nhận (mặc định là không check)</param>
        /// <param name="sizeFile">Dung lượng tối đa của file (mặc định là 10MB)</param>
        /// <returns>
        /// Đường dẫn đến file đã upload thành công.
        /// Trả về "1" nếu file upload không đúng định dạng.
        /// Trả về "2" nếu file upload quá dung lượng quy định.
        /// Trả về "" nếu upload sảy ra lỗi.
        /// </returns>
        public static string SaveFileUpload(IFormFile file, string folder = "/public/images/upload/", string fileName = "", List<string> typeFiles = null, bool isRoot = false, int sizeFile = 10)
        {
            try
            {
                // Lấy đuôi file để check
                string mimeType = Path.GetExtension(file.FileName).ToLower();
                if (fileName == "")
                {
                    // Nếu không truyền file name thì rename theo thời gian upload
                    fileName = DateTimeOffset.Now.ToString("yyyyMMddHHmmssffff") + mimeType;
                }
                // Lấy đường dẫn vật lý của máy chủ
                string rootPath = isRoot ? StartupState.Instance.WebHostEnvironment.ContentRootPath : StartupState.Instance.WebHostEnvironment.WebRootPath;
                string path = rootPath + folder + fileName;
                long fileSize = file.Length;
                // Kiểm tra loại file
                if (typeFiles != null && typeFiles.FirstOrDefault(x => x == mimeType) == null)
                {
                    return "1";
                }
                // Kiểm tra dung lượng file
                if (fileSize / 1024 / 1024 > 10)
                {
                    return "2";
                }
                // Tạo thư mục lưu file nếu chưa tồn tại
                Directory.CreateDirectory(rootPath + folder);
                // Nếu file đã có thì xóa file cũ trước khi lưu file mới vào
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                // Tiến hành save file
                FileStream fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                return folder + fileName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Upload nhiều hình ảnh 1 lúc đồng thời tạo thumb cho các hình ảnh đó.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="files">Các file hình ảnh được gửi lên từ client</param>
        /// <param name="folder">Thư mục sẽ lưu ảnh chính (mặc định là /public/images/upload/)</param>
        /// <param name="folderThumb">Thư mục sẽ lưu Thumb của nahr chính (mặc định là /public/images/upload/thumb/)</param>
        /// <param name="maxSize">Kích thước theo chiều nagng của Thumb, tính theo pixcel</param>
        /// <param name="typeFiles">Loại file được phé tải lêm (mặc định là không check)</param>
        /// <param name="sizeFile">Dung lượng tối đa mỗi file (mặc định là 10MB)</param>
        /// <returns>
        /// Trả về list đối tượng chưa link đến hình ảnh chính và thumb tương ứng
        /// </returns>
        //public static List<ImgUploadWithThumb> SaveListFileUpload(List<IFormFile> files, string folder = "/public/images/upload/", string folderThumb = "/public/images/upload/thumb/", int maxSize = 200, List<string> typeFiles = null, int sizeFile = 10)
        //{
        //    try
        //    {
        //        List<ImgUploadWithThumb> result = new List<ImgUploadWithThumb>();
        //        string rootPath = StartupState.Instance.WebHostEnvironment.WebRootPath;
        //        if (files.Count > 0)
        //        {
        //            int idx = 1;
        //            // upload từng file, nếu bị lỗi ngang nào thì dừng upload và trả về các file đã upload trước đó
        //            foreach (IFormFile file in files)
        //            {
        //                // Lấy đuôi file để check
        //                string mimeType = Path.GetExtension(file.FileName).ToLower();
        //                ImgUploadWithThumb imgAfterUpload = new ImgUploadWithThumb();
        //                // Rename theo thời gian upload và thứ tự upload
        //                string fileName = DateTimeOffset.Now.ToString("yyyyMMddHHmmssffff") + idx.ToString() + mimeType;
        //                idx++;
        //                // Lấy đường dẫn vật lý của máy chủ
        //                string path = rootPath + folder + fileName;
        //                long fileSize = file.Length;
        //                // Kiểm tra loại file
        //                if (typeFiles != null && typeFiles.FirstOrDefault(x => x == mimeType) == null)
        //                {
        //                    imgAfterUpload.LinkImg = "1";
        //                }
        //                // Kiểm tra dung lượng file
        //                if (fileSize / 1024 / 1024 > 10)
        //                {
        //                    imgAfterUpload.LinkImg = "2";
        //                }
        //                if (imgAfterUpload.LinkImg != "1" && imgAfterUpload.LinkImg != "2")
        //                {
        //                    // Tạo thư mục lưu file nếu chưa có
        //                    Directory.CreateDirectory(rootPath + folder);
        //                    // Nếu file đã tồn tại thì xóa file trước
        //                    if (File.Exists(path))
        //                    {
        //                        File.Delete(path);
        //                    }
        //                    // Lưu file và lưu lại thông tin đường dẫn
        //                    FileStream fileStream = new FileStream(path, FileMode.Create);
        //                    file.CopyTo(fileStream);
        //                    imgAfterUpload.LinkImg = folder + fileName;
        //                }
        //                else
        //                {
        //                    // Nếu lỗi thì trả về các file đã lưu trước đó
        //                    result.Add(imgAfterUpload);
        //                    return result;
        //                }
        //                // Nếu upload file chính thành công thiftieeps tục tao Thumb
        //                if (imgAfterUpload.LinkImg != "")
        //                {
        //                    // Tính toán lại các kích thước của hình ảnh
        //                    Image sourceImage = Image.FromStream(file.OpenReadStream());
        //                    int originalWidth = sourceImage.Width;
        //                    int originalHeight = sourceImage.Height;

        //                    float ratioX = (float)maxSize / (float)originalWidth;
        //                    float ratioY = (float)maxSize / (float)originalHeight;
        //                    float ratio = Math.Min(ratioX, ratioY);

        //                    int newWidth = (int)(originalWidth * ratio);
        //                    int newHeight = (int)(originalHeight * ratio);

        //                    // Resize hình ảnh
        //                    Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
        //                    using (Graphics graphics = Graphics.FromImage(newImage))
        //                    {
        //                        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                        graphics.CompositingMode = CompositingMode.SourceCopy;
        //                        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //                        graphics.DrawImage(sourceImage, 0, 0, newWidth, newHeight);
        //                    }

        //                    EncoderParameters encoderParams = new EncoderParameters(1);
        //                    long[] quality = new long[1];
        //                    quality[0] = 80;
        //                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //                    ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

        //                    // Tạo thư mục lưu Thumb
        //                    Directory.CreateDirectory(rootPath + folderThumb);
        //                    string pathThumb = rootPath + folderThumb + fileName;
        //                    // Lưu file Thumb và thông tin đường dẫn
        //                    newImage.Save(pathThumb, encoders[1], encoderParams);
        //                    imgAfterUpload.Thumb = folderThumb + fileName;
        //                }
        //                else
        //                {
        //                    // Nếu lỗi thì trả về các file đã lưu trước đó
        //                    result.Add(imgAfterUpload);
        //                    return result;
        //                }
        //                result.Add(imgAfterUpload);
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// Thay thế các ký tự trong chuỗi search để có thể dùng được ở Store procedure.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi cần thay thế</param>
        /// <returns>Chuỗi sau khi đã thay thế</returns>
        public static string SqlServerEscapeString(string str)
        {
            try
            {
                if (String.IsNullOrEmpty(str))
                {
                    return str;
                }
                str = str.Replace("[", "[[]");
                str = str.Replace("%", "[%]");
                str = str.Replace("_", "[_]");
                str = str.Replace("\\", "[\\]");
                str = str.Replace("'", "''");
                return str;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Tạo mã OTP ngẫu nhiên
        /// <para>Author: DungNT</para>
        /// <para>Created: 21/04/2021</para>
        /// </summary>
        public static string GenerationOTP(int length = 6)
        {
            try
            {
                string token = "";
                Random ran = new Random();
                string tmp = "0123456789";
                for (int i = 0; i < length; i++)
                {
                    token += tmp.Substring(ran.Next(0, 10), 1);
                }
                return token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string GenerationInviteCode(int length = 6)
        {
            try
            {
                string token = "";
                Random ran = new Random();
                string tmp = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                for (int i = 0; i < length; i++)
                {
                    token += tmp.Substring(ran.Next(0, 61), 1);
                }
                return token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///     Calculate the difference between 2 strings using the Levenshtein distance algorithm
        /// </summary>
        /// <param name="source1">First string</param>
        /// <param name="source2">Second string</param>
        /// <returns></returns>
        public static int DifferenceCalculate(string source1, string source2) //O(n*m)
        {
            var source1Length = String.IsNullOrEmpty(source1) ? 0 : source1.Length;
            var source2Length = String.IsNullOrEmpty(source2) ? 0 : source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return 100;

            if (source2Length == 0)
                return 100;

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            // return result
            return (int)((float)matrix[source1Length, source2Length] / (float)Math.Min(source1Length, source2Length)) * 100;
        }

        /// <summary>
        /// Hàm tạo code cho tài nguyên
        /// </summary>
        /// <param name="prefix">code prefix</param>
        /// <param name="maxLength">max length</param>
        /// <param name="Id">base Id</param>
        /// <param name="charHolder">Hodler character</param>
        /// <returns></returns>
        public static string GenerateCode(string prefix, int maxLength, int Id, char charHolder = '0')
        {
            return prefix + new string(charHolder, maxLength - Id.ToString().Length - prefix.Length) + Id.ToString();
        }
    }

    /// <summary>
    /// Thông tin đường dẫn của hình ảnh đã upload đi kèm đường dẫn đến Thumb.
    /// <para>Author: QuyPN</para>
    /// <para>Created: 15/02/2020</para>
    /// </summary>
    public class ImgUploadWithThumb
    {
        /// <summary>
        /// Link đến hình ảnh chính
        /// </summary>
        public string LinkImg { set; get; }
        /// <summary>
        /// Link đến hình thumb
        /// </summary>
        public string Thumb { set; get; }
    }
}
