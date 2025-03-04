
#nullable disable

namespace cloudmenu_api.CheckTypeAttr
{
    /// <summary>
    /// チェックタイプ分類
    /// </summary>
    public enum ItemCheckType
    {
        /// <summary>
        /// 文字列チェックタイプ
        /// </summary>
        STR = 1,
        /// <summary>
        /// 整数チェックタイプ
        /// </summary>
        INT_NUM = 2,
        /// <summary>
        /// 小数チェックタイプ
        /// </summary>
        DEC_NUM = 3,
        /// <summary>
        /// 日付チェックタイプ
        /// </summary>
        DATE = 4
    }
}
