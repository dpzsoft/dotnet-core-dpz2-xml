using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpz2.Xml {

    /// <summary>
    /// XML存储单元
    /// </summary>
    public class XmlNode : dpz2.Object {

        /// <summary>
        /// 获取节点名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取是否为单层节点
        /// </summary>
        public bool IsSingleNode { get; set; }

        /// <summary>
        /// 获取父对象ID
        /// </summary>
        public XmlNode Parent { get; internal set; }

        /// <summary>
        /// 属性管理器
        /// </summary>
        public XmlNodeAttributes Attr { get; private set; }

        /// <summary>
        /// 获取节点集合
        /// </summary>
        public XmlNodeCollection Nodes { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public XmlNode(string name = "") {
            this.Name = name;
            this.Nodes = new XmlNodeCollection(this);
            this.Attr = new XmlNodeAttributes();
            this.IsSingleNode = false;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public XmlNode this[int index] { get { return this.Nodes[index]; } }

        /// <summary>
        /// 获取第一个满足条件的子节点,如不存在则创建
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlNode this[string name] {
            get {
                XmlNode xmlNode = this.Nodes.GetFirstNodeByName(name);
                if (xmlNode == null) xmlNode = this.Nodes.Add(name);
                return xmlNode;
            }
        }

        /// <summary>
        /// 添加XML对象
        /// </summary>
        /// <param name="name"></param>
        public XmlNode Add(string name) {
            return this.Nodes.Add(name);
        }

        /// <summary>
        /// 添加文本数据
        /// </summary>
        /// <param name="text"></param>
        public XmlNode AddText(string text) {
            XmlNode xmlNode = new XmlNode();
            xmlNode.Attr["type"] = "text";
            xmlNode.Attr["text"] = text;
            this.Nodes.Add(xmlNode);
            return this;
        }

        /// <summary>
        /// 添加Data数据
        /// </summary>
        /// <param name="data"></param>
        public XmlNode AddData(string data) {
            XmlNode xmlNode = new XmlNode();
            xmlNode.Attr["type"] = "data";
            xmlNode.Attr["data"] = data;
            this.Nodes.Add(xmlNode);
            return this;
        }

        /// <summary>
        /// 获取或设置所属XML内容
        /// </summary>
        public string InnerXml {
            get { return this.Nodes.ToString(); }
            set {
                this.Nodes.ClearWithDispose();
                XmlParser xmlParser = new XmlParser(this);
                xmlParser.Resolve(value);
            }
        }

        //在对象中查找满足条件的子节点
        internal void FindNodesByName(XmlNodeCollection nodes, string name, bool isSearchChildNodes = true) {
            foreach (var node in this.Nodes) {
                if (node.Name.ToLower() == name) nodes.Add(node);
                if (isSearchChildNodes) node.FindNodesByName(nodes, name);
            }
        }

        //在对象中查找满足条件的子节点
        internal void FindNodesByAttr(XmlNodeCollection nodes, string key, string value, bool isSearchChildNodes = true) {
            foreach (var node in this.Nodes) {
                if (node.Attr.ContainsKey(key)) {
                    if (node.Attr[key] == value) nodes.Add(node);
                }
                if (isSearchChildNodes) node.FindNodesByAttr(nodes, key, value);
            }
        }

        /// <summary>
        /// 根据属性值获取节点集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isSearchChildNodes">是否搜索子节点</param>
        /// <returns></returns>
        public XmlNodeCollection GetNodesByAttrValue(string key, string value, bool isSearchChildNodes = true) {
            XmlNodeCollection nodes = new XmlNodeCollection();
            key = key.ToLower();
            FindNodesByAttr(nodes, key, value, isSearchChildNodes);
            return nodes;
        }

        /// <summary>
        /// 根据属性值获取单个节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isSearchChildNodes">是否搜索子节点</param>
        /// <returns></returns>
        public XmlNode GetNodeByAttrValue(string key, string value, bool isSearchChildNodes = true) {
            foreach (var node in this.Nodes) {
                if (node.Attr.ContainsKey(key)) {
                    if (node.Attr[key] == value) return node;
                }
                if (isSearchChildNodes) {
                    XmlNode xmlNode = node.GetNodeByAttrValue(key, value);
                    if (xmlNode != null) return xmlNode;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据名称获取节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isSearchChildNodes">是否搜索子节点</param>
        /// <returns></returns>
        public XmlNodeCollection GetNodesByName(string name, bool isSearchChildNodes = true) {
            XmlNodeCollection nodes = new XmlNodeCollection();
            name = name.ToLower();
            FindNodesByName(nodes, name, isSearchChildNodes);
            return nodes;
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            //return base.OnParseString();
            if (this.Name == "") {
                //普通文本
                if (this.Attr["type"] == "text") {
                    return XmlEscape.Encode(this.Attr["text"]);
                } else if (this.Attr["type"] == "data") {
                    return $"<![CDATA[{this.Attr["data"]}]]>";
                } else {
                    throw new Exception("未知数据格式");
                }
            } else {
                if (this.IsSingleNode) {
                    return $"<{this.Name}{this.Attr.ToString()} />";
                } else {
                    return $"<{this.Name}{this.Attr.ToString()}>{this.Nodes.ToString()}</{this.Name}>";
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            this.Name = null;
            this.Attr.Dispose();
            this.Nodes.ClearWithDispose();
            this.Nodes.Dispose();
        }

    }
}
