using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
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
using cloudmenu_api.dailyReport;
using cloudmenu_api.DailyReport;
using cloudmenu_api.DbEntitiesExt;

namespace cloudmenu_api.Services
{
    public class ChitPrintService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ChitPrintService> _logger;

        private readonly string printXmlHeaderBody =
        @"
        <s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
            <s:Header>
                <parameter xmlns='http://www.epson-pos.com/schemas/2011/03/epos-print'>
                <devid>local_printer</devid>
                <timeout>6000</timeout>
                <printjobid>ABC123</printjobid>
                </parameter>
            </s:Header>
            <s:Body>
                <epos-print xmlns='http://www.epson-pos.com/schemas/2011/03/epos-print'>
                    <text lang='mul'/>
                    <text font='font_a'/>
                    <text smooth='true'/>
        ";
        private readonly string printXmlBodyHeaderEnd =
        @"
                </epos-print>
            </s:Body>
        </s:Envelope> 
        ";

        private readonly string chitLine = "------------------------------------------------";
        private readonly string rowLine = "  --------------------------------------------  ";

        public ChitPrintService(ILogger<ChitPrintService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation(1, "NLog injected into ChitPrintService");
        }

        public async Task<RpnMdPrint> getOrdersCookChitXmlData(ReqMdSeatPrintCookChit reqDto)
        {
            RpnMdPrint rpnDto = new RpnMdPrint();
            // 受付情報
            var seatInfoQuery =
                    from rcp in _dbContext.TReceptions
                    join st in _dbContext.MSeats
                    on new { StoreNumber = rcp.StoreNumber, SeatLevel = rcp.SeatLevel, SeatNumber = rcp.SeatNumber } equals
                       new { StoreNumber = st.StoreNumber, SeatLevel = st.SeatLevel, SeatNumber = st.SeatNumber }
                    into stGp
                    from stLj in stGp.DefaultIfEmpty()
                    where rcp.StoreNumber == reqDto.storeNumber &&
                        rcp.ReceptionNumber == reqDto.selectedRecpNumber
                    select new DbExtSeatSumInfos
                    {
                        ReceptionNumber = rcp.ReceptionNumber,
                        SeatName = stLj.SeatName,
                        ReceptionKbn = rcp.ReceptionKbn,
                        SeatPeopleMan = rcp.SeatPeopleMan ?? 0,
                        SeatPeopleWoman = rcp.SeatPeopleWoman ?? 0,
                        SeatPeopleChildren = rcp.SeatPeopleChildren ?? 0
                    };
            var seatRcpInfo = seatInfoQuery.FirstOrDefault();

            var allPdtsQuery =
                from pd in _dbContext.MProductFoods
                where pd.StoreNumber == reqDto.storeNumber
                   && pd.DelFlg == FlgConst.off
                select new
                {
                    pd.ProductNumber,
                    pd.ProductNameJp
                };
            var allPdts = await allPdtsQuery.ToListAsync();

            var orderToPrintOng = await getPrintOrders(reqDto.storeNumber, reqDto.ordersToPrint);

            //注文一覧
            var ordersToPrintList = orderToPrintOng.Select(
                od =>
                new DbExtOrderDetail
                {
                    OrderVoucherNumber = od.OrderVoucherNumber,
                    OrderVoucherDetails = od.OrderVoucherDetails,
                    OrderDate = od.OrderDate,
                    ProductSetName = od.ProductSetName,
                    ProductNameCh = od.ProductNameCh,
                    ProductNameJp = allPdts.FirstOrDefault(pdt => pdt.ProductNumber == od.ProductNumber).ProductNameJp ?? "",
                    ProductTypeKbn = od.ProductTypeKbn,
                    OrderQuantity = od.OrderQuantity,
                    OrderRemarks = od.OrderRemarks,
                    OrderDiscountYen = od.OrderDiscountYen
                }
            ).ToList();

            // プリンタ調理指示用
            var cookChitPrinters = await _dbContext.MCategories
                .Where(cg =>
                    cg.StoreNumber == reqDto.storeNumber
                    && cg.CategoryClassNumber == CategoryConst.C_020_プリンタ情報.value
                    && (cg.CategoryKbn == CategoryConst.C_020_プリンタ情報.ctg_010_プリンタ１調理指示用
                        || cg.CategoryKbn == CategoryConst.C_020_プリンタ情報.ctg_011_プリンタ２調理指示用)
                        )
                .ToListAsync();

            // 調理指示用プリンタのデータを作る
            var productTypeForCook = await _dbContext.MCategories
                .Where(ct =>
                    ct.StoreNumber == reqDto.storeNumber
                    && ct.CategoryClassNumber == CategoryConst.C_100_キッチン出力用商品分類区分.value)
                .ToListAsync();

            foreach (var ctgPrinter in cookChitPrinters)
            {
                RpnMdPrintData cookPrintData = new RpnMdPrintData();
                cookPrintData.printerUri = $"{ctgPrinter.CategoryKbnAbbreviation}/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                cookPrintData.printerName = ctgPrinter.CategoryKbnName;

                // 2021/07/02 伝票出力(調理指示)仕様変更
                // StringBuilder printXmlOrders = new StringBuilder();
                bool orderData = false;
                StringBuilder printXml = new StringBuilder();
                printXml
                .AppendLine(printXmlHeaderBody);

                printXml.AppendLine($"<text>{seatRcpInfo.ReceptionNumber}&#9;人数：{seatRcpInfo.SeatPelpleAll}名&#9;</text>");

                string seatNameToShow = seatRcpInfo.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? "持帰" : seatRcpInfo.SeatName;
                //*****************座席名
                printXml.AppendLine("<text width='2' height='2'/>");
                printXml.AppendLine($"<text x='450'>{seatNameToShow}</text>");
                printXml.AppendLine("<feed/>");
                printXml.AppendLine("<feed/>");

                foreach (var odToPrint in ordersToPrintList)
                {
                    //料理の注文の場合
                    if (productTypeForCook.Any(ctgForCook => ctgForCook.CategoryKbn == odToPrint.ProductTypeKbn))
                    {
                        // printXmlOrders.AppendLine(getOrderXml(seatRcpInfo, odToPrint));
                        string orderXmlStr = getOrderDataXml(seatRcpInfo, odToPrint);
                        printXml.AppendLine(orderXmlStr);
                        orderData = true;
                    }
                }
                printXml.AppendLine("<cut type='feed'/>");
                printXml
                .AppendLine(printXmlBodyHeaderEnd);

                if (orderData)
                {
                    cookPrintData.printXmlDatas.Add(printXml.ToString());
                }
                // if (printXmlOrders.Length > 0)
                // {
                //     StringBuilder printXml = new StringBuilder();
                //     printXml
                //         .AppendLine(printXmlHeaderBody)
                //         .AppendLine(printXmlOrders.ToString())
                //         .AppendLine(printXmlBodyHeaderEnd);
                //     cookPrintData.printXmlData = printXml.ToString();
                //     rpnDto.printerDataList.Add(cookPrintData);
                // }
                if (cookPrintData.printXmlDatas.Count > 0)
                {
                    rpnDto.printerDataList.Add(cookPrintData);
                }
            }

            // プリンタデシャップ指示用
            var dishUpChitPrinters = await _dbContext.MCategories
                .Where(cg =>
                    cg.StoreNumber == reqDto.storeNumber
                    && cg.CategoryClassNumber == CategoryConst.C_020_プリンタ情報.value
                    && cg.CategoryKbn == CategoryConst.C_020_プリンタ情報.ctg_012_プリンタ３調理指示用デシャップ
                )
                .ToListAsync();
            // デシャップ指示用プリンタのデータを作る
            foreach (var dishPrinter in dishUpChitPrinters)
            {
                RpnMdPrintData cookPrintData = new RpnMdPrintData();
                cookPrintData.printerUri = $"{dishPrinter.CategoryKbnAbbreviation}/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                cookPrintData.printerName = dishPrinter.CategoryKbnName;

                var productTypeForDish = await _dbContext.MCategories
                    .Where(ct =>
                        ct.StoreNumber == reqDto.storeNumber
                        && ct.CategoryClassNumber == CategoryConst.C_101_デシャップ出力用商品分類区分.value)
                    .ToListAsync();

                // 2021/07/26 伝票出力(デシャップ指示)仕様変更
                bool orderData = false;
                StringBuilder printXml = new StringBuilder();
                printXml
                .AppendLine(printXmlHeaderBody);

                printXml.AppendLine($"<text>{seatRcpInfo.ReceptionNumber}&#9;人数：{seatRcpInfo.SeatPelpleAll}名&#9;</text>");

                string seatNameToShow = seatRcpInfo.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? "持帰" : seatRcpInfo.SeatName;
                //*****************座席名
                printXml.AppendLine("<text width='2' height='2'/>");
                printXml.AppendLine($"<text x='450'>{seatNameToShow}</text>");
                printXml.AppendLine("<feed/>");
                printXml.AppendLine("<feed/>");

                foreach (var odToPrint in ordersToPrintList)
                {
                    //デシャップの注文の場合
                    if (productTypeForDish.Any(ctgForCook => ctgForCook.CategoryKbn == odToPrint.ProductTypeKbn))
                    {
                        // printXmlOrders.AppendLine(getOrderXml(seatRcpInfo, odToPrint));
                        string orderXmlStr = getOrderDataXml(seatRcpInfo, odToPrint);
                        printXml.AppendLine(orderXmlStr);
                        orderData = true;
                    }
                }
                printXml.AppendLine("<cut type='feed'/>");
                printXml
                .AppendLine(printXmlBodyHeaderEnd);

                if (orderData)
                {
                    cookPrintData.printXmlDatas.Add(printXml.ToString());
                }

                // if (printXmlOrders.Length > 0)
                // {
                //     StringBuilder printXml = new StringBuilder();
                //     printXml
                //         .AppendLine(printXmlHeaderBody)
                //         .AppendLine(printXmlOrders.ToString())
                //         .AppendLine(printXmlBodyHeaderEnd);
                //     cookPrintData.printXmlData = printXml.ToString();
                //     rpnDto.printerDataList.Add(cookPrintData);
                // }
                if (cookPrintData.printXmlDatas.Count > 0)
                {
                    rpnDto.printerDataList.Add(cookPrintData);
                }
            }

            return rpnDto;
        }

