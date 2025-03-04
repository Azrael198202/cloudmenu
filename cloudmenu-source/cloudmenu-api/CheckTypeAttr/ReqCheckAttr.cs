using cloudmenu_api.CheckTypeAttr;
namespace cloudmenu_api.CheckTypeAttr
{
    /// <summary>
    /// Attribute自定義
    /// </summary>
    public class ReqCheckAttr : System.Attribute
    {
        public bool require { get; set; }//必須チェック
        public ItemCheckType itemType { get; set; }//チェックタイプ分類
        public CheckStrType strType { get; set; }//文字列チェックタイプ分類
        public int Min_Num { get; set; }//最小桁数(文字列)
        public int Max_Num { get; set; }//最大桁数(文字列)
        public string RegexPattern { get; set; }//正規表現でチェック
        public CheckIntNumType IntNumType { get; set; }//整数チェックタイプ分類
        public CheckDecNumType DecNumType { get; set; }//小数チェックタイプ分類
        public double Min_Value { get; set; }//最小値(整数、小数)
        public double Max_Value { get; set; }//最大値(整数、小数)
        public int SeiYuKou_Ketasu { get; set; }//整数有効桁数(小数)
        public int ShouYuKou_Ketasu { get; set; }//小数有効桁数(小数)
    }
}