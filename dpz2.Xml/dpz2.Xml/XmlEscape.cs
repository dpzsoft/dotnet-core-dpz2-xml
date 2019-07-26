using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpz2.Xml {

    /// <summary>
    /// HTML转义
    /// </summary>
    public class XmlEscape {

        /// <summary>
        /// 获取HTML转码后的序列化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str) {
            string res = str;
            res = res.Replace("&", "&amp;");//处理特殊输入
            //res = res.Replace("\r", "").Replace("\n", "&enter;");//处理换行
            res = res.Replace("\"", "&quot;");//处理双引号
            //res = res.Replace(" ", "&nbsp;");//处理空格
            res = res.Replace("<", "&lt;");//处理小于号
            res = res.Replace(">", "&gt;");//处理大于号
            res = res.Replace("'", "&apos;");//处理单引号
            return res;
        }

        /// <summary>
        /// 获取HTML反转码后的序列化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string str) {
            string res = str;
            //res = res.Replace("&enter;", "\r\n");//处理换行
            res = res.Replace("&quot;", "\"");//处理双引号
            res = res.Replace("&apos;", "'");//处理双引号
            //res = res.Replace("&nbsp;", " ");//处理空格
            res = res.Replace("&lt;", "<");//处理小于号
            res = res.Replace("&gt;", ">");//处理大于号
            res = res.Replace("&amp;", "&");//处理特殊输入
            return res;
        }

    }
}
