using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.RequestModels;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.LwUtils;
using cloudmenu_api.ResponseModels;
using cloudmenu_api.Services;

namespace cloudmenu_api.Controllers
{
    [ApiController]
    [Route("api/cashier")]
    public class CashierController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CashierController> _logger;
        private readonly CashierService _cashierService;
        // private readonly ChitPrintService _chitPrintService;


        public CashierController(ILogger<CashierController> logger, AppDbContext dbContext, CashierService cashierService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cashierService = cashierService;
            _logger.LogInformation(1, "NLog injected into CashiersController");
        }

        /*****
        * 受付に関するデータを取得
        *****/
        [HttpPost("selectReciptionInfo")]
        public async Task<RpnMdCashierInfos> selectReciptionInfo(ReqMdSeatCurrentInfos reqDto)
        {
            return await _cashierService.selectReciptionInfo(reqDto);
        }

        /*****
        * 受付に関するデータを取得
        *****/
        [HttpPost("orderPayment")]
        public async Task<RpnMdCashierInfos> orderPayment(ReqMdCashierPrint reqDto)
        {
            _dbContext.Database.BeginTransaction();

            string maxPaymentNumberStr = _dbContext.TPayments.Max(pm => pm.PaymentNumber);
            int maxPaymentNumber = 1;
            if (int.TryParse(maxPaymentNumberStr, out maxPaymentNumber))
            {
                maxPaymentNumber = maxPaymentNumber + 1;
            }
            else
            {
                maxPaymentNumber = 1;
            }

            string newPaymentNumber = maxPaymentNumber.ToString().PadLeft(11, '0');

            DateTime dtNow = DateTime.Now;
            string dateNowStr = dtNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //小計個数
            long subTotalQuantity = 0;
            //小計金額
            long subTotalAmount = 0;
            //内消費税
            // long totalTaxAmount = 0;

            foreach (var od in reqDto.orderInfoList)
            {

                if (od.paymentOver == 0 && od.casherPaySelected == 1)
                {
                    subTotalQuantity = subTotalQuantity + od.OrderQuantity ?? 0;
                    subTotalAmount = subTotalAmount + od.OrderAfterPrice ?? 0;
                    // totalTaxAmount = totalTaxAmount + od.includedTaxAmount;

                    string updSql =
                        $@"
                        update t_order
                        set payment_number = '{newPaymentNumber}'
                           ,update_user='{reqDto.storeNumber}'
                           ,update_datetime = '{dateNowStr}'
                        where store_number = '{reqDto.storeNumber}'
                        and   reception_number = '{od.ReceptionNumber}'
                        and   order_voucher_number = '{od.OrderVoucherNumber}'
                        and   order_voucher_details = '{od.OrderVoucherDetails}'
                        ";
                    _logger.LogInformation(1, updSql);
                    _dbContext.Database.ExecuteSqlRaw(updSql);
                }
            }

            //合計
            long totalAmount = subTotalAmount - reqDto.discountAmount + reqDto.serviceAmount;

            //会計テーブル
            TPayment newPayment = new TPayment();
            newPayment.StoreNumber = reqDto.storeNumber;
            newPayment.PaymentNumber = newPaymentNumber;
            newPayment.PaymentDatetime = dtNow;
            //会計金額
            newPayment.PaymentPrice = (uint)subTotalAmount;
            //確定金額
            newPayment.PaymentConfirmPrice = (int)totalAmount;
            newPayment.PaymentClassKbn = reqDto.paymentClassKbn;
            newPayment.PaymentKbn = reqDto.paymentKbn;
            newPayment.PaymentRbFlg = FlgConst.off;
            newPayment.ClosingFlg = FlgConst.off;
            newPayment.RegistrationUser = reqDto.storeNumber;
            newPayment.RegistrationDatetime = dtNow;
            newPayment.UpdateUser = reqDto.storeNumber;
            newPayment.UpdateDatetime = dtNow;

            _dbContext.TPayments.Add(newPayment);
            _dbContext.SaveChanges();

            ushort pyBrchNo = 0;
            // サービス料
            if (reqDto.serviceAmount > 0)
            {
                pyBrchNo++;
                TPaymentDiscount newServicePym = new TPaymentDiscount();
                newServicePym.StoreNumber = reqDto.storeNumber;
                newServicePym.PaymentNumber = newPaymentNumber;
                newServicePym.PaymentBranchNumber = pyBrchNo;
                newServicePym.DiscountPremiumKbn = "001";
                newServicePym.PremiumPercent = 10;
                newServicePym.PremiumYen = (uint)reqDto.serviceAmount;
                newServicePym.RegistrationUser = reqDto.storeNumber;
                newServicePym.RegistrationDatetime = dtNow;
                newServicePym.UpdateUser = reqDto.storeNumber;
                newServicePym.UpdateDatetime = dtNow;

                _dbContext.TPaymentDiscounts.Add(newServicePym);
                _dbContext.SaveChanges();
            }

            // 割引
            if (reqDto.discountAmount > 0)
            {
                pyBrchNo++;
                TPaymentDiscount newServicePym = new TPaymentDiscount();
                newServicePym.StoreNumber = reqDto.storeNumber;
                newServicePym.PaymentNumber = newPaymentNumber;
                newServicePym.PaymentBranchNumber = pyBrchNo;
                newServicePym.DiscountPremiumKbn = "002";
                newServicePym.DiscountPercent = (byte)reqDto.discountPercent;
                newServicePym.DiscountYen = (uint)reqDto.discountAmount;
                newServicePym.RegistrationUser = reqDto.storeNumber;
                newServicePym.RegistrationDatetime = dtNow;
                newServicePym.UpdateUser = reqDto.storeNumber;
                newServicePym.UpdateDatetime = dtNow;

                _dbContext.TPaymentDiscounts.Add(newServicePym);
                _dbContext.SaveChanges();
            }


            // //入出金テーブル
            // string maxMnyIoNumberStr = _dbContext.TMoneyInouts.Max(mny => mny.MoneyInoutNumber);
            // int maxMnyIoNumber = 1;
            // if (int.TryParse(maxMnyIoNumberStr, out maxMnyIoNumber))
            // {
            //     maxMnyIoNumber = maxMnyIoNumber + 1;
            // }
            // else
            // {
            //     maxMnyIoNumber = 1;
            // }
            // string newMnyIoNumber = maxMnyIoNumber.ToString().PadLeft(11, '0');

            // TMoneyInout newMnyIo = new TMoneyInout();
            // newMnyIo.StoreNumber = reqDto.storeNumber;
            // newMnyIo.MoneyInoutNumber = newMnyIoNumber;
            // newMnyIo.MoneyInoutDatetime = dtNow;
            // newMnyIo.MoneyInoutPrice = (uint)totalAmount;
            // newMnyIo.MoneyInoutKbn = "001";
            // newMnyIo.MoneyInoutRbFlg = FlgConst.off;
            // newMnyIo.ClosingFlg = FlgConst.off;
            // newMnyIo.RegistrationUser = reqDto.storeNumber;
            // newMnyIo.RegistrationDatetime = dtNow;
            // newMnyIo.UpdateUser = reqDto.storeNumber;
            // newMnyIo.UpdateDatetime = dtNow;
            // _dbContext.TMoneyInouts.Add(newMnyIo);
            // _dbContext.SaveChanges();


            //すべてレジ完了の場合
            if (!reqDto.orderInfoList.Any(od => od.paymentOver == 0 && od.casherPaySelected == 0))
            {
                _dbContext.Database.ExecuteSqlRaw(
                    $@"
                    update t_reception
                    set    seat_end_date = '{dateNowStr}'
                          ,update_user='{reqDto.storeNumber}'
                          ,update_datetime = '{dateNowStr}'
                    where store_number = '{reqDto.storeNumber}'
                    and   reception_number = '{reqDto.seatTakeoutRecpInfo.ReceptionNumber}'
                    "
                );
            }

            _dbContext.Database.CommitTransaction();

            ReqMdSeatCurrentInfos qryDto = new ReqMdSeatCurrentInfos();
            qryDto.storeNumber = reqDto.storeNumber;
            qryDto.SelectedRecpNumber = reqDto.seatTakeoutRecpInfo.ReceptionNumber;
            RpnMdCashierInfos rtnDto = await this.selectReciptionInfo(qryDto);

            return rtnDto;
        }
    }
}
