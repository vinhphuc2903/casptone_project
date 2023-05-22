using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class ShowtimeInfo
	{
        public int BranchId { get; set; }
        /// <summary>
        /// Tên chi nhánh
        /// </summary>
        public string? BranchesName { get; set; }
        /// <summary>
        /// Mã chi nhánh
        /// </summary>
        public string? BranchesCode { get; set; }
        /// <summary>
        /// Địa chỉ chi nhánh
        /// </summary>
        public string? BranchesAddress { get; set; }
        /// <summary>
        /// Tỉnh 
        /// </summary>
        public string? BranchesProvince { get; set; }
        /// <summary>
        /// Quận/ huyện
        /// </summary>
        public string? BranchesDistrict { get; set; }
        /// <summary>
        /// Thị xã
        /// </summary>
        public string? BranchesCommune { get; set; }
        /// <summary>
        /// Danh sach lich chieu theo ngay
        /// </summary>
        public List<ShowTimeData> ListShowTimeData { get; set; }
    }
    public class ShowTimeData
    {
        /// <summary>
        /// Ngày chiếu
        /// </summary>
        public DateTime DateShow { get; set; }

        public List<ShowTimeDetailData> ShowTimeDetailDatas { get; set; }
    }
    public class ShowTimeDetailData
    {
        public int Id { get; set; }
        /// <summary>
        /// Ten xuat chieu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ma xuat chieu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Mã phòng chiếu
        /// </summary>
        public int CinemaRoomId { get; set; }

        /// <summary>
        /// Từ giờ
        /// </summary>
        public int FromHour { get; set; }
        /// <summary>
        /// Đến giờ
        /// </summary>
        public int ToHour { get; set; }
        /// <summary>
        /// Từ phút
        /// </summary>
        public int FromMinus { get; set; }
        /// <summary>
        /// Đến phút
        /// </summary>
        public int ToMinus { get; set; }
    }
}

