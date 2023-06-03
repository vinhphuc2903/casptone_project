//using System;
using CapstoneProject.Models;
using CapstoneProject.Areas.Orders.Models.Schemas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Commons.Schemas;
using ShowTimeDb = CapstoneProject.Databases.Schemas.System.Ticket.ShowTime;
using OrderDb = CapstoneProject.Databases.Schemas.System.Orders.Orders;
using OrderFoodDetailDb = CapstoneProject.Databases.Schemas.System.Orders.OrderFoodDetail;
using OrderTicketDetailDb = CapstoneProject.Databases.Schemas.System.Orders.OrderTicketDetail;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CapstoneProject.Commons.Enum;
//using System.Drawing;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using CapstoneProject.Models.Schemas;

namespace CapstoneProject.Services
{
    public interface IMediaService
    {
        Task<string> UploadImageToS3(IFormFile imageFile, bool isResize = false, int width = 800, int height = 1800);
    }
    public class MediaService : IMediaService
    {
        private readonly ILogger<MediaService> _logger;
        private readonly IConfiguration _configuration;
        public MediaService(
            IConfiguration configuration,
            ILogger<MediaService> logger
        ) 
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<string> UploadImageToS3(IFormFile imageFile, bool isResize = false, int width = 800, int height = 1800)
        {
            try
            {
                ResponseInfo responseInfo = new ResponseInfo();
                // Đọc thông tin chứng thực từ tệp cấu hình hoặc cách khác
                var awsAccessKeyId = "AKIA52TTURQTNWTEW6UX";
                var awsSecretAccessKey = "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";
                var region = RegionEndpoint.APSoutheast1; // Chỉ định khu vực APSoutheast1

                string bucketName = "phucawsbucketcsp1";
                string fileExtension = Path.GetExtension(imageFile.FileName);
                string fileName = $"{Guid.NewGuid()}{fileExtension}";
                string objectKey = fileName;
                //string objectKey = Path.GetFileNameWithoutExtension(imageFile.FileName) + ".jpg"; // Chỉnh đuôi file thành .jpg
                // Tạo yêu cầu tải lên
                //var putRequest = new PutObjectRequest
                //{
                //    BucketName = bucketName, // Tên bucket S3
                //    Key = objectKey, // Tên đối tượng trong bucket
                //    InputStream = imageFile.OpenReadStream() // Luồng dữ liệu từ IFormFile
                //};

                //// Tạo phiên làm việc Amazon S3
                //using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, region))
                //{
                //    // Thực hiện tải lên
                //    var response = await client.PutObjectAsync(putRequest);

                //    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        // Lấy đường dẫn URL của file đã tải lên
                //        //var url = client.GetPreSignedURL(new GetPreSignedUrlRequest
                //        //{
                //        //    BucketName = bucketName,
                //        //    Key = objectKey,
                //        //    Expires = DateTime.UtcNow.AddMinutes(5) // Thời gian hết hạn của URL
                //        //});

                //        //return url;
                //        string cdn = AppState.Instance.Setting.Cdn;
                //        return fileName;
                //    }
                //    else
                //    {
                //        responseInfo.Code = CodeResponse.HAVE_ERROR;
                //        responseInfo.MsgNo = MSG_NO.TIME_SHOW_ERROR;
                //        return null;
                //    }
                //}

                // Resize kích thước ảnh trước khi tải lên
                using (var stream = new MemoryStream())
                {
                    if(isResize)
                    {
                        using (var image = Image.Load(imageFile.OpenReadStream()))
                        {
                            // Resize ảnh với kích thước mới
                            int newWidth = width; // Đặt kích thước mới cho chiều rộng
                            //int newHeight = (int)(((float)image.Height / image.Width) * newWidth); // Tính toán chiều cao mới tỷ lệ theo chiều rộng mới
                            int newHeight = height;
                            image.Mutate(x => x.Resize(newWidth, newHeight));
                            image.Save(stream, new JpegEncoder()); // Lưu ảnh với định dạng JPEG
                        }
                    }
                    
                    // Tạo yêu cầu tải lên
                    var putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = objectKey,
                        InputStream = stream // Sử dụng MemoryStream chứa ảnh đã resize
                    };

                    // Tạo phiên làm việc Amazon S3
                    using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, region))
                    {
                        // Thực hiện tải lên
                        var response = await client.PutObjectAsync(putRequest);

                        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //return objectKey;
                            // Lấy đường dẫn URL của file đã tải lên
                            var url = client.GetPreSignedURL(new GetPreSignedUrlRequest
                            {
                                BucketName = bucketName,
                                Key = objectKey,
                                Expires = DateTime.UtcNow.AddMinutes(5) // Thời gian hết hạn của URL
                            });

                            //return url;
                            string cdn = AppState.Instance.Setting.Cdn;
                            return fileName;
                        }
                        else
                        {
                            responseInfo.Code = CodeResponse.HAVE_ERROR;
                            responseInfo.MsgNo = MSG_NO.TIME_SHOW_ERROR;
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw;
            }
        }
    }
}