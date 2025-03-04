using System.Globalization;
using System.Runtime.Serialization;
using System.Reflection.PortableExecutable;
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

namespace cloudmenu_api.Services
{
    public class CashierService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CashierService> _logger;

        public CashierService(ILogger<CashierService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation(1, "NLog injected into CashierService");
        }

        public async Task<RpnMdCashierInfos> selectReciptionInfo(ReqMdSeatCurrentInfos reqDto)
        {
            RpnMdCashierInfos rtnDto = new RpnMdCashierInfos();

            // 支払い区分
            var ctgcLst = await _dbContext.MCategoryClasses
                .Where(ctgc =>
                    String.Compare(ctgc.CategoryClassNumber, CategoryConst.C_010_現金.value) >= 0
                    && String.Compare(ctgc.CategoryClassNumber, CategoryConst.C_017_未回収.value) <= 0
                    )
                .Select(ctgc => ctgc)
                .ToListAsync();
            var ctgLst = await _dbContext.MCategories
                .Where(ctg =>
                    String.Compare(ctg.CategoryClassNumber, CategoryConst.C_010_現金.value) >= 0
                    && String.Compare(ctg.CategoryClassNumber, CategoryConst.C_017_未回収.value) <= 0
                    )
                .Select(ctg => ctg)
                .ToListAsync();
            rtnDto.paymentGpClassLst = ctgcLst;
            rtnDto.paymentClassLst = ctgLst;

            //サービス料設定
            var serviceFeeCtgSettings = await _dbContext.MCategories
                .Where(ctg => ctg.StoreNumber == reqDto.storeNumber &&
                    ctg.CategoryClassNumber == CategoryConst.C_090_サービス料自動加算設定.value)
                .Select(ctg => ctg)
                .ToListAsync();

            //店情報
            var storeInfo = await _dbContext.MStoreInfos
                .Where(st => st.StoreNumber == reqDto.storeNumber)
                .Select(st => new MStoreInfo
                {
                    StoreName = st.StoreName,
                    StoreAddress = st.StoreAddress,
                    StoreAddress2 = st.StoreAddress2,
                    StoreTel = st.StoreTel
                })
                .FirstAsync();
            rtnDto.storeInfo = storeInfo;

            //消費税計算
            String sysDate = DateTime.Now.ToString("yyyyMMdd");
            var taxInfosList = await _dbContext.MTaxes
                .Where(tx => tx.StoreNumber == reqDto.storeNumber && String.Compare(tx.TaxDateStart, sysDate) <= 0 && String.Compare(tx.TaxDateEnd, sysDate) >= 0)
                .ToListAsync();
            var taxStore = taxInfosList.Where(tx => tx.TaxKbn == "010").First();
            var taxTakeout = taxInfosList.Where(tx => tx.TaxKbn == "020").First();

            //受付情報
            var rcptQuery =
                from rcpt in _dbContext.TReceptions
                join seat in _dbContext.MSeats
                on new { rcpt.StoreNumber, rcpt.SeatLevel, rcpt.SeatNumber } equals new { seat.StoreNumber, seat.SeatLevel, seat.SeatNumber }
                into seatGp
                from seatLj in seatGp.DefaultIfEmpty()
                join cstm in _dbContext.MCustomers
                on new { rcpt.StoreNumber, rcpt.CustomerNumber } equals new { cstm.StoreNumber, cstm.CustomerNumber }
                into cstmGp
                from cstmLj in cstmGp.DefaultIfEmpty()
                where rcpt.StoreNumber == reqDto.storeNumber && rcpt.ReceptionNumber == reqDto.SelectedRecpNumber
                orderby rcpt.ReceptionNumber, rcpt.ReceptionBranchNumber
                select new
                {
                    rcpt.StoreNumber,
                    rcpt.ReceptionNumber,
                    rcpt.ReceptionBranchNumber,
                    rcpt.ReceptionKbn,
                    rcpt.SeatStartDate,
                    seatLj.SeatName,
                    seatLj.SeatKbn,
                    SeatPeopleMan = rcpt.SeatPeopleMan ?? 0,
                    SeatPeopleWoman = rcpt.SeatPeopleWoman ?? 0,
                    SeatPeopleChildren = rcpt.SeatPeopleChildren ?? 0,
                    cstmLj.CustomerName,
                    rcpt.TakeoutName
                };
            var rcptInfoList = await rcptQuery.ToListAsync();

            DbExtSeatSumInfos rcptInfo = new DbExtSeatSumInfos();
            if (rcptInfoList.Count > 0)
            {
                rcptInfo.StoreNumber = rcptInfoList[0].StoreNumber;
                rcptInfo.ReceptionNumber = rcptInfoList[0].ReceptionNumber;
                rcptInfo.ReceptionKbn = rcptInfoList[0].ReceptionKbn;
                rcptInfo.SeatStartDate = rcptInfoList[0].SeatStartDate;
                rcptInfo.seatMergeAll = String.Join(" ", rcptInfoList.Select(rcp => rcp.SeatName));
                rcptInfo.SeatPeopleMan = rcptInfoList[0].SeatPeopleMan;
                rcptInfo.SeatPeopleWoman = rcptInfoList[0].SeatPeopleWoman;
                rcptInfo.SeatPeopleChildren = rcptInfoList[0].SeatPeopleChildren;
                rcptInfo.customRepresentName = String.IsNullOrEmpty(rcptInfoList[0].CustomerName) ? rcptInfoList[0].TakeoutName : rcptInfoList[0].CustomerName;
            }
            //サービス料加算設定
            var srvFeeOn = serviceFeeCtgSettings.Any(ctg => ctg.CategoryKbn == CategoryConst.C_090_サービス料自動加算設定.ctg_001_1 && ctg.CategoryKbnName == FlgConst.on);
            var srvFeeStartHm = serviceFeeCtgSettings.First(ctg => ctg.CategoryKbn == CategoryConst.C_090_サービス料自動加算設定.ctg_002_1730).CategoryKbnName;
            if (srvFeeOn &&
               rcptInfoList.Any(rcpt => rcpt.SeatKbn == CategoryConst.C_003_座席区分.ctg_003_個室) &&
               String.Compare(srvFeeStartHm, rcptInfo.seatStartDateHm) <= 0)
            {
                rcptInfo.seatServicePercent = taxStore.TaxPercent;
            }
            else
            {
                rcptInfo.seatServicePercent = 0;
            }

            rcptInfo.seatServiceTaxPercent = taxStore.TaxPercent;
            rcptInfo.discountTaxPercent = rcptInfo.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? taxTakeout.TaxPercent : taxStore.TaxPercent;

            rtnDto.seatTakeoutRecpInfo = rcptInfo;


            //注文情報 List<DbExtOrderDetail>
            var orderInfoQuery =
                from od in _dbContext.TOrders
                join ctg in _dbContext.MCategories
                    .Where(ctg => ctg.CategoryClassNumber == CategoryConst.C_031_商品分類区分.value
                        && ctg.DelFlg == FlgConst.off)
                on new { StoreNumber = od.StoreNumber, ProductTypeKbn = od.ProductTypeKbn } equals new { StoreNumber = ctg.StoreNumber, ProductTypeKbn = ctg.CategoryKbn }
                into ctgGp
                from ctgLeft in ctgGp.DefaultIfEmpty()
                join prd in _dbContext.MProductFoods.Where(pd => pd.DelFlg == FlgConst.off)
                on new { od.StoreNumber, od.ProductNumber } equals new { prd.StoreNumber, prd.ProductNumber }
                into prdGp
                from prdLeft in prdGp.DefaultIfEmpty()
                where od.StoreNumber == reqDto.storeNumber
                    && od.ReceptionNumber == reqDto.SelectedRecpNumber
                    && od.OrderCancelFlg == FlgConst.off
                select new DbExtOrderDetail
                {
                    StoreNumber = od.StoreNumber,
                    ReceptionNumber = od.ReceptionNumber,
                    ReceptionBranchNumber = od.ReceptionBranchNumber,
                    OrderVoucherNumber = od.OrderVoucherNumber,
                    OrderVoucherDetails = od.OrderVoucherDetails,
                    OrderDate = od.OrderDate,
                    ProductNumber = od.ProductNumber,
                    ProductSetNumber = od.ProductSetNumber,
                    ProductNameCh = od.ProductNameCh,
                    ProductSetName = od.ProductSetName,
                    ProductTypeKbn = od.ProductTypeKbn,
                    ProductPrice = od.ProductPrice,
                    OrderQuantity = od.OrderQuantity,
                    OrderPrice = od.OrderPrice,
                    OrderDiscountPercent = od.OrderDiscountPercent,
                    OrderDiscountYen = od.OrderDiscountYen,
                    OrderAfterPrice = od.OrderAfterPrice,
                    OrderTakeoutFlg = od.OrderTakeoutFlg,
                    OrderCookingFlg = od.OrderCookingFlg,
                    OrderServeFlg = od.OrderServeFlg,
                    OrderCancelFlg = od.OrderCancelFlg,
                    OrderRemarks = od.OrderRemarks,
                    PaymentNumber = od.PaymentNumber,
                    RegistrationDatetime = od.RegistrationDatetime,
                    ProductNameJp = prdLeft.ProductNameJp,
                    ProductTypeName = ctgLeft.CategoryKbnName,
                    paymentOver = String.IsNullOrEmpty(od.PaymentNumber) ? 0 : 1
                };
            var orderInfoList = await orderInfoQuery.ToListAsync();
            orderInfoList = orderInfoList.OrderBy(od => od.paymentOver).ThenBy(od => od.OrderVoucherDetails).ToList();



            foreach (var order in orderInfoList)
            {
                order.taxRate = order.OrderTakeoutFlg == FlgConst.on ? taxTakeout.TaxPercent : taxStore.TaxPercent;
                decimal amountIncludTax = order.OrderAfterPrice ?? 0;

                order.includedTaxAmount = CmnUtils.calsIncludeTax(amountIncludTax, order.taxRate);
                // ((long)Math.Floor(amountIncludTax - amountIncludTax * 100 / (order.taxRate + 100)));
            }
            rtnDto.orderInfoList = orderInfoList;

            return rtnDto;
        }
    }
}