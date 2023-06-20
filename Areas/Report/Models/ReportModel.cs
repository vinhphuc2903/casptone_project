using System;
using CapstoneProject.Areas.Report.Models.Schemas;
using CapstoneProject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Report.Models
{
	public interface IReportModel
	{
		Task<List<RevenueByBranch>> GetRevenueByBranch(SearchCondition searchCondition);
        Task<ListRevenueByDate> GetRevenueByDate(SearchCondition searchCondition);
        Task<ListRevenueByOrder> GetRevenueByOrder(SearchCondition searchCondition);
        Task<ListRevenueByFilm> GetRevenueByFilm(SearchCondition searchCondition);
    }
	public class ReportModel : CapstoneProjectModels, IReportModel
    {
        private readonly ILogger<ReportModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        public ReportModel(
                IConfiguration configuration,
                ILogger<ReportModel> logger,
                IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<List<RevenueByBranch>> GetRevenueByBranch(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                if(searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DbConnection connection = _context.GetConnection();
                List<RevenueByBranch> revenueByBranches = new List<RevenueByBranch>();

                string queryGetRevenueByBranch = $@"
                    SELECT 
                        Branches.Name AS BranchName
                        , Branches.Code AS BranchCode
                        , SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                        , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                        , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                        , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                        , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                    FROM Payments
                    LEFT JOIN Orders
                    ON (
                        Orders.Id = Payments.OrderId
                        AND Orders.DelFlag = 0
                    )
                    LEFT JOIN Branches
                    ON (
                        Orders.BranchId = Branches.Id
                        AND Branches.DelFlag = 0
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderFoodDetail.OrderId,
                            SUM(OrderFoodDetail.Quantity) AS Quantity,
                            SUM(OrderFoodDetail.OriginPrice) AS OriginPrice,
                            SUM(OrderFoodDetail.SalePrice) AS SalePrice,
                            SUM(OrderFoodDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderFoodDetail.Price) AS Price,
                            SUM(OrderFoodDetail.DiscountPrice) AS DiscountPrice
                        FROM OrderFoodDetail
                        WHERE 
                            OrderFoodDetail.DelFlag = 0
                        GROUP BY 
                            OrderFoodDetail.OrderId
                    ) AS OrderFoodData
                    ON (
                        OrderFoodData.OrderId = Payments.OrderId
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderTicketDetail.OrderId,
                            COUNT(OrderId) AS Quantity,
                            SUM(OrderTicketDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderTicketDetail.SalePrice) AS SalePrice,
                            SUM(OrderTicketDetail.DiscountPrice) AS DiscountPrice,
                            SUM(OrderTicketDetail.Price) AS Price
                        FROM OrderTicketDetail
                        WHERE 
                            OrderTicketDetail.DelFlag = 0
                        GROUP BY 
                            OrderTicketDetail.OrderId
                    ) AS OrderTicketData
                    ON (
                        OrderTicketData.OrderId = Payments.OrderId
                    )
                    WHERE
                        Payments.Status =  '30'
                        AND (
                            @BranchId IS NULL
                            OR @BranchId = Branches.Id
                        )
                        AND (
                            @DateFrom IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) >= CAST(@DateFrom AS DATE)
                        )
                        AND (
                            @DateTo IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) <= CAST(@DateTo AS DATE)
                        )
                    GROUP BY
                        Branches.Name
                        , Branches.Code
                ";
                revenueByBranches = (List<RevenueByBranch>)await connection.QueryAsync<RevenueByBranch>(queryGetRevenueByBranch, searchCondition);
                return revenueByBranches;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }

        public async Task<ListRevenueByDate> GetRevenueByDate(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                if (searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DbConnection connection = _context.GetConnection();
                ListRevenueByDate revenueByDates = new ListRevenueByDate();
                string sqlCount = $@"SELECT
                                        COUNT(1)";
                string selectQuery = $@"
                    SELECT 
                        CONVERT(varchar(10), PaymentAt, 103) AS PaymentAt
                        , SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                        , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                        , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                        , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                        , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                ";
                string selectSumQuery = $@"
                    SELECT
                        SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                    , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                    , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                    , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                    , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                ";
                string queryGetRevenueByBranch = $@"
                    FROM Payments
                    LEFT JOIN Orders
                    ON (
                        Orders.Id = Payments.OrderId
                        AND Orders.DelFlag = 0
                    )
                    LEFT JOIN ShowTime
                    ON (
                        ShowTime.Id = Orders.ShowTimeId
                        AND ShowTime.DelFlag = 0
                    )   
                    LEFT JOIN Films
                    ON (
                        Films.Id = ShowTime.FilmId
                        AND Films.DelFlag = 0
                    )
                    LEFT JOIN Branches
                    ON (
                        Orders.BranchId = Branches.Id
                        AND Branches.DelFlag = 0
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderFoodDetail.OrderId,
                            SUM(OrderFoodDetail.Quantity) AS Quantity,
                            SUM(OrderFoodDetail.OriginPrice) AS OriginPrice,
                            SUM(OrderFoodDetail.SalePrice) AS SalePrice,
                            SUM(OrderFoodDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderFoodDetail.Price) AS Price,
                            SUM(OrderFoodDetail.DiscountPrice) AS DiscountPrice
                        FROM OrderFoodDetail
                        WHERE 
                            OrderFoodDetail.DelFlag = 0
                        GROUP BY 
                            OrderFoodDetail.OrderId
                    ) AS OrderFoodData
                    ON (
                        OrderFoodData.OrderId = Payments.OrderId
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderTicketDetail.OrderId,
                            COUNT(OrderId) AS Quantity,
                            SUM(OrderTicketDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderTicketDetail.SalePrice) AS SalePrice,
                            SUM(OrderTicketDetail.DiscountPrice) AS DiscountPrice,
                            SUM(OrderTicketDetail.Price) AS Price
                        FROM OrderTicketDetail
                        WHERE 
                            OrderTicketDetail.DelFlag = 0
                        GROUP BY 
                            OrderTicketDetail.OrderId
                    ) AS OrderTicketData
                    ON (
                        OrderTicketData.OrderId = Payments.OrderId
                    )
                    WHERE
                        Payments.Status =  '30'
                        AND (
                            @BranchId IS NULL
                            OR @BranchId = Branches.Id
                        )
                        AND (
                            @DateFrom IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) >= CAST(@DateFrom AS DATE)
                        )
                        AND (
                            @DateTo IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) <= CAST(@DateTo AS DATE)
                        )
                        AND (
                            @FilmId IS NULL
                            OR @FilmId = Films.Id
                        )
                ";
                string GroupByQuery = $@"
                    GROUP BY
                        CONVERT(varchar(10), PaymentAt, 103)
                    ORDER BY
                        PaymentAt DESC
                ";
                int count = await connection.QueryFirstAsync<int>(sqlCount + queryGetRevenueByBranch, searchCondition);
                revenueByDates.Paging = new Paging(count, searchCondition.CurrentPage, searchCondition.PageSize);
                string limitPage = $@"
                    OFFSET {(revenueByDates.Paging.CurrentPage - 1) * revenueByDates.Paging.NumberOfRecord}
                    ROWS FETCH NEXT {revenueByDates.Paging.NumberOfRecord} ROWS ONLY
                ";
                revenueByDates.ListRevenueByDates = (List<RevenueByDate>)await connection.QueryAsync<RevenueByDate>(selectQuery + queryGetRevenueByBranch + GroupByQuery + limitPage, searchCondition);
                revenueByDates.TotalRevenueByDates = await connection.QueryFirstOrDefaultAsync<TotalRevenueByDate>(selectSumQuery + queryGetRevenueByBranch, searchCondition);
                return revenueByDates;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ListRevenueByOrder> GetRevenueByOrder(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                if (searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DbConnection connection = _context.GetConnection();
                ListRevenueByOrder revenueByOrder = new ListRevenueByOrder();
                string sqlCount = $@"SELECT
                                        COUNT(1)";
                string selectQuery = $@"
                    SELECT
                        Users.Name
                        , Orders.OrderCode
                        , CONVERT(varchar(10), PaymentAt, 103) AS PaymentAt
                        , SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                        , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                        , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                        , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                        , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                ";
                string selectSumQuery = $@"
                    SELECT
                        SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                    , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                    , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                    , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                    , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                ";
                string queryGetRevenueByBranch = $@"
                    FROM Payments
                    LEFT JOIN Orders
                    ON (
                        Orders.Id = Payments.OrderId
                        AND Orders.DelFlag = 0
                    )
                    LEFT JOIN ShowTime
                    ON (
                        ShowTime.Id = Orders.ShowTimeId
                        AND ShowTime.DelFlag = 0
                    )   
                    LEFT JOIN Films
                    ON (
                        Films.Id = ShowTime.FilmId
                        AND Films.DelFlag = 0
                    )
                    LEFT JOIN Users
                    ON (
                        Users.Id = Orders.UserId
                        AND Users.DelFlag = 0
                    )   
                    LEFT JOIN Branches
                    ON (
                        Orders.BranchId = Branches.Id
                        AND Branches.DelFlag = 0
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderFoodDetail.OrderId,
                            SUM(OrderFoodDetail.Quantity) AS Quantity,
                            SUM(OrderFoodDetail.OriginPrice) AS OriginPrice,
                            SUM(OrderFoodDetail.SalePrice) AS SalePrice,
                            SUM(OrderFoodDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderFoodDetail.Price) AS Price,
                            SUM(OrderFoodDetail.DiscountPrice) AS DiscountPrice
                        FROM OrderFoodDetail
                        WHERE 
                            OrderFoodDetail.DelFlag = 0
                        GROUP BY 
                            OrderFoodDetail.OrderId
                    ) AS OrderFoodData
                    ON (
                        OrderFoodData.OrderId = Payments.OrderId
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderTicketDetail.OrderId,
                            COUNT(OrderId) AS Quantity,
                            SUM(OrderTicketDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderTicketDetail.SalePrice) AS SalePrice,
                            SUM(OrderTicketDetail.DiscountPrice) AS DiscountPrice,
                            SUM(OrderTicketDetail.Price) AS Price
                        FROM OrderTicketDetail
                        WHERE 
                            OrderTicketDetail.DelFlag = 0
                        GROUP BY 
                            OrderTicketDetail.OrderId
                    ) AS OrderTicketData
                    ON (
                        OrderTicketData.OrderId = Payments.OrderId
                    )
                    WHERE
                        Payments.Status =  '30'
                        AND (
                            @BranchId IS NULL
                            OR @BranchId = Branches.Id
                        )
                        AND (
                            @DateFrom IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) >= CAST(@DateFrom AS DATE)
                        )
                        AND (
                            @DateTo IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) <= CAST(@DateTo AS DATE)
                        )
                        AND (
                            @FilmId IS NULL
                            OR @FilmId = Films.Id
                        )
                ";
                string GroupByQuery = $@"
                    GROUP BY
                        Users.Name
                        , Orders.OrderCode
                        , CONVERT(varchar(10), PaymentAt, 103)
                    ORDER BY
                        PaymentAt DESC
                ";
                int count = await connection.QueryFirstAsync<int>(sqlCount + queryGetRevenueByBranch, searchCondition);
                revenueByOrder.Paging = new Paging(count, searchCondition.CurrentPage, searchCondition.PageSize);
                string limitPage = $@"
                    OFFSET {(revenueByOrder.Paging.CurrentPage - 1) * revenueByOrder.Paging.NumberOfRecord}
                    ROWS FETCH NEXT {revenueByOrder.Paging.NumberOfRecord} ROWS ONLY
                ";
                revenueByOrder.ListRevenueByOrders = (List<RevenueByOrder>)await connection.QueryAsync<RevenueByOrder>(selectQuery + queryGetRevenueByBranch + GroupByQuery + limitPage, searchCondition);
                revenueByOrder.TotalRevenueByOrders = await connection.QueryFirstOrDefaultAsync<TotalRevenueByDate>(selectSumQuery + queryGetRevenueByBranch, searchCondition);
                return revenueByOrder;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ListRevenueByFilm> GetRevenueByFilm(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                if (searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DbConnection connection = _context.GetConnection();
                ListRevenueByFilm revenueByFilm = new ListRevenueByFilm();
                string sqlCount = $@"SELECT
                                        COUNT(1)";
                string selectQuery = $@"
                    SELECT
                        Films.Name
                        , Films.Country
                        , Films.Director
                        , Films.Actor
                        , CONVERT(varchar(10), DateStart, 103) AS DateStart
                        , CONVERT(varchar(10), DateEnd, 103) AS DateEnd
                        , Films.Status
                        , CONVERT(varchar(10), PaymentAt, 103) AS PaymentAt
                        , SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                        , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                        , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                        , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                        , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                ";
                string selectSumQuery = $@"
                    SELECT
                        SUM(COALESCE(OrderTicketData.Quantity, 0)) AS TotalTicketSold
                    , SUM(COALESCE(OrderFoodData.Quantity, 0)) AS TotalFoodSold
                    , SUM(COALESCE(OrderTicketData.Price, 0)) AS TotalRevenueTicket
                    , SUM(COALESCE(OrderFoodData.Price, 0)) AS TotalRevenueFood
                    , SUM(COALESCE(OrderTicketData.DiscountPrice, 0) + COALESCE(OrderFoodData.DiscountPrice, 0)) AS DiscountPrice
                ";
                string queryGetRevenueByBranch = $@"
                    FROM Payments
                    LEFT JOIN Orders
                    ON (
                        Orders.Id = Payments.OrderId
                        AND Orders.DelFlag = 0
                    )
                    LEFT JOIN ShowTime
                    ON (
                        ShowTime.Id = Orders.ShowTimeId
                        AND ShowTime.DelFlag = 0
                    )   
                    LEFT JOIN Films
                    ON (
                        Films.Id = ShowTime.FilmId
                        AND Films.DelFlag = 0
                    )
                    LEFT JOIN Branches
                    ON (
                        Orders.BranchId = Branches.Id
                        AND Branches.DelFlag = 0
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderFoodDetail.OrderId,
                            SUM(OrderFoodDetail.Quantity) AS Quantity,
                            SUM(OrderFoodDetail.OriginPrice) AS OriginPrice,
                            SUM(OrderFoodDetail.SalePrice) AS SalePrice,
                            SUM(OrderFoodDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderFoodDetail.Price) AS Price,
                            SUM(OrderFoodDetail.DiscountPrice) AS DiscountPrice
                        FROM OrderFoodDetail
                        WHERE 
                            OrderFoodDetail.DelFlag = 0
                        GROUP BY 
                            OrderFoodDetail.OrderId
                    ) AS OrderFoodData
                    ON (
                        OrderFoodData.OrderId = Payments.OrderId
                    )
                    LEFT JOIN (
                        SELECT 
                            OrderTicketDetail.OrderId,
                            COUNT(OrderId) AS Quantity,
                            SUM(OrderTicketDetail.PaymentPrice) AS PaymentPrice,
                            SUM(OrderTicketDetail.SalePrice) AS SalePrice,
                            SUM(OrderTicketDetail.DiscountPrice) AS DiscountPrice,
                            SUM(OrderTicketDetail.Price) AS Price
                        FROM OrderTicketDetail
                        WHERE 
                            OrderTicketDetail.DelFlag = 0
                        GROUP BY 
                            OrderTicketDetail.OrderId
                    ) AS OrderTicketData
                    ON (
                        OrderTicketData.OrderId = Payments.OrderId
                    )
                    WHERE
                        Payments.Status =  '30'
                        AND (
                            @BranchId IS NULL
                            OR @BranchId = Branches.Id
                        )
                        AND (
                            @DateFrom IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) >= CAST(@DateFrom AS DATE)
                        )
                        AND (
                            @DateTo IS NULL
                            OR CAST(Payments.PaymentAt AS DATE) <= CAST(@DateTo AS DATE)
                        )
                        AND (
                            @FilmId IS NULL
                            OR @FilmId = Films.Id
                        )
                ";
                string GroupByQuery = $@"
                    GROUP BY
                         Films.Name
                        , Films.Country
                        , Films.Director
                        , Films.Actor
                        , CONVERT(varchar(10), DateStart, 103)
                        , CONVERT(varchar(10), DateEnd, 103)
                        , Films.Status
                        , CONVERT(varchar(10), PaymentAt, 103)
                    ORDER BY
                        PaymentAt DESC
                ";
                int count = await connection.QueryFirstAsync<int>(sqlCount + queryGetRevenueByBranch, searchCondition);
                revenueByFilm.Paging = new Paging(count, searchCondition.CurrentPage, searchCondition.PageSize);
                string limitPage = $@"
                    OFFSET {(revenueByFilm.Paging.CurrentPage - 1) * revenueByFilm.Paging.NumberOfRecord}
                    ROWS FETCH NEXT {revenueByFilm.Paging.NumberOfRecord} ROWS ONLY
                ";
                revenueByFilm.RevenueByFilms = (List<RevenueByFilm>)await connection.QueryAsync<RevenueByFilm>(selectQuery + queryGetRevenueByBranch + GroupByQuery + limitPage, searchCondition);
                revenueByFilm.TotalRevenueByDates = await connection.QueryFirstOrDefaultAsync<TotalRevenueByDate>(selectSumQuery + queryGetRevenueByBranch, searchCondition);
                return revenueByFilm;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
    }
}

