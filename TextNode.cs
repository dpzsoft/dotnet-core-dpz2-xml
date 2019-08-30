using System;
using System.Collections.Generic;
using System.Text;

namespace dpz2.Xml {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class TextNode : BasicNode {

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public TextNode() : base(NodeType.Text) { }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterXml() {
            return this.Value;
        }

        /// <summary>
        /// 获取内置文本
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerText() {
            return this.Value;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Value = null;

            base.OnDispose();
        }

    }
}