        /**
        *  調理注文料理の印刷データ(2021/07/05 仕様変更)
        **/
        private string getOrderDataXml(DbExtSeatSumInfos seatRcpInfo, DbExtOrderDetail odInfo)
        {
            string seatNameToShow = seatRcpInfo.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? "持帰" : seatRcpInfo.SeatName;
            StringBuilder orderXml = new StringBuilder();

            orderXml.AppendLine("<text align='left'/>");
            //*****************注文番号
            orderXml.AppendLine($"<text width='1' height='1'>{odInfo.OrderVoucherDetails}</text>");
            //*****************注文数
            orderXml.AppendLine($"<text x='55' width='1' height='2'>{odInfo.OrderQuantity}</text>");
            orderXml.AppendLine("<text width='1' height='1'>個</text>");
            //*****************商品名
            orderXml.AppendLine($"<text width='2' height='2' x='155'>{odInfo.productShowName}</text>");
            orderXml.AppendLine("<feed/>");
            //*****************商品名(日本語)
            orderXml.AppendLine($"<text width='1' height='1' x='155'>{odInfo.productShowNameJp}</text>");
            orderXml.AppendLine("<feed/>");
            //*****************商品セット名(単価)
            if (!string.IsNullOrEmpty(odInfo.ProductSetName))
            {
                orderXml.AppendLine($"<text width='1' height='1' x='150'>{odInfo.ProductSetName}({CmnUtils.formatCurrency(odInfo.OrderDiscountYen ?? 0, 0)})</text>");
                orderXml.AppendLine("<feed/>");
            }
            orderXml.AppendLine("<feed/>");
            //***************** 時間；備考
            orderXml.AppendLine($"<text>{odInfo.orderDateHHmm}</text>");
            orderXml.AppendLine($"<text x='155'>{odInfo.OrderRemarks}</text>");
            orderXml.AppendLine("<feed/>");
            orderXml.AppendLine("<text>------------------------------------------------</text>");
            orderXml.AppendLine("<feed/>");
            // orderXml.AppendLine("<cut type='feed'/>");
            return orderXml.ToString();
        }

        /**
        *一つの注文の印刷データ
        **/
        private string getOrderXml(DbExtSeatSumInfos seatRcpInfo, DbExtOrderDetail odInfo)
        {
            string seatNameToShow = seatRcpInfo.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? "持帰" : seatRcpInfo.SeatName;
            StringBuilder orderXml = new StringBuilder();
            orderXml.AppendLine("<sound pattern='pattern_b' repeat='3'/>");
            //*****************座席名
            orderXml.AppendLine("<text align='right'/>");
            orderXml.AppendLine("<text width='2' height='2'/>");
            orderXml.AppendLine($"<text>{seatNameToShow}</text>");
            orderXml.AppendLine("<feed/>");
            orderXml.AppendLine("<feed/>");
            //*****************注文数
            orderXml.AppendLine("<text align='left'/>");
            orderXml.AppendLine($"<text x='50'>{odInfo.OrderQuantity}</text>");
            orderXml.AppendLine("<text width='1' height='1'/>");
            orderXml.AppendLine("<text>個</text>");
            //*****************商品名
            orderXml.AppendLine($"<text width='2' height='2' x='150'>{odInfo.productShowName}</text>");
            orderXml.AppendLine("<feed/>");
            //*****************商品名(日本語)
            orderXml.AppendLine($"<text width='1' height='1' x='150'>{odInfo.productShowNameJp}</text>");
            orderXml.AppendLine("<feed/>");
            //*****************商品セット名
            if (!string.IsNullOrEmpty(odInfo.ProductSetName))
            {
                orderXml.AppendLine($"<text width='1' height='1' x='150'>{odInfo.ProductSetName}</text>");
                orderXml.AppendLine("<feed/>");
            }
            orderXml.AppendLine("<feed/>");
            //*****************備考
            orderXml.AppendLine("<text x='50'>備考</text>");
            orderXml.AppendLine("<text width='2' height='2'/>");
            orderXml.AppendLine($"<text x='150'>{odInfo.OrderRemarks}</text>");
            orderXml.AppendLine("<feed/>");
            orderXml.AppendLine("<feed/>");
            orderXml.AppendLine("<text width='1' height='1'/>");
            orderXml.AppendLine($"<text>{seatRcpInfo.ReceptionNumber}-{odInfo.OrderVoucherDetails}&#9;人数：{seatRcpInfo.SeatPelpleAll}名&#9;日付：{odInfo.orderDateMMdd}&#10;</text>");
            orderXml.AppendLine("<text width='1' height='2'/>");
            orderXml.AppendLine("<text align='right'/>");
            orderXml.AppendLine($"<text>時刻：{odInfo.orderDateHHmm}</text>");
            orderXml.AppendLine("<feed/>");
            orderXml.AppendLine("<cut type='feed'/>");
            return orderXml.ToString();
        }

