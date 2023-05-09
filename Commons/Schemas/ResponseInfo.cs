using System;
using System;
using System.Collections.Generic;
using CapstoneProject.Commons.Enum;

namespace CapstoneProject.Commons.Schemas
{
    /// <summary>
    /// Lấy thông tin trả về từ ajax.
    /// Author: QuyPN
    /// Created at: 01/01/2020
    /// </summary>
    public class ResponseInfo
    {
        /// <summary>
        /// Mã trả về tương ứng cho xử lý
        /// <para>200: Thành công</para>
        /// <para>201: Lỗi dữ liệu nhập vào</para>
        /// <para>202: Có lỗi khác phát sinh</para>
        /// <para>403: Không có quyền truy cập</para>
        /// <para>500: Lỗi server</para>
        /// </summary>
        public int Code { set; get; }
        /// <summary>
        /// Mã của thông báo sẽ hiển thị
        /// </summary>
        public string MsgNo { set; get; }
        /// <summary>
        /// Danh sách lỗi phát sinh
        /// </summary>
        public Dictionary<string, string> ListError { set; get; }
        /// <summary>
        /// Dữ liệu cần trả về client
        /// </summary>
        public Dictionary<string, string> Data { set; get; }
        /// <summary>
        /// Khởi tạo giá trị
        /// </summary>
        public ResponseInfo()
        {
            Code = CodeResponse.OK;
            MsgNo = "";
            Data = new Dictionary<string, string>();
            ListError = new Dictionary<string, string>();
        }
    }
}
