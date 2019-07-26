using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpz2.Xml {
    internal class XmlParser {

        public enum XMLPartTypes { None, NodeName, CData, PropertyName, PropertyValue, Note }

        private XmlNode gObj;
        private XmlNode dp;

        XMLPartTypes csNow;

        private int nLine;
        private int nIndex;

        public XmlParser(XmlNode obj) {
            gObj = obj;
            nLine = 0;
            nIndex = 0;
        }

        /// <summary>
        /// 返回父对象
        /// </summary>
        private void BackToParent() {
            if (dp == gObj) {
                //throw new Exception("多余的关键字!");
                dp = null;
            } else {
                dp = dp.Parent;
                //dp = dp.Parent;
            }
        }

        /// <summary>
        /// 设置XML子对象
        /// </summary>
        /// <param name="szXml"></param>
        public void Resolve(string szXml) {

            dp = gObj;
            XMLPartTypes xpt = XMLPartTypes.None;

            string szName = "";
            //bool bName = false;

            string szPName = "";
            //bool bPName = false;

            string szPValue = "";
            //bool bPValue = false;

            string szValue = "";
            //bool bCData = false;

            bool bChrAllow = true;

            int nLine = 1;
            int nSite = 0;

            //try {
            for (int i = 0; i < szXml.Length; i++) {

                nSite++;

                char chr = szXml[i];

                switch (chr) {
                    case '<':
                        #region [=====左尖括号=====]

                        switch (xpt) {
                            case XMLPartTypes.None:
                                if (szValue != "") {
                                    //dp["Children"].Add("", szValue);//添加一个无名称属性，既纯字符串
                                    //XMLObject xml = new XMLObject();
                                    XmlNode xup = dp.AddText(XmlEscape.Decode(szValue));
                                    //xup.Value = DecodeXml(szValue);
                                    szValue = "";//清空字符串缓存
                                }

                                //bName = true;//设置为节点名称
                                bChrAllow = true;//设置为允许字符输入
                                xpt = XMLPartTypes.NodeName;//设置为节点名称模式
                                break;
                            case XMLPartTypes.CData:
                                szValue += chr;
                                break;
                            default:
                                throw new Exception("意外的\"<\"字符,节点尚未结束!");
                        }

                        break;
                    #endregion
                    case '>':
                        #region [=====右尖括号=====]

                        switch (xpt) {
                            case XMLPartTypes.NodeName:
                                if (szName == "") throw new Exception("缺少节点名称!");

                                //节点名称以!开头为忽略项目
                                if (szName.StartsWith("!")) {
                                    xpt = XMLPartTypes.None;
                                    szName = "";
                                    break;
                                }

                                if (szName.StartsWith("/")) {
                                    szName = szName.Substring(1);
                                    if (dp.Name == szName) {
                                        //System.Windows.Forms.MessageBox.Show(dp.ToXMLString());
                                        //dp = (XMLObject)dp.Parent;//结束本节点的读取，返回上一个节点
                                        BackToParent();//结束本节点的读取，返回上一个节点
                                    } else {
                                        throw new Exception("节点<" + dp.Name + ">必须以</" + dp.Name + ">来闭合，如果为独立节点，也可以使用<" + dp.Name + " />这样的书写方式!");
                                    }
                                } else {
                                    XmlNode xup = dp.Add(szName);
                                    //XMLObject xup = dp.AppendChild(szName);
                                    //dp = dp["Children"].Add(szName);//添加一个新节点并作为当前的工作节点
                                    dp = xup;
                                }

                                szName = "";//清空节点名称缓存
                                xpt = XMLPartTypes.None;//设置当前段为无模式
                                break;
                            case XMLPartTypes.CData:
                                if (szValue.EndsWith("]]")) {
                                    szValue = szValue.Substring(0, szValue.Length - 2);
                                    //dp["Children"].Add("!CDATA", szValue);//添加一个CDATA段
                                    //XMLObject xup = dp.AppendChild("!CDATA");
                                    //xup.Value = DecodeXml(szValue);
                                    dp.AddData(szValue);
                                    szValue = "";//清空CDATA段缓存
                                    xpt = XMLPartTypes.None;//设置当前段为无模式
                                } else {
                                    szValue += ">";
                                }
                                break;
                            case XMLPartTypes.PropertyName:
                                if (szPName == "/") {
                                    //bName = false;//设置为非节点名称
                                    dp.IsSingleNode = true;
                                    //System.Windows.Forms.MessageBox.Show(dp.ToXMLString());
                                    //dp = (XMLObject)dp.Parent;//结束本节点的读取，返回上一个节点
                                    BackToParent();
                                    xpt = XMLPartTypes.None;//设置当前段为无模式
                                    szPName = "";
                                } else {
                                    if (szPName != "") {
                                        if (szPName == "?") {
                                            if (dp.Name == "?xml") {
                                                dp.IsSingleNode = true;
                                                //dp = (XMLObject)dp.Parent;//结束本节点的读取，返回上一个节点
                                                BackToParent();//
                                                xpt = XMLPartTypes.None;//设置当前段为无模式
                                                szPName = "";
                                            } else {
                                                throw new Exception("意外的\">\"字符，节点尚未结束!");
                                            }
                                        } else {
                                            throw new Exception("意外的\">\"字符，节点尚未结束!");
                                        }
                                    } else {
                                        xpt = XMLPartTypes.None;
                                    }
                                    //throw new Exception("意外的\">\"字符，节点尚未结束!");
                                }
                                break;
                            case XMLPartTypes.Note://结束注释模式
                                xpt = XMLPartTypes.None;
                                break;
                            case XMLPartTypes.PropertyValue:
                                if (bChrAllow) {
                                    throw new Exception("意外的\">\"字符，节点尚未结束!");
                                } else {
                                    //bPValue = false;//设置为非属性值
                                    //dp["Properties"].Add(szPName, szValue);//设置当前对象的属性信息
                                    dp.Attr.Add(szPName, szPValue);
                                    szPName = "";//清空属性名称缓存
                                    szPValue = "";//清空属性值缓存
                                    xpt = XMLPartTypes.None;//设置当前段为无模式
                                    bChrAllow = true;//后面允许增加字符
                                }
                                break;
                            default:
                                throw new Exception("意外的\">\"字符，节点尚未结束!");
                        }

                        break;
                    #endregion
                    case '/':
                        #region [=====斜杠处理=====]

                        switch (xpt) {
                            case XMLPartTypes.CData:
                                //CDATA段
                                szValue += chr;
                                break;
                            case XMLPartTypes.NodeName:
                                if (szName != "") throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                szName += chr;
                                //bChrAllow = false;//后面允许增加字符
                                break;
                            case XMLPartTypes.PropertyName:
                                if (szPName != "") throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                szPName += chr;
                                bChrAllow = false;//后面允许增加字符
                                break;
                            case XMLPartTypes.PropertyValue:
                                if (bChrAllow) {
                                    //bChrAllow = false;
                                    szPValue += chr;
                                } else {
                                    dp.Attr.Add(szPName, szPValue);
                                    szPName = "/";//清空属性名称缓存
                                    szPValue = "";//清空属性值缓存
                                    xpt = XMLPartTypes.PropertyName;//设置当前段为无模式
                                    bChrAllow = false;//后面允许增加字符
                                }
                                break;
                            case XMLPartTypes.Note:
                                break;
                            case XMLPartTypes.None:
                                szValue += chr;
                                break;
                            default:
                                throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                //break;
                        }

                        break;
                    #endregion
                    case ' ':
                        #region [=====空格处理=====]

                        switch (xpt) {
                            case XMLPartTypes.CData:
                                //CDATA段
                                szValue += chr;
                                break;
                            case XMLPartTypes.NodeName:
                                if (szName == "") throw new Exception("缺少节点名称!");

                                //节点名称以叹号开头个，为注释
                                if (szName.StartsWith("!")) {
                                    xpt = XMLPartTypes.Note;
                                    szName = "";
                                    break;
                                }

                                //XMLObject xup = dp.AppendChild(szName);
                                XmlNode xup = dp.Add(szName);
                                dp = xup;
                                //xup.Value = szValue;
                                //dp = dp["Children"].Add(szName);//添加一个新节点并作为当前的工作节点
                                szName = "";//清空节点名称缓存
                                xpt = XMLPartTypes.PropertyName;//设置当前段为属性名称模式
                                break;
                            case XMLPartTypes.PropertyName:
                                if (szPName != "") {
                                    xpt = XMLPartTypes.None;//设置当前段为无模式
                                } else {
                                    bChrAllow = false;
                                }
                                break;
                            case XMLPartTypes.PropertyValue:
                                if (bChrAllow) {
                                    szPValue += " ";//将空格填充到属性字符串中
                                } else {
                                    //bPValue = false;//设置为非属性值
                                    //dp["Properties"].Add(szPName, szValue);//设置当前对象的属性信息
                                    dp.Attr.Add(szPName, szPValue);
                                    szPName = "";//清空属性名称缓存
                                    szPValue = "";//清空属性值缓存
                                    xpt = XMLPartTypes.PropertyName;//设置当前段为无模式
                                    bChrAllow = true;//后面允许增加字符
                                }
                                break;
                            default:
                                //接受普通字符串
                                //szValue += chr;
                                break;
                        }

                        break;
                    #endregion
                    case '=':
                        #region [====等号处理=====]

                        switch (xpt) {
                            case XMLPartTypes.CData:
                                //CDATA段
                                szValue += chr;
                                break;
                            case XMLPartTypes.NodeName:
                                //接收节点名称
                                //szName += chr;
                                throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                            //break;
                            case XMLPartTypes.PropertyName:
                                if (szPName == "") throw new Exception("缺少属性名称!");
                                xpt = XMLPartTypes.PropertyValue;////设置当前段为属性值模式
                                bChrAllow = false;
                                break;
                            case XMLPartTypes.PropertyValue:
                                //接受属性值
                                if (!bChrAllow) throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                szPValue += chr;//将字符填充到属性字符串中
                                break;
                            default:
                                //接受普通字符串
                                if (!bChrAllow) throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                szValue += chr;
                                //throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                break;
                        }

                        break;
                    #endregion
                    case '"':
                        #region [=====引号处理=====]

                        switch (xpt) {
                            case XMLPartTypes.CData:
                                //CDATA段
                                szValue += chr;
                                break;
                            case XMLPartTypes.NodeName:
                            case XMLPartTypes.PropertyName:
                                throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                            case XMLPartTypes.PropertyValue:
                                if (bChrAllow) {
                                    bChrAllow = false;
                                } else {
                                    if (szPValue == "") {
                                        //bChrAllow = !bChrAllow;//允许输入字符
                                        bChrAllow = true;
                                    } else {
                                        //bPValue = false;//停止输入属性值
                                        //bChrAllow = false;//不允许之后出现字符
                                        //xpt = XMLPartTypes.PropertyName;
                                        throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                    }
                                }
                                break;
                            case XMLPartTypes.Note:
                                break;
                            default:
                                //接受普通字符串
                                szValue += chr;
                                //throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                break;
                        }

                        break;
                    #endregion
                    case '\r':
                        break;
                    case '\n':
                        #region [=====换行处理=====]
                        switch (xpt) {
                            case XMLPartTypes.None:
                            case XMLPartTypes.Note:
                                break;
                            case XMLPartTypes.CData:
                                szValue += chr;
                                break;
                            default:
                                throw new Exception("换行时节点尚未结束!");
                        }
                        nLine++;
                        nSite = 0;
                        break;
                    #endregion
                    case '[':
                        #region [=====右中括号=====]

                        switch (xpt) {
                            case XMLPartTypes.NodeName:
                                if (szName == "!") {
                                    szName += "[";
                                } else if (szName == "![CDATA") {
                                    //bName = false;//设置为非节点名称
                                    //bCData = true;//设置为CDATA段
                                    szName = "";//清空节点名称缓存
                                    xpt = XMLPartTypes.CData;
                                } else {
                                    throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                                }
                                break;
                            case XMLPartTypes.PropertyValue:
                                szPValue += "[";
                                break;
                            case XMLPartTypes.CData:
                                szValue += "[";
                                break;
                            default:
                                throw new Exception("出现意外的格式外字符\"" + chr + "\"");
                        }

                        break;
                    #endregion
                    default:
                        #region [=====普通字符处理=====]

                        if (!bChrAllow) throw new Exception("出现意外的格式外字符\"" + chr + "\"");

                        switch (xpt) {
                            case XMLPartTypes.CData:
                                //CDATA段
                                szValue += chr;
                                break;
                            case XMLPartTypes.NodeName:
                                //接收节点名称
                                szName += chr;
                                break;
                            case XMLPartTypes.PropertyName:
                                //接受属性名称
                                szPName += chr;
                                break;
                            case XMLPartTypes.PropertyValue:
                                //接受属性值
                                szPValue += chr;
                                break;
                            case XMLPartTypes.Note://注释模式不进行任何处理
                                break;
                            default:
                                //接受普通字符串
                                szValue += chr;
                                break;
                        }

                        break;
                        #endregion
                }

            }
            //} catch (Exception ex) {
            //    throw new Exception("第" + nLine + "行,第" + nSite + "个字符解析时发生异常:" + ex.Message + "\r\n当前状态:" + xpt + "\r\n字符模式:" + bChrAllow + "\r\n变量信息:{Name:\"" + szName + "\",Value:\"" + szValue + "\",PName:\"" + szPName + "\",PValue:\"" + szPValue + "\"}");
            //}


            if (xpt != XMLPartTypes.None) throw new Exception("数据不完整!");

            if (szValue != "") {
                dp.AddText(XmlEscape.Decode(szValue));
                //XMLObject xup = dp.AppendChild("");
                //xup.Value = DecodeXml(szValue);
                //dp["Children"].Add("", szValue);//添加一个无名称属性，既纯字符串
                szValue = "";//清空字符串缓存
            }

        }

    }
}
