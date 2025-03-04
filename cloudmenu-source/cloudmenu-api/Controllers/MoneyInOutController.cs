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
    [Route("api/moneyinout")]
    public class MoneyInOutController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<MoneyInOutController> _logger;

        public MoneyInOutController(ILogger<MoneyInOutController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation(1, "NLog injected into MoneyInOutController");
        }

        /*****
        * 指定日付の入出金データ検索
        *****/
        [HttpPost("getDailyMoneyInOut")]
        public async Task<RpnMdMoneyInOut> getDailyMoneyInOut(ReqMdMoneyInOut reqDto)
        {
            RpnMdMoneyInOut rsltDto = new RpnMdMoneyInOut();
            DateTime dayBegin = new DateTime();
            DateTime dayEnd = new DateTime();
            DateTime.TryParse(reqDto.MoneyInoutDatetime + " 00:00:00.000", out dayBegin);
            DateTime.TryParse(reqDto.MoneyInoutDatetime + " 23:59:59.999", out dayEnd);
            var moneyIOLstQuery =
                from mny in _dbContext.TMoneyInouts
                join ctgC in _dbContext.MCategoryClasses
                on new { StoreNumber = mny.StoreNumber, MoneyInoutKbn = mny.MoneyInoutKbn } equals new { StoreNumber = ctgC.StoreNumber, MoneyInoutKbn = ctgC.CategoryClassNumber }
                into ctgCGp
                from ctgCLj in ctgCGp.DefaultIfEmpty()
                join ctg in _dbContext.MCategories
                on new { StoreNumber = mny.StoreNumber, MoneyInoutKbn = mny.MoneyInoutKbn, MoneyInoutReasonKbn = mny.MoneyInoutReasonKbn }
                    equals new { StoreNumber = ctg.StoreNumber, MoneyInoutKbn = ctg.CategoryClassNumber, MoneyInoutReasonKbn = ctg.CategoryKbn }
                into ctgGp
                from ctgLj in ctgGp.DefaultIfEmpty()
                where mny.StoreNumber == reqDto.storeNumber &&
                    mny.ClosingFlg == FlgConst.off &&
                    DateTime.Compare(mny.MoneyInoutDatetime, dayBegin) >= 0 &&
                    DateTime.Compare(mny.MoneyInoutDatetime, dayEnd) <= 0
                select new DbExtMoneyInOutInfo
                {
                    StoreNumber = mny.StoreNumber,
                    MoneyInoutNumber = mny.MoneyInoutNumber,
                    MoneyInoutDatetime = mny.MoneyInoutDatetime,
                    MoneyInoutPrice = mny.MoneyInoutPrice,
                    MoneyInoutKbn = mny.MoneyInoutKbn,
                    MoneyInoutReasonKbn = mny.MoneyInoutReasonKbn,
                    MoneyInoutRbFlg = mny.MoneyInoutRbFlg,
                    MoneyInoutOriginNumber = mny.MoneyInoutOriginNumber,
                    MoneyInoutRemarks = mny.MoneyInoutRemarks,
                    moneyInoutKbnName = ctgCLj.CategoryClassOptionValues,
                    MoneyInoutReasonKbnName = ctgLj.CategoryKbnName
                };

            var moneyIOLst = await moneyIOLstQuery.ToListAsync();

            rsltDto.moneyInOutList = moneyIOLst;
            return rsltDto;
        }

        /*****
        * 入出金データ登録
        *****/
        [HttpPost("addMoneyInOut")]
        public async Task<RpnMdMoneyInOut> addMoneyInOut(ReqMdMoneyInOut reqDto)
        {
            _dbContext.Database.BeginTransaction();
            string maxMoneyInoutNumberStr = _dbContext.TMoneyInouts.Max(pm => pm.MoneyInoutNumber);
            int maxMoneyInoutNumber = 1;
            if (int.TryParse(maxMoneyInoutNumberStr, out maxMoneyInoutNumber))
            {
                maxMoneyInoutNumber = maxMoneyInoutNumber + 1;
            }
            else
            {
                maxMoneyInoutNumber = 1;
            }

            string newMoneyInoutNumber = maxMoneyInoutNumber.ToString().PadLeft(11, '0');
            DateTime dtNow = DateTime.Now;
            DateTime MoneyInoutDatetime = new DateTime();
            DateTime.TryParse(reqDto.MoneyInoutDatetime, out MoneyInoutDatetime);

            TMoneyInout newMnyInout = new TMoneyInout();
            newMnyInout.StoreNumber = reqDto.storeNumber;
            newMnyInout.MoneyInoutNumber = newMoneyInoutNumber;
            newMnyInout.MoneyInoutDatetime = MoneyInoutDatetime;
            newMnyInout.MoneyInoutPrice = reqDto.MoneyInoutPrice;
            newMnyInout.MoneyInoutKbn = reqDto.MoneyInoutKbn;
            newMnyInout.MoneyInoutReasonKbn = reqDto.MoneyInoutReasonKbn;
            newMnyInout.MoneyInoutRbFlg = reqDto.MoneyInoutKbn == CategoryConst.C_300_入金区分.value ? "0" : "1";
            newMnyInout.MoneyInoutOriginNumber = "";
            newMnyInout.MoneyInoutRemarks = reqDto.MoneyInoutRemarks;
            newMnyInout.ClosingFlg = FlgConst.off;
            newMnyInout.RegistrationUser = reqDto.storeNumber;
            newMnyInout.RegistrationDatetime = dtNow;
            newMnyInout.UpdateUser = reqDto.storeNumber;
            newMnyInout.UpdateDatetime = dtNow;

            _dbContext.TMoneyInouts.Add(newMnyInout);
            _dbContext.SaveChanges();

            _dbContext.Database.CommitTransaction();

            return await this.getDailyMoneyInOut(reqDto);
        }

        /*****
        * 入出金データ削除
        *****/
        [HttpPost("delMoneyInOut")]
        public async Task<RpnMdMoneyInOut> delMoneyInOut(ReqMdMoneyInOut reqDto)
        {
            var moneyInoutToDel = await _dbContext.TMoneyInouts
                .Where(mny => mny.StoreNumber == reqDto.storeNumber
                    && mny.MoneyInoutNumber == reqDto.MoneyInoutNumber
                )
                .Select(mny => mny)
                .FirstOrDefaultAsync();

            if (moneyInoutToDel != null)
            {
                moneyInoutToDel.ClosingFlg = FlgConst.on;
                _dbContext.SaveChanges();
            }

            return await this.getDailyMoneyInOut(reqDto);
        }
    }
}