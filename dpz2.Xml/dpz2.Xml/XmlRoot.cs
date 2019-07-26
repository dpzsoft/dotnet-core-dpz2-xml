using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpz2.Xml {

    /// <summary>
    /// XML根节点
    /// </summary>
    public class XmlRoot : XmlNode {

        private new XmlNodeAttributes Attr { get { return base.Attr; } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public XmlRoot() : base("") { }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public XmlRoot(string xml) : base("") {
            base.InnerXml = xml;
            base.Parent = null;
            //base.ParentUnitID = 0;
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            //return base.OnParseString();
            string res = "";
            if (this.Attr.Count > 0) {
                res = $"<{this.Attr.ToString()}>";
            }
            res += this.Nodes.ToString();
            return res;
        }

    }
}
