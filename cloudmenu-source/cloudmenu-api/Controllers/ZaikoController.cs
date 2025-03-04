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
    [Route("api/zaiko")]
    public class ZaikoController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ZaikoController> _logger;

        public ZaikoController(ILogger<ZaikoController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /*　対象テーブル：
  　　      説明：棚卸検索処理（棚卸入力）
        */
        [HttpPost("searchTana.do")]
        public async Task<RpnMdTana> SearchTana(ReqMdTana reqDto)
        {
            _logger.LogDebug("OrderController-SearchTana request:{0}", reqDto);
            try
            {
                String selectSql = string.Empty;
                String inventoryDate = string.Empty;
                if (!String.IsNullOrEmpty(reqDto.inventoryDate))
                {
                    inventoryDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, "");
                }

                if (CategoryConst.C_031_商品分類区分.value.Equals(reqDto.itemKbn))
                {
                    selectSql = $@"
                        select now.product_number as targetNumber
                            , food.product_name_ch as targetName
                            , now.前回棚卸日 as lastDate
                            , now.前回確定数 as lastQuantity
                            , now.入庫 as storingQuantity
                            , now.消費 as consumptionQuantity
                            , now.論理在庫数 as stockQuantity
                        from v_product_inventory_now now
                        left join m_product_food food
                        on now.store_number = food.store_number
                        and now.product_number = food.product_number
                        where now.前回棚卸日 < '{inventoryDate}' 
                        or now.前回棚卸日 is null
                        and now.store_number = '{reqDto.storeNumber}'
                        and food.product_type_kbn ='{reqDto.categoryKbn}'
                    ";
                }
                else if (CategoryConst.C_032_原材料分類区分.value.Equals(reqDto.itemKbn))
                {
                    selectSql = $@"
                        select now.material_number as targetNumber
                            , material.material_name as targetName
                            , now.前回棚卸日 as lastDate
                            , now.前回確定数 as lastQuantity
                            , now.入庫 as storingQuantity
                            , now.消費 as consumptionQuantity
                            , now.論理在庫数 as stockQuantity
                        from v_material_inventory_now now
                        left join m_material material
                        on now.store_number = material.store_number
                        and now.material_number = material.material_number
                        where now.前回棚卸日 < '{inventoryDate}'
                        or now.前回棚卸日 is null 
                        and now.store_number = '{reqDto.storeNumber}'
                        and material.material_type_kbn ='{reqDto.categoryKbn}'
                        ";
                }
                else if (CategoryConst.C_033_備品分類区分.value.Equals(reqDto.itemKbn))
                {
                    selectSql = $@"
                        select now.equipment_number as targetNumber
                            , equipment.equipment_name as targetName
                            , now.前回棚卸日 as lastDate
                            , now.前回確定数 as lastQuantity
                            , now.入庫 as storingQuantity
                            , now.消費 as consumptionQuantity
                            , now.論理在庫数 as stockQuantity
                        from v_equipment_inventory_now now
                        left join m_equipment equipment
                        on now.store_number = equipment.store_number
                        and now.equipment_number = equipment.equipment_number
                        where now.前回棚卸日 < '{inventoryDate}' 
                        or now.前回棚卸日 is null
                        and now.store_number = '{reqDto.storeNumber}'
                        and equipment.equipment_type_kbn ='{reqDto.categoryKbn}'
                    ";
                }

                var quaryRslt = _dbContext.ExtInventory.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMdTana rslt = new RpnMdTana
                {
                    searchList = rsltLst,
                    status = RepMdStatus.S_200_OK
                };

                return rslt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00010,
                    msgOption = String.Format(MsgOption.E00010, MsgOption_パラメタ.E00010_棚卸検索)

                };
                RpnMdTana rsltErr = new RpnMdTana
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：商品入庫追加処理
            SQL ID：
　　         説明：
        */
        [HttpPost("saveTana.do")]
        public RpnMdBase SaveTana(ReqSaveTana reqDto)
        {
            _logger.LogDebug("OrderController-SaveTana request:{0}", reqDto);
            DateTime dateNow = DateTime.Now;
            var date_Now = dateNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                if (CategoryConst.C_031_商品分類区分.value.Equals(reqDto.itemKbn))
                {
                    foreach (TanaList tanaList in reqDto.tanaList)
                    {
                        TProductInventory tProductInventory = new TProductInventory
                        {
                            StoreNumber = reqDto.storeNumber,
                            ProductInventoryDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                            ProductNumber = tanaList.targetNumber,
                            ProductInventoryQuantity = tanaList.inventoryQuantity,
                            RegistrationUser = reqDto.storeNumber,
                            RegistrationDatetime = dateNow,
                            UpdateUser = reqDto.storeNumber,
                            UpdateDatetime = dateNow
                        };
                        _dbContext.ProductInventory.Add(tProductInventory);
                        _dbContext.SaveChanges();

                        String selectSql = $@"
                            select
                                count(*) as StoringTime
                            from m_product_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and product_number = '{tanaList.targetNumber}'
                        ";
                        var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                        if (countRslt[0].StoringTime == 0)
                        {
                            String insertSql = $@"
                            INSERT 
                                INTO m_product_stock( 
                                    store_number
                                    , product_number
                                    , product_stock_quantity
                                    , registration_user
                                    , registration_datetime
                                    , update_user
                                    , update_datetime
                                ) 
                                VALUES ( 
                                    '{reqDto.storeNumber}'
                                    , '{tanaList.targetNumber}'
                                    , '{tanaList.inventoryQuantity}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                )
                            ";
                            var quaryRslt = _dbContext.Database.ExecuteSqlRaw(insertSql);
                        }
                        else
                        {
                            String updateSql = $@"
                                UPDATE m_product_stock
                                    SET 
                                    product_stock_quantity = '{tanaList.inventoryQuantity}'
                                    , update_user = '{reqDto.storeNumber}'
                                    , update_datetime = '{date_Now}'
                                    WHERE store_number = '{reqDto.storeNumber}'
                                    AND product_number = '{tanaList.targetNumber}'";
                            var quaryRslt = _dbContext.Database.ExecuteSqlRaw(updateSql);
                        }

                    }

                    foreach (UchiwakeList uchiwakeList in reqDto.uchiwakeList)
                    {
                        if (uchiwakeList.breakdownQuantity != null)
                        {
                            if (FlgConst.off.Equals(uchiwakeList.targetFlg))
                            {
                                String selectSql = $@"
                            select
                                IFNULL( MAX(tsi.product_storing_issue_times), 0) as StoringTime
                            from t_product_storing_issue tsi 
                            where
                                tsi.store_number = '{reqDto.storeNumber}' 
                            and tsi.product_number = '{uchiwakeList.targetNumber}'
                            and tsi.product_storing_issue_kbn = '{uchiwakeList.breakdownKbn}'
                        ";

                                var quaryRslt1 = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                                TProductStoringIssue tProductStoringIssue = new TProductStoringIssue
                                {
                                    StoreNumber = reqDto.storeNumber,
                                    ProductNumber = uchiwakeList.targetNumber,
                                    ProductStoringIssueDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                                    ProductStoringIssueKbn = uchiwakeList.breakdownKbn,
                                    ProductStoringIssueQuantity = uchiwakeList.breakdownQuantity,
                                    ProductStoringIssueTimes = (byte)(quaryRslt1[0].StoringTime + 1),
                                    RegistrationUser = reqDto.storeNumber,
                                    RegistrationDatetime = dateNow,
                                    UpdateUser = reqDto.storeNumber,
                                    UpdateDatetime = dateNow
                                };
                                _dbContext.ProductStoringIssue.Add(tProductStoringIssue);
                                _dbContext.SaveChanges();
                            }
                            if (FlgConst.on.Equals(uchiwakeList.targetFlg))
                            {
                                String selectSql = $@"
                                select
                                    IFNULL( MAX(tpc.product_consumption_times), 0) as StoringTime
                                from t_product_consumption tpc 
                                where
                                    tpc.store_number = '{reqDto.storeNumber}' 
                                and tpc.product_number = '{uchiwakeList.targetNumber}'
                                and tpc.product_consumption_kbn = '{uchiwakeList.breakdownKbn}'
                            ";

                                var quaryRslt1 = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                                TProductConsumption tProductConsumption = new TProductConsumption
                                {
                                    StoreNumber = reqDto.storeNumber,
                                    ProductNumber = uchiwakeList.targetNumber,
                                    ProductConsumptionDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                                    ProductConsumptionKbn = uchiwakeList.breakdownKbn,
                                    ProductConsumptionQuantity = uchiwakeList.breakdownQuantity,
                                    ProductConsumptionTimes = (byte)(quaryRslt1[0].StoringTime + 1),
                                    RegistrationUser = reqDto.storeNumber,
                                    RegistrationDatetime = dateNow,
                                    UpdateUser = reqDto.storeNumber,
                                    UpdateDatetime = dateNow
                                };
                                _dbContext.ProductConsumption.Add(tProductConsumption);
                                _dbContext.SaveChanges();
                            }
                        }

                    }
                }
                else if (CategoryConst.C_032_原材料分類区分.value.Equals(reqDto.itemKbn))
                {
                    foreach (TanaList tanaList in reqDto.tanaList)
                    {
                        TMaterialInventory tMaterialInventory = new TMaterialInventory
                        {
                            StoreNumber = reqDto.storeNumber,
                            MaterialInventoryDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                            MaterialNumber = tanaList.targetNumber,
                            MaterialInventoryQuantity = tanaList.inventoryQuantity,
                            RegistrationUser = reqDto.storeNumber,
                            RegistrationDatetime = dateNow,
                            UpdateUser = reqDto.storeNumber,
                            UpdateDatetime = dateNow
                        };
                        _dbContext.MaterialInventory.Add(tMaterialInventory);
                        _dbContext.SaveChanges();

                        String selectSql = $@"
                            select
                                count(*) as StoringTime
                            from m_material_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and material_number = '{tanaList.targetNumber}'
                        ";
                        var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                        if (countRslt[0].StoringTime == 0)
                        {
                            String insertSql = $@"
                            INSERT 
                                INTO m_material_stock( 
                                    store_number
                                    , material_number
                                    , material_stock_quantity
                                    , registration_user
                                    , registration_datetime
                                    , update_user
                                    , update_datetime
                                ) 
                                VALUES ( 
                                    '{reqDto.storeNumber}'
                                    , '{tanaList.targetNumber}'
                                    , '{tanaList.inventoryQuantity}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                )
                            ";
                            var quaryRslt = _dbContext.Database.ExecuteSqlRaw(insertSql);
                        }
                        else
                        {
                            String updateSql = $@"
                            UPDATE m_material_stock
                                SET 
                                material_stock_quantity = '{tanaList.inventoryQuantity}'
                                , update_user = '{reqDto.storeNumber}'
                                , update_datetime = '{date_Now}'
                                WHERE store_number = '{reqDto.storeNumber}'
                                AND material_number = '{tanaList.targetNumber}'";
                            var quaryRslt = _dbContext.Database.ExecuteSqlRaw(updateSql);
                        }
                    }

                    foreach (UchiwakeList uchiwakeList in reqDto.uchiwakeList)
                    {
                        if (uchiwakeList.breakdownQuantity != null)
                        {
                            if (FlgConst.off.Equals(uchiwakeList.targetFlg))
                            {
                                String selectSql = $@"
                                select
                                    IFNULL( MAX(tsi.material_storing_issue_times), 0) as StoringTime
                                from t_material_storing_issue tsi 
                                where
                                    tsi.store_number = '{reqDto.storeNumber}' 
                                and tsi.material_number = '{uchiwakeList.targetNumber}'
                                and tsi.material_storing_issue_kbn = '{uchiwakeList.breakdownKbn}'
                            ";

                                var quaryRslt1 = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                                TMaterialStoringIssue tMaterialStoringIssue = new TMaterialStoringIssue
                                {
                                    StoreNumber = reqDto.storeNumber,
                                    MaterialNumber = uchiwakeList.targetNumber,
                                    MaterialStoringIssueDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                                    MaterialStoringIssueKbn = uchiwakeList.breakdownKbn,
                                    MaterialStoringIssueQuantity = uchiwakeList.breakdownQuantity,
                                    MaterialStoringIssueTimes = (byte)(quaryRslt1[0].StoringTime + 1),
                                    RegistrationUser = reqDto.storeNumber,
                                    RegistrationDatetime = dateNow,
                                    UpdateUser = reqDto.storeNumber,
                                    UpdateDatetime = dateNow
                                };
                                _dbContext.MaterialStoringIssue.Add(tMaterialStoringIssue);
                                _dbContext.SaveChanges();
                            }
                            if (FlgConst.on.Equals(uchiwakeList.targetFlg))
                            {
                                String selectSql = $@"
                                select
                                    IFNULL( MAX(tpc.material_consumption_times), 0) as StoringTime
                                from t_material_consumption tpc 
                                where
                                    tpc.store_number = '{reqDto.storeNumber}' 
                                and tpc.material_number = '{uchiwakeList.targetNumber}'
                                and tpc.material_consumption_kbn = '{uchiwakeList.breakdownKbn}'
                            ";

                                var quaryRslt1 = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                                TMaterialConsumption tMaterialConsumption = new TMaterialConsumption
                                {
                                    StoreNumber = reqDto.storeNumber,
                                    MaterialNumber = uchiwakeList.targetNumber,
                                    MaterialConsumptionDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                                    MaterialConsumptionKbn = uchiwakeList.breakdownKbn,
                                    MaterialConsumptionQuantity = uchiwakeList.breakdownQuantity,
                                    MaterialConsumptionTimes = (byte)(quaryRslt1[0].StoringTime + 1),
                                    RegistrationUser = reqDto.storeNumber,
                                    RegistrationDatetime = dateNow,
                                    UpdateUser = reqDto.storeNumber,
                                    UpdateDatetime = dateNow
                                };
                                _dbContext.MaterialConsumption.Add(tMaterialConsumption);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                }
                else if (CategoryConst.C_033_備品分類区分.value.Equals(reqDto.itemKbn))
                {
                    foreach (TanaList tanaList in reqDto.tanaList)
                    {
                        TEquipmentInventory tEquipmentInventory = new TEquipmentInventory
                        {
                            StoreNumber = reqDto.storeNumber,
                            EquipmentInventoryDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                            EquipmentNumber = tanaList.targetNumber,
                            EquipmentInventoryQuantity = tanaList.inventoryQuantity,
                            RegistrationUser = reqDto.storeNumber,
                            RegistrationDatetime = dateNow,
                            UpdateUser = reqDto.storeNumber,
                            UpdateDatetime = dateNow
                        };
                        _dbContext.EquipmentInventory.Add(tEquipmentInventory);
                        _dbContext.SaveChanges();
                        String selectSql = $@"
                            select
                                count(*) as StoringTime
                            from m_equipment_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and equipment_number = '{tanaList.targetNumber}'
                        ";
                        var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                        if (countRslt[0].StoringTime == 0)
                        {
                            String insertSql = $@"
                            INSERT 
                                INTO m_equipment_stock( 
                                    store_number
                                    , equipment_number
                                    , equipment_stock_quantity
                                    , registration_user
                                    , registration_datetime
                                    , update_user
                                    , update_datetime
                                ) 
                                VALUES ( 
                                    '{reqDto.storeNumber}'
                                    , '{tanaList.targetNumber}'
                                    , '{tanaList.inventoryQuantity}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                )
                            ";
                            var quaryRslt = _dbContext.Database.ExecuteSqlRaw(insertSql);
                        }
                        else
                        {
                            String updateSql = $@"
                                UPDATE m_equipment_stock
                                    SET 
                                    equipment_stock_quantity = '{tanaList.inventoryQuantity}'
                                    , update_user = '{reqDto.storeNumber}'
                                    , update_datetime = '{date_Now}'
                                    WHERE store_number = '{reqDto.storeNumber}'
                                    AND equipment_number = '{tanaList.targetNumber}'";
                            var quaryRslt = _dbContext.Database.ExecuteSqlRaw(updateSql);
                        }
                    }
                    foreach (UchiwakeList uchiwakeList in reqDto.uchiwakeList)
                    {
                        if (uchiwakeList.breakdownQuantity != null)
                        {
                            if (FlgConst.off.Equals(uchiwakeList.targetFlg))
                            {
                                String selectSql = $@"
                                select
                                    IFNULL( MAX(tsi.equipment_storing_issue_times), 0) as StoringTime
                                from t_equipment_storing_issue tsi 
                                where
                                    tsi.store_number = '{reqDto.storeNumber}' 
                                and tsi.equipment_number = '{uchiwakeList.targetNumber}'
                                and tsi.equipment_storing_issue_kbn = '{uchiwakeList.breakdownKbn}'
                            ";

                                var quaryRslt1 = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                                TEquipmentStoringIssue tEquipmentStoringIssue = new TEquipmentStoringIssue
                                {
                                    StoreNumber = reqDto.storeNumber,
                                    EquipmentNumber = uchiwakeList.targetNumber,
                                    EquipmentStoringIssueDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                                    EquipmentStoringIssueKbn = uchiwakeList.breakdownKbn,
                                    EquipmentStoringIssueQuantity = uchiwakeList.breakdownQuantity,
                                    EquipmentStoringIssueTimes = (byte)(quaryRslt1[0].StoringTime + 1),
                                    RegistrationUser = reqDto.storeNumber,
                                    RegistrationDatetime = dateNow,
                                    UpdateUser = reqDto.storeNumber,
                                    UpdateDatetime = dateNow
                                };
                                _dbContext.EquipmentStoringIssue.Add(tEquipmentStoringIssue);
                                _dbContext.SaveChanges();
                            }
                            if (FlgConst.on.Equals(uchiwakeList.targetFlg))
                            {
                                String selectSql = $@"
                                select
                                    IFNULL( MAX(tpc.equipment_consumption_times), 0) as StoringTime
                                from t_equipment_consumption tpc 
                                where
                                    tpc.store_number = '{reqDto.storeNumber}' 
                                and tpc.equipment_number = '{uchiwakeList.targetNumber}'
                                and tpc.equipment_consumption_kbn = '{uchiwakeList.breakdownKbn}'
                            ";

                                var quaryRslt1 = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();
                                TEquipmentConsumption tEquipmentConsumption = new TEquipmentConsumption
                                {
                                    StoreNumber = reqDto.storeNumber,
                                    EquipmentNumber = uchiwakeList.targetNumber,
                                    EquipmentConsumptionDate = reqDto.inventoryDate.Replace(ConstCode.STRING_SLASH, ""),
                                    EquipmentConsumptionKbn = uchiwakeList.breakdownKbn,
                                    EquipmentConsumptionQuantity = uchiwakeList.breakdownQuantity,
                                    EquipmentConsumptionTimes = (byte)(quaryRslt1[0].StoringTime + 1),
                                    RegistrationUser = reqDto.storeNumber,
                                    RegistrationDatetime = dateNow,
                                    UpdateUser = reqDto.storeNumber,
                                    UpdateDatetime = dateNow
                                };
                                _dbContext.EquipmentConsumption.Add(tEquipmentConsumption);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00080,
                    msgOption = MsgOption.E00080

                };

                RpnMdBase rcolt1 = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rcolt1;

            }

            RpnMdBase rcolt = new RpnMdBase
            {
                status = RepMdStatus.S_200_OK
            };

            return rcolt;
        }

        /*　対象テーブル：
  　　      説明：入庫リスト検索処理（入庫）
        */
        [HttpPost("searchNyukoList.do")]
        public async Task<RpnMdStoring> SearchNyukoList(ReqMdStoring reqDto)
        {
            _logger.LogDebug("OrderController-SearchNyukoList request:{0}", reqDto);
            try
            {
                string storingDate = string.Empty;
                if (!String.IsNullOrEmpty(reqDto.storingDate))
                {
                    storingDate = reqDto.storingDate.Replace(ConstCode.STRING_SLASH, string.Empty);
                }
                String selectSql = string.Empty;
                if (CategoryConst.C_031_商品分類区分.value.Equals(reqDto.itemKbn))
                {
                    selectSql = $@"
                        select
                            t.product_storing_issue_quantity as storingQuantity
                        , food.product_name_ch as targetName
                        , food.product_number as targetNumber
                        , category.category_kbn_name as categoryName
                        , food.product_type_kbn as categoryKbn
                        , m.category_kbn_name as unitName
                        , food.product_unit_kbn as unitKbn
                        from t_product_storing_issue t 
                        left join m_product_food food 
                        on t.product_number = food.product_number 
                        left join m_category category 
                        on food.store_number = category.store_number 
                            and food.product_type_kbn = category.category_kbn
                            and category.category_class_number = '{CategoryConst.C_031_商品分類区分.value}'
                        left join m_category m 
                        on food.store_number = m.store_number 
                            and food.product_unit_kbn = m.category_kbn 
                            and m.category_class_number = '{CategoryConst.C_005_単位区分.value}'
                        where
                            t.product_storing_issue_date = '{storingDate}'
                            and t.product_storing_issue_kbn = '{CategoryConst.C_040_入出庫区分.ctg_001_入庫}'
                            and t.store_number = '{reqDto.storeNumber}'
                    ";
                }
                else if (CategoryConst.C_032_原材料分類区分.value.Equals(reqDto.itemKbn))
                {
                    selectSql = $@"
                        select
                            t.material_storing_issue_quantity as storingQuantity
                        , material.material_name as targetName
                        , material.material_number as targetNumber
                        , category.category_kbn_name as categoryName
                        , material.material_type_kbn as categoryKbn
                        , m.category_kbn_name as unitName
                        , material.material_unit_kbn as unitKbn
                        from t_material_storing_issue t 
                        left join m_material material 
                        on t.material_number = material.material_number 
                        left join m_category category 
                        on material.store_number = category.store_number 
                            and material.material_type_kbn = category.category_kbn 
                            and category.category_class_number = '{CategoryConst.C_032_原材料分類区分.value}'
                        left join m_category m 
                        on material.store_number = m.store_number 
                            and material.material_unit_kbn = m.category_kbn 
                            and m.category_class_number = '{CategoryConst.C_005_単位区分.value}'
                        where
                            t.material_storing_issue_date = '{storingDate}' 
                            and t.material_storing_issue_kbn = '{CategoryConst.C_040_入出庫区分.ctg_001_入庫}' 
                            and t.store_number = '{reqDto.storeNumber}'
                    ";
                }
                else if (CategoryConst.C_033_備品分類区分.value.Equals(reqDto.itemKbn))
                {
                    selectSql = $@"
                        select
                            t.equipment_storing_issue_quantity as storingQuantity
                        , equipment.equipment_name as targetName
                        , equipment.equipment_number as targetNumber
                        , category.category_kbn_name as categoryName
                        , equipment.equipment_type_kbn as categoryKbn
                        , m.category_kbn_name as unitName
                        , equipment.equipment_unit_kbn as unitKbn
                        from t_equipment_storing_issue t 
                        left join m_equipment equipment 
                        on t.equipment_number = equipment.equipment_number 
                        left join m_category category 
                        on equipment.store_number = category.store_number 
                            and equipment.equipment_type_kbn = category.category_kbn 
                            and category.category_class_number = '{CategoryConst.C_033_備品分類区分.value}'
                        left join m_category m 
                        on equipment.store_number = m.store_number 
                            and equipment.equipment_unit_kbn = m.category_kbn 
                            and m.category_class_number = '{CategoryConst.C_005_単位区分.value}'
                        where
                            t.equipment_storing_issue_date = '{storingDate}' 
                            and t.equipment_storing_issue_kbn = '{CategoryConst.C_040_入出庫区分.ctg_001_入庫}' 
                            and t.store_number = '{reqDto.storeNumber}'
                    ";
                }

                var quaryRslt = _dbContext.ExtStoring.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMdStoring rslt = new RpnMdStoring
                {
                    searchList = rsltLst,
                    status = RepMdStatus.S_200_OK
                };

                return rslt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00010,
                    msgOption = String.Format(MsgOption.E00010, MsgOption_パラメタ.E00010_入庫検索)

                };
                RpnMdStoring rsltErr = new RpnMdStoring
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

    }
}
