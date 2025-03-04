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
using cloudmenu_api.Services;

namespace cloudmenu_api.Controllers
{
    [ApiController]
    [Route("api/seat")]
    public class SeatController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<SeatController> _logger;
        private readonly SeatService _seatService;
        private readonly ChitPrintService _chitPrintService;


        public SeatController(ILogger<SeatController> logger, AppDbContext dbContext, SeatService seatService, ChitPrintService chitPrintService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _seatService = seatService;
            _chitPrintService = chitPrintService;
            _logger.LogInformation(1, "NLog injected into SeatController");
        }

        /*****
        * 座席最新データ検索
        *****/
        [HttpPost("selectZasekiAllData")]
        public async Task<RpnMdSeatCurrentInfos> selectZasekiCurrentInfos(ReqMdSeatCurrentInfos reqDto)
        {
            return await _seatService.selectZasekiCurrentInfos(reqDto);
        }

        /*****
        * 注文データの数量、金額を修正する
        *****/
        [HttpPost("updateOrder")]
        public async Task<RpnMdSeatCurrentInfos> updateOrder(ReqMdSeatUpdateOrder reqDto)
        {
            _logger.LogDebug("SeatController-updateOrder request:{0}", reqDto);

            var orderInfo = _dbContext.TOrders
                .Where(od =>
                    od.StoreNumber == reqDto.storeNumber
                    && od.OrderCancelFlg == FlgConst.off
                    && od.ReceptionNumber == reqDto.ReceptionNumber
                    && od.ReceptionBranchNumber == reqDto.ReceptionBranchNumber
                    && od.OrderVoucherNumber == reqDto.OrderVoucherNumber
                    && od.OrderVoucherDetails == reqDto.OrderVoucherDetails
                ).FirstOrDefault();

            if (orderInfo != null)
            {
                orderInfo.OrderQuantity = reqDto.OrderQuantity;
                orderInfo.OrderDiscountYen = reqDto.OrderDiscountYen;

                orderInfo.OrderPrice = orderInfo.OrderQuantity * orderInfo.ProductPrice;
                orderInfo.OrderAfterPrice = orderInfo.OrderQuantity * orderInfo.OrderDiscountYen;

                orderInfo.UpdateUser = reqDto.storeNumber;
                orderInfo.UpdateDatetime = DateTime.Now;

                if (orderInfo.OrderQuantity == 0)
                {
                    orderInfo.OrderCancelFlg = FlgConst.on;
                }

                await _dbContext.SaveChangesAsync();
            }

            //座席最新データを取得
            ReqMdSeatCurrentInfos selectReqDto = new ReqMdSeatCurrentInfos() { storeNumber = reqDto.storeNumber, SelectedRecpNumber = reqDto.ReceptionNumber };
            RpnMdSeatCurrentInfos rpnDto = await _seatService.selectZasekiCurrentInfos(selectReqDto);

            return rpnDto;
        }
        /*****
        * 受付又は注文の備考を更新
        *****/
        [HttpPost("updateRemark")]
        public async Task<RpnMdSeatCurrentInfos> updateRemark(ReqMdSeatUpdateOrder reqDto)
        {
            //受付の場合
            if (string.IsNullOrEmpty(reqDto.OrderVoucherDetails))
            {
                var rcptsToUp = await _dbContext.TReceptions
                    .Where(rp =>
                        rp.StoreNumber == reqDto.storeNumber
                        && rp.ReceptionNumber == reqDto.ReceptionNumber
                        && rp.ReceptionDelFlg == FlgConst.off
                    ).Select(rp => rp)
                    .ToListAsync();

                foreach (var rcpt in rcptsToUp)
                {
                    rcpt.ReceptionRemarks = reqDto.Remark;
                    rcpt.UpdateUser = reqDto.storeNumber;
                    rcpt.UpdateDatetime = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
            }
            else
            {
                var ordersToUp = await _dbContext.TOrders
                    .Where(od =>
                        od.StoreNumber == reqDto.storeNumber
                        && od.ReceptionNumber == reqDto.ReceptionNumber
                        && od.OrderVoucherDetails == reqDto.OrderVoucherDetails
                    ).Select(od => od)
                    .ToListAsync();

                foreach (var od in ordersToUp)
                {
                    od.OrderRemarks = reqDto.Remark;
                    od.UpdateUser = reqDto.storeNumber;
                    od.UpdateDatetime = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
            }

            //座席最新データを取得
            ReqMdSeatCurrentInfos selectReqDto = new ReqMdSeatCurrentInfos() { storeNumber = reqDto.storeNumber, SelectedRecpNumber = reqDto.ReceptionNumber };
            RpnMdSeatCurrentInfos rpnDto = await _seatService.selectZasekiCurrentInfos(selectReqDto);

            return rpnDto;
        }

        /*****
        * 注文データの調理指示伝票
        *****/
        [HttpPost("updateOrdersCookFlg")]
        public async Task<RpnMdSeatCurrentInfos> updateOrdersCookFlg(ReqMdSeatPrintCookChit reqDto)
        {
            //
            List<TOrder> orderToUpd = await _chitPrintService.getPrintOrders(reqDto.storeNumber, reqDto.ordersToPrint);

            foreach (TOrder od in orderToUpd)
            {
                od.OrderCookingFlg = FlgConst.on;
                od.UpdateDatetime = DateTime.Now;
                od.UpdateUser = reqDto.storeNumber;
            }
            await _dbContext.SaveChangesAsync();

            //座席最新データを取得
            ReqMdSeatCurrentInfos selectReqDto = new ReqMdSeatCurrentInfos() { storeNumber = reqDto.storeNumber, SelectedRecpNumber = reqDto.selectedRecpNumber };
            RpnMdSeatCurrentInfos rpnDto = await _seatService.selectZasekiCurrentInfos(selectReqDto);

            return rpnDto;
        }

        /*****
        * 配膳情報を更新
        *****/
        [HttpPost("toggleServeFlg")]
        public async Task<RpnMdSeatCurrentInfos> toggleServeFlg(ReqMdSeatUpdateOrder reqDto)
        {
            TOrder od = await _dbContext.TOrders
                .Where(od => od.StoreNumber == reqDto.storeNumber
                && od.ReceptionNumber == reqDto.ReceptionNumber
                && od.ReceptionBranchNumber == reqDto.ReceptionBranchNumber
                && od.OrderVoucherNumber == reqDto.OrderVoucherNumber
                && od.OrderVoucherDetails == reqDto.OrderVoucherDetails
                ).FirstOrDefaultAsync();

            if (od != null)
            {
                od.OrderServeFlg = od.OrderServeFlg == FlgConst.off ? FlgConst.on : FlgConst.off;
                await _dbContext.SaveChangesAsync();
            }

            //座席最新データを取得
            ReqMdSeatCurrentInfos selectReqDto = new ReqMdSeatCurrentInfos() { storeNumber = reqDto.storeNumber, SelectedRecpNumber = reqDto.ReceptionNumber };
            RpnMdSeatCurrentInfos rpnDto = await _seatService.selectZasekiCurrentInfos(selectReqDto);

            return rpnDto;
        }

        /*****
        * 受付データ削除
        *****/
        [HttpPost("deleteReception")]
        public async Task<RpnMdSeatCurrentInfos> deleteReception(ReqMdSeatUpdateOrder reqDto)
        {
            _logger.LogDebug("SeatController-deleteTakeoutReception request:{0}", reqDto);

            _dbContext.Database.BeginTransaction();
            _dbContext.Database.ExecuteSqlRaw(
                $@"update t_reception set reception_del_flg = '{ConstCode.NUM_1}' where reception_number = '{reqDto.ReceptionNumberToDel}' and store_number = '{reqDto.storeNumber}'"
            );
            _dbContext.Database.ExecuteSqlRaw(
                $@"update t_order set order_cancel_flg = '{ConstCode.NUM_1}' where reception_number = '{reqDto.ReceptionNumberToDel}' and store_number = '{reqDto.storeNumber}'"
            );
            _dbContext.Database.CommitTransaction();


            //座席最新データを取得
            ReqMdSeatCurrentInfos selectReqDto = new ReqMdSeatCurrentInfos() { storeNumber = reqDto.storeNumber, SelectedRecpNumber = reqDto.ReceptionNumber };
            RpnMdSeatCurrentInfos rpnDto = await _seatService.selectZasekiCurrentInfos(selectReqDto);

            return rpnDto;
        }
    }
}