        public async Task<List<TOrder>> getPrintOrders(string storeNumber, List<TOrder> ordersToPrint)
        {
            // 印刷注文を取得
            // and od.order_voucher_number = '{od.OrderVoucherNumber}' 
            string orConditions = String.Join(" or ",
                ordersToPrint
                .Select(od => $@"
                    (od.reception_number = '{od.ReceptionNumber}' 
                     and od.order_voucher_details = '{od.OrderVoucherDetails}'
                     )")
                .ToList());

            // 2021/07/02 追加
            string ordersToPrintSql = $@"
                select od.*
                      ,pd.product_name_jp as ProductNameJp
                from t_order od
                left join m_product_food pd
                on  od.store_number = pd.store_number
                and od.product_number = pd.product_number
                where od.store_number = '{storeNumber}'
                and   od.order_cancel_flg = '{FlgConst.off}'
                and   ({orConditions})
            ";

            var orderToPrintOng = await _dbContext.TOrders.
                FromSqlRaw(
                    ordersToPrintSql
                ).ToListAsync();

            return orderToPrintOng;
        }
       
        public async Task<RpnMdPrint> getOrdersCashierChitPrintData(ReqMdCashierPrint reqDto)
        {
            RpnMdPrint rpnDto = new RpnMdPrint();

            // プリンター：レジ用
            var cashierChitPrinters = await _dbContext.MCategories
                .Where(cg => cg.CategoryClassNumber == CategoryConst.C_020_プリンタ情報.value
                    && cg.CategoryKbn == CategoryConst.C_020_プリンタ情報.ctg_020_プリンタ２レシートプリンタ
                    )
                .ToListAsync();

            // 商品
            var pdAmountQuery =
                from pd in reqDto.orderInfoList
                where pd.casherPaySelected == 1
                select new
                {
                    pdTtpe = "pd",
                    quantity = (int)(pd.OrderQuantity ?? 0),
                    amount = (int)(pd.OrderAfterPrice ?? 0),
                    taxRate = pd.taxRate,
                    includedTaxAmount = pd.includedTaxAmount,
                }
                ;

            var pdAmountLst = pdAmountQuery.ToList();

            // 割引
            if (reqDto.discountAmount > 0)
            {
                int amount = reqDto.discountAmount;
                int taxRate = reqDto.seatTakeoutRecpInfo.discountTaxPercent;
                long includedTaxAmount = CmnUtils.calsIncludeTax(amount, taxRate);
                pdAmountLst.Add(new
                {
                    pdTtpe = "dis",
                    quantity = 1,
                    amount = -amount,
                    taxRate = taxRate,
                    includedTaxAmount = -includedTaxAmount,
                });
            }

            // サービス料
            if (reqDto.serviceAmount > 0)
            {
                int amount = reqDto.serviceAmount;
                int taxRate = reqDto.seatTakeoutRecpInfo.seatServiceTaxPercent;
                long includedTaxAmount = CmnUtils.calsIncludeTax(amount, taxRate);
                pdAmountLst.Add(new
                {
                    pdTtpe = "srv",
                    quantity = 1,
                    amount = amount,
                    taxRate = taxRate,
                    includedTaxAmount = includedTaxAmount,
                });
            }

            StringBuilder printXml = new StringBuilder();
            printXml.AppendLine(printXmlHeaderBody);
            //店舗情報
            printXml.AppendLine("<text align='center'/>");
            // printXml.AppendLine($"<text width='2' height='2'>{reqDto.storeInfo.StoreName}</text>");
            printXml.AppendLine(SystemSetting.laoshanghaiImgXml);
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>{reqDto.storeInfo.StoreAddress}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>{reqDto.storeInfo.StoreAddress2}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>{reqDto.storeInfo.StoreTel}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>登録番号：T9290001090581</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<feed/>");

            //受付情報
            printXml.AppendLine("<text align='left'/>");
            string sysDateStr = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            printXml.AppendLine($"<text>{sysDateStr}</text>");
            printXml.AppendLine($"<text x='480'>{reqDto.seatTakeoutRecpInfo.seatMergeAll}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>No.{reqDto.seatTakeoutRecpInfo.ReceptionNumber}</text>");
            printXml.AppendLine($"<text x='480'>人数:{reqDto.seatTakeoutRecpInfo.SeatPelpleAll}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>受付時間:{reqDto.seatTakeoutRecpInfo.seatStartDateHm}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine($"<text>{chitLine}</text>");

            //注文データ商品ごとをまとめ
            var orderProductSumList = reqDto.orderInfoList
                .Where(od => od.casherPaySelected == 1)
                .GroupBy(od => new { od.productShowName, od.ProductSetName })
                .Select(odg => new
                {
                    casherPaySelected = odg.First().casherPaySelected,
                    productShowName = odg.Key.productShowName,
                    productShowNameJp = odg.First().productShowNameJp,
                    ProductSetName = odg.Key.ProductSetName,
                    OrderQuantity = odg.Sum(od => od.OrderQuantity),
                    OrderAfterPrice = odg.Sum(od => od.OrderAfterPrice)
                }).ToList();
            foreach (var orderInfo in orderProductSumList)
            {
                if (orderInfo.casherPaySelected == 1)
                {
                    // // 小計数量
                    // subTotalQuantity = subTotalQuantity + orderInfo.OrderQuantity ?? 0;
                    // // 小計金額
                    // subTotalAmount = subTotalAmount + orderInfo.OrderAfterPrice ?? 0;
                    // // 内消費税
                    // totalTaxAmount = totalTaxAmount + orderInfo.includedTaxAmount;

                    printXml.AppendLine($"<text>{orderInfo.productShowName}</text>");
                    printXml.AppendLine($"<text x='380'>{orderInfo.OrderQuantity}</text>");
                    printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(orderInfo.OrderAfterPrice ?? 0, 9)}</text>");
                    printXml.AppendLine("<feed/>");
                    printXml.AppendLine($"<text>{orderInfo.productShowNameJp}</text>");
                    printXml.AppendLine("<feed/>");
                    if (!string.IsNullOrEmpty(orderInfo.ProductSetName))
                    {
                        printXml.AppendLine($"<text>{orderInfo.ProductSetName}</text>");
                        printXml.AppendLine("<feed/>");
                    }
                    printXml.AppendLine("<feed/>");
                }
            }

            // 小計数量
            long subTotalQuantity = pdAmountLst.Where(pd => pd.pdTtpe == "pd").Sum(pd => pd.quantity);
            // 小計金額
            long subTotalAmount = pdAmountLst.Where(pd => pd.pdTtpe == "pd").Sum(pd => pd.amount);

            //小計
            printXml.AppendLine($"<text>{chitLine}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<text>商品計</text>");
            printXml.AppendLine($"<text x='380'>{subTotalQuantity}</text>");
            printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(subTotalAmount, 9)}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<text>サービス料</text>");
            printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(reqDto.serviceAmount, 9)}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<text>小計</text>");
            printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(subTotalAmount + reqDto.serviceAmount, 9)}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<text>割引</text>");
            printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(reqDto.discountAmount * -1, 9)}</text>");
            printXml.AppendLine("<feed/>");

            // 内消費税
            // long totalTaxAmount = pdAmountLst.Sum(pd => pd.includedTaxAmount);
            var pdTaxGpLst = pdAmountLst
                .GroupBy(pd => pd.taxRate)
                .Select(pdG =>
                new
                {
                    taxRate = pdG.Key,
                    amount = pdG.Sum(pd => pd.amount)
                })
                .ToList();
            long totalTaxAmount = pdTaxGpLst.Sum(pd => CmnUtils.calsIncludeTax(pd.amount, pd.taxRate));

            // 2021/07/26 追加 支払い方法の表示
            var paymentClass =
                from ctg in _dbContext.MCategories
                join ctgC in _dbContext.MCategoryClasses
                on new { StoreNumber = ctg.StoreNumber, CategoryClassNumber = ctg.CategoryClassNumber }
                    equals new { StoreNumber = ctgC.StoreNumber, CategoryClassNumber = ctgC.CategoryClassNumber }
                where
                    ctg.StoreNumber == reqDto.storeNumber
                    && ctg.CategoryClassNumber == reqDto.paymentClassKbn
                    && ctg.CategoryKbn == reqDto.paymentKbn
                select new
                {
                    CategoryClassName = ctgC.CategoryClassName,
                    CategoryKbnName = ctg.CategoryKbnName
                };

            var paymentClassName = await paymentClass.FirstOrDefaultAsync();
            string CategoryClassName = "";
            string CategoryKbnName = "";
            if (paymentClassName != null)
            {
                CategoryClassName = paymentClassName.CategoryClassName;
                CategoryKbnName = paymentClassName.CategoryKbnName;
            }
            //合計
            long totalAmount = subTotalAmount - reqDto.discountAmount + reqDto.serviceAmount;
            printXml.AppendLine($"<text>{chitLine}</text>");
            printXml.AppendLine("<feed/>");
            // 2021/07/26 追加 支払い方法の表示
            printXml.AppendLine($"<text>合計 ({CategoryClassName}-{CategoryKbnName})</text>");
            printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(totalAmount, 9)}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<text>内消費税</text>");
            printXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(totalTaxAmount, 9)}</text>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<feed/>");
            printXml.AppendLine("<cut type='feed'/>");

            printXml.AppendLine(printXmlBodyHeaderEnd);


            // 領収書出力
            StringBuilder printXmlReciept = new StringBuilder();
            if (reqDto.withReceiptChit)
            {

                string sysDateYMDStr = DateTime.Now.ToString("yyyy年MM月dd日");

                printXmlReciept.AppendLine(printXmlHeaderBody);
                printXmlReciept.AppendLine($"<text align='right'>{sysDateYMDStr}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='center' width='2' height='2'>領収書</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='center' width='1' height='2' ul='true'>{CmnUtils.padWith(PadOption.PadLeft, 46, "")}様</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                //総額
                printXmlReciept.AppendLine($"<text align='center' width='1' height='2' ul='true'>{CmnUtils.padWith(PadOption.PadBoth, 30, CmnUtils.formatCurrency(totalAmount))}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                //税抜金額
                long totalAmountExcludeTax = totalAmount - totalTaxAmount;
                printXmlReciept.AppendLine($"<text align='center' width='1' height='1' ul='false'>税抜金額{CmnUtils.padWith(PadOption.PadLeft, 30 - 9, CmnUtils.formatCurrency(totalAmountExcludeTax, 9))} </text>");
                printXmlReciept.AppendLine("<feed/>");
                //商品税
                printXmlReciept.AppendLine($"<text align='center'>消費税{CmnUtils.padWith(PadOption.PadLeft, 30 - 7, CmnUtils.formatCurrency(totalTaxAmount, 9))} </text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                //商品税ごと
                // var taxGroupLst = pdAmountLst
                //     .GroupBy(pd => pd.taxRate)
                //     .Select(g => new
                //     {
                //         taxRate = g.Key,
                //         amountSum = g.Sum(pd => pd.amount),
                //         taxSum = g.Sum(pd => pd.includedTaxAmount)
                //     })
                //     .ToList();
                //消費税
                var taxGroupLst = pdAmountLst
                    .GroupBy(pd => pd.taxRate)
                    .Select(g =>
                    {
                        var taxRate = g.Key;
                        var amountSum = g.Sum(pd => pd.amount);
                        return new
                        {
                            taxRate = taxRate,
                            amountSum = amountSum,
                            taxSum = CmnUtils.calsIncludeTax(amountSum, taxRate),
                        };
                    })
                    .ToList();
                foreach (var taxG in taxGroupLst)
                {
                    string taxGRate2 = CmnUtils.padWith(PadOption.PadLeft, 2, taxG.taxRate.ToString());
                    string taxGAmountSum = CmnUtils.padWith(PadOption.PadLeft, 30 - 9, CmnUtils.formatCurrency(taxG.amountSum, 9));
                    string taxGSum = CmnUtils.padWith(PadOption.PadLeft, 30 - 13, CmnUtils.formatCurrency(taxG.taxSum, 9));
                    printXmlReciept.AppendLine($"<text align='center'>税率 {taxGRate2}%{taxGAmountSum} </text>");
                    printXmlReciept.AppendLine("<feed/>");
                    printXmlReciept.AppendLine($"<text align='center'>   (内消費税{taxGSum})</text>");
                    printXmlReciept.AppendLine("<feed/>");
                }
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<text align='left'>上記正に領収いたしました。</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<text align='left'>印刷面を内側に折って保管願います。</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='left' ul='true'>但し{CmnUtils.padWith(PadOption.PadRight, 44, "")}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='left' ul='false'>{reqDto.storeInfo.StoreName}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='left'>{reqDto.storeInfo.StoreAddress}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='left'>{reqDto.storeInfo.StoreAddress2}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='left'>登録番号：T9290001090581</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine($"<text align='right'>No.{reqDto.seatTakeoutRecpInfo.ReceptionNumber}</text>");
                printXmlReciept.AppendLine("<feed/>");
                printXmlReciept.AppendLine("<cut type='feed'/>");

                printXmlReciept.AppendLine(printXmlBodyHeaderEnd);
            }

            // レジ用プリンタのデータを作る
            foreach (var ctgPrinter in cashierChitPrinters)
            {
                // 2021/07/06 領収書だけ出力場合追加
                if (reqDto.withReceipt)
                {
                    RpnMdPrintData cashierChitPtDate = new RpnMdPrintData();
                    cashierChitPtDate.printerUri = $"{ctgPrinter.CategoryKbnAbbreviation}/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                    // cashierChitPtDate.printerUri = $"192.168.1.122/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                    cashierChitPtDate.printerName = ctgPrinter.CategoryKbnName;
                    cashierChitPtDate.printXmlDatas.Add(printXml.ToString());
                    rpnDto.printerDataList.Add(cashierChitPtDate);
                }
                if (reqDto.withReceiptChit)
                {
                    RpnMdPrintData receiptPtDate = new RpnMdPrintData();
                    receiptPtDate.printerUri = $"{ctgPrinter.CategoryKbnAbbreviation}/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                    // receiptPtDate.printerUri = $"192.168.1.122/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                    receiptPtDate.printerName = ctgPrinter.CategoryKbnName;
                    receiptPtDate.printXmlDatas.Add(printXmlReciept.ToString());
                    rpnDto.printerDataList.Add(receiptPtDate);
                }
            }

            return rpnDto;
        }

        public async Task<RpnMdPrint> getDailyReportsData(ReqMdDailyReport reqDto)
        {
            RpnMdPrint rpnDto = new RpnMdPrint();

            List<string> allXmls = new List<string>();

            DailyReportData dailyReportData = await getOrgData(reqDto);
            if (dailyReportData == null)
            {
                rpnDto.status = RepMdStatus.S_204_NoContent;
                return rpnDto;
            }

            string xmlType1 = await this.getDailyReportType1(reqDto, dailyReportData);
            if (!string.IsNullOrEmpty(xmlType1))
            {
                allXmls.Add(xmlType1);
            }
            string xmlType2 = await this.getDailyReportType2(reqDto, dailyReportData);
            if (!string.IsNullOrEmpty(xmlType2))
            {
                allXmls.Add(xmlType2);
            }
            string xmlType3 = await this.getDailyReportType3(reqDto, dailyReportData);
            if (!string.IsNullOrEmpty(xmlType3))
            {
                allXmls.Add(xmlType3);
            }
            string xmlType4 = this.getDailyReportType4(reqDto, dailyReportData);
            if (!string.IsNullOrEmpty(xmlType4))
            {
                allXmls.Add(xmlType4);
            }
            string xmlType5 = this.getDailyReportType5(reqDto, dailyReportData);
            if (!string.IsNullOrEmpty(xmlType5))
            {
                allXmls.Add(xmlType5);
            }

            if (allXmls.Count > 0)
            {
                // プリンター：レジ用
                var cashierChitPrinters = await _dbContext.MCategories
                    .Where(cg => cg.CategoryClassNumber == CategoryConst.C_020_プリンタ情報.value
                        && cg.CategoryKbn == CategoryConst.C_020_プリンタ情報.ctg_020_プリンタ２レシートプリンタ
                        )
                    .ToListAsync();
                // レジ用プリンタのデータを作る
                foreach (var ctgPrinter in cashierChitPrinters)
                {
                    RpnMdPrintData cashierChitPtDate = new RpnMdPrintData();
                    cashierChitPtDate.printerUri = $"{ctgPrinter.CategoryKbnAbbreviation}/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                    cashierChitPtDate.printerName = ctgPrinter.CategoryKbnName;
                    foreach (var xml in allXmls)
                    {
                        cashierChitPtDate.printXmlDatas.Add(xml);
                    }
                    rpnDto.printerDataList.Add(cashierChitPtDate);
                }

            }

            return rpnDto;
        }

        private async Task<DailyReportData> getOrgData(ReqMdDailyReport reqDto)
        {
            DailyReportData dailyReportData = new DailyReportData();

            DateTime reportDateStart = new DateTime();
            DateTime reportDateEnd = new DateTime();
            string dateFrom = reqDto.reportDate;
            string dateTo = reqDto.reportDate;
            if (reqDto.reportIsMonth)
            {
                dateFrom = reqDto.reportDate + "-01";
                dateTo = reqDto.reportDate + "-01";
            }
            DateTime.TryParse(dateFrom + " 00:00:00.000", out reportDateStart);
            DateTime.TryParse(dateTo + " 23:59:59.999", out reportDateEnd);
            if (reqDto.reportIsMonth)
            {
                reportDateEnd = reportDateEnd.AddMonths(1).AddDays(-1);
            }
            dailyReportData.drDateTimeS = reportDateStart;
            dailyReportData.drDateTimeE = reportDateEnd;
            dailyReportData.drDateStrJpS = reportDateStart.ToString("yyyy年MM月dd日");
            dailyReportData.drDateStrJpE = reportDateEnd.ToString("yyyy年MM月dd日");

            var dailyRecptsQuery =
                from rcp in _dbContext.TReceptions
                join seat in _dbContext.MSeats
                on new { rcp.StoreNumber, rcp.SeatNumber } equals new { seat.StoreNumber, seat.SeatNumber }
                into seatGp
                from seatL in seatGp.DefaultIfEmpty()
                where rcp.StoreNumber == reqDto.storeNumber
                    && rcp.ReceptionBranchNumber == 1
                    && DateTime.Compare(rcp.SeatStartDate ?? DateTime.MinValue, reportDateStart) >= 0
                    && DateTime.Compare(rcp.SeatStartDate ?? DateTime.MinValue, reportDateEnd) <= 0
                    && rcp.SeatEndDate != null
                    && rcp.ReceptionDelFlg == FlgConst.off
                select new DRReceiption
                {
                    StoreNumber = rcp.StoreNumber,
                    ReceptionNumber = rcp.ReceptionNumber,
                    ReceptionBranchNumber = rcp.ReceptionBranchNumber,
                    ReceptionKbn = rcp.ReceptionKbn,
                    SeatStartDate = rcp.SeatStartDate,
                    SeatEndDate = rcp.SeatEndDate,
                    SeatPeopleMan = rcp.SeatPeopleMan,
                    SeatPeopleWoman = rcp.SeatPeopleWoman,
                    SeatPeopleChildren = rcp.SeatPeopleChildren,
                    SeatName = seatL.SeatName
                };
            var dailyRecpts = await dailyRecptsQuery.ToListAsync();


            // ※データなしの場合
            if (dailyRecpts.Count <= 0)
            {
                return null;
            }
            dailyReportData.drReceiptions = dailyRecpts;

            //注文***************************************************************
            var recpNumberList = dailyRecpts.Select(rcp => rcp.ReceptionNumber).ToList();
            var dailyOrders = await _dbContext.TOrders.
                Where(od =>
                    od.StoreNumber == reqDto.storeNumber
                    && od.OrderCancelFlg == FlgConst.off
                    && recpNumberList.Contains(od.ReceptionNumber)
                ).Select(od =>
                    new DROrder
                    {
                        StoreNumber = od.StoreNumber,
                        ReceptionNumber = od.ReceptionNumber,
                        ReceptionBranchNumber = od.ReceptionBranchNumber,
                        OrderVoucherNumber = od.OrderVoucherNumber,
                        OrderVoucherDetails = od.OrderVoucherDetails,
                        OrderDate = od.OrderDate,
                        ProductNumber = od.ProductNumber,
                        ProductName = od.ProductNameCh,
                        ProductTypeKbn = od.ProductTypeKbn,
                        OrderQuantity = od.OrderQuantity,
                        OrderDiscountYen = od.OrderDiscountYen,
                        OrderAfterPrice = od.OrderAfterPrice,
                        OrderTakeoutFlg = od.OrderTakeoutFlg,
                        PaymentNumber = od.PaymentNumber
                    }
                )
                .ToListAsync();
            // ※データなしの場合
            if (dailyOrders.Count <= 0)
            {
                return null;
            }

            //店舗情報***************************************************************
            var storeInfo = await _dbContext.MStoreInfos
                .Where(
                    st => st.StoreNumber == reqDto.storeNumber
                ).FirstAsync();
            dailyReportData.drStoreInfo = storeInfo;
            //消費税計算***************************************************************
            String reportDateYMD = reportDateStart.ToString("yyyyMMdd");
            var taxInfosList = await _dbContext.MTaxes
                .Where(tx => tx.StoreNumber == reqDto.storeNumber && String.Compare(tx.TaxDateStart, reportDateYMD) <= 0 && String.Compare(tx.TaxDateEnd, reportDateYMD) >= 0)
                .ToListAsync();
            var taxStore = taxInfosList.Where(tx => tx.TaxKbn == "010").First();
            var taxTakeout = taxInfosList.Where(tx => tx.TaxKbn == "020").First();
            //支払い区分***************************************************************
            var dailyPayClassKbns = await _dbContext.MCategoryClasses.
                Where(ctgC =>
                    ctgC.StoreNumber == reqDto.storeNumber
                    && string.Compare(ctgC.CategoryClassNumber, "010") >= 0
                    && string.Compare(ctgC.CategoryClassNumber, "020") < 0
                ).ToListAsync();
            dailyReportData.drPaymentClassKbns = dailyPayClassKbns;

            var dailyPayKbns = await _dbContext.MCategories.
                Where(ctg =>
                    ctg.StoreNumber == reqDto.storeNumber
                    && string.Compare(ctg.CategoryClassNumber, "010") >= 0
                    && string.Compare(ctg.CategoryClassNumber, "020") < 0
                ).ToListAsync();
            dailyReportData.drPaymentKbns = dailyPayKbns;

            //注文消費税の計算
            dailyOrders.ForEach(od =>
            {
                int taxRate = od.OrderTakeoutFlg == FlgConst.on ? taxTakeout.TaxPercent : taxStore.TaxPercent;
                od.taxRate = taxRate;
                decimal amountIncludTax = od.OrderAfterPrice ?? 0;
                od.taxAmount = CmnUtils.calsIncludeTax(amountIncludTax, taxRate);
            });
            dailyReportData.drOrders = dailyOrders;

            //会計***************************************************************
            var payMentNumList = dailyOrders.Select(od => od.PaymentNumber).ToList().Distinct().ToList();
            var dailyPayments = await _dbContext.TPayments
                .Where(py =>
                    py.StoreNumber == reqDto.storeNumber
                    && payMentNumList.Contains(py.PaymentNumber)
                ).Select(py =>
                    new DRPayment
                    {
                        StoreNumber = py.StoreNumber,
                        PaymentNumber = py.PaymentNumber,
                        PaymentDatetime = py.PaymentDatetime,
                        PaymentPrice = py.PaymentPrice,
                        PaymentConfirmPrice = py.PaymentConfirmPrice,
                        PaymentClassKbn = py.PaymentClassKbn,
                        PaymentKbn = py.PaymentKbn,
                    }
                )
                .ToListAsync();
            //消費税計算
            dailyPayments.ForEach(py =>
            {
                py.PaymentPriceTax = dailyOrders
                    .Where(od => od.PaymentNumber == py.PaymentNumber)
                    .Sum(od => od.taxAmount);
            });
            dailyReportData.drPayments = dailyPayments;

            //会計割引***************************************************************
            var dailyPaymentDiscounts = await _dbContext.TPaymentDiscounts
                .Where(pyd =>
                    pyd.StoreNumber == reqDto.storeNumber
                    && payMentNumList.Contains(pyd.PaymentNumber)
                ).Select(pyd =>
                    new DRPaymentDiscount
                    {
                        StoreNumber = pyd.StoreNumber,
                        PaymentNumber = pyd.PaymentNumber,
                        PaymentBranchNumber = pyd.PaymentBranchNumber,
                        DiscountPremiumKbn = pyd.DiscountPremiumKbn,
                        DiscountPercent = pyd.DiscountPercent,
                        DiscountYen = pyd.DiscountYen,
                        DiscountTax = CmnUtils.calsIncludeTax(pyd.DiscountYen ?? 0, taxStore.TaxPercent),
                        PremiumPercent = pyd.PremiumPercent,
                        PremiumYen = pyd.PremiumYen,
                        PremiumTax = CmnUtils.calsIncludeTax(pyd.PremiumYen ?? 0, taxStore.TaxPercent),
                    }
                )
                .ToListAsync();
            dailyReportData.drPaymentDiscounts = dailyPaymentDiscounts;

            return dailyReportData;
        }
        /**
        * 日計レポート:取引別	
        */
        private async Task<string> getDailyReportType1(ReqMdDailyReport reqDto, DailyReportData dailyReportData)
        {
            if (!reqDto.reportTypes.Contains("1"))
            {
                return "";
            }

            // 組数（受付数）
            int rcptCount = dailyReportData.drReceiptions.Count();
            // 男性
            int manSum = dailyReportData.drReceiptions.Sum(rcp => rcp.SeatPeopleMan ?? 0);
            // 女性
            int womenSum = dailyReportData.drReceiptions.Sum(rcp => rcp.SeatPeopleWoman ?? 0);
            // 子供
            int childrenSum = dailyReportData.drReceiptions.Sum(rcp => rcp.SeatPeopleChildren ?? 0);
            // 客数
            int peopleSum = manSum + womenSum + childrenSum;
            // サービス料件数
            int serviceCount = dailyReportData.drPaymentDiscounts.Where(pyd => pyd.DiscountPremiumKbn == "001").Count();
            long serviceAmount = dailyReportData.drPaymentDiscounts.Where(pyd => pyd.DiscountPremiumKbn == "001").Sum(pyd => pyd.PremiumYen ?? 0);
            long serviceTaxAmount = dailyReportData.drPaymentDiscounts.Where(pyd => pyd.DiscountPremiumKbn == "001").Sum(pyd => pyd.PremiumTax);
            // 値割引件数
            int discountCount = dailyReportData.drPaymentDiscounts.Where(pyd => pyd.DiscountPremiumKbn == "002" || pyd.DiscountPremiumKbn == "003").Count();
            long discountAmount = dailyReportData.drPaymentDiscounts.Where(pyd => pyd.DiscountPremiumKbn == "002" || pyd.DiscountPremiumKbn == "003").Sum(pyd => pyd.DiscountYen ?? 0);
            long discountTaxAmount = dailyReportData.drPaymentDiscounts.Where(pyd => pyd.DiscountPremiumKbn == "002" || pyd.DiscountPremiumKbn == "003").Sum(pyd => pyd.DiscountTax);
            // 総売上点数
            int orderPdSum = dailyReportData.drOrders.Sum(od => od.OrderQuantity ?? 0);
            // 売上(税込み)
            long includeTaxAmount = dailyReportData.drPayments.Sum(py => py.PaymentConfirmPrice ?? 0);
            // 消費税 
            long taxAmount = dailyReportData.drPayments.Sum(py => py.PaymentPriceTax) + serviceTaxAmount - discountTaxAmount;
            // 純売上（税抜き）
            long excludeTaxAmount = includeTaxAmount - taxAmount;
            // 客単価
            int peopleSumCal = peopleSum > 0 ? peopleSum : 1;
            double amountPerPeopleD = includeTaxAmount / peopleSumCal;
            long amountPerPeople = (long)Math.Floor(amountPerPeopleD);


            // 支払い区分ごと
            var payClassKbnSumList = dailyReportData.drPaymentClassKbns
                .Select(ctgc => new
                {
                    payClassKbn = ctgc.CategoryClassNumber,
                    payClassKbnName = ctgc.CategoryClassName,
                    payClassKbnCount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == ctgc.CategoryClassNumber).Count(),
                    payClassKbnAmount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == ctgc.CategoryClassNumber).Sum(py => py.PaymentConfirmPrice ?? 0)
                })
                .ToList();

            // 現金件数
            int cashCount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_010_現金.value).Count();
            long cashAmount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_010_現金.value).Sum(py => py.PaymentPrice ?? 0);
            // クレジット
            int creditCardCount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_011_クレジット.value).Count();
            long creditCardAmount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_011_クレジット.value).Sum(py => py.PaymentPrice ?? 0);
            // 電子マネー
            int eMoneyCount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_012_電子マネー.value).Count();
            long eMoneyAmount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_012_電子マネー.value).Sum(py => py.PaymentPrice ?? 0);
            // 掛売
            int creidtCount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_013_掛売.value).Count();
            long creditAmount = dailyReportData.drPayments.Where(py => py.PaymentClassKbn == CategoryConst.C_013_掛売.value).Sum(py => py.PaymentPrice ?? 0);

            var moneyDayBegin = await _dbContext.TMoneyInouts
                .Where(mIO =>
                    mIO.StoreNumber == reqDto.storeNumber
                    && mIO.MoneyInoutDatetime < dailyReportData.drDateTimeS
                ).SumAsync(mIO =>
                    mIO.MoneyInoutKbn == CategoryConst.C_300_入金区分.value ? mIO.MoneyInoutPrice ?? 0 : -mIO.MoneyInoutPrice ?? 0
                );
            var moneyIOList = await _dbContext.TMoneyInouts
                .Where(mIO =>
                    mIO.StoreNumber == reqDto.storeNumber
                    && mIO.MoneyInoutDatetime >= dailyReportData.drDateTimeS
                    && mIO.MoneyInoutDatetime <= dailyReportData.drDateTimeE
                ).GroupBy(mIO => mIO.MoneyInoutKbn)
                .Select(mIoG =>
                    new
                    {
                        mIOname = mIoG.Key == CategoryConst.C_300_入金区分.value ? "入金" : "出金",
                        mIOFlg = mIoG.Key == CategoryConst.C_300_入金区分.value ? 1 : -1,
                        mIOcount = mIoG.Count(),
                        mIOamount = mIoG.Sum(mIO => mIO.MoneyInoutPrice ?? 0),
                    }
                ).ToListAsync();
            var moneyDayEnd = moneyDayBegin + moneyIOList.Sum(mIO => mIO.mIOFlg * mIO.mIOamount);

            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine(printXmlHeaderBody);
            //ヘッダ部
            reportXml.AppendLine(getDailyReportCommonHead("取引別", dailyReportData));
            if (!reqDto.reportIsMonth)
            {
                //明細一覧
                reportXml.AppendLine(getDailyReportDetailList(dailyReportData));
                reportXml.AppendLine($"<text x='0'>{chitLine}</text>");
                reportXml.AppendLine("<feed/>");
            }
            //合計
            reportXml.AppendLine($"<text>組数</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCommaNum(rcptCount, 8)}組</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>客数</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCommaNum(peopleSum, 8)}客</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>  男性</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCommaNum(manSum, 8)}客</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>  女性</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCommaNum(womenSum, 8)}客</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>  子供</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCommaNum(childrenSum, 8)}客</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>客単価</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(amountPerPeople, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>総売上点数</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCommaNum(orderPdSum, 8)}点</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>売上</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(includeTaxAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<text>消費税</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(taxAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<text>純売上</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(excludeTaxAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<text>サービス料</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(serviceCount, 4)}件</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(serviceAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<text>値割引</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(discountCount, 4)}件</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(discountAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text align='center'>※支払情報※</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text align='left'></text>");
            foreach (var payClassKbn in payClassKbnSumList)
            {
                reportXml.AppendLine($"<text>{payClassKbn.payClassKbnName}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(payClassKbn.payClassKbnCount, 4)}件</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(payClassKbn.payClassKbnAmount, 9)}</text>");
                reportXml.AppendLine("<feed/>");
            }
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text align='center'>※入出金情報※</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text align='left'></text>");
            reportXml.AppendLine($"<text>レジオープン時現金</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(1, 4)}件</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(moneyDayBegin, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            foreach (var mIo in moneyIOList)
            {
                reportXml.AppendLine($"<text>{mIo.mIOname}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(mIo.mIOcount, 4)}件</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(mIo.mIOamount, 9)}</text>");
                reportXml.AppendLine("<feed/>");
            }
            reportXml.AppendLine($"<text>現在残高</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(moneyDayEnd, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<cut type='feed'/>");

            reportXml.AppendLine(printXmlBodyHeaderEnd);

            return reportXml.ToString();
        }

        /**
        * 日計レポート:部門別
        */
        private async Task<string> getDailyReportType2(ReqMdDailyReport reqDto, DailyReportData dailyReportData)
        {
            if (!reqDto.reportTypes.Contains("2"))
            {
                return "";
            }

            var productTypeForCook = await _dbContext.MCategories
                .Where(ct =>
                    ct.StoreNumber == reqDto.storeNumber
                    && ct.CategoryClassNumber == CategoryConst.C_100_キッチン出力用商品分類区分.value)
                .Select(ctg => ctg.CategoryKbn)
                .ToListAsync();

            var productTypeForDish = await _dbContext.MCategories
                .Where(ct =>
                    ct.StoreNumber == reqDto.storeNumber
                    && ct.CategoryClassNumber == CategoryConst.C_101_デシャップ出力用商品分類区分.value)
                .Select(ctg => ctg.CategoryKbn)
                .ToListAsync();

            var footSumQuantity = dailyReportData.drOrders
                .Where(od => productTypeForCook.Contains(od.ProductTypeKbn))
                .Sum(od => od.OrderQuantity ?? 0);
            var footSumAmount = dailyReportData.drOrders
                .Where(od => productTypeForCook.Contains(od.ProductTypeKbn))
                .Sum(od => od.OrderAfterPrice ?? 0);

            var drinkSumQuantity = dailyReportData.drOrders
                .Where(od => productTypeForDish.Contains(od.ProductTypeKbn))
                .Sum(od => od.OrderQuantity ?? 0);
            var drinkSumAmount = dailyReportData.drOrders
                .Where(od => productTypeForDish.Contains(od.ProductTypeKbn))
                .Sum(od => od.OrderAfterPrice ?? 0);

            double footAmountPercent = footSumAmount / (footSumAmount + drinkSumAmount) * 100;
            string footAmountPercentStr = Math.Round(footAmountPercent, 1).ToString() + "%";
            string drinkAmountPercentStr = (100 - footAmountPercent).ToString() + "%";

            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine(printXmlHeaderBody);
            reportXml.AppendLine(getDailyReportCommonHead("部門別", dailyReportData));
            reportXml.AppendLine($"<text>フード</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.padWith(PadOption.PadLeft, 10, footAmountPercentStr)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(footSumQuantity, 4)}点</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(footSumAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>ドリンク</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.padWith(PadOption.PadLeft, 10, drinkAmountPercentStr)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(drinkSumQuantity, 4)}点</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(drinkSumAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<cut type='feed'/>");
            reportXml.AppendLine(printXmlBodyHeaderEnd);

            return reportXml.ToString();
        }
        /**
        * 日計レポート:分類別
        */
        private async Task<string> getDailyReportType3(ReqMdDailyReport reqDto, DailyReportData dailyReportData)
        {
            if (!reqDto.reportTypes.Contains("3"))
            {
                return "";
            }

            var productTypes = await _dbContext.MCategories
                .Where(ct =>
                    ct.StoreNumber == reqDto.storeNumber
                    && ct.CategoryClassNumber == CategoryConst.C_031_商品分類区分.value)
                .ToListAsync();

            var productSumPerType = dailyReportData.drOrders
                .GroupBy(od => od.ProductTypeKbn)
                .Select(odG => new
                {
                    typeKey = odG.Key,
                    typeName = productTypes.Where(ct => ct.CategoryKbn == odG.Key).FirstOrDefault().CategoryKbnName ?? "",
                    typeSumQuantity = odG.Sum(od => od.OrderQuantity ?? 0),
                    typeSumAmount = odG.Sum(od => od.OrderAfterPrice ?? 0)
                }
                ).ToList();

            var allAmount = productSumPerType.Sum(od => od.typeSumAmount);
            var sumRslt = productSumPerType
                .Select(od => new
                {
                    typeKey = od.typeKey,
                    typeName = od.typeName,
                    typeSumQuantity = od.typeSumQuantity,
                    typeSumAmount = od.typeSumAmount,
                    typeSumAmountPercent = Math.Round(((double)od.typeSumAmount / (double)allAmount * 100), 1).ToString() + "%"
                }
                ).ToList();

            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine(printXmlHeaderBody);
            reportXml.AppendLine(getDailyReportCommonHead("分類別", dailyReportData));
            foreach (var od in sumRslt)
            {
                reportXml.AppendLine($"<text>{od.typeName}</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.padWith(PadOption.PadLeft, 10, od.typeSumAmountPercent)}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(od.typeSumQuantity, 4)}点</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(od.typeSumAmount, 9)}</text>");
                reportXml.AppendLine("<feed/>");
            }
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<cut type='feed'/>");
            reportXml.AppendLine(printXmlBodyHeaderEnd);

            return reportXml.ToString();
        }
        /**
        * 日計レポート:会計取引種別
        */
        private string getDailyReportType4(ReqMdDailyReport reqDto, DailyReportData dailyReportData)
        {
            if (!reqDto.reportTypes.Contains("4"))
            {
                return "";
            }

            var sumList = dailyReportData.drPayments.GroupBy(py => py.PaymentClassKbn)
                .Select(pyCkG => new
                {
                    payClsKbnGpId = pyCkG.Key,
                    payClsKbnGpName = dailyReportData.drPaymentClassKbns.Where(pck => pck.CategoryClassNumber == pyCkG.Key).FirstOrDefault().CategoryClassName ?? "",
                    payClsKbnGpCount = pyCkG.Count(),
                    payClsKbnGpAmount = pyCkG.Sum(py => py.PaymentConfirmPrice ?? 0),
                    pykGpList = pyCkG.GroupBy(py => py.PaymentKbn)
                        .Select(pykGp => new
                        {
                            payKbnGpId = pykGp.Key,
                            payKbnGpName = dailyReportData.drPaymentKbns.Where(pk => pk.CategoryClassNumber == pyCkG.Key && pk.CategoryKbn == pykGp.Key).FirstOrDefault().CategoryKbnName ?? "",
                            payKbnGpCount = pykGp.Count(),
                            payKbnGpAmount = pykGp.Sum(py => py.PaymentConfirmPrice ?? 0),
                        })
                        .ToList()
                })
                .ToList();
            var allAmount = dailyReportData.drPayments.Sum(py => py.PaymentConfirmPrice ?? 0);
            var sumRslt = sumList.Select(sumGp => new
            {
                payClsKbnGpId = sumGp.payClsKbnGpId,
                payClsKbnGpName = sumGp.payClsKbnGpName,
                payClsKbnGpCount = sumGp.payClsKbnGpCount,
                payClsKbnGpAmount = sumGp.payClsKbnGpAmount,
                payClsKbnGpAmountPercent = Math.Round(((double)sumGp.payClsKbnGpAmount / (double)allAmount * 100), 1).ToString() + "%",
                payGpList = sumGp.pykGpList.Select(sum => new
                {
                    payKbnGpId = sum.payKbnGpId,
                    payKbnGpName = sum.payKbnGpName,
                    payKbnGpCount = sum.payKbnGpCount,
                    payKbnGpAmount = sum.payKbnGpAmount,
                    payKbnGpAmountPercent = Math.Round(((double)sum.payKbnGpAmount / (double)sumGp.payClsKbnGpAmount * 100), 1).ToString() + "%",
                }).ToList()
            }
            ).ToList();

            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine(printXmlHeaderBody);
            reportXml.AppendLine(getDailyReportCommonHead("会計取引種別", dailyReportData));
            foreach (var pyCkG in sumRslt)
            {
                reportXml.AppendLine($"<text>{pyCkG.payClsKbnGpName}</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.padWith(PadOption.PadLeft, 10, pyCkG.payClsKbnGpAmountPercent)}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(pyCkG.payClsKbnGpCount, 4)}組</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(pyCkG.payClsKbnGpAmount, 9)}</text>");
                reportXml.AppendLine("<feed/>");
                if (pyCkG.payGpList.Count > 1)
                {
                    foreach (var pyG in pyCkG.payGpList)
                    {
                        reportXml.AppendLine($"<text x='200'>{pyG.payKbnGpName}</text>");
                        reportXml.AppendLine($"<text x='440'>{CmnUtils.padWith(PadOption.PadLeft, 10, pyG.payKbnGpAmountPercent)}</text>");
                        reportXml.AppendLine("<feed/>");
                        reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(pyG.payKbnGpCount, 4)}点</text>");
                        reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(pyG.payKbnGpAmount, 9)}</text>");
                        reportXml.AppendLine("<feed/>");

                    }
                }
            }
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<cut type='feed'/>");
            reportXml.AppendLine(printXmlBodyHeaderEnd);

            return reportXml.ToString();
        }
        private string getDailyReportCommonHead(string typeName, DailyReportData dailyReportData)
        {
            string dailyTypeName = dailyReportData.drDateStrJpS == dailyReportData.drDateStrJpE ? "日計" : "月計";
            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine($"<text align='center' width='1' height='2'>{dailyTypeName}レポート {typeName}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text align='left' width='1' height='1'></text>");
            reportXml.AppendLine($"<text >店舗名  : {dailyReportData.drStoreInfo.StoreName}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text >営業日付: {dailyReportData.drDateStrJpS} ～ {dailyReportData.drDateStrJpE}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<text >担当者  : </text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<text >POS     : 全て</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{chitLine}</text>");
            reportXml.AppendLine("<feed/>");
            return reportXml.ToString();
        }

        /**
        * 
        */
        private string getDailyReportDetailList(DailyReportData dailyReportData)
        {
            StringBuilder reportXml = new StringBuilder();
            int dateX = 20;
            int seatX = 140;
            int amountX = 240;
            int peopleCountX = 360;
            int paymentX = 440;
            reportXml.AppendLine($"<text x='{dateX}'>日時</text>");
            reportXml.AppendLine($"<text x='{seatX}'>座席</text>");
            reportXml.AppendLine($"<text x='{amountX}'>金額</text>");
            reportXml.AppendLine($"<text x='{peopleCountX}'>人数</text>");
            reportXml.AppendLine($"<text x='{paymentX}'>支払</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text x='0'>{rowLine}</text>");
            reportXml.AppendLine("<feed/>");
            for (int i = 0; i < dailyReportData.drReceiptions.Count; i++)
            {
                // reportXml.AppendLine($"<text x='0'>{rowLine}</text>");
                // reportXml.AppendLine("<feed/>");

                var recp = dailyReportData.drReceiptions[i];
                string dayTime = recp.SeatStartDate == null ? "" : ((DateTime)(recp.SeatStartDate)).ToString("hh:mm");
                reportXml.AppendLine($"<text x='{dateX}'>{dayTime}</text>");

                string seatOrTakeName = recp.ReceptionKbn == CategoryConst.C_009_受付区分.ctg_001_店内 ? recp.SeatName : "持帰";
                reportXml.AppendLine($"<text x='{seatX}'>{seatOrTakeName}</text>");

                var paymentList = dailyReportData.drPayments.Where(
                    pay =>
                        dailyReportData.drOrders
                            .Where(ord => ord.ReceptionNumber == recp.ReceptionNumber)
                            .ToList()
                            .Any(ord => ord.PaymentNumber == pay.PaymentNumber)
                ).ToList();
                var amountSum = paymentList.Sum(pay => pay.PaymentConfirmPrice);
                long amountSumL = amountSum == null ? 0 : (long)amountSum;
                reportXml.AppendLine($"<text x='{amountX}'>{CmnUtils.formatCurrency(amountSumL, 7)}</text>");

                var peopleCount =
                    (recp.SeatPeopleChildren ?? 0) +
                    (recp.SeatPeopleMan ?? 0) +
                    (recp.SeatPeopleWoman ?? 0);
                reportXml.AppendLine($"<text x='{peopleCountX}'>{CmnUtils.formatCommaNum(peopleCount, 4)}</text>");

                var payMent = paymentList.FirstOrDefault();
                var paymentKbn = dailyReportData.drPaymentKbns.Where(
                    payKbn =>
                        payKbn.CategoryClassNumber == payMent.PaymentClassKbn &&
                        payKbn.CategoryKbn == payMent.PaymentKbn
                ).FirstOrDefault();
                var paymentKbnName = paymentKbn.CategoryKbnAbbreviation;
                if (paymentList.Count > 1)
                {
                    paymentKbnName = paymentKbnName + "と他";
                }

                reportXml.AppendLine($"<text x='{paymentX}'>{paymentKbnName}</text>");
                reportXml.AppendLine("<feed/>");
            }
            return reportXml.ToString();
        }

        /**
        * 日計レポート:商品別
        */
        private string getDailyReportType5(ReqMdDailyReport reqDto, DailyReportData dailyReportData)
        {
            if (!reqDto.reportTypes.Contains("5"))
            {
                return "";
            }

            var sumList = dailyReportData.drOrders
                .GroupBy(od => od.ProductName)
                .Select(odG => new
                {
                    prdName = odG.Key,
                    prdCount = odG.Sum(od => od.OrderQuantity ?? 0),
                    prdAmount = odG.Sum(odG => odG.OrderAfterPrice ?? 0)
                }).ToList();
            var sumCount = sumList.Sum(od => od.prdCount);
            var sumAmount = sumList.Sum(od => od.prdAmount);
            var sumRslt = sumList.Select(od => new
            {
                prdName = od.prdName,
                prdCount = od.prdCount,
                prdAmount = od.prdAmount,
                prdAmountPercent = Math.Round(((double)od.prdAmount / (double)sumAmount * 100), 1).ToString() + "%"
            }).ToList();

            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine(printXmlHeaderBody);
            reportXml.AppendLine(getDailyReportCommonHead("商品別", dailyReportData));
            foreach (var sum in sumRslt)
            {
                reportXml.AppendLine($"<text>{sum.prdName}</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.padWith(PadOption.PadLeft, 10, sum.prdAmountPercent)}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(sum.prdCount, 4)}点</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(sum.prdAmount, 9)}</text>");
                reportXml.AppendLine("<feed/>");
            }
            reportXml.AppendLine($"<text>合計</text>");
            reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(sumCount, 4)}点</text>");
            reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(sumAmount, 9)}</text>");
            reportXml.AppendLine("<feed/>");

            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<cut type='feed'/>");
            reportXml.AppendLine(printXmlBodyHeaderEnd);

            return reportXml.ToString();
        }
    
    #region 取消履歴印刷
        /// <summary>
        /// 取消履歴印刷
        /// </summary>
        /// <param name="dailyReportData"></param>
        /// <returns></returns>
        private string getCancelReportType(CancelReport dailyReportData){

            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine(printXmlHeaderBody);
            reportXml.AppendLine(getCancelReportCommonHead(dailyReportData));

            var sumList = dailyReportData.CancelOrder
                .GroupBy(od => new { od.ReceptioNumber, od.CancelDate })
                .Select(odG => new
                {
                    prdOrder = odG.Key.ReceptioNumber,
                    prdDate = odG.Key.CancelDate,
                    prdUser = odG.FirstOrDefault().CancelUser,
                    prdCancel = odG.FirstOrDefault().CancelUser != ""  || odG.FirstOrDefault().CancelUser != null ? "注文取消":"",
                    items = dailyReportData.LstCancelOrder
                        .Where( c=>c.ReceptioNumber == odG.Key.ReceptioNumber)
                        .Select(od => new
                    {
                        prdName = od.ProductName,
                        prdAcount = od.OrderQuantity ?? 0,
                        prdAmount = od.PaymentPrice ?? 0,
                        prdFlg = od.CancelFlg == "1"? "消":"",
                    }).ToList()
                }).ToList();

            foreach (var group in sumList)
            {
                reportXml.AppendLine($"<text>{chitLine}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text>No.：{group.prdOrder}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text>日付：{group.prdDate}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text x='150'>{group.prdCancel}</text>");
                reportXml.AppendLine($"<text x='440'>サイン：{group.prdUser}</text>");

                long ALLMoney = 0;   

                foreach (var item in group.items)
                {
                    

                    reportXml.AppendLine($"<text>{item.prdName}</text>");
                    reportXml.AppendLine("<feed/>");
                    reportXml.AppendLine($"<text x='150'>{CmnUtils.formatCommaNum(item.prdAcount, 4)}点</text>");
                    reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(item.prdAmount, 9)}</text>");
                    reportXml.AppendLine($"<text x='440'>{item.prdFlg}</text>");
                    reportXml.AppendLine("<feed/>");

                    ALLMoney +=  + item.prdAmount;
                }

                reportXml.AppendLine($"<text>{chitLine}</text>");
                reportXml.AppendLine("<feed/>");
                reportXml.AppendLine($"<text>金額：</text>");
                reportXml.AppendLine($"<text x='440'>{CmnUtils.formatCurrency(ALLMoney, 9)}</text>");
            }

            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{DateTime.Now.ToString("yyyy/MM/dd HH:mm")}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<cut type='feed'/>");
            reportXml.AppendLine(printXmlBodyHeaderEnd);

            return reportXml.ToString();
        }

        /// <summary>
        /// 取消履歴ヘッダー部
        /// </summary>
        /// <param name="dailyReportData"></param>
        /// <returns></returns>
        private string getCancelReportCommonHead(CancelReport dailyReportData)
        {
            string dailyTypeName ="取消履歴";
            StringBuilder reportXml = new StringBuilder();
            reportXml.AppendLine($"<text align='center' width='1' height='2'>{dailyTypeName}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text align='left' width='1' height='1'></text>");
            reportXml.AppendLine($"<text >店舗名  : {dailyReportData.drStoreInfo.StoreName}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text >営業日付: {dailyReportData.drDateStrJpS} ～ {dailyReportData.drDateStrJpE}</text>");
            reportXml.AppendLine("<feed/>");
            reportXml.AppendLine($"<text>{chitLine}</text>");
            reportXml.AppendLine("<feed/>");
            return reportXml.ToString();
        }

        // <summary>
        /// 取消履歴データを取得
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>
         public async Task<RpnMdPrint> getCancelReportsData(ReqMdDailyReport reqDto)
        {
            RpnMdPrint rpnDto = new RpnMdPrint();

            List<string> allXmls = new List<string>();

            CancelReport dailyReportData = await getCancelData(reqDto);
            if (dailyReportData == null)
            {
                rpnDto.status = RepMdStatus.S_204_NoContent;
                return rpnDto;
            }

            string xmlType = this.getCancelReportType(dailyReportData);
            if (!string.IsNullOrEmpty(xmlType))
            {
                allXmls.Add(xmlType);
            }
            
            if (allXmls.Count > 0)
            {
                // プリンター：レジ用
                var cashierChitPrinters = await _dbContext.MCategories
                    .Where(cg => cg.CategoryClassNumber == CategoryConst.C_020_プリンタ情報.value
                        && cg.CategoryKbn == CategoryConst.C_020_プリンタ情報.ctg_020_プリンタ２レシートプリンタ
                        )
                    .ToListAsync();
                // レジ用プリンタのデータを作る
                foreach (var ctgPrinter in cashierChitPrinters)
                {
                    RpnMdPrintData cashierChitPtDate = new RpnMdPrintData();
                    cashierChitPtDate.printerUri = $"{ctgPrinter.CategoryKbnAbbreviation}/cgi-bin/epos/service.cgi?devid=local_printer&timeout={SystemSetting.printerTimeout}";
                    cashierChitPtDate.printerName = ctgPrinter.CategoryKbnName;
                    foreach (var xml in allXmls)
                    {
                        cashierChitPtDate.printXmlDatas.Add(xml);
                    }
                    rpnDto.printerDataList.Add(cashierChitPtDate);
                }

            }

            return rpnDto;
        }
   
         /// <summary>
        /// 取消レポートデータ作成
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>        
        private async Task<CancelReport> getCancelData(ReqMdDailyReport reqDto){
            CancelReport cancelReport = new CancelReport();

            string dateFrom = reqDto.reportDate;
            string dateTo = reqDto.reportDate;
            if (reqDto.reportIsMonth)
            {
                dateFrom = reqDto.reportDate + "-01";
                dateTo = reqDto.reportDate + "-01";
            }

            DateTime reportDateStart = new DateTime();
            DateTime reportDateEnd = new DateTime();

            DateTime.TryParse(dateFrom + " 00:00:00.000", out reportDateStart);
            DateTime.TryParse(dateTo + " 23:59:59.999", out reportDateEnd);

            if (reqDto.reportIsMonth)
            {
                reportDateEnd = reportDateEnd.AddMonths(1).AddDays(-1);
            }

            var selectSql = @"
                select
                    tco.reception_number,tco.order_date,mu.user_name from t_cancel_order tco
                    left join m_user mu 
                    on mu.user_id  = tco.cancel_user
                    where tco.order_date between {reportDateStart} and {reportDateEnd}
            ";

            var quaryRslt = _dbContext.ExtTCancelOrder.FromSqlRaw(selectSql);
            var dailyRecpts = await quaryRslt.ToListAsync();

            // ※データなしの場合
            if (dailyRecpts.Count <= 0)
            {
                return null;
            }
            cancelReport.CancelOrder = dailyRecpts;

            //注文***************************************************************
            var recpNumberList = dailyRecpts.Select(rcp => rcp.ReceptioNumber).ToList();

            var dailyOrders = await _dbContext.TOrders.
                Where(od =>
                    od.StoreNumber == reqDto.storeNumber
                    && recpNumberList.Contains(od.ReceptionNumber)
                ).Select(od =>
                    new CancelPrintReport
                    {
                        StoreNumber = od.StoreNumber,
                        ReceptioNumber = od.ReceptionNumber,
                        ProductName = od.ProductNameCh,
                        CancelFlg = od.OrderCancelFlg,
                        OrderQuantity = od.OrderQuantity,
                        ProductPrice = od.ProductPrice,
                        PaymentPrice = od.OrderAfterPrice,
                    }
                )
                .ToListAsync();
            // ※データなしの場合
            if (dailyOrders.Count <= 0)
            {
                return null;
            }
            cancelReport.LstCancelOrder = dailyOrders;
            
            //店舗情報***************************************************************
            var storeInfo = await _dbContext.MStoreInfos
                .Where(
                    st => st.StoreNumber == reqDto.storeNumber
                ).FirstAsync();
            cancelReport.drStoreInfo = storeInfo;

            cancelReport.drDateTimeS = reportDateStart;
            cancelReport.drDateTimeE = reportDateEnd;
            cancelReport.drDateStrJpS = reportDateStart.ToString("yyyy年MM月dd日");
            cancelReport.drDateStrJpE = reportDateEnd.ToString("yyyy年MM月dd日");
            return cancelReport;
        }
    #endregion
    }
}