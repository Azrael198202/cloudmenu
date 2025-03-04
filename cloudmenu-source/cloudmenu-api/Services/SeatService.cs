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
    public class SeatService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<SeatService> _logger;

        public SeatService(ILogger<SeatService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation(1, "NLog injected into SeatService");
        }

        public async Task<RpnMdSeatCurrentInfos> selectZasekiCurrentInfos(ReqMdSeatCurrentInfos reqDto)
        {
            _logger.LogDebug("SeatController-selectZasekiCurrentInfos request:{0}", reqDto);
            //戻るデータ
            RpnMdSeatCurrentInfos rpnDto = new RpnMdSeatCurrentInfos();

            //更新間隔
            var refeshIntervalTimeStr = await _dbContext.MCategories
                .Where(ctg => ctg.StoreNumber == reqDto.storeNumber
                    && ctg.CategoryClassNumber == CategoryConst.C_201_座席状況一覧更新間隔.value
                    && ctg.CategoryKbn == CategoryConst.C_201_座席状況一覧更新間隔.ctg_001_更新間隔_秒)
                .Select(ctg => ctg.CategoryOptionValues)
                .FirstOrDefaultAsync();

            int refeshIntervalTimeInt = 3500;
            if (int.TryParse(refeshIntervalTimeStr, out refeshIntervalTimeInt))
            {
                rpnDto.refeshIntervalTime = refeshIntervalTimeInt;
            }

            //座席状況一覧警告時間
            var errTimes = await _dbContext.MCategories
                .Where(ctg =>
                    ctg.StoreNumber == reqDto.storeNumber &&
                    ctg.CategoryClassNumber == CategoryConst.C_200_座席状況一覧警告時間.value
                )
                .Select(ctg => ctg)
                .ToListAsync();
            int storeOrderWaitMinutes = int.Parse(errTimes.Where(ctg => ctg.CategoryKbn == CategoryConst.C_200_座席状況一覧警告時間.ctg_001_40).First().CategoryKbnName);
            int takeoutOrderLeftMinutes = int.Parse(errTimes.Where(ctg => ctg.CategoryKbn == CategoryConst.C_200_座席状況一覧警告時間.ctg_002_10).First().CategoryKbnName);

            // 2021/07/02 ドリンク時間追加
            int drinkOrderWaitMinutes = int.Parse(errTimes.Where(ctg => ctg.CategoryKbn == CategoryConst.C_200_座席状況一覧警告時間.ctg_003_15).First().CategoryKbnName);

            //***** すべての座席情報を取得
            var seatSumInfoQuery = _dbContext.MSeats
                .Where(st =>
                    st.StoreNumber == reqDto.storeNumber
                    && st.DelFlg == FlgConst.off
                )
                .Select(st => new DbExtSeatSumInfos()
                {
                    StoreNumber = st.StoreNumber,
                    SeatLevel = st.SeatLevel,
                    SeatNumber = st.SeatNumber,
                    SeatKbn = st.SeatKbn,
                    SeatName = st.SeatName,
                    SeatQuantity = st.SeatQuantity,
                    SeatSizeVertical = st.SeatSizeVertical,
                    SeatSizeHorizontal = st.SeatSizeHorizontal,
                    SeatPositionVertical = st.SeatPositionVertical,
                    SeatPositionHorizontal = st.SeatPositionHorizontal,
                })
                ;
            var seatSumInfoList = await seatSumInfoQuery.ToListAsync();

            //*****  完了してない店内注文情報と顧客情報を取る
            var recCstmListQuery =
                from trcp in _dbContext.TReceptions
                join cstm in _dbContext.MCustomers
                    .Where(ct => ct.CustomerDelFlg == FlgConst.off)
                on new { trcp.StoreNumber, trcp.CustomerNumber } equals new { cstm.StoreNumber, cstm.CustomerNumber } into cstmGp
                from cstmLeft in cstmGp.DefaultIfEmpty()
                where trcp.StoreNumber == reqDto.storeNumber
                    // 店内
                    && trcp.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_001_店内
                    && trcp.SeatEndDate == null
                    && trcp.ReceptionDelFlg == FlgConst.off
                select new
                {
                    trcp = trcp,
                    CustomerTel = cstmLeft.CustomerTel,
                    CustomerName = cstmLeft.CustomerName
                };
            var recCstmList = await recCstmListQuery.ToListAsync();

            //座席情報に注文情報を付ける
            foreach (var seatSumInfo in seatSumInfoList)
            {
                //対応受付情報を取得
                var recInfo = recCstmList.
                Where(rec =>
                    rec.trcp.SeatLevel == seatSumInfo.SeatLevel
                    && rec.trcp.SeatNumber == seatSumInfo.SeatNumber
                ).FirstOrDefault();

                if (recInfo == null)
                {
                    //0:空き
                    seatSumInfo.SeatStatusKbn = CategoryConst.C_007_座席状況区分.ctg_001_空き;
                    seatSumInfo.SeatStatusNo = 0;
                }
                else
                {
                    seatSumInfo.ReceptionNumber = recInfo.trcp.ReceptionNumber;                 //受付番号
                    seatSumInfo.ReceptionBranchNumber = recInfo.trcp.ReceptionBranchNumber;     //受付枝番
                    seatSumInfo.ReceptionKbn = recInfo.trcp.ReceptionKbn;                       //受付区分
                    seatSumInfo.SeatStartDate = recInfo.trcp.SeatStartDate;                     //着席日時
                    seatSumInfo.SeatEndDate = recInfo.trcp.SeatEndDate;                         //退席日時
                    seatSumInfo.SeatStatusKbn = recInfo.trcp.SeatStatusKbn;                     //座席状況区分
                    seatSumInfo.SeatPeopleMan = recInfo.trcp.SeatPeopleMan ?? 0;                //男性人数
                    seatSumInfo.SeatPeopleWoman = recInfo.trcp.SeatPeopleWoman ?? 0;            //女性人数
                    seatSumInfo.SeatPeopleChildren = recInfo.trcp.SeatPeopleChildren ?? 0;      //子供人数
                    seatSumInfo.ReceptionRemarks = recInfo.trcp.ReceptionRemarks;               //備考
                    seatSumInfo.CustomerNumber = recInfo.trcp.CustomerNumber;                   //顧客コード
                    seatSumInfo.TakeoutName = recInfo.trcp.TakeoutName;                         //持帰り氏名
                    seatSumInfo.TakeoutTel = recInfo.trcp.TakeoutTel;                           //持帰り電話番号
                    seatSumInfo.TakeoutReceiptTime = recInfo.trcp.TakeoutReceiptTime;           //持帰り受取予定日時
                    seatSumInfo.customRepresentName = recInfo.CustomerName ?? "";               //受付代表者名
                    seatSumInfo.customRepresentTel = recInfo.CustomerTel ?? "";                 //電話番号
                    // 注文明細
                    // 2021/07/02 ドリンク処理追加ProductTypeKbn
                    var ordersQuery = from od in _dbContext.TOrders
                                      where od.StoreNumber == recInfo.trcp.StoreNumber
                                      && od.ReceptionNumber == recInfo.trcp.ReceptionNumber
                                      select new
                                      {
                                          od.OrderAfterPrice,
                                          od.OrderDate,
                                          od.OrderTakeoutFlg,
                                          od.OrderCookingFlg,
                                          od.OrderServeFlg,
                                          od.ProductTypeKbn,
                                          od.OrderCancelFlg,
                                      };
                    var ordersAll = await ordersQuery.ToListAsync();
                    var ordersList = ordersAll.Where(od => od.OrderCancelFlg == FlgConst.off).ToList();

                    // 注文総額
                    seatSumInfo.OrderPriceSum = ordersList.Sum(od => od.OrderAfterPrice ?? 0);
                    // 注文総件数
                    seatSumInfo.orderedAllCount = ordersAll.Count();
                    // 注文総件数
                    seatSumInfo.orderDetailCount = ordersList.Count();

                    //座席状況 設定
                    if (ordersList.Count == 0)
                    {
                        //1:注文中
                        seatSumInfo.SeatStatusNo = 1;
                    }
                    else
                    {
                        // 持ち帰り以外 未配膳料理がある場合
                        // 2021/07/02 ドリンク処理追加
                        if (ordersList.Any(od => od.OrderTakeoutFlg == FlgConst.off && od.OrderServeFlg == FlgConst.off))
                        {
                            //2:配膳中
                            seatSumInfo.SeatStatusNo = 2;
                            // キッチン出力用商品分類区分取得(2021/07/21 追加)
                            var productTypeForCook = await _dbContext.MCategories
                                    .Where(ct =>
                                        ct.StoreNumber == reqDto.storeNumber
                                        && ct.CategoryClassNumber == CategoryConst.C_100_キッチン出力用商品分類区分.value)
                                    .Select(ctg => ctg.CategoryKbn)
                                    .ToListAsync();

                            // デシャップ出力用商品分類区分取得(2021/07/21 追加)
                            var productTypeForDish = await _dbContext.MCategories
                                .Where(ct =>
                                    ct.StoreNumber == reqDto.storeNumber
                                    && ct.CategoryClassNumber == CategoryConst.C_101_デシャップ出力用商品分類区分.value)
                                .Select(ctg => ctg.CategoryKbn)
                                .ToListAsync();

                            var drinkCook = ordersList
                                .Where(od => productTypeForDish.Contains(od.ProductTypeKbn)).ToList();
                            var foodCook = ordersList
                                .Where(od => productTypeForCook.Contains(od.ProductTypeKbn)).ToList();

                            if (foodCook.Count != 0)
                            {
                                // ドリンク以外の商品(2021/07/21 修正)
                                //注文待ち時間を超える場合、4:通知メッセージ有り
                                var waitTimeOverOrder = ordersList.Where(
                                    od => od.OrderTakeoutFlg == FlgConst.off
                                        && od.OrderServeFlg == FlgConst.off
                                        && productTypeForCook.Contains(od.ProductTypeKbn)
                                        && DateTime.Compare(od.OrderDate.AddMinutes(storeOrderWaitMinutes), DateTime.Now) < 0
                                    )
                                    .OrderBy(od => od.OrderDate)
                                    .FirstOrDefault();
                                if (waitTimeOverOrder != null)
                                {
                                    seatSumInfo.SeatStatusNo = 4;

                                    double overTimes = DateTime.Now.Subtract(waitTimeOverOrder.OrderDate).TotalMinutes;
                                    // 代表座席のみ通知を付ける
                                    if (seatSumInfo.ReceptionBranchNumber == 1)
                                    {
                                        rpnDto.notifyList.Add(new NotifyEntity
                                        {
                                            //1:ホール 2:個室 3:持ち帰り
                                            notifyKbn = seatSumInfo.SeatKbn == CategoryConst.C_003_座席区分.ctg_001_ホール ? 1 : 2,
                                            seatTakeoutName = seatSumInfo.SeatName,
                                            notifyMsg = $"のお客様のご注文は{((int)overTimes)}分が経過しました。(料理)"
                                        });
                                    }
                                }
                            }
                            if (drinkCook.Count != 0)
                            {
                                //注文待ち時間を超える場合、4:通知メッセージ有り(2021/07/21 修正)
                                var drinkWaitTimeOverOrder = ordersList.Where(
                                    od => od.OrderTakeoutFlg == FlgConst.off
                                        && od.OrderServeFlg == FlgConst.off
                                        && productTypeForDish.Contains(od.ProductTypeKbn)
                                        && DateTime.Compare(od.OrderDate.AddMinutes(drinkOrderWaitMinutes), DateTime.Now) < 0
                                    )
                                    .OrderBy(od => od.OrderDate)
                                    .FirstOrDefault();
                                if (drinkWaitTimeOverOrder != null)
                                {
                                    seatSumInfo.SeatStatusNo = 4;

                                    double overTimes = DateTime.Now.Subtract(drinkWaitTimeOverOrder.OrderDate).TotalMinutes;
                                    // 代表座席のみ通知を付ける
                                    if (seatSumInfo.ReceptionBranchNumber == 1)
                                    {
                                        rpnDto.notifyList.Add(new NotifyEntity
                                        {
                                            //1:ホール 2:個室 3:持ち帰り
                                            notifyKbn = seatSumInfo.SeatKbn == CategoryConst.C_003_座席区分.ctg_001_ホール ? 1 : 2,
                                            seatTakeoutName = seatSumInfo.SeatName,
                                            notifyMsg = $"のお客様のご注文は{((int)overTimes)}分が経過しました。(ドリンク)"
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            //3:待ちなし
                            seatSumInfo.SeatStatusNo = 3;
                        }
                    }
                }
            }

            // 座席共用状況を更新
            foreach (var seatSumInfo in seatSumInfoList)
            {
                if (!String.IsNullOrEmpty(seatSumInfo.ReceptionNumber))
                {
                    if (seatSumInfo.ReceptionBranchNumber != 1)
                    {
                        //座席共用(代表座席名)
                        var mergeSeat = seatSumInfoList
                            .Where(st => st.ReceptionNumber == seatSumInfo.ReceptionNumber
                                && st.ReceptionBranchNumber == 1)
                            .FirstOrDefault();
                        if (mergeSeat != null)
                        {
                            seatSumInfo.SeatMergeWith = $"{mergeSeat.SeatName}と共用中";
                        }
                    }

                    //座席共用(全座席名)
                    var mergeSeatAll = seatSumInfoList
                        .Where(st => st.ReceptionNumber == seatSumInfo.ReceptionNumber)
                        .Select(st => st.SeatName)
                        .ToList();
                    seatSumInfo.seatMergeAll = String.Join(" ", mergeSeatAll);
                }
            }

            //座席総合情報
            rpnDto.seatSumInfoList = seatSumInfoList;

            //*****  完了してないの持ち帰り注文情報と顧客情報を取る
            var takeOutRecCstmQuery =
                from trcp in _dbContext.TReceptions
                where trcp.StoreNumber == reqDto.storeNumber
                    // 持ち帰り
                    && trcp.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト
                    && trcp.SeatEndDate == null
                    && trcp.ReceptionDelFlg == FlgConst.off
                select new DbExtSeatSumInfos
                {
                    ReceptionNumber = trcp.ReceptionNumber,
                    ReceptionBranchNumber = trcp.ReceptionBranchNumber,
                    ReceptionKbn = trcp.ReceptionKbn,
                    SeatStartDate = trcp.SeatStartDate,
                    SeatEndDate = trcp.SeatEndDate,
                    SeatStatusKbn = trcp.SeatStatusKbn,
                    SeatPeopleMan = trcp.SeatPeopleMan ?? 0,
                    SeatPeopleWoman = trcp.SeatPeopleWoman ?? 0,
                    SeatPeopleChildren = trcp.SeatPeopleChildren ?? 0,
                    ReceptionRemarks = trcp.ReceptionRemarks,
                    CustomerNumber = trcp.CustomerNumber,
                    TakeoutName = trcp.TakeoutName,
                    TakeoutTel = trcp.TakeoutTel,
                    TakeoutReceiptTime = trcp.TakeoutReceiptTime,
                    customRepresentName = trcp.TakeoutName,
                    customRepresentTel = trcp.TakeoutTel,
                };
            var takeOutRecCstmList = await takeOutRecCstmQuery.ToListAsync();
            foreach (var takeoutRecvInfo in takeOutRecCstmList)
            {

                takeoutRecvInfo.orderedAllCount = await _dbContext.TOrders
                    .Where(od =>
                        od.StoreNumber == reqDto.storeNumber
                        && od.ReceptionNumber == takeoutRecvInfo.ReceptionNumber)
                    .CountAsync();
                if (takeoutRecvInfo.TakeoutReceiptTime != null)
                {
                    if (((DateTime)takeoutRecvInfo.TakeoutReceiptTime).Subtract(DateTime.Now).TotalMinutes < takeoutOrderLeftMinutes)
                    {
                        if (_dbContext.TOrders
                            .Any(od => od.StoreNumber == reqDto.storeNumber
                            && od.ReceptionNumber == takeoutRecvInfo.ReceptionNumber
                            && od.OrderCancelFlg == FlgConst.off
                            && od.OrderServeFlg == FlgConst.off
                            )
                        )
                        {
                            rpnDto.notifyList.Add(new NotifyEntity
                            {
                                //1:ホール 2:個室 3:持ち帰り
                                notifyKbn = 3,
                                seatTakeoutName = $"持ち帰り{takeoutRecvInfo.ReceptionNumber}",
                                notifyMsg = $"のお客様のご注文は受け取りまで{takeoutOrderLeftMinutes}分以内、調理がまだ完了してない。"
                            });

                        }
                    }
                }
            }
            rpnDto.takeOutSumInfoList = takeOutRecCstmList;

            //選択された座席の受付情報
            var selectedRecpNumber = reqDto.SelectedRecpNumber;
            if (!String.IsNullOrEmpty(selectedRecpNumber))
            {
                //座席受付より探す
                var selectedSeatSumInfo = seatSumInfoList.Where(st => st.ReceptionNumber == selectedRecpNumber).FirstOrDefault();
                if (selectedSeatSumInfo == null)
                {
                    //持ち帰り受付より探す
                    selectedSeatSumInfo = takeOutRecCstmList.Where(st => st.ReceptionNumber == selectedRecpNumber).FirstOrDefault();
                }

                if (selectedSeatSumInfo != null)
                {
                    //選択された座席の注文明細
                    var selectedOdquery =
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
                        join rcp in _dbContext.TReceptions
                        on new { od.StoreNumber, od.ReceptionNumber, od.ReceptionBranchNumber } equals new { rcp.StoreNumber, rcp.ReceptionNumber, rcp.ReceptionBranchNumber }
                        into rcpGp
                        from rcpLj in rcpGp.DefaultIfEmpty()
                        join seat in _dbContext.MSeats
                        on new { rcpLj.StoreNumber, rcpLj.SeatLevel, rcpLj.SeatNumber } equals new { seat.StoreNumber, seat.SeatLevel, seat.SeatNumber }
                        into seatGp
                        from seatLj in seatGp.DefaultIfEmpty()
                        where od.StoreNumber == reqDto.storeNumber
                            && od.ReceptionNumber == selectedRecpNumber
                            && od.OrderCancelFlg == FlgConst.off
                        select new DbExtOrderDetail
                        {
                            StoreNumber = od.StoreNumber,
                            ReceptionNumber = od.ReceptionNumber,
                            ReceptionBranchNumber = od.ReceptionBranchNumber,
                            SeatName = seatLj.SeatName,
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
                            ProductTypeName = ctgLeft.CategoryKbnName
                        };
                    var selectedOdList = await selectedOdquery.ToListAsync();
                    rpnDto.selectingOrderList = selectedOdList;

                    // 注文総額
                    selectedSeatSumInfo.OrderPriceSum = selectedOdList.Sum(od => od.OrderAfterPrice ?? 0);
                    // 注文総件数
                    selectedSeatSumInfo.orderDetailCount = selectedOdList.Count();
                    rpnDto.selectedSeatTakeoutRecpInfo = selectedSeatSumInfo;
                }
            }

            rpnDto.hallNotifyCount = rpnDto.notifyList.Count(nf => nf.notifyKbn == 1);
            rpnDto.roomNotifyCount = rpnDto.notifyList.Count(nf => nf.notifyKbn == 2);
            return rpnDto;
        }
    }
}