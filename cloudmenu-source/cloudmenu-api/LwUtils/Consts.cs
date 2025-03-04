using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.LwUtils
{
    public static class FlgConst
    {
        public static string off = "0";
        public static string on = "1";
    }
    public static class ConstCode
    {
        public static int NUM_0 = 0;
        public static int NUM_1 = 1;
        public static int NUM_2 = 2;
        public static int NUM_8 = 8;
        public static string STRING_SLASH = "/";
        public static string STRING_yyyyMMdd = "yyyyMMdd";
        public static string STRING_001 = "001";
        public static string STRING_yyyyMMddhhmm = "yyyy/MM/dd hh:mm";
    }
    /*****
    * 
    *****/
    public static class CategoryConst
    {
        public static class C_001_商品カテゴリ区分
        {
            public static string value = "001";
            public static string ctg_001_単品 = "001";
            public static string ctg_002_肉料理 = "002";
            public static string ctg_003_魚料理 = "003";
            public static string ctg_004_ドリンク = "004";
            public static string ctg_005_アルコール = "005";
        }
        public static class C_002_商品アレルギー区分
        {
            public static string value = "002";
            public static string ctg_001_鶏卵 = "001";
            public static string ctg_002_牛乳 = "002";
            public static string ctg_003_小麦 = "003";
            public static string ctg_004_ピーナツ = "004";
            public static string ctg_005_果物類 = "005";
            public static string ctg_006_魚卵 = "006";
            public static string ctg_007_甲殻類 = "007";
            public static string ctg_008_ナッツ類 = "008";
            public static string ctg_009_ソバ = "009";
            public static string ctg_010_魚類 = "010";
            public static string ctg_011_その他 = "011";
        }
        public static class C_003_座席区分
        {
            public static string value = "003";
            public static string ctg_001_ホール = "001";
            public static string ctg_002_カウンター = "002";
            public static string ctg_003_個室 = "003";
            public static string ctg_004_座敷 = "004";
            public static string ctg_005_広間 = "005";
        }
        public static class C_004_店舗ジャンル区分
        {
            public static string value = "004";
            public static string ctg_001_中華料理 = "001";
            public static string ctg_002_和食 = "002";
            public static string ctg_003_洋食西洋料理 = "003";
            public static string ctg_004_アジアエスニック = "004";
        }
        public static class C_005_単位区分
        {
            public static string value = "005";
            public static string ctg_001_個 = "001";
            public static string ctg_002_本 = "002";
            public static string ctg_003_枚 = "003";
            public static string ctg_004_箱 = "004";
            public static string ctg_005_ｇ = "005";
            public static string ctg_006_kg = "006";
            public static string ctg_007_ml = "007";
            public static string ctg_008_l = "008";
        }
        public static class C_007_座席状況区分
        {
            public static string value = "007";
            public static string ctg_001_空き = "001";
            public static string ctg_002_使用中 = "002";
            public static string ctg_003_予約済 = "003";
        }
        public static class C_008_利用者区分
        {
            public static string value = "008";
            public static string ctg_010_PC操作スタッフ = "010";
            public static string ctg_011_スマホ操作スタッフ = "011";
            public static string ctg_020_顧客 = "020";
        }
        public static class C_009_受付区分
        {
            public static string value = "009";
            public static string ctg_001_店内 = "001";
            public static string ctg_002_テイクアウト = "002";
        }
        public static class C_010_現金
        {
            public static string value = "010";
            public static string ctg_001_現金 = "001";
        }
        public static class C_011_クレジット
        {
            public static string value = "011";
            public static string ctg_101_VISA = "101";
            public static string ctg_102_MasterCard = "102";
            public static string ctg_103_ダイナースクラブ = "103";
            public static string ctg_104_アメリカンエキスプレス = "104";
            public static string ctg_105_JCB = "105";
            public static string ctg_106_銀聯 = "106";
            public static string ctg_199_その他 = "199";
        }
        public static class C_012_電子マネー
        {
            public static string value = "012";
            public static string ctg_201_交通系電子マネー = "201";
        }
        public static class C_013_掛売
        {
            public static string value = "013";
            public static string ctg_301_掛売 = "301";
            public static string ctg_302_地域共通クーポン = "302";
            public static string ctg_303_会食 = "303";
            public static string ctg_304_Uber = "304";
            public static string ctg_305_出前館 = "305";
        }
        public static class C_014_商品券釣有り
        {
            public static string value = "014";
            public static string ctg_401_商品券釣有り = "401";
        }
        public static class C_015_商品券釣無し
        {
            public static string value = "015";
            public static string ctg_501_商品券釣無し = "501";
        }
        public static class C_016_QRコード決済
        {
            public static string value = "016";
            public static string ctg_601_QRコード決済 = "601";
        }
        public static class C_017_未回収
        {
            public static string value = "017";
            public static string ctg_701_未回収 = "701";
        }
        public static class C_020_プリンタ情報
        {
            public static string value = "020";
            public static string ctg_010_プリンタ１調理指示用 = "010";
            public static string ctg_011_プリンタ２調理指示用 = "011";
            public static string ctg_012_プリンタ３調理指示用デシャップ = "012";
            public static string ctg_020_プリンタ２レシートプリンタ = "020";
        }
        public static class C_030_物区分
        {
            public static string value = "030";
            public static string ctg_010_商品 = "010";
            public static string ctg_020_原材料 = "020";
            public static string ctg_030_備品 = "030";
        }
        public static class C_031_商品分類区分
        {
            public static string value = "031";
            public static string ctg_001_前菜 = "001";
            public static string ctg_002_海鮮老上海 = "002";
            public static string ctg_003_家常小炒 = "003";
            public static string ctg_004_野菜豆腐 = "004";
            public static string ctg_005_スープ羹 = "005";
            public static string ctg_006_麺飯 = "006";
            public static string ctg_007_甜品デザート = "007";
            public static string ctg_008_燕翅鮑参肚 = "008";
            public static string ctg_010_料理長お薦めコース = "010";
            public static string ctg_011_ビール = "011";
            public static string ctg_012_ウイスキー = "012";
            public static string ctg_013_焼酎 = "013";
            public static string ctg_014_日本酒 = "014";
            public static string ctg_015_紹興酒 = "015";
            public static string ctg_016_白酒 = "016";
            public static string ctg_017_サワー = "017";
            public static string ctg_018_赤ワイン = "018";
            public static string ctg_019_白ワイン = "019";
            public static string ctg_020_スパークリングワイン = "020";
            public static string ctg_021_シャンパン = "021";
            public static string ctg_022_中国茶 = "022";
            public static string ctg_030_裏メニュー = "030";
            public static string ctg_023_ソフトドリンク = "023";
        }
        public static class C_032_原材料分類区分
        {
            public static string value = "032";
            public static string ctg_010_肉 = "010";
            public static string ctg_020_魚 = "020";
            public static string ctg_030_野菜 = "030";
            public static string ctg_090_その他 = "090";
        }
        public static class C_033_備品分類区分
        {
            public static string value = "033";
            public static string ctg_010_食器 = "010";
            public static string ctg_020_装飾 = "020";
            public static string ctg_090_その他 = "090";
        }
        public static class C_040_入出庫区分
        {
            public static string value = "040";
            public static string ctg_001_入庫 = "001";
            public static string ctg_002_出庫 = "002";
        }
        public static class C_041_消費区分
        {
            public static string value = "041";
            public static string ctg_001_使用 = "001";
            public static string ctg_002_破棄 = "002";
            public static string ctg_003_破損 = "003";
            public static string ctg_090_不明 = "090";
        }
        public static class C_050_商品メニュー分類区分
        {
            public static string value = "050";
            public static string ctg_001_前菜 = "001";
            public static string ctg_002_海鮮老上海 = "002";
            public static string ctg_003_家常小炒 = "003";
            public static string ctg_004_野菜豆腐 = "004";
            public static string ctg_005_スープ羹 = "005";
            public static string ctg_006_麺飯 = "006";
            public static string ctg_007_甜品デザート = "007";
            public static string ctg_008_燕翅鮑参肚 = "008";
            public static string ctg_010_コース = "010";
            public static string ctg_011_アルコール = "011";
            public static string ctg_012_ソフトドリンク = "012";
            public static string ctg_020_裏メニュー = "020";
        }
        public static class C_060_顧客区分
        {
            public static string value = "060";
            public static string ctg_001_VIP = "001";
            public static string ctg_002_準VIP = "002";
            public static string ctg_003_関係者 = "003";
            public static string ctg_004_ブラックリスト = "004";
        }
        public static class C_070_テーブル備考雛形
        {
            public static string value = "070";
            public static string ctg_001_団体日本人 = "001";
            public static string ctg_002_団体中国人 = "002";
        }
        public static class C_071_商品注文備考雛形
        {
            public static string value = "071";
            public static string ctg_001_辛さ控えめ = "001";
            public static string ctg_002_辛め = "002";
            public static string ctg_003_激辛 = "003";
        }
        public static class C_080_数量限定区分
        {
            public static string value = "080";
            public static string ctg_001_セット合算 = "001";
            public static string ctg_002_セット毎 = "002";
            public static string ctg_900_管理外 = "900";
        }
        public static class C_090_サービス料自動加算設定
        {
            public static string value = "090";
            public static string ctg_001_1 = "001";
            public static string ctg_002_1730 = "002";
        }
        public static class C_100_キッチン出力用商品分類区分
        {
            public static string value = "100";
            public static string ctg_001_キッチン用指示伝票 = "001";
            public static string ctg_002_キッチン用指示伝票 = "002";
            public static string ctg_003_キッチン用指示伝票 = "003";
            public static string ctg_004_キッチン用指示伝票 = "004";
            public static string ctg_005_キッチン用指示伝票 = "005";
            public static string ctg_006_キッチン用指示伝票 = "006";
            public static string ctg_007_キッチン用指示伝票 = "007";
            public static string ctg_008_キッチン用指示伝票 = "008";
            public static string ctg_010_キッチン用指示伝票 = "010";
        }
        public static class C_101_デシャップ出力用商品分類区分
        {
            public static string value = "101";
            public static string ctg_011_デシャップ用指示伝票 = "011";
            public static string ctg_012_デシャップ用指示伝票 = "012";
        }
        public static class C_200_座席状況一覧警告時間
        {
            public static string value = "200";
            public static string ctg_001_40 = "001";
            public static string ctg_002_10 = "002";
            public static string ctg_003_15 = "003";
        }
        public static class C_201_座席状況一覧更新間隔
        {
            public static string value = "201";
            public static string ctg_001_更新間隔_秒 = "001";
        }
        public static class C_300_入金区分
        {
            public static string value = "300";
            public static string ctg_001_おつり用で入金 = "001";
        }
        public static class C_301_出金区分
        {
            public static string value = "301";
            public static string ctg_001_小口用で出金 = "001";
        }
    }
}
