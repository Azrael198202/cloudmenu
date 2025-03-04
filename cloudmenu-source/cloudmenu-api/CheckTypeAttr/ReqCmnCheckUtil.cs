using System.Reflection;
using System;
using cloudmenu_api.ResponseModels;
using System.Text.RegularExpressions;

namespace cloudmenu_api.CheckTypeAttr
{
    /// <summary>
    /// チェック処理
    /// </summary>
    public class ReqCmnCheckUtil
    {
        static DateTime dateValue;
        public static bool doCustomCheck(Object obj)
        {
            var props = obj.GetType().GetRuntimeProperties();
            foreach (PropertyInfo prop in props)
            {
                ReqCheckAttr dispNameAttr = Attribute.GetCustomAttribute(prop, typeof(ReqCheckAttr)) as ReqCheckAttr;

                if (dispNameAttr != null)
                {
                    //　必須チェック処理
                    if (dispNameAttr.require)
                    {
                        bool reqChRes = RequireCheck(prop.GetValue(obj).ToString());
                        //　空値の場合
                        if (!reqChRes)
                        {
                            return false;
                        }
                    }
                    //　文字列チェック処理
                    if (dispNameAttr.itemType.Equals(ItemCheckType.STR))
                    {
                        bool strChRes = StrCheck(prop.GetValue(obj).ToString(), dispNameAttr);
                        // 文字列チェック結果がFALSEの場合
                        if (!strChRes)
                        {
                            return false;
                        }
                    }
                    //　整数チェック処理
                    if (dispNameAttr.itemType.Equals(ItemCheckType.INT_NUM))
                    {
                        bool intChRes = IntNumCheck(prop.GetValue(obj).ToString(), dispNameAttr);
                        //　整数チェック結果がFALSEの場合
                        if (!intChRes)
                        {
                            return false;
                        }
                    }
                    //　小数チェック処理
                    if (dispNameAttr.itemType.Equals(ItemCheckType.DEC_NUM))
                    {
                        bool decChRes = DecNumCheck(prop.GetValue(obj).ToString(), dispNameAttr);
                        //　小数チェック結果がFALSEの場合
                        if (!decChRes)
                        {
                            return false;
                        }
                    }
                    //　日付チェック処理
                    if (dispNameAttr.itemType.Equals(ItemCheckType.DATE))
                    {
                        bool dateChRes = DateCheck(prop.GetValue(obj).ToString());
                        //　日付チェック結果がFALSEの場合
                        if (!dateChRes)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #region 必須チェック処理
        /// <summary>
        /// 必須チェック処理
        /// </summary>
        public static bool RequireCheck(string requireValue)
        {
            if (requireValue == "")
                return false;
            return true;
        }
        #endregion
        #region 文字列チェック処理
        /// <summary>
        /// 文字列チェック処理
        /// </summary>
        public static bool StrCheck(string requireValue, ReqCheckAttr ReqCheck)
        {
            // 最小桁数チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.MIN_NUM))
            {
                if (requireValue.Length < ReqCheck.Min_Num)
                    return false;
            }
            // 最大桁数チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.MAX_NUM))
            {
                if (requireValue.Length > ReqCheck.Max_Num)
                    return false;
            }
            // 数字チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.NUMBER))
            {
                string pattern = "^[０-９0-9]*$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            // 英字チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.ENG_TEXT))
            {
                string pattern = "^[A-Za-z0-9]*$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            // 英数字チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.ENG_NUM))
            {
                string pattern = "^[0-9]*$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            // 全角チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.ZENKAKU))
            {
                string pattern = "^[\x01-\x7E\uFF65-\uFF9F]*$";
                Regex regex = new Regex(pattern);

                if (regex.IsMatch(requireValue))
                    return false;
            }
            // 電話番号チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.TEL))
            {
                string pattern = @"^0\d{1,4}-\d{1,4}-\d{4}$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            // メールアドレスチェック処理
            if (ReqCheck.strType.Equals(CheckStrType.MAILADDRESS))
            {
                string pattern = @"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            // 郵便番号チェック処理
            if (ReqCheck.strType.Equals(CheckStrType.ZIP_CODE))
            {
                string pattern = "^[0-9]{3}-[0-9]{4}$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            // 正規表現でチェック処理
            if (ReqCheck.strType.Equals(CheckStrType.REGEX))
            {
                string pattern = ReqCheck.RegexPattern;
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(requireValue))
                    return false;
            }
            return true;
        }
        #endregion
        #region 　整数チェック処理
        /// <summary>
        /// 　整数チェック処理
        /// </summary>
        public static bool IntNumCheck(string requireValue, ReqCheckAttr ReqCheck)
        {
            // 最小値チェック処理
            if (ReqCheck.IntNumType.Equals(CheckIntNumType.MIN_VALUE))
            {
                if (double.Parse(requireValue) < ReqCheck.Min_Value)
                    return false;
            }
            // 最大値チェック処理
            if (ReqCheck.IntNumType.Equals(CheckIntNumType.MAX_VALUE))
            {
                if (double.Parse(requireValue) > ReqCheck.Max_Value)
                    return false;
            }
            return true;
        }
        #endregion
        #region 　小数チェック処理
        /// <summary>
        /// 　小数チェック処理
        /// </summary>
        public static bool DecNumCheck(string requireValue, ReqCheckAttr ReqCheck)
        {
            // 最小値チェック処理
            if (ReqCheck.DecNumType.Equals(CheckDecNumType.MIN_VALUE))
            {
                if (double.Parse(requireValue) < ReqCheck.Min_Value)
                    return false;
            }
            // 最大値チェック処理
            if (ReqCheck.DecNumType.Equals(CheckDecNumType.MAX_VALUE))
            {
                if (double.Parse(requireValue) > ReqCheck.Max_Value)
                    return false;
            }
            // 整数有効桁数(小数)チェック処理
            if (ReqCheck.DecNumType.Equals(CheckDecNumType.SEI_YUKOU_KETASU))
            {
                int SeiKetasu = requireValue.Contains(".") ? requireValue.IndexOf(".") : requireValue.Length;
                if (!SeiKetasu.Equals(ReqCheck.SeiYuKou_Ketasu))
                    return false;
            }
            // 小数有効桁数(小数)チェック処理
            if (ReqCheck.DecNumType.Equals(CheckDecNumType.SHOU_YUKOU_KETASU))
            {
                //　小数有効桁数(小数)を取得
                int ShouKetasu = requireValue.Contains(".") ? requireValue.Length - requireValue.IndexOf(".") - 1 : 0;
                if (!ShouKetasu.Equals(ReqCheck.ShouYuKou_Ketasu))
                    return false;
            }
            return true;
        }
        #endregion
        #region 日付チェック処理
        /// <summary>
        /// 日付チェック処理
        /// </summary>
        public static bool DateCheck(string requireValue)
        {
            if (!DateTime.TryParse(requireValue, out dateValue))
                return false;
            return true;
        }
        #endregion
    }
}