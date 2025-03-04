using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.LwUtils
{
    public static class MsgCode
    {
        public static string E00010 = "E_00010";
        public static string E00200 = "E_00200";
        public static string E00220 = "E_00220";
        public static string E00050 = "E_00050";
        public static string E00080 = "E_00080";
    }

    public static class MsgOption
    {
        public static string E00010 = "{0}検索でエラーが発生しました。";
        public static string E00200 = "受付処理に失敗しました。";
        public static string E00220 = "注文可能数登録に失敗しました。";
        public static string E00050 = "注文明細書き込みに失敗しました。";
        public static string E00080 = "DBの処理に失敗しました。";
    }

    public static class MsgOption_パラメタ
    {
        public static string E00010_商品メニュー分類 = "商品メニュー分類";
        public static string E00010_区分取得 = "区分取得";
        public static string E00010_座席データ全件 = "座席データ全件";
        public static string E00010_商品マスタ詳細 = "商品マスタ（詳細）";
        public static string E00010_商品分類区分商品 = "商品分類区分商品";
        public static string E00010_選択座席データ = "選択座席データ";
        public static string E00010_棚卸検索 = "棚卸検索";
        public static string E00010_商品メニュー注文番号 = "商品メニュー注文番号";
        public static string E00010_注文履歴 = "注文履歴";
        public static string E00010_入庫検索 = "入庫検索";
        public static string E00010_注文データ = "注文データ";
        public static string E00010_税区分取得 = "税区分取得";

    }
}
