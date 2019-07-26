using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpz2.Xml {

    /// <summary>
    /// XML对象集合
    /// </summary>
    public class XmlNodeCollection : dpz2.Object, IList<XmlNode> {

        private List<XmlNode> list;
        private XmlNode parent;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="id"></param>
        public XmlNodeCollection(XmlNode node = null) {
            parent = node;
            list = new List<XmlNode>();
        }

        /// <summary>
        /// 获取或设置XML对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public XmlNode this[int index] { get { return list[index]; } set { list[index] = value; } }

        /// <summary>
        /// 获取集合元素数量
        /// </summary>
        public int Count { get { return list.Count; } }

        /// <summary>
        /// 获取只读属性
        /// </summary>
        public bool IsReadOnly { get { return true; } }

        /// <summary>
        /// 添加XML对象
        /// </summary>
        /// <param name="item"></param>
        public void Add(XmlNode item) {
            //throw new NotImplementedException();
            if (parent != null) item.Parent = parent;
            list.Add(item);
        }

        /// <summary>
        /// 添加XML对象
        /// </summary>
        /// <param name="name"></param>
        public XmlNode Add(string name = "") {
            //throw new NotImplementedException();
            XmlNode xmlNode = new XmlNode(name);
            this.Add(xmlNode);
            return xmlNode;
        }

        /// <summary>
        /// 清理所属元素
        /// </summary>
        public void Clear() {
            //throw new NotImplementedException();
            list.Clear();
        }

        /// <summary>
        /// 清理所属元素
        /// </summary>
        public void ClearWithDispose() {
            //throw new NotImplementedException();
            foreach (var item in list) {
                item.Dispose();
            }
            list.Clear();
        }

        /// <summary>
        /// 获取是否存在相关项目
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(XmlNode item) {
            //throw new NotImplementedException();
            return list.Contains(item);
        }

        /// <summary>
        /// 拷贝到数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(XmlNode[] array, int arrayIndex) {
            //throw new NotImplementedException();
            list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        public IEnumerator<XmlNode> GetEnumerator() {
            //throw new NotImplementedException();
            return list.GetEnumerator();
        }

        /// <summary>
        /// 获取元索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(XmlNode item) {
            //throw new NotImplementedException();
            return list.IndexOf(item);
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, XmlNode item) {
            //throw new NotImplementedException();
            if (parent != null) item.Parent = parent;
            list.Insert(index, item);
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(XmlNode item) {
            //throw new NotImplementedException();
            return list.Remove(item);
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) {
            //throw new NotImplementedException();
            list.RemoveAt(index);
        }

        /// <summary>
        /// 根据名称获取第一个子节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlNode GetFirstNodeByName(string name) {
            name = name.ToLower();
            foreach (var node in this) {
                if (node.Name.ToLower() == name) {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            //throw new NotImplementedException();
            return list.GetEnumerator();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            list.Clear();
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            //return base.OnParseString();
            string res = "";
            foreach (var item in list) {
                res += item.ToString();
            }
            return res;
        }
    }
}
