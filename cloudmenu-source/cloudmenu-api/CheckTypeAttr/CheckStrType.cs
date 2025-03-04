
#nullable disable

namespace cloudmenu_api.CheckTypeAttr
{
    /// <summary>
    /// 文字列チェックタイプ分類
    /// </summary>
    public enum CheckStrType
    {
        /// <summary>
        /// 最小桁数チェック
        /// </summary>
        MIN_NUM,
        /// <summary>
        /// 最大桁数チェック
        /// </summary>
        MAX_NUM,
        /// <summary>
        /// 数字チェック
        /// </summary>
        NUMBER,
        /// <summary>
        /// 英字チェック
        /// </summary>
        ENG_TEXT,
        /// <summary>
        /// 英数字チェック
        /// </summary>
        ENG_NUM,
        /// <summary>
        /// 全角チェック
        /// </summary>
        ZENKAKU,
        /// <summary>
        /// 電話番号チェック
        /// </summary>
        TEL,
        /// <summary>
        /// メールアドレスチェック
        /// </summary>
        MAILADDRESS,
        /// <summary>
        /// 郵便番号チェック
        /// </summary>
        ZIP_CODE,
        /// <summary>
        /// 正規表現チェック
        /// </summary>
        REGEX
    }
}
