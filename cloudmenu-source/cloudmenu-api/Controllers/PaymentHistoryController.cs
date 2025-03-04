using System.ComponentModel;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.PaymentHistory;
using cloudmenu_api.LwUtils;



namespace cloudmenu_api.Controllers
{
    [ApiController]
    [Route("api/pymtHis")]
    public class PaymentHistoryController : ControllerBase
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<PaymentHistoryController> _logger;

        public PaymentHistoryController(ILogger<PaymentHistoryController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation(1, "NLog injected into PaymentHistoryController");
        }

        /*****
        * 受付履歴検索
        *****/
        [HttpPost("getDailyReceiptHistory")]
        public RpnReceiptHistory getReceiptHistory(ReqMdGetRcpHis reqDto)
        {
            RpnReceiptHistory rslt = new RpnReceiptHistory();

            DateTime dayBegin = new DateTime();
            DateTime dayEnd = new DateTime();
            DateTime.TryParse(reqDto.receiptDate + " 00:00:00.000", out dayBegin);
            DateTime.TryParse(reqDto.receiptDate + " 23:59:59.999", out dayEnd);

            var rcptHisQuery =
                from rcp in _dbContext.TReceptions
                join st in _dbContext.MSeats
                    on new { rcp.StoreNumber, rcp.SeatNumber } equals new { st.StoreNumber, st.SeatNumber }
                into stGp
                from stL in stGp.DefaultIfEmpty()
                where rcp.StoreNumber == reqDto.storeNumber
                    && rcp.ReceptionDelFlg == FlgConst.off
                    && rcp.SeatEndDate != null
                    && DateTime.Compare(rcp.SeatStartDate ?? DateTime.MinValue, dayBegin) >= 0
                    && DateTime.Compare(rcp.SeatStartDate ?? DateTime.MinValue, dayEnd) <= 0
                select new ReceiptHisData
                {
                    StoreNumber = rcp.StoreNumber,
                    ReceptionNumber = rcp.ReceptionNumber,
                    ReceptionBranchNumber = rcp.ReceptionBranchNumber,
                    SeatNumber = rcp.SeatNumber,
                    SeatName = stL.SeatName,
                    SeatPeopleAll = (rcp.SeatPeopleMan ?? 0) + (rcp.SeatPeopleWoman ?? 0) + (rcp.SeatPeopleChildren ?? 0)
                };
            var rcptHisListDupl = rcptHisQuery.ToList();

            var rcptHisList = rcptHisListDupl
                .GroupBy(rcp => rcp.ReceptionNumber)
                .Select(rcpG => new ReceiptHisData
                {
                    StoreNumber = rcpG.First().StoreNumber,
                    ReceptionNumber = rcpG.Key,
                    // SeatNumbers = string.Join(' ', rcpG.Select(rcp => rcp.SeatNumber).ToList()),
                    SeatNames = string.Join(' ', rcpG.Select(rcp => rcp.SeatName).ToList()),
                    SeatPeopleAll = rcpG.First().SeatPeopleAll
                })
                .ToList();

            rcptHisList.ForEach(rcpt =>
            {
                var orderPyNoList = _dbContext.TOrders
                    .Where(od =>
                        od.StoreNumber == rcpt.StoreNumber
                        && od.ReceptionNumber == rcpt.ReceptionNumber
                        && od.OrderCancelFlg == FlgConst.off)
                    .Select(od => od.PaymentNumber)
                    .ToList();
                if (orderPyNoList.Count == 0)
                {
                    rcpt.paymentStatus = 0;
                    rcpt.paymentStatusName = "未注文";
                }
                else
                {
                    var exitPayed = orderPyNoList.Any(pmtNo => String.IsNullOrEmpty(pmtNo));
                    if (orderPyNoList.All(pmtNo => String.IsNullOrEmpty(pmtNo)))
                    {
                        rcpt.paymentStatus = 1;
                        rcpt.paymentStatusName = "未会計";
                    }
                    else if (orderPyNoList.All(pmtNo => !String.IsNullOrEmpty(pmtNo)))
                    {
                        rcpt.paymentStatus = 3;
                        rcpt.paymentStatusName = "会計済み";
                    }
                    else
                    {
                        rcpt.paymentStatus = 2;
                        rcpt.paymentStatusName = "一部会計";
                    }

                    var paymentNoList = orderPyNoList.Distinct();

                    var paymentList = _dbContext.TPayments
                        .Where(py =>
                            py.StoreNumber == reqDto.storeNumber
                            && paymentNoList.Contains(py.PaymentNumber))
                        .Select(py => new PaymentData
                        {
                            PaymentNumber = py.PaymentNumber,
                            PaymentDatetime = py.PaymentDatetime,
                            PaymentConfirmPriceStr = CmnUtils.formatCurrency(py.PaymentConfirmPrice ?? 0, 0)
                        }).ToList();

                    rcpt.paymentHisList = paymentList;
                }
            });

            rslt.receiptionHisList = rcptHisList;
            return rslt;
        }

        /*****
        * 受付履歴検索
        *****/
        [HttpPost("deletePaymentsFromOrder")]
        public async Task<RpnMdDelPymt> deletePaymentsFromOrder(ReqMdDelPymt reqDto)
        {
            await _dbContext.Database.BeginTransactionAsync();
            foreach (var paymentNo in reqDto.paymentHisList)
            {
                await _dbContext.Database.ExecuteSqlRawAsync($@"
               update t_order
               set payment_number = Null
               where store_number = '{reqDto.storeNumber}'
               and payment_number = '{paymentNo}'
               ");
            }
            await _dbContext.Database.CommitTransactionAsync();

            return new RpnMdDelPymt();
        }
    }
}
