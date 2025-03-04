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
    public partial class OrderController
    {

        /*　対象テーブル：m_product_food
        　　説明：入出庫商品取得処理（入庫）
        */
        [HttpPost("searchNyukoProduct.do")]
        public async Task<RpnMdTypeOrder> SearchNyukoProduct(ReqMdTypeKbn reqDto)
        {
            _logger.LogDebug("OrderController-SearchNyukoProduct request:{0}", reqDto);
            try
            {
                String selectSql = $@"
                     select
                         food.product_number as categoryKbn
                       , food.product_name_ch as categoryName
                     from m_product_food food 
                     where
                           food.product_type_kbn = '{reqDto.itemKbn}' 
                       and food.store_number = '{reqDto.storeNumber}'
                       and food.product_stok_flg = '1'
                    ";

                var quaryRslt = _dbContext.ExtTypeName.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMdTypeOrder rslt = new RpnMdTypeOrder
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
                RpnMdTypeOrder rsltErr = new RpnMdTypeOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：m_product_food
        　　説明：入出庫原材料取得処理（入庫）
        */
        [HttpPost("searchNyukoMaterial.do")]
        public async Task<RpnMdTypeOrder> SearchNyukoMaterial(ReqMdTypeKbn reqDto)
        {
            _logger.LogDebug("OrderController-SearchNyukoMaterial request:{0}", reqDto);
            try
            {
                String selectSql = $@"
                     select
                         material.material_number as categoryKbn
                       , material.material_name as categoryName
                     from m_material material 
                     where
                           material.material_type_kbn = '{reqDto.itemKbn}' 
                       and material.store_number = '{reqDto.storeNumber}'
                    ";

                var quaryRslt = _dbContext.ExtTypeName.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMdTypeOrder rslt = new RpnMdTypeOrder
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
                RpnMdTypeOrder rsltErr = new RpnMdTypeOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：m_product_food
        　　説明：入出庫備品取得処理（入庫）
        */
        [HttpPost("searchNyukoEquipment.do")]
        public async Task<RpnMdTypeOrder> SearchNyukoEquipment(ReqMdTypeKbn reqDto)
        {
            _logger.LogDebug("OrderController-SearchNyukoEquipment request:{0}", reqDto);
            try
            {
                String selectSql = $@"
                     select
                         equipment.equipment_number as categoryKbn
                       , equipment.equipment_name as categoryName
                     from m_equipment equipment 
                     where
                           equipment.equipment_type_kbn = '{reqDto.itemKbn}' 
                       and equipment.store_number = '{reqDto.storeNumber}'
                    ";

                var quaryRslt = _dbContext.ExtTypeName.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnMdTypeOrder rslt = new RpnMdTypeOrder
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
                RpnMdTypeOrder rsltErr = new RpnMdTypeOrder
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
        [HttpPost("addNyukoProduct.do")]
        public RpnMdBase AddNyukoProduct(ReqMdAddStoring reqDto)
        {
            _logger.LogDebug("OrderController-AddNyukoProduct request:{0}", reqDto);
            try
            {
                DateTime dateNow = DateTime.Now;
                var date_Now = dateNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                String selectSql = $@"
                     select
                         IFNULL( MAX(tsi.product_storing_issue_times), 0) as StoringTime
                     from t_product_storing_issue tsi 
                     where
                           tsi.store_number = '{reqDto.storeNumber}' 
                       and tsi.product_number = '{reqDto.productNumber}'
                       and tsi.product_storing_issue_kbn = '001'
                    ";

                var quaryRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();

                TProductStoringIssue StoringIssue = new TProductStoringIssue
                {
                    StoreNumber = reqDto.storeNumber,
                    ProductNumber = reqDto.productNumber,
                    ProductStoringIssueDate = reqDto.storingDate,
                    ProductStoringIssueKbn = ConstCode.STRING_001,
                    ProductStoringIssueQuantity = reqDto.storingQuantity,
                    ProductStoringIssueRemarks = reqDto.remarks,
                    ProductStoringIssueTimes = (byte)(quaryRslt[0].StoringTime + 1),
                    RegistrationUser = reqDto.storeNumber,
                    RegistrationDatetime = dateNow,
                    UpdateUser = reqDto.storeNumber,
                    UpdateDatetime = dateNow
                };

                _dbContext.ProductStoringIssue.Add(StoringIssue);
                _dbContext.SaveChanges();
                // 在庫テーブル追加
                String selectSql1 = $@"
                            select
                                count(*) as StoringTime
                            from m_product_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and product_number = '{reqDto.productNumber}'
                        ";
                var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql1).ToList();
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
                                    , '{reqDto.productNumber}'
                                    , '{reqDto.storingQuantity}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                )
                            ";
                    var quaryRslt1 = _dbContext.Database.ExecuteSqlRaw(insertSql);
                }
                else
                {
                    String selectSql2 = $@"
                            select
                                product_stock_quantity as StoringTime
                            from m_product_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and product_number = '{reqDto.productNumber}'
                        ";
                    var quantity = _dbContext.ExtStoringTime.FromSqlRaw(selectSql2).ToList();
                    int storingQuantity = quantity[0].StoringTime + reqDto.storingQuantity;
                    String updateSql = $@"
                                UPDATE m_product_stock
                                    SET 
                                    product_stock_quantity = '{storingQuantity}'
                                    , update_user = '{reqDto.storeNumber}'
                                    , update_datetime = '{date_Now}'
                                    WHERE store_number = '{reqDto.storeNumber}'
                                    AND product_number = '{reqDto.productNumber}'";
                    var quaryRslt2 = _dbContext.Database.ExecuteSqlRaw(updateSql);
                }
                RpnMdBase rcolt = new RpnMdBase { status = RepMdStatus.S_200_OK };

                return rcolt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00080,
                    msgOption = MsgOption.E00080

                };
                RpnMdBase rsltErr = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：原材料入庫追加処理
            SQL ID：
　　         説明：
        */
        [HttpPost("addNyukoMaterial.do")]
        public RpnMdBase AddNyukoMaterial(ReqMdAddStoring reqDto)
        {
            _logger.LogDebug("OrderController-AddNyukoMaterial request:{0}", reqDto);
            try
            {
                DateTime dateNow = DateTime.Now;
                var date_Now = dateNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                String selectSql = $@"
                     select
                         IFNULL( MAX(tsi.material_storing_issue_times), 0) as StoringTime
                     from t_material_storing_issue tsi 
                     where
                           tsi.store_number = '{reqDto.storeNumber}' 
                       and tsi.material_number = '{reqDto.productNumber}'
                       and tsi.material_storing_issue_kbn = '001'
                    ";

                var quaryRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();

                TMaterialStoringIssue StoringIssue = new TMaterialStoringIssue
                {
                    StoreNumber = reqDto.storeNumber,
                    MaterialNumber = reqDto.productNumber,
                    MaterialStoringIssueDate = reqDto.storingDate,
                    MaterialStoringIssueKbn = ConstCode.STRING_001,
                    MaterialStoringIssueQuantity = reqDto.storingQuantity,
                    MaterialStoringIssueRemarks = reqDto.remarks,
                    MaterialStoringIssueTimes = (byte)(quaryRslt[0].StoringTime + 1),
                    RegistrationUser = reqDto.storeNumber,
                    RegistrationDatetime = dateNow,
                    UpdateUser = reqDto.storeNumber,
                    UpdateDatetime = dateNow
                };

                _dbContext.MaterialStoringIssue.Add(StoringIssue);
                _dbContext.SaveChanges();
                // 在庫テーブル追加
                String selectSql1 = $@"
                            select
                                count(*) as StoringTime
                            from m_material_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and material_number = '{reqDto.productNumber}'
                        ";
                var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql1).ToList();
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
                                    , '{reqDto.productNumber}'
                                    , '{reqDto.storingQuantity}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                )
                            ";
                    var quaryRslt1 = _dbContext.Database.ExecuteSqlRaw(insertSql);
                }
                else
                {
                    String selectSql2 = $@"
                            select
                                material_stock_quantity as StoringTime
                            from m_material_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and material_number = '{reqDto.productNumber}'
                        ";
                    var quantity = _dbContext.ExtStoringTime.FromSqlRaw(selectSql2).ToList();
                    int storingQuantity = quantity[0].StoringTime + reqDto.storingQuantity;
                    String updateSql = $@"
                                UPDATE m_material_stock
                                    SET 
                                    material_stock_quantity = '{storingQuantity}'
                                    , update_user = '{reqDto.storeNumber}'
                                    , update_datetime = '{date_Now}'
                                    WHERE store_number = '{reqDto.storeNumber}'
                                    AND material_number = '{reqDto.productNumber}'";
                    var quaryRslt2 = _dbContext.Database.ExecuteSqlRaw(updateSql);
                }
                RpnMdBase rcolt = new RpnMdBase { status = RepMdStatus.S_200_OK };

                return rcolt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00080,
                    msgOption = MsgOption.E00080

                };
                RpnMdBase rsltErr = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：備品入庫追加処理
            SQL ID：
　　         説明：
        */
        [HttpPost("addNyukoEquipment.do")]
        public RpnMdBase AddNyukoEquipment(ReqMdAddStoring reqDto)
        {
            _logger.LogDebug("OrderController-AddNyukoEquipment request:{0}", reqDto);
            try
            {
                DateTime dateNow = DateTime.Now;
                var date_Now = dateNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                String selectSql = $@"
                     select
                         IFNULL( MAX(tsi.equipment_storing_issue_times), 0) as StoringTime
                     from t_equipment_storing_issue tsi 
                     where
                           tsi.store_number = '{reqDto.storeNumber}' 
                       and tsi.equipment_number = '{reqDto.productNumber}'
                       and tsi.equipment_storing_issue_kbn = '001'
                ";

                var quaryRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql).ToList();

                TEquipmentStoringIssue StoringIssue = new TEquipmentStoringIssue
                {
                    StoreNumber = reqDto.storeNumber,
                    EquipmentNumber = reqDto.productNumber,
                    EquipmentStoringIssueDate = reqDto.storingDate,
                    EquipmentStoringIssueKbn = ConstCode.STRING_001,
                    EquipmentStoringIssueQuantity = reqDto.storingQuantity,
                    EquipmentStoringIssueRemarks = reqDto.remarks,
                    EquipmentStoringIssueTimes = (byte)(quaryRslt[0].StoringTime + 1),
                    RegistrationUser = reqDto.storeNumber,
                    RegistrationDatetime = dateNow,
                    UpdateUser = reqDto.storeNumber,
                    UpdateDatetime = dateNow
                };

                _dbContext.EquipmentStoringIssue.Add(StoringIssue);
                _dbContext.SaveChanges();
                // 在庫テーブル追加
                String selectSql1 = $@"
                            select
                                count(*) as StoringTime
                            from m_equipment_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and equipment_number = '{reqDto.productNumber}'
                        ";
                var countRslt = _dbContext.ExtStoringTime.FromSqlRaw(selectSql1).ToList();
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
                                    , '{reqDto.productNumber}'
                                    , '{reqDto.storingQuantity}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                    , '{reqDto.storeNumber}'
                                    , '{date_Now}'
                                )
                            ";
                    var quaryRslt1 = _dbContext.Database.ExecuteSqlRaw(insertSql);
                }
                else
                {
                    String selectSql2 = $@"
                            select
                                equipment_stock_quantity as StoringTime
                            from m_equipment_stock 
                            where
                                store_number = '{reqDto.storeNumber}' 
                            and equipment_number = '{reqDto.productNumber}'
                        ";
                    var quantity = _dbContext.ExtStoringTime.FromSqlRaw(selectSql2).ToList();
                    int storingQuantity = quantity[0].StoringTime + reqDto.storingQuantity;
                    String updateSql = $@"
                                UPDATE m_equipment_stock
                                    SET 
                                    equipment_stock_quantity = '{storingQuantity}'
                                    , update_user = '{reqDto.storeNumber}'
                                    , update_datetime = '{date_Now}'
                                    WHERE store_number = '{reqDto.storeNumber}'
                                    AND equipment_number = '{reqDto.productNumber}'";
                    var quaryRslt2 = _dbContext.Database.ExecuteSqlRaw(updateSql);
                }
                RpnMdBase rcolt = new RpnMdBase { status = RepMdStatus.S_200_OK };

                return rcolt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00080,
                    msgOption = MsgOption.E00080

                };
                RpnMdBase rsltErr = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：受付テーブルt_reception
            SQL ID：
        　　説明：受付テーブル持ち帰り作成（持ち帰り注文受付）
        */
        [HttpPost("insertReceptionTakeoutData.do")]
        public RpnIReceptionOrder InsertReceptionTakeoutData(ReqIReceptionTakeOut reqDto)
        {
            _logger.LogDebug("OrderController-InsertReceptionTakeoutData request:{0}", reqDto);
            try
            {

                var dateNowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                String selectSql = $@"
                     select
                         MAX(t.reception_number) as receptionNumber
                     from t_reception t 
                     where
                        t.store_number = '{reqDto.storeNumber}' 
            ";

                var quaryRslt1 = _dbContext.ExtReceptionNumber.FromSqlRaw(selectSql).ToList();
                string dateTime = DateTime.Now.ToString(ConstCode.STRING_yyyyMMdd);
                string receptionNumber = dateTime + ConstCode.STRING_001;
                if (!String.IsNullOrEmpty(quaryRslt1[ConstCode.NUM_0].receptionNumber))
                {
                    string receptionDate =
                      quaryRslt1[ConstCode.NUM_0].receptionNumber.Substring(ConstCode.NUM_0, ConstCode.NUM_8);
                    if (dateTime.CompareTo(receptionDate) == ConstCode.NUM_0)
                    {
                        receptionNumber =
                          (Convert.ToInt64(quaryRslt1[ConstCode.NUM_0].receptionNumber) + ConstCode.NUM_1).ToString();
                    }
                }

                // DateTime takeoutReceiptTime = Convert.ToDateTime(reqDto.takeoutReceiptTime);
                String insertSql = $@"
                INSERT 
                    INTO t_reception( 
                        store_number
                        , reception_number
                        , reception_branch_number
                        , reception_kbn
                        , takeout_name
                        , takeout_tel 
                        , takeout_receipt_time
                        , seat_start_date
                        , reception_del_flg
                        , registration_user
                        , registration_datetime
                        , update_user
                        , update_datetime
                    ) 
                    VALUES ( 
                        '{reqDto.storeNumber}'
                        ,'{receptionNumber}'
                        ,'1'
                        ,'{reqDto.receptionKbn}'
                        ,'{reqDto.takeoutName}'
                        ,'{reqDto.takeoutTel}'
                        ,'{reqDto.takeoutReceiptTime}'
                        ,'{dateNowString}'
                        ,'{FlgConst.off}'
                        ,'{reqDto.storeNumber}'
                        ,'{dateNowString}'
                        ,'{reqDto.storeNumber}'
                        ,'{dateNowString}'
                        )";

                var quaryRslt = _dbContext.Database.ExecuteSqlRaw(insertSql);

                DbExtReception rsltLst = new DbExtReception();

                RpnIReceptionOrder rcolt = new RpnIReceptionOrder();

                if (quaryRslt == 1)
                {
                    rsltLst = new DbExtReception
                    {
                        receptionNumber = receptionNumber,
                        receptionBranchNumber = 1,
                        receptionKbn = reqDto.receptionKbn
                    };
                    rcolt.receptionList.Add(rsltLst);
                    rcolt.status = RepMdStatus.S_200_OK;
                }
                else
                {
                    rcolt.receptionList.Add(rsltLst);
                }
                return rcolt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00200,
                    msgOption = MsgOption.E00200

                };
                RpnIReceptionOrder rsltErr = new RpnIReceptionOrder
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：m_product_food
        　　説明：商品分類区分商品検索（注文可能数変更）
        */
        [HttpPost("selectShohinTypeKbnData.do")]
        public async Task<RpnShohinType> SelectShohinTypeKbnData(ReqShohinType reqDto)
        {
            _logger.LogDebug("OrderController-SelectShohinTypeKbnData request:{0}", reqDto);
            try
            {
                String selectSql = $@"
                    SELECT
                        food.product_number as productNumber
                       , m_product_set.product_set_number as productSetNumber
                       , food.product_name_ch as productNameCh
                       , m_product_set.product_set_name as productSetName
                       , m_product_set.product_limited_quantity as productLimitedQuantity
                       , food.product_set_existence_flg as setExistenceFlg
                       , food.product_limited_kbn as limitedKbn
                    FROM
                       m_product_food food 
                    LEFT JOIN m_product_set 
                       ON food.product_number = m_product_set.product_number 
                    WHERE
                       food.product_type_kbn = '{reqDto.productTypeKbn}' 
                       AND food.store_number = '{reqDto.storeNumber}' 
                    ORDER BY
                       food.product_number
                     , m_product_set.product_set_number ASC
                ";

                var quaryRslt = _dbContext.ExtShohinType.FromSqlRaw(selectSql);
                var rsltLst = await quaryRslt.ToListAsync();
                RpnShohinType rslt = new RpnShohinType
                {
                    productTypeKbnList = rsltLst,
                    status = RepMdStatus.S_200_OK
                };

                return rslt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00010,
                    msgOption = String.Format(MsgOption.E00010, MsgOption_パラメタ.E00010_商品分類区分商品)

                };
                RpnShohinType rsltErr = new RpnShohinType
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

        /*　対象テーブル：注文可能数変更（注文可能数変更）
            SQL ID：
　　         説明：
        */
        [HttpPost("updateLimitedQuantity.do")]
        public RpnMdBase UpdateLimitedQuantity(ReqLimitedQuantity reqDto)
        {
            _logger.LogDebug("OrderController-UpdateLimitedQuantity request:{0}", reqDto);
            try
            {
                var dateNowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                foreach (ProductList productList in reqDto.selectedTypeKbnList[0].productList)
                {
                    String updateSql1 = $@"
                UPDATE m_product_food food
                    SET 
                      product_limited_kbn = '{productList.productLimitedKbnSw}'
                    , update_user = '{reqDto.storeNumber}'
                    , update_datetime = '{dateNowString}'
                    WHERE store_number = '{reqDto.storeNumber}'
                      AND product_number = '{productList.productNumber}'";
                    var quaryRslt1 = _dbContext.Database.ExecuteSqlRaw(updateSql1);

                    if (!String.IsNullOrWhiteSpace(productList.productSetNumber))
                    {
                        String updateSql2 = $@"
                    UPDATE m_product_set 
                        SET 
                          product_limited_quantity = '{productList.productLimitedQuantity}'
                        WHERE store_number = '{reqDto.storeNumber}'
                          AND product_set_number = '{productList.productSetNumber}'
                          AND product_number = '{productList.productNumber}'";
                        var quaryRslt2 = _dbContext.Database.ExecuteSqlRaw(updateSql2);

                    }
                }

                RpnMdBase rcolt = new RpnMdBase
                {
                    status = RepMdStatus.S_200_OK
                };

                return rcolt;
            }
            catch (Exception)
            {
                RpnMdMsg msg = new RpnMdMsg
                {
                    msgCode = MsgCode.E00220,
                    msgOption = MsgOption.E00220

                };
                RpnMdBase rsltErr = new RpnMdBase
                {
                    status = RepMdStatus.S_602_SysInfoMsg,
                    msgList = msg
                };
                return rsltErr;
            }
        }

    }
}
