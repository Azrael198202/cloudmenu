using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       メニューのリスト
    *****/
    public class DbExtSystemMenuReception
    {
        public byte menuYPosition { get; set; }//ボタン縦位置
        public byte menuXPosition { get; set; }//ボタン横位置
        public string menuId { get; set; }//画面ID
        public string menuName { get; set; }//ボタン名
        public string menuLink { get; set; }//ボタンリンク
        public string menuButtonColor { get; set; }//ボタン色
        public string menuButtonIcon { get; set; }//ボタンアイコン
        public string menuFontColor { get; set; }//フォント色
        public string menuUnusedFlg { get; set; }//未使用フラグ
    }
}
