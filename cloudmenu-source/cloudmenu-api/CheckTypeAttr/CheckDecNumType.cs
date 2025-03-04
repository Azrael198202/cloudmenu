
#nullable disable

namespace cloudmenu_api.CheckTypeAttr
{
    /// <summary>
    /// 小数チェックタイプ分類
    /// </summary>
    public enum CheckDecNumType
    {
        /// <summary>
        /// 最小値チェック
        /// </summary>
        MIN_VALUE,
        /// <summary>
        /// 最大値チェック
        /// </summary>
        MAX_VALUE,
        /// <summary>
        /// 整数有効桁数チェック
        /// </summary>
        SEI_YUKOU_KETASU,
        /// <summary>
        /// 小数有効桁数チェック
        /// </summary>
        SHOU_YUKOU_KETASU
    }
}
