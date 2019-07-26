using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpz2.Xml {

    /// <summary>
    /// XML节点属性管理器
    /// </summary>
    public class XmlNodeAttributes : dpz2.Object, IDictionary<string, string> {

        private Dictionary<string, string> pairs;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public XmlNodeAttributes() {
            pairs = new Dictionary<string, string>();
        }

        /// <summary>
        /// 获取或设置属性值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get {
                key = key.ToLower();
                if (pairs.ContainsKey(key))
                    return pairs[key];
                return "";
            }
            set {
                this.Set(key, value);
            }
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        public ICollection<string> Keys { get { return pairs.Keys; } }

        /// <summary>
        /// 获取内容集合
        /// </summary>
        public ICollection<string> Values { get { return pairs.Values; } }

        /// <summary>
        /// 获取属性数量
        /// </summary>
        public int Count { get { return pairs.Count; } }

        /// <summary>
        /// 获取是否为只读
        /// </summary>
        public bool IsReadOnly { get { return true; } }

        /// <summary>
        /// 添加一条属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value) {
            //throw new NotImplementedException();
            key = key.ToLower();
            if (pairs.ContainsKey(key)) {
                pairs[key] = value;
                return;
            }
            pairs.Add(key, value);
        }

        /// <summary>
        /// 添加一条属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value) {
            this.Set(key, value);
        }

        /// <summary>
        /// 添加一条属性
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, string> item) {
            //throw new NotImplementedException();
            this.Set(item.Key, item.Value);
        }

        /// <summary>
        /// 清空属性集合
        /// </summary>
        public void Clear() {
            //throw new NotImplementedException();
            pairs.Clear();
        }

        /// <summary>
        /// 获取属性设定是否存在
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, string> item) {
            //throw new NotImplementedException();
            return pairs.Contains(item);
        }

        /// <summary>
        /// 获取属性是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            //throw new NotImplementedException();
            key = key.ToLower();
            return pairs.ContainsKey(key);
        }

        /// <summary>
        /// 复制集合到数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            throw new NotImplementedException();
            //pairs.CopyTo()
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            //throw new NotImplementedException();
            return pairs.GetEnumerator();
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key) {
            //throw new NotImplementedException();
            return pairs.Remove(key);
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, string> item) {
            throw new NotImplementedException();
            //return pairs.Remove(item);
        }

        /// <summary>
        /// 尝试获取内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out string value) {
            //throw new NotImplementedException();
            if (pairs.ContainsKey(key)) {
                value = pairs[key];
                return true;
            }
            value = "";
            return false;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            //throw new NotImplementedException();
            return pairs.GetEnumerator();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            pairs.Clear();
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            //return base.OnParseString();
            string res = "";
            foreach (var item in pairs) {
                res += $" {item.Key}=\"{item.Value}\"";
            }
            return res;
        }
    }
}
