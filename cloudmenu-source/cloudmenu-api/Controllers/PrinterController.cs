using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.RequestModels;
using cloudmenu_api.ResponseModels;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.Services;
using cloudmenu_api.dailyReport;
using cloudmenu_api.PaymentHistory;
using cloudmenu_api.LwUtils;

namespace cloudmenu_api.Controllers
{
    [ApiController]
    [Route("api/print")]
    public class PrinterController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PrinterController> _logger;
        private readonly ChitPrintService _chitPrintService;
        private readonly CashierService _cashierService;


        public PrinterController(
            ILogger<PrinterController> logger,
            AppDbContext dbContext,
            ChitPrintService chitPrintService,
            CashierService cashierService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _chitPrintService = chitPrintService;
            _cashierService = cashierService;
            _logger.LogInformation(1, "NLog injected into PrinterController");
        }

        /*****
        * 注文データの調理指示伝票
        *****/
        [HttpPost("getOrdersCookChitPrintData")]
        public async Task<RpnMdPrint> getOrdersCookChitPrintData(ReqMdSeatPrintCookChit reqDto)
        {
            RpnMdPrint rtnDto = await _chitPrintService.getOrdersCookChitXmlData(reqDto);
            return rtnDto;
        }
        /*****
        * 会計レジ伝票
        *****/
        [HttpPost("getOrdersCashierChitPrintData")]
        public async Task<RpnMdPrint> getOrdersCashierChitPrintData(ReqMdCashierPrint reqDto)
        {
            RpnMdPrint rtnDto = await _chitPrintService.getOrdersCashierChitPrintData(reqDto);
            return rtnDto;
        }

        /*****
        * 会計よりレジ、領収書伝票再発行
        *****/
        [HttpPost("getCashierChitPrintDataByPayment")]
        public async Task<RpnMdPrint> getCashierChitPrintDataByPayment(ReqMdPaymentReceipt reqDto)
        {
            //受付、注文のデータを取得
            ReqMdSeatCurrentInfos reqCashier = new ReqMdSeatCurrentInfos();
            reqCashier.storeNumber = reqDto.storeNumber;
            reqCashier.SelectedRecpNumber = reqDto.receptionNumber;
            RpnMdCashierInfos rsltCashier = await _cashierService.selectReciptionInfo(reqCashier);
            //会計レジ伝票呼ぶ
            ReqMdCashierPrint reqChit = new ReqMdCashierPrint();
            reqChit.storeInfo = rsltCashier.storeInfo;
            reqChit.seatTakeoutRecpInfo = rsltCashier.seatTakeoutRecpInfo;
            reqChit.orderInfoList = rsltCashier.orderInfoList;
            //帳票対象注文を設定
            reqChit.orderInfoList.ForEach(od =>
            {
                od.casherPaySelected = od.PaymentNumber == reqDto.paymentNumber ? 1 : 0;
            });
            //割引・サービス料を設定
            var discountInfos = await _dbContext.TPaymentDiscounts
                .Where(pyDs =>
                    pyDs.StoreNumber == reqDto.storeNumber
                    && pyDs.PaymentNumber == reqDto.paymentNumber)
                .ToListAsync();
            reqChit.discountAmount = 0;
            reqChit.serviceAmount = 0;
            foreach (var dcInf in discountInfos)
            {
                if (dcInf.DiscountPremiumKbn == "001")
                {
                    reqChit.serviceAmount += (int)(dcInf.PremiumYen ?? 0);
                }
                else
                {
                    reqChit.discountAmount += (int)(dcInf.DiscountYen ?? 0);
                }
            }

            var payment = await _dbContext.TPayments.Where(pay => pay.StoreNumber == reqDto.storeNumber && pay.PaymentNumber == reqDto.paymentNumber).FirstOrDefaultAsync();
            if (payment != null)
            {
                reqChit.storeNumber = reqDto.storeNumber;
                reqChit.paymentKbn = payment.PaymentKbn;
                reqChit.paymentClassKbn = payment.PaymentClassKbn;
            }

            //レシート・領収書設定
            reqChit.withReceipt = reqDto.withReceipt;
            reqChit.withReceiptChit = reqDto.withReceiptChit;


            RpnMdPrint rtnDto = await _chitPrintService.getOrdersCashierChitPrintData(reqChit);
            return rtnDto;
        }

        /*****
        * 日計レポート
        *****/
        [HttpPost("getDailyReportsData")]
        public async Task<RpnMdPrint> getDailyReportsData(ReqMdDailyReport reqDto)
        {
            RpnMdPrint rtnDto = await _chitPrintService.getDailyReportsData(reqDto);
            return rtnDto;
        }

                /*****
        * 日計レポート
        *****/
        [HttpPost("getDailyReportsData")]
        public async Task<RpnMdPrint> getCancelReportsData(ReqMdDailyReport reqDto)
        {
            RpnMdPrint rtnDto = await _chitPrintService.getCancelReportsData(reqDto);
            return rtnDto;
        }
    }
}
