using System.ComponentModel;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.RequestModels;
using cloudmenu_api.ResponseModels;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.LwUtils;


namespace cloudmenu_api.Controllers
{
    [ApiController]
    [Route("api/order")]
    public partial class OrderController : ControllerBase
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation(1, "NLog injected into OrderController");
        }

        /*　対象テーブル：区分マスタ
            SQL ID：0001
        　　説明：区分分類を指定し、全ての区分値を取得する。
        */
        [HttpPost("searchKubun.do")]
        public RpnKbnOrder getKbnListInitDatas(ReqKbnOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getKbnListInitDatas request:{0}", reqDto);

                var quaryRslt = from m1 in _dbContext.MCategories
                                where m1.StoreNumber == reqDto.storeNumber
                                   && m1.CategoryClassNumber == reqDto.categoryClassNumber
                                   && m1.DelFlg == FlgConst.off
                                select new DbExtKbnReception
                                {
                                    CategoryKbn = m1.CategoryKbn,
                                    CategoryKbnName = m1.CategoryKbnName,
                                    CategoryKbnAbbr = m1.CategoryKbnAbbreviation
                                };


                RpnKbnOrder rko = new RpnKbnOrder { categoryList = quaryRslt.ToList() };
                return rko;
            }
            catch (Exception)
            {
                RpnKbnOrder rko = new RpnKbnOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_区分取得)
                };
                return rko;
            }
        }

        /*　対象テーブル：座席マスタ
            SQL ID：0005
        　　説明：座席マスタより座席を全件検索する。現在の使用状況付で取得
        */
        [HttpPost("selectZasekiAllData.do")]
        public async Task<RpnMdOrder> getListInitDatas(ReqMdBase reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getListInitDatas request:{0}", reqDto);
                String selectSql = $@"
                    SELECT
                        m1.seat_level as SeatLevel
                        ,m1.seat_number as SeatNumber
                        ,m1.seat_kbn as SeatKbn
                        ,m1.seat_name as SeatName
                        ,t1.reception_number as receptionNumber
                        ,t1.reception_branch_number as receptionBranchNumber
                        ,t1.seat_start_date as seatStartDate
                        ,ifnull(t1.seat_status_kbn, '{CategoryConst.C_007_座席状況区分.ctg_001_空き}') as SeatStatusKbn
                        ,t1.seat_people_man as SeatPeopleMan
                        ,t1.seat_people_woman as SeatPeopleWoman
                        ,t1.seat_people_children as SeatPeopleChildren
                        ,t1.reception_remarks as receptionRemarks
                        ,t1.reception_kbn as receptionKbn
                        ,t1.takeout_name as takeoutName
                        ,t1.takeout_tel as takeoutTel
                        ,t1.takeout_receipt_time as takeoutReceiptTime
                    FROM
                        m_seat m1
                        LEFT JOIN
                            (
                                SELECT
                                    store_number
                                    ,seat_level
                                    ,seat_number
                                    ,seat_start_date
                                    ,seat_end_date
                                    ,seat_status_kbn
                                    ,seat_people_man
                                    ,seat_people_woman
                                    ,seat_people_children
                                    ,reception_number
                                    ,reception_branch_number
                                    ,reception_remarks
                                    ,reception_kbn
                                    ,takeout_name
                                    ,takeout_tel
                                    ,takeout_receipt_time
                                FROM
                                    t_reception
                                WHERE
                                    (
                                        NOW() BETWEEN seat_start_date AND ifnull(seat_end_date, NOW())
                                    )
                                AND reception_del_flg = '{FlgConst.off}'
                            ) t1
                        ON  t1.store_number = m1.store_number
                        AND t1.seat_level = m1.seat_level
                        AND t1.seat_number = m1.seat_number
                    WHERE
                        m1.store_number = '{reqDto.storeNumber}'
                    AND m1.del_flg = '{FlgConst.off}'
                    ORDER BY
                        m1.seat_level
                        ,t1.seat_start_date DESC
                        ,m1.seat_number
                ";
                String selectSql2 = $@"
                SELECT
                     m1.seat_level as SeatLevel
                    ,m1.seat_number as SeatNumber
                    ,m1.seat_kbn as SeatKbn
                    ,m1.seat_name as SeatName
                    ,t1.reception_number as receptionNumber
                    ,t1.reception_branch_number as receptionBranchNumber
                    ,t1.seat_start_date as seatStartDate
                    ,ifnull(t1.seat_status_kbn, '{CategoryConst.C_007_座席状況区分.ctg_001_空き}') as SeatStatusKbn
                    ,t1.seat_people_man as SeatPeopleMan
                    ,t1.seat_people_woman as SeatPeopleWoman
                    ,t1.seat_people_children as SeatPeopleChildren
                    ,t1.reception_remarks as receptionRemarks
                    ,t1.reception_kbn as receptionKbn
                    ,t1.takeout_name as takeoutName
                    ,t1.takeout_tel as takeoutTel
                    ,t1.takeout_receipt_time as takeoutReceiptTime
                FROM
                    t_reception t1
                    LEFT JOIN
                        (
                            SELECT *
                            FROM
                                m_seat
                            WHERE
                                store_number = '{reqDto.storeNumber}'
                                AND del_flg = '{FlgConst.off}'
                        ) m1
                    ON  t1.store_number = m1.store_number
                    AND t1.seat_level = m1.seat_level
                    AND t1.seat_number = m1.seat_number
                WHERE
                (
                   NOW() BETWEEN seat_start_date AND ifnull(seat_end_date, NOW())
                )
                AND reception_del_flg = '{FlgConst.off}'
                AND reception_kbn = '002'
                ORDER BY
                    m1.seat_level
                    ,t1.seat_start_date DESC
                    ,m1.seat_number
                ";
                Console.WriteLine('1');
                var quaryRslt1 = _dbContext.ExtSeatReceptions.FromSqlRaw(selectSql);
                var rsltLst1 = await quaryRslt1.ToListAsync();
                Console.WriteLine('2');
                var quaryRslt2 = _dbContext.ExtSeatReceptions.FromSqlRaw(selectSql2);
                var rsltLst2 = await quaryRslt2.ToListAsync();
                Console.WriteLine('3');
                for (int i = 0; i < rsltLst2.ToArray().Length; i++)
                {
                    Console.WriteLine(rsltLst2[i]);
                    rsltLst1.Add(rsltLst2[i]);
                }
                Console.WriteLine('4');
                var rsltLst = quaryRslt1.Concat(quaryRslt2).ToList();

                RpnMdOrder rslt = new RpnMdOrder { seatList = rsltLst1 };

                return rslt;
            }
            catch (Exception)
            {
                RpnMdOrder rslt = new RpnMdOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_座席データ全件)
                };
                return rslt;
            }
        }

        /*　対象テーブル：座席マスタ
            SQL ID：0005
        　　説明：選択座席データ検索
        */
        [HttpPost("selectZasekiChoiceData.do")]
        public async Task<RpnMdOrder> getZasekiChoiceInitDatas(ReqMdOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getZasekiChoiceInitDatas request:{0}", reqDto);
                String selectSql = $@"
                SELECT
                     m1.seat_level as SeatLevel
                    ,m1.seat_number as SeatNumber
                    ,m1.seat_kbn as SeatKbn
                    ,m1.seat_name as SeatName
                    ,t1.reception_number as receptionNumber
                    ,t1.reception_branch_number as receptionBranchNumber
                    ,ifnull(t1.seat_status_kbn, '{CategoryConst.C_007_座席状況区分.ctg_001_空き}') as SeatStatusKbn
                    ,t1.seat_people_man as SeatPeopleMan
                    ,t1.seat_people_woman as SeatPeopleWoman
                    ,t1.seat_people_children as SeatPeopleChildren
                    ,t1.reception_remarks as receptionRemarks
                    ,t1.seat_start_date as seatStartDate
                    ,t1.reception_kbn as receptionKbn
                    ,t1.takeout_name as takeoutName
                    ,t1.takeout_tel as takeoutTel
                    ,t1.takeout_receipt_time as takeoutReceiptTime
                FROM
                    m_seat m1
                    LEFT JOIN
                        (
                            SELECT
                                store_number
                                ,seat_level
                                ,seat_number
                                ,seat_start_date
                                ,seat_end_date
                                ,seat_status_kbn
                                ,seat_people_man
                                ,seat_people_woman
                                ,seat_people_children
                                ,reception_number
                                ,reception_branch_number
                                ,reception_remarks
                                ,reception_kbn
                                ,takeout_name
                                ,takeout_tel
                                ,takeout_receipt_time
                            FROM
                                t_reception
                            WHERE
                                (
                                    NOW() BETWEEN seat_start_date AND ifnull(seat_end_date, NOW())
                                )
                            AND reception_del_flg = '{FlgConst.off}'
                        ) t1
                    ON  t1.store_number = m1.store_number
                    AND t1.seat_level = m1.seat_level
                    AND t1.seat_number = m1.seat_number
                WHERE
                    m1.store_number = '{reqDto.storeNumber}'
                AND m1.seat_number = '{reqDto.SeatNumber}'
                AND m1.del_flg = '{FlgConst.off}'
                ORDER BY
                    m1.seat_level
                    ,m1.seat_number
            ";

                var quaryRslt = _dbContext.ExtSeatReceptions.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMdOrder rslt = new RpnMdOrder { seatList = rsltLst };

                return rslt;
            }
            catch (Exception)
            {
                RpnMdOrder rslt = new RpnMdOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_選択座席データ)
                };

                return rslt;
            }
        }

        /*　対象テーブル：商品メニューマスタ
            SQL ID：0003
        　　説明：商品メニュー分類を全件検索する。非表示のメニュー分類は取得しない。
        */
        [HttpPost("selectShohinBunrui.do")]
        public async Task<RpnMenuOrder> getShohinBunruiListInitDatas(ReqMenuOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getShohinBunruiListInitDatas request:{0}", reqDto);
                String selectSql = string.Empty;
                if (reqDto.menuNovisibleFlg.Equals(FlgConst.off))
                {
                    selectSql = $@"
                                SELECT
                                        p1.product_menu_number as MenuNumber
                                        ,p2.product_menu_line_number as MenuLineNumber
                                        ,c1.category_kbn_name as MenuClassName
                                        ,p2.product_menu_takeout_flg as MenuTakeoutFlg
                                    FROM
                                        m_product_menu p1
                                        INNER JOIN
                                            m_product_menu_class p2
                                        ON  p2.store_number = p1.store_number
                                        AND p2.product_menu_number = p1.product_menu_number
                                        LEFT JOIN
                                            m_category c1
                                        ON  c1.store_number = p2.store_number
                                        AND c1.category_class_number = '{CategoryConst.C_050_商品メニュー分類区分.value}'
                                        AND c1.category_kbn = p2.product_menu_class_kbn
                                    WHERE
                                        p1.store_number = '{reqDto.storeNumber}'
                                    AND p1.product_menu_apply_flg = '{FlgConst.on}'
                                    AND p1.del_flg = '{FlgConst.off}'
                                    AND p2.product_menu_novisible_flg = '{FlgConst.off}'
                                    ORDER BY
                                        p2.product_menu_line_number
                            ";
                }
                else
                {
                    selectSql = $@"
                                SELECT
                                        p1.product_menu_number as MenuNumber
                                        ,p2.product_menu_line_number as MenuLineNumber
                                        ,c1.category_kbn_name as MenuClassName
                                        ,p2.product_menu_takeout_flg as MenuTakeoutFlg
                                    FROM
                                        m_product_menu p1
                                        INNER JOIN
                                            m_product_menu_class p2
                                        ON  p2.store_number = p1.store_number
                                        AND p2.product_menu_number = p1.product_menu_number
                                        LEFT JOIN
                                            m_category c1
                                        ON  c1.store_number = p2.store_number
                                        AND c1.category_class_number = '{CategoryConst.C_050_商品メニュー分類区分.value}'
                                        AND c1.category_kbn = p2.product_menu_class_kbn
                                    WHERE
                                        p1.store_number = '{reqDto.storeNumber}'
                                    AND p1.product_menu_apply_flg = '{FlgConst.on}'
                                    AND p1.del_flg = '{FlgConst.off}'
                                    ORDER BY
                                        p2.product_menu_line_number
                            ";
                }
                var quaryRslt = _dbContext.ExtMenuReception.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMenuOrder rmolt = new RpnMenuOrder { menuclassList = rsltLst };

                return rmolt;
            }
            catch (Exception)
            {
                RpnMenuOrder rmolt = new RpnMenuOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_商品メニュー分類)
                };

                return rmolt;
            }
        }

        /*　対象テーブル：v_product_menu_item
            SQL ID：0002
        　　説明：指定された商品メニュー分類区分の商品メニュー検索を行う
        */
        [HttpPost("selectShohinBChoiceData.do")]
        public async Task<RpnChoiceOrder> getShohinBChoiceInitDatas(ReqChoiceOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getShohinBChoiceInitDatas request:{0}", reqDto);
                String selectSql = $@"
                SELECT
                        product_menu_number
                        ,product_menu_line_number as menuLineNumber
                        ,product_menu_class_kbn
                        ,menu_class_kbn_name
                        ,menu_class_kbn_abbr
                        ,product_menu_novisible_flg
                        ,product_menu_order_number as menuOrderNumber
                        ,product_number as number
                        ,product_set_number as setNumber
                        ,product_name_ch as nameCh
                        ,product_name_jp as nameJp
                        ,product_set_name as setName
                        ,product_price as price
                        ,product_tax_kbn as taxKbn
                        ,product_offertime_from as offertimeFrom
                        ,product_offertime_to as offertimeTo
                        ,product_calorie as calorie
                        ,product_cookingtime as cookingtime
                        ,product_picup_flg as pickupFlg
                        ,product_stok_flg
                        ,product_set_existence_flg as setExistenceFlg
                        ,product_limited_kbn as limitedKbn
                        ,product_type_kbn as typeKbn
                        ,type_kbn_name
                        ,type_kbn_abbr
                        ,product_course_flg
                        ,product_course_order
                        ,product_unit_kbn
                        ,unit_kbn_name
                        ,unit_kbn_abbr
                        ,product_image as image
                        ,product_introduction
                        ,product_preparation_flg as preparationFlg
                        ,product_remarks as remarks
                        ,limited_quantity as limitedQuantity
                    FROM
                        v_product_menu_item
                    WHERE
                        store_number = '{reqDto.storeNumber}'
                    AND IF( '{reqDto.menuLineNumber}' = '99','{reqDto.menuLineNumber}' = '{reqDto.menuLineNumber}', product_menu_line_number = '{reqDto.menuLineNumber}')
                    ORDER BY
                        product_menu_order_number
            ";

                var quaryRslt = _dbContext.ExtBChoiceReception.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnChoiceOrder rcolt = new RpnChoiceOrder { shohinList = rsltLst };

                return rcolt;
            }
            catch (Exception)
            {
                RpnChoiceOrder rcolt = new RpnChoiceOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_商品メニュー分類)
                };

                return rcolt;
            }
        }

        /*　対象テーブル：v_product_menu_item
            SQL ID：0002
        　　説明：指定された商品メニュー分類区分の商品メニュー検索を行う
        */
        [HttpPost("selectShohinCChoiceData.do")]
        public async Task<RpnChoiceOrder> getShohinCChoiceInitDatas(ReqChoiceOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getShohinCChoiceInitDatas request:{0}", reqDto);
                String selectSql = $@"
                SELECT
                        product_menu_number
                        ,product_menu_line_number as menuLineNumber
                        ,product_menu_class_kbn
                        ,menu_class_kbn_name
                        ,menu_class_kbn_abbr
                        ,product_menu_novisible_flg
                        ,product_menu_order_number as menuOrderNumber
                        ,product_number as number
                        ,product_set_number as setNumber
                        ,product_name_ch as nameCh
                        ,product_name_jp as nameJp
                        ,product_set_name as setName
                        ,product_price as price
                        ,product_tax_kbn as taxKbn
                        ,product_offertime_from as offertimeFrom
                        ,product_offertime_to as offertimeTo
                        ,product_calorie as calorie
                        ,product_cookingtime as cookingtime
                        ,product_picup_flg as pickupFlg
                        ,product_stok_flg
                        ,product_set_existence_flg as setExistenceFlg
                        ,product_limited_kbn as limitedKbn
                        ,product_type_kbn as typeKbn
                        ,type_kbn_name
                        ,type_kbn_abbr
                        ,product_course_flg
                        ,product_course_order
                        ,product_unit_kbn
                        ,unit_kbn_name
                        ,unit_kbn_abbr
                        ,product_image as image
                        ,product_introduction
                        ,product_preparation_flg as preparationFlg
                        ,product_remarks as remarks
                        ,limited_quantity as limitedQuantity
                    FROM
                        v_product_menu_item
                    WHERE
                        store_number = '{reqDto.storeNumber}'
                    AND product_menu_order_number = '{reqDto.menuOrderNumber}'
                    ORDER BY
                        product_menu_order_number
            ";

                var quaryRslt = _dbContext.ExtBChoiceReception.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnChoiceOrder rcolt = new RpnChoiceOrder { shohinList = rsltLst };

                return rcolt;
            }
            catch (Exception)
            {
                RpnChoiceOrder rcolt = new RpnChoiceOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_商品メニュー分類)
                };

                return rcolt;
            }
        }

        /*　対象テーブル：受付テーブル
            SQL ID：
        　　説明：
        */
        [HttpPost("insertReceptionInsideData.do")]
        public RpnIReceptionOrder InsertReceptionInsideData(ReqIReception reqIRec)
        {
            try
            {
                _logger.LogDebug("OrderController-InsertReceptionInsideData request:{0}", reqIRec);

                DbExtReception rsltLst = new DbExtReception();
                RpnIReceptionOrder rcolt = new RpnIReceptionOrder();
                var date_Now = DateTime.Now;

                // _dbContext.Database.BeginTransaction();

                String newRcptNum = "";

                foreach (var selectedSt in reqIRec.seatList)
                {
                    if (selectedSt != null && !String.IsNullOrEmpty(selectedSt.receptionNumber))
                    {
                        newRcptNum = selectedSt.receptionNumber;
                        _dbContext.Database.ExecuteSqlRaw(
                            $@"delete from t_reception where reception_number = '{newRcptNum}' and store_number = '{reqIRec.storeNumber}'"
                        );
                        // _dbContext.SaveChanges();

                        // 注文テーブル枝番の更新
                        _dbContext.Database.ExecuteSqlRaw(
                            $@"UPDATE t_order SET reception_branch_number = '{ConstCode.NUM_1}' WHERE store_number = '{reqIRec.storeNumber}' and reception_number = '{newRcptNum}'"
                        );
                    }
                }

                if (String.IsNullOrEmpty(newRcptNum))
                {
                    var dateNow = DateTime.Now.ToString(ConstCode.STRING_yyyyMMdd);
                    //採番する
                    var maxRcvQury = from rcv in _dbContext.TReceptions
                                     where rcv.ReceptionNumber.StartsWith(dateNow)
                                     orderby rcv.ReceptionNumber descending
                                     select rcv.ReceptionNumber;
                    var maxRcv = maxRcvQury.FirstOrDefault();

                    if (maxRcv != null)
                    {
                        var maxRcvNum = int.Parse(maxRcv.Substring(8));
                        newRcptNum = DateTime.Now.ToString("yyyyMMdd") + (maxRcvNum + 1).ToString("D3");
                    }
                    else
                    {
                        newRcptNum = DateTime.Now.ToString("yyyyMMdd") + "001";
                    }
                }

                ushort branchNm = 1;
                byte seatPeopleMan = 0;
                byte seatPeopleWoman = 0;
                byte seatPeopleChildren = 0;

                foreach (var selectedSt in reqIRec.selectedSeatList)
                {
                    var rcvIn = reqIRec.seatList.Where(recSt => recSt.seatNumber == selectedSt.selectedSeatNumber).FirstOrDefault();

                    // 人数
                    byte selectedSeatPeopleMan = rcvIn.seatPeopleMan ?? 0;
                    seatPeopleMan = seatPeopleMan < selectedSeatPeopleMan ? selectedSeatPeopleMan : seatPeopleMan;

                    byte selectedSeatPeopleWoman = rcvIn.seatPeopleWoman ?? 0;
                    seatPeopleWoman = seatPeopleWoman < selectedSeatPeopleWoman ? selectedSeatPeopleWoman : seatPeopleWoman;

                    byte selectedSeatPeopleChildren = rcvIn.seatPeopleChildren ?? 0;
                    seatPeopleChildren = seatPeopleChildren < selectedSeatPeopleChildren ? selectedSeatPeopleChildren : seatPeopleChildren;
                }
                var count = 0;
                var dateNow1 = DateTime.Now.ToString(ConstCode.STRING_yyyyMMdd);
                foreach (var selectedSt in reqIRec.selectedSeatList)
                {
                    String selectCount = $@"
                            select
                                count(*) as StoringTime
                            from t_reception t
                            where
                                store_number = '{reqIRec.storeNumber}'
                            and t.seat_number = '{selectedSt.selectedSeatNumber}'
                            and SUBSTRING(reception_number, 1, 8) = '{dateNow1}'
                            and t.seat_end_date is null
                            and t.reception_del_flg != '1'
                        ";
                    var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectCount).ToList();
                    if ((byte)countRslt[0].StoringTime > 0)
                    {
                        RpnIReceptionOrder rcolt1 = new RpnIReceptionOrder
                        {
                            status = RepMdStatus.S_602_SysInfoMsg,
                            msgList = SetRpnBase(MsgCode.E00200, MsgOption.E00200, "")
                        };
                        return rcolt1;
                    }

                    var rcvIn = reqIRec.seatList.Where(recSt => recSt.seatNumber == selectedSt.selectedSeatNumber).FirstOrDefault();

                    var newRcv = new TReception();
                    count++;
                    if (count == 1)
                    {
                        newRcv.StoreNumber = reqIRec.storeNumber;
                        newRcv.ReceptionNumber = newRcptNum;
                        newRcv.ReceptionBranchNumber = branchNm++;
                        newRcv.ReceptionKbn = CategoryConst.C_009_受付区分.ctg_001_店内;
                        newRcv.SeatLevel = rcvIn.seatLevel;
                        newRcv.SeatNumber = rcvIn.seatNumber;
                        newRcv.SeatStartDate = date_Now;
                        newRcv.SeatStatusKbn = CategoryConst.C_007_座席状況区分.ctg_002_使用中;
                        newRcv.SeatPeopleMan = seatPeopleMan;
                        newRcv.SeatPeopleWoman = seatPeopleWoman;
                        newRcv.SeatPeopleChildren = seatPeopleChildren;
                        newRcv.ReceptionRemarks = rcvIn.receptionRemarks;
                        newRcv.ReceptionDelFlg = FlgConst.off;
                        newRcv.RegistrationUser = reqIRec.storeNumber;
                        newRcv.RegistrationDatetime = date_Now;
                        newRcv.UpdateUser = reqIRec.storeNumber;
                        newRcv.UpdateDatetime = date_Now;
                    }
                    else
                    {
                        newRcv.StoreNumber = reqIRec.storeNumber;
                        newRcv.ReceptionNumber = newRcptNum;
                        newRcv.ReceptionBranchNumber = branchNm++;
                        newRcv.ReceptionKbn = CategoryConst.C_009_受付区分.ctg_001_店内;
                        newRcv.SeatLevel = rcvIn.seatLevel;
                        newRcv.SeatNumber = rcvIn.seatNumber;
                        newRcv.SeatStartDate = date_Now;
                        newRcv.SeatStatusKbn = CategoryConst.C_007_座席状況区分.ctg_002_使用中;
                        newRcv.ReceptionRemarks = rcvIn.receptionRemarks;
                        newRcv.ReceptionDelFlg = FlgConst.off;
                        newRcv.RegistrationUser = reqIRec.storeNumber;
                        newRcv.RegistrationDatetime = date_Now;
                        newRcv.UpdateUser = reqIRec.storeNumber;
                        newRcv.UpdateDatetime = date_Now;
                    }


                    _dbContext.TReceptions.Add(newRcv);

                    var insertRes = _dbContext.SaveChanges();

                    if (insertRes == 1)
                    {
                        rsltLst = new DbExtReception
                        {
                            SeatLevel = rcvIn.seatLevel,
                            SeatNumber = rcvIn.seatNumber,
                            receptionNumber = newRcptNum,
                            receptionBranchNumber = branchNm,
                            receptionKbn = CategoryConst.C_007_座席状況区分.ctg_002_使用中
                        };
                        rcolt.receptionList.Add(rsltLst);
                    }
                    else
                    {
                        rcolt.receptionList.Add(rsltLst);
                    }
                }
                return rcolt;
            }
            catch (Exception)
            {
                RpnIReceptionOrder rcolt = new RpnIReceptionOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00200, MsgOption.E00200, "")
                };
                return rcolt;
            }
        }

        /*　対象テーブル：システムメニューマスタ
            SQL ID：
        　　説明：
        */
        [HttpPost("searchSystemMenu.do")]
        public RpnSystemMenuOrder getSystemMenuListInitDatas(ReqSystemMenuOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getSystemMenuListInitDatas request:{0}", reqDto);

                var quaryRslt = from m1 in _dbContext.MSystemMenus
                                where m1.StoreNumber == reqDto.storeNumber
                                   && m1.SystemMenuUserKbn == reqDto.menuUserKbn
                                select new DbExtSystemMenuReception
                                {
                                    menuYPosition = m1.SystemMenuYPosition,
                                    menuXPosition = m1.SystemMenuXPosition,
                                    menuId = m1.SystemMenuId,
                                    menuName = m1.SystemMenuName,
                                    menuLink = m1.SystemMenuLink,
                                    menuButtonColor = m1.SystemMenuButtonColor,
                                    menuButtonIcon = m1.SystemMenuButtonIcon,
                                    menuFontColor = m1.SystemMenuFontColor,
                                    menuUnusedFlg = m1.SystemMenuUnusedFlg
                                };

                RpnSystemMenuOrder rsmo = new RpnSystemMenuOrder { menuList = quaryRslt.ToList() };
                return rsmo;
            }
            catch (Exception)
            {
                RpnSystemMenuOrder rsmo = new RpnSystemMenuOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase("", "", "")
                };
                return rsmo;
            }
        }

        /*　対象テーブル：店舗情報マスタ
            SQL ID：
        　　説明：
        */
        [HttpPost("searchStoreInfo.do")]
        public RpnStoreInfoOrder getStoreInfoInitDatas(ReqMdBase reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getStoreInfoInitDatas request:{0}", reqDto);

                var quaryRslt = from m1 in _dbContext.MStoreInfos
                                where m1.StoreNumber == reqDto.storeNumber
                                select new DbExtStoreInfoReception
                                {
                                    Name = m1.StoreName,
                                    Address = m1.StoreAddress,
                                    Tel = m1.StoreTel,
                                    Url = m1.StoreUrl,
                                    WeekdaysFrom = m1.StoreWeekdaysFrom,
                                    WeekdaysTo = m1.StoreWeekdaysTo,
                                    SaturdayFrom = m1.StoreSaturdayFrom,
                                    SaturdayTo = m1.StoreSaturdayTo,
                                    HolidaysFrom = m1.StoreHolidaysFrom,
                                    HolidaysTo = m1.StoreHolidaysTo,
                                    SeatQuantity = m1.StoreSeatQuantity,
                                    AllergiesDisplayFlg = m1.StoreAllergiesDisplayFlg,
                                    LogoImage = m1.StoreLogoImage,
                                    Image = m1.StoreImage,
                                    Introduction = m1.StoreIntroduction,
                                    StaffWord = m1.StoreStaffWord
                                };

                RpnStoreInfoOrder rsio = new RpnStoreInfoOrder { storeList = quaryRslt.ToList() };
                return rsio;
            }
            catch (Exception)
            {
                RpnStoreInfoOrder rsio = new RpnStoreInfoOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase("", "", "")
                };
                return rsio;
            }
        }
        /*　対象テーブル：注文テーブル
            SQL ID：
        　　説明：
        */
        [HttpPost("insertOrderData.do")]
        public async Task<RpnInsertOrder> InsertOrderInsideData(ReqInsertOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-InsertOrderInsideData request:{0}", reqDto);

                //全部の金額
                uint orderPriceSum = 0;
                //調理指示フラグ
                string orderCookingFlg = "0";

                //注文伝票番号を採番する
                String selectSql = $@"
                                select 
                                    MAX(order_voucher_details) as orderVoucherDetails
                                from t_order
                                where store_number = '{reqDto.storeNumber}'
                                and reception_number = '{reqDto.receptionNumber}'
                                ";

                var quaryRslt_s = _dbContext.ExOrderNumReception.FromSqlRaw(selectSql);
                var rsltLst_s = await quaryRslt_s.ToListAsync();
                DbExOrderNumReception deonr = new DbExOrderNumReception();

                DbExtOrderReception rsltLst = new DbExtOrderReception();

                RpnInsertOrder rcolt = new RpnInsertOrder();

                // 注文テーブルが空いている場合
                if (rsltLst_s[0].orderVoucherDetails != null)
                {
                    deonr = new DbExOrderNumReception { orderVoucherDetails = rsltLst_s[0].orderVoucherDetails };
                }
                else
                {
                    deonr = new DbExOrderNumReception { orderVoucherDetails = "0" };
                }
                for (int i = 0; i < reqDto.orderCart.Count; i++)
                {
                    orderPriceSum = reqDto.orderCart[i].orderPrice * reqDto.orderCart[i].orderQuantity;
                    if (reqDto.orderCart[i].orderSlipFlg.Equals("1"))
                    {
                        orderCookingFlg = "1";
                    }
                    else
                    {
                        orderCookingFlg = "0";
                    }
                    var DetailsNumber = (int.Parse(deonr.orderVoucherDetails) + 1 + i).ToString().PadLeft(3, '0');
                    var dateNowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    String selectLimited = $@"
                            select
                                product_limited_quantity as StoringTime
                            from m_product_set 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and product_number = '{reqDto.orderCart[i].productNumber}'
                        ";
                    var limitedQuantity = _dbContext.ExtStoringTime.FromSqlRaw(selectLimited).ToList();
                    var limitedQ = (byte)(limitedQuantity[0].StoringTime);
                    String selectOrder = $@"
                            select
                                IFNULL(sum(order_quantity), 0 )as StoringTime
                            from t_order 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and product_number = '{reqDto.orderCart[i].productNumber}'
                            and SUBSTRING(reception_number, 1, 8) = SUBSTRING('{reqDto.receptionNumber}', 1, 8)
                        ";
                    var orderQuantity = _dbContext.ExtStoringTime.FromSqlRaw(selectOrder).ToList();
                    var limitedO = (byte)(orderQuantity[0].StoringTime + reqDto.orderCart[i].orderQuantity);
                    if (limitedQ > 0)
                    {
                        if (limitedO > limitedQ)
                        {
                            RpnInsertOrder rcoltChe = new RpnInsertOrder
                            {
                                status = RepMdStatus.S_602_SysInfoMsg,
                                msgList = SetRpnBase(MsgCode.E00050, MsgOption.E00050, "")
                            };
                            return rcoltChe;
                        }

                    }
                    String insertSql = $@"
                                        insert into t_order(
                                                            store_number
                                                            , reception_number
                                                            , reception_branch_number
                                                            , order_voucher_number
                                                            , order_voucher_details
                                                            , order_date
                                                            , product_number
                                                            , product_set_number
                                                            , product_name_ch
                                                            , product_set_name
                                                            , product_type_kbn
                                                            , product_price
                                                            , order_quantity
                                                            , order_price
                                                            , order_discount_yen
                                                            , order_after_price
                                                            , order_takeout_flg
                                                            , order_cooking_flg
                                                            , order_serve_flg
                                                            , order_cancel_flg
                                                            , order_remarks
                                                            , registration_user
                                                            , registration_datetime
                                                            , update_user
                                                            , update_datetime
                                                        ) 
                                                    VALUES(  '{reqDto.storeNumber}'
                                                            ,'{reqDto.receptionNumber}'
                                                            ,'{reqDto.receptionBranchNumber}'
                                                            ,'{reqDto.receptionNumber.Substring(3, 8)}'
                                                            ,'{DetailsNumber}'
                                                            ,'{dateNowString}' 
                                                            ,'{reqDto.orderCart[i].productNumber}' 
                                                            ,'{reqDto.orderCart[i].productSetNumber}'
                                                            ,'{reqDto.orderCart[i].productNameCh}'
                                                            ,'{reqDto.orderCart[i].productSetName}'
                                                            ,'{reqDto.orderCart[i].productTypeKbn}'
                                                            ,'{reqDto.orderCart[i].orderPrice}'
                                                            ,'{reqDto.orderCart[i].orderQuantity}'
                                                            ,'{orderPriceSum}'
                                                            ,'{reqDto.orderCart[i].orderPrice}'
                                                            ,'{orderPriceSum}'
                                                            ,'{reqDto.orderCart[i].orderTakeoutFlg}'
                                                            ,'{orderCookingFlg}' 
                                                            ,'{FlgConst.off}' 
                                                            ,'{FlgConst.off}' 
                                                            ,'{reqDto.orderCart[i].orderRemarks}'
                                                            ,'{reqDto.storeNumber}'
                                                            ,'{dateNowString}' 
                                                            ,'{reqDto.storeNumber}'
                                                            ,'{dateNowString}'
                                                            )";

                    var quaryRslt = _dbContext.Database.ExecuteSqlRaw(insertSql);

                    if (quaryRslt == 1)
                    {
                        rsltLst = new DbExtOrderReception
                        {
                            orderDatetime = reqDto.orderCart[0].orderDatetime,
                            orderVoucherNumber = reqDto.receptionNumber.Substring(3, 8),
                            orderVoucherDetails = DetailsNumber,
                            orderProductNumber = reqDto.orderCart[i].productNumber,
                            orderProductSetNumber = reqDto.orderCart[i].productSetNumber,
                            orderProductNameCh = reqDto.orderCart[i].productNameCh,
                            orderProductSetName = reqDto.orderCart[i].productSetName,
                            orderProductTypeKbn = reqDto.orderCart[i].productTypeKbn,
                            orderQuantity = reqDto.orderCart[i].orderQuantity,
                            orderTakeoutFlg = reqDto.orderCart[i].orderTakeoutFlg,
                            orderRemarks = reqDto.orderCart[i].orderRemarks,
                            orderSlipFlg = orderCookingFlg
                        };
                        rcolt.orderList.Add(rsltLst);
                    }
                    else
                    {
                        rcolt.orderList.Add(rsltLst);
                    }
                }
                return rcolt;
            }
            catch (Exception)
            {
                RpnInsertOrder rcolt = new RpnInsertOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00050, MsgOption.E00050, "")
                };
                return rcolt;
            }
        }

        /*　対象テーブル：注文テーブル
            SQL ID：
        　　説明：注文履歴検索
        */
        [HttpPost("selectOrderHistoryData.do")]
        public RpnHistoryOrder getOrderHistoryListInitDatas(ReqInsertOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getOrderHistoryListInitDatas request:{0}", reqDto);

                uint? orderPriceTotal = 0;//合計金額

                var quaryRslt = from m1 in _dbContext.TOrders
                                join m2 in _dbContext.MProductFoods
                                on new { m1.StoreNumber, m1.ProductNumber } equals
                                   new { m2.StoreNumber, m2.ProductNumber }
                                where m1.StoreNumber == reqDto.storeNumber
                                   && m1.ReceptionNumber == reqDto.receptionNumber
                                   && m1.OrderCancelFlg == "0"
                                   && String.IsNullOrEmpty(m1.PaymentNumber)
                                orderby m1.OrderDate
                                select new DbHistoryOrderReception
                                {
                                    orderDatetime = m1.OrderDate,
                                    orderVoucherNumber = m1.OrderVoucherNumber,
                                    orderVoucherDetails = m1.OrderVoucherDetails,
                                    orderProductNumber = m1.ProductNumber,
                                    orderProductSetNumber = m1.ProductSetNumber,
                                    orderProductNameCh = m1.ProductNameCh,
                                    orderProductNameJp = m2.ProductNameJp,
                                    orderProductSetName = m1.ProductSetName,
                                    orderProductTypeKbn = m1.ProductTypeKbn,
                                    orderQuantity = m1.OrderQuantity,
                                    orderPrice = m1.OrderPrice,
                                    orderDiscountPrice = m1.OrderDiscountYen,
                                    orderTakeoutFlg = m1.OrderTakeoutFlg,
                                    orderCookingFlg = m1.OrderCookingFlg,
                                    orderServeFlg = m1.OrderServeFlg,
                                    orderRemarks = m1.OrderRemarks
                                };

                RpnHistoryOrder rhsolt = new RpnHistoryOrder();

                foreach (DbHistoryOrderReception d in quaryRslt)
                {
                    rhsolt.orderHistoryList.Add(d);

                    orderPriceTotal += d.orderDiscountPrice * d.orderQuantity;
                }

                rhsolt.orderPriceTotal = orderPriceTotal;

                rhsolt.orderQuantityTotal = rhsolt.orderHistoryList.Count;

                return rhsolt;
            }
            catch (Exception)
            {
                RpnHistoryOrder rhsolt = new RpnHistoryOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_注文履歴)
                };
                return rhsolt;
            }
        }

        /*　対象テーブル：
            SQL ID：
        　　説明：商品詳細内容検索
        */
        [HttpPost("selectShohinDetails.do")]
        public RpnShohinDetails getShohinDetailsListInitDatas(ReqShohinDetails reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getShohinDetailsListInitDatas request:{0}", reqDto);

                RpnShohinDetails rsdlt = new RpnShohinDetails();

                //指定された店舗番号の店舗情報の検索を行う
                RpnStoreInfoOrder rsio = getStoreInfoInitDatas(reqDto);

                if (rsio.storeList.Count > 0)
                {
                    //店舗情報リストに格納する
                    DbExtStoreList dsl = new DbExtStoreList { allergiesDisplayFlg = rsio.storeList[0].AllergiesDisplayFlg };
                    rsdlt.storeList.Add(dsl);

                    //指定された商品番号の商品詳細内容の検索を行う
                    var quaryProductFoodsRslt = from m1 in _dbContext.VProductMenuItems
                                                where m1.ProductNumber == reqDto.productNumber
                                                select new DbExtShohinDetailsReception
                                                {
                                                    productNumber = m1.ProductNumber,
                                                    setNumber = m1.ProductSetNumber,
                                                    image = m1.ProductImage,
                                                    nameCh = m1.ProductNameCh,
                                                    nameJp = m1.ProductNameJp,
                                                    calorie = m1.ProductCalorie,
                                                    offertimeFrom = m1.ProductOffertimeFrom,
                                                    offertimeTo = m1.ProductOffertimeTo,
                                                    price = m1.ProductPrice,
                                                    introduction = m1.ProductIntroduction,
                                                    pickupFlg = m1.ProductPicupFlg,
                                                    existenceFlg = m1.ProductSetExistenceFlg,
                                                    courseFlg = m1.ProductCourseFlg,
                                                    preparationFlg = m1.ProductPreparationFlg,
                                                    remarks = m1.ProductRemarks,
                                                    typeKbn = m1.ProductTypeKbn
                                                };
                    //商品詳細リストに格納する
                    foreach (DbExtShohinDetailsReception d in quaryProductFoodsRslt)
                    {
                        rsdlt.shohinDetailsList.Add(d);
                    }

                    //アレルギー表示有無フラグ＝１
                    if (dsl.allergiesDisplayFlg == "1")
                    {
                        //指定された商品番号の商品アレルギー情報の検索を行う
                        var quaryAllergiesRslt = from m1 in _dbContext.MProductAllergies
                                                 where m1.ProductNumber == reqDto.productNumber
                                                 select new DbExtAllergiesList
                                                 {
                                                     allergiesKbn = m1.ProductAllergiesKbn,
                                                     allergiesFlg = m1.ProductAllergiesFlg
                                                 };
                        //商品アレルギー情報のリストに格納する
                        foreach (DbExtAllergiesList d in quaryAllergiesRslt)
                        {
                            rsdlt.allergiesList.Add(d);
                        }
                    }

                    if (rsdlt.shohinDetailsList.Count > 0)
                    {
                        //コース料理フラグ＝１（コース料理品）
                        if (rsdlt.shohinDetailsList[0].courseFlg == "1")
                        {
                            //指定された商品番号のコース料理情報の検索を行う
                            var quaryCourseRslt = from m1 in _dbContext.MProductFoods
                                                  where m1.ProductTypeKbn == rsdlt.shohinDetailsList[0].typeKbn
                                                        && m1.ProductCourseOrder > 0
                                                  select new DbExtCourseList
                                                  {
                                                      courseProductName = m1.ProductNameCh,
                                                      courseOrder = m1.ProductCourseOrder
                                                  };
                            //商品アレルギー情報のリストに格納する
                            foreach (DbExtCourseList d in quaryCourseRslt)
                            {
                                rsdlt.courseList.Add(d);
                            }
                        }
                    }
                }
                return rsdlt;
            }
            catch (Exception)
            {
                RpnShohinDetails rsdlt = new RpnShohinDetails
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_商品マスタ詳細)
                };
                return rsdlt;
            }
        }

        /*　対象テーブル：注文テーブル
            SQL ID：
        　　説明：調理指示伝票印刷処理
        */
        [HttpPost("cookingInstFlgUpdate.do")]
        public RpnMdBase UpdateCookingInstFlgInitDatas(ReqMdSeatUpdateOrder reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-UpdateCookingInstFlgInitDatas request:{0}", reqDto);

                String updateSql = $@"
                                    UPDATE t_order 
                                    SET
                                        order_cooking_flg = '{FlgConst.on}'
                                    WHERE
                                        store_number = '{reqDto.storeNumber}'
                                        and reception_number = '{reqDto.ReceptionNumber}'
                                        and reception_branch_number = '{reqDto.ReceptionBranchNumber}'
                                        and order_voucher_number = '{reqDto.OrderVoucherNumber}'
                                        and order_voucher_details = '{reqDto.OrderVoucherDetails}'";
                var quaryRslt1 = _dbContext.Database.ExecuteSqlRaw(updateSql);

                RpnMdBase rcolt = new RpnMdBase
                {
                    status = RepMdStatus.S_200_OK
                };

                return rcolt;
            }
            catch (Exception)
            {
                RpnMdBase rhsolt = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00050, MsgOption.E00050, "")
                };
                return rhsolt;
            }
        }

        /*　対象テーブル：税区分マスタテーブル
            SQL ID：0001
        　　説明：税区分取得処理
        */
        [HttpPost("serachCTaxKubun.do")]
        public RpbCTaxList getCTaxKubunInitDatas(ReqMdBase reqDto)
        {
            try
            {
                _logger.LogDebug("OrderController-getCTaxKubunInitDatas request:{0}", reqDto);

                var dateNow = DateTime.Now.ToString("yyyyMMdd");

                var quaryRslt = from m1 in _dbContext.MTaxes
                                where m1.StoreNumber == reqDto.storeNumber
                                   && String.Compare(m1.TaxDateStart, dateNow) <= 0
                                   && String.Compare(m1.TaxDateEnd, dateNow) >= 0
                                select new DbExtCTaxList
                                {
                                    taxKbn = m1.TaxKbn,
                                    taxPercent = m1.TaxPercent.ToString()
                                };


                RpbCTaxList rko = new RpbCTaxList { ctaxList = quaryRslt.ToList() };
                return rko;
            }
            catch (Exception)
            {
                RpbCTaxList rko = new RpbCTaxList
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_税区分取得)
                };
                return rko;
            }
        }
        [HttpPost("selectOrderList.do")]
        public RpnMdBase SelectOrderList(ReqMdSeatUpdateOrder reqDto)
        {
            try
            {
                RpnMdBase rko = new RpnMdBase();
                _logger.LogDebug("OrderController-SelectOrderList request:{0}", reqDto);

                var SqlSelectRes = from t1 in _dbContext.TOrders
                                   where t1.ReceptionNumber == reqDto.ReceptionNumber
                                   && t1.StoreNumber == reqDto.storeNumber
                                   select t1.ReceptionNumber;

                var sqlFirstRow = SqlSelectRes.FirstOrDefault();
                if (sqlFirstRow == null)
                {
                    rko = new RpnMdBase
                    {
                        status = RepMdStatus.S_200_OK
                    };
                }
                else
                {
                    rko = new RpnMdBase
                    {
                        status = RepMdStatus.S_200_OK,
                        msgList = SetRpnBase("", "1", "")
                    };
                }
                return rko;
            }
            catch (Exception)
            {
                RpnMdBase rko = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00010, MsgOption.E00010, MsgOption_パラメタ.E00010_税区分取得)
                };
                return rko;
            }
        }
        /*　対象テーブル：受付テーブル
            SQL ID：
        　　説明：受付削除
        */
        [HttpPost("deleteReception.do")]
        public RpnMdBase DeleteReception(ReqMdSeatUpdateOrder reqDto)
        {
            try
            {
                RpnMdBase rko = new RpnMdBase();
                _logger.LogDebug("OrderController-DeleteReception request:{0}", reqDto);

                _dbContext.Database.ExecuteSqlRaw(
                            $@"update t_reception set reception_del_flg = '{ConstCode.NUM_1}' where reception_number = '{reqDto.ReceptionNumber}' and store_number = '{reqDto.storeNumber}'"
                );
                rko = new RpnMdBase
                {
                    status = RepMdStatus.S_200_OK
                };
                return rko;
            }
            catch (Exception)
            {
                RpnMdBase rko = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00200, MsgOption.E00200, "")
                };
                return rko;
            }
        }
        /*　対象テーブル：注文テーブル
            SQL ID：
        　　説明：注文備考更新
        */
        [HttpPost("updateOrder.do")]
        public RpnMdBase UpdateOrder(ReqMdSeatUpdateOrder reqDto)
        {
            try
            {
                RpnMdBase rko = new RpnMdBase();
                _logger.LogDebug("OrderController-UpdateOrder request:{0}", reqDto);

                _dbContext.Database.ExecuteSqlRaw(
                            $@"UPDATE t_order
                                SET
                                    order_remarks = '{reqDto.Remark}'
                                WHERE
                                    store_number ='{reqDto.storeNumber}'
                                    and reception_number = '{reqDto.ReceptionNumber}'
                                    and reception_branch_number ='{reqDto.ReceptionBranchNumber}'
                                    and order_voucher_number = '{reqDto.OrderVoucherNumber}'
                                    and order_voucher_details = '{reqDto.OrderVoucherDetails}'"
                );
                rko = new RpnMdBase
                {
                    status = RepMdStatus.S_200_OK
                };
                return rko;
            }
            catch (Exception)
            {
                RpnMdBase rko = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = SetRpnBase(MsgCode.E00200, MsgOption.E00200, "")
                };
                return rko;
            }
        }

        [NonAction]
        public RpnMdMsg SetRpnBase(string msgCode, string msg, string param)
        {
            RpnMdMsg Repmsg = new RpnMdMsg
            {
                msgCode = msgCode,
                msgItemId = "",
                msgOption = String.Format(msg, param)
            };
            return Repmsg;
        }
    }
}
