using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Web;

//using DiffPlex;
//using DiffPlex.DiffBuilder;
//using DiffPlex.DiffBuilder.Model;
using DigitalPlatform.Xml;
using DigitalPlatform.Text;


namespace DigitalPlatform.Marc
{
    public class MarcDiff
    {
        public static string SEP = "<td class='sep'></td>";
        // 根据字段权限定义过滤出允许的内容
        // return:
        //      -1  出错
        //      0   成功
        //      1   有部分字段被修改或滤除
        public static int FilterFields(string strFieldNameList,
            ref string strMarc,
            out string strError)
        {
            strError = "";
            int nRet = 0;
            bool bChanged = false;

            if (string.IsNullOrEmpty(strMarc) == true)
                return 0;

            string strHeader = "";
            string strBody = "";

            SplitMarc(strMarc,
                out strHeader,
                out strBody);

            if (strHeader.Length < 24)
            {
                strHeader = strHeader.PadRight(24, '?');
                bChanged = true;
            }

            FieldNameList list = new FieldNameList();
            nRet = list.Build(strFieldNameList, out strError);
            if (nRet == -1)
            {
                strError = "字段权限定义 '"+strFieldNameList+"' 不合法: " + strError;
                return -1;
            }

            StringBuilder text = new StringBuilder(4096);
            if (list.Contains("###") == true)
                text.Append(strHeader);
            else
            {
                bChanged = true;
                text.Append(new string('?', 24));
            }

            string[] fields = strBody.Split(new char[] {(char)30}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in fields)
            {
                if (string.IsNullOrEmpty(s) == true)
                    continue;

                string strFieldName = GetFieldName(s);
                if (list.Contains(strFieldName) == false)
                {
                    bChanged = true;
                    continue;
                }

                text.Append(s);
                text.Append((char)30);
            }

            strMarc = text.ToString();
            if (bChanged == true)
                return 1;
            return 0;
        }

        // 末尾是否有内码为 1 的字符？
        static bool HasMask(string strText)
        {
            if (string.IsNullOrEmpty(strText) == true)
                return false;
            if (strText[strText.Length - 1] == (char)1)
                return true;
            return false;
        }

        // 去掉末尾的内码为 1 的字符
        static string RemoveMask(string strText)
        {
            if (HasMask(strText) == true)
                return strText.Substring(0, strText.Length - 1);
            return strText;
        }



        // 获得一段文本前三个字符，作为字段名
        static string GetFieldName(string strText)
        {
            if (string.IsNullOrEmpty(strText) == true)
                return null;

            if (strText.Length >= 3)
                return strText.Substring(0, 3);
            return strText.PadRight(3, '?');
        }

        // 把一个 MARC 记录拆分为 头标区 和 字段区 两个部分
        static void SplitMarc(string strMARC,
            out string strHeader,
            out string strBody)
        {
            strHeader = "";
            strBody = "";

            if (String.IsNullOrEmpty(strMARC) == true)
                return;

            // 整理尾部字符
            char tail = strMARC[strMARC.Length - 1];
            if (tail == 29)
            {
                strMARC = strMARC.Substring(0, strMARC.Length - 1);

                if (String.IsNullOrEmpty(strMARC) == true)
                    return;
            }

            if (strMARC.Length < 24)
            {
                strHeader = strMARC;
                strHeader = strHeader.PadRight(24, '?');
                return;
            }

            strHeader = strMARC.Substring(0, 24);

            // 2021/4/12
            // 将两个地址部分替换为问号
            MarcHeader header = new MarcHeader();
            header[0, 24] = strHeader;
            header.reclen = "?????";
            header.baseaddr = "?????";
            strHeader = header.ToString();

            strBody = strMARC.Substring(24);
        }



        static string GetImageHtml(string strImageFragment)
        {
            return "\r\n<td class='content' colspan='3'>"    //  
                + strImageFragment
                + "</td>";
        }



        public static string GetIndentXml(string strXml)
        {
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strXml);
            }
            catch // (Exception ex)
            {
                return strXml;
            }

            return DomUtil.GetIndentXml(dom.DocumentElement);
        }

        public static string GetIndentInnerXml(string strXml)
        {
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strXml);
            }
            catch // (Exception ex)
            {
                return strXml;
            }

            return DomUtil.GetIndentInnerXml(dom.DocumentElement);
        }





        // 根据 XML 字符串，构造出第一级子元素字符串列表
        static int GetComparableXmlString(string strXml,
            out string strChildrenText,
            out List<XmlNode> childnodes,
            out string strElementName,
            out string strRootBegin,
            out string strRootEnd)
        {
            strChildrenText = "";
            strRootBegin = "";
            strRootEnd = "";
            childnodes = new List<XmlNode>();
            strElementName = "";

            if (string.IsNullOrEmpty(strXml) == true)
                return 0;

            XmlDocument dom = new XmlDocument();
            dom.PreserveWhitespace = false;
#if NO
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                return -1;
            }
#endif
            dom.LoadXml(strXml);    // 可能会抛出异常

            StringBuilder result = new StringBuilder(4096);
            foreach (XmlNode node in dom.DocumentElement.ChildNodes)
            {
                string strText = node.OuterXml.Replace("\r\n", "").Trim();
                if (string.IsNullOrEmpty(strText) == false)
                {
                    if (result.Length > 0)
                        result.Append("\r\n");
                    result.Append(strText);
                    childnodes.Add(node);
                }
            }

            // TODO: 遇到 <record> 元素要跳过
            strChildrenText = result.ToString();
            strRootBegin = GetElementBeginString(dom.DocumentElement);
            strRootEnd = GetElementEndString(dom.DocumentElement);
            strElementName = dom.DocumentElement.Name;
            return 0;
        }

        static string GetElementBeginString(XmlElement element)
        {
            StringBuilder result = new StringBuilder(4096);
            result.Append("<" + element.Name);
            foreach (XmlAttribute attr in element.Attributes)
            {
                result.Append(" " + attr.Name + "=\"" + attr.Value + "\"");
            }
            result.Append(">");
            return result.ToString();
        }

        static string GetElementEndString(XmlElement element)
        {
            return "</" + element.Name + ">";
        }

        // 将前方连续的若干空格字符替换为 &nbsp;
        public static string ReplaceLeadingTab(string strText)
        {
            StringBuilder result = new StringBuilder(4096);
            int i = 0;
            foreach (char c in strText)
            {
                if (c == ' ')
                    result.Append("&nbsp;");
                else 
                {
                    result.Append(strText.Substring(i));
                    break;
                }

                i ++;
            }

            return result.ToString();
        }

        // 创建一个 XML 字段的 HTML 局部 三个 <td>
        static string BuildFragmentFieldHtml(
            int nLevel,
            ChangeType type,
            string strField)
        {
            if (string.IsNullOrEmpty(strField) == true)
                return "\r\n<td class='content' colspan='3'></td>";

            string strTypeClass = "";
            if (type == ChangeType.Modified)
                strTypeClass = " modified";
            else if (type == ChangeType.Inserted)
                strTypeClass = " inserted";
            else if (type == ChangeType.Deleted)
                strTypeClass = " deleted";
            else if (type == ChangeType.Imaginary)
                strTypeClass = " imaginary";
            else if (type == ChangeType.Unchanged)
                strTypeClass = " unchanged";

            string strLevel = "";
            if (nLevel > 0)
                strLevel = strLevel.PadRight(nLevel, ' ').Replace(" ","    ");  // .Replace(" ", "&nbsp;&nbsp;&nbsp;&nbsp;");
            // 
            string[] lines = HttpUtility.HtmlEncode(strField).Replace("\r\n", "\n").Split(new char[] { '\n' });
            StringBuilder result = new StringBuilder(4096);
            foreach (string line in lines)
            {
                if (result.Length > 0)
                    result.Append("<br/>");
                result.Append(ReplaceLeadingTab(strLevel + line));
            }

            return "\r\n<td class='content" + strTypeClass + "' colspan='3'>" + result + "</td>";
        }
        // 创建一个字段的 HTML 局部 三个 <td>
        static string BuildFieldHtml(
            ChangeType type,
            string strField,
            bool bSubfieldReturn = false)
        {
            if (string.IsNullOrEmpty(strField) == true)
                return "\r\n<td class='content' colspan='3'></td>";

            string strLineClass = "";
            string strFieldName = "";
            string strIndicatior = "";
            string strContent = "";

            // 取字段名
            if (strField.Length < 3)
            {
                strFieldName = strField;
                strField = "";
            }
            else
            {
                strFieldName = strField.Substring(0, 3);
                strField = strField.Substring(3);
            }

            // 取指示符
            if (MarcUtil.IsControlFieldName(strFieldName) == true)
            {
                strLineClass = "controlfield";
                strField = strField.Replace(' ', '_');
            }
            else
            {
                if (strField.Length < 2)
                {
                    strIndicatior = strField;
                    strField = "";
                }
                else
                {
                    strIndicatior = strField.Substring(0, 2);
                    strField = strField.Substring(2);
                }
                strIndicatior = strIndicatior.Replace(' ', '_');

                strLineClass = "datafield";

                // 1XX字段有定长内容
                if (strFieldName.Length >= 1 && strFieldName[0] == '1')
                {
                    strField = strField.Replace(' ', '_');
                    strLineClass += " fixedlengthsubfield";
                }
            }

            strContent = MarcUtil.GetHtmlFieldContent(strField,
    bSubfieldReturn);

            string strTypeClass = "";
            if (type == ChangeType.Modified)
                strTypeClass = " modified";
            else if (type == ChangeType.Inserted)
                strTypeClass = " inserted";
            else if (type == ChangeType.Deleted)
                strTypeClass = " deleted";
            else if (type == ChangeType.Imaginary)
                strTypeClass = " imaginary";
            else if (type == ChangeType.Unchanged)
                strTypeClass = " unchanged";

            // 
            return "\r\n<td class='fieldname" + strTypeClass + "'>" + strFieldName + "</td>"
                + "<td class='indicator" + strTypeClass + "'>" + strIndicatior + "</td>"
                + "<td class='content" + strTypeClass + "'>" + strContent + "</td>";
        }

        static string BuildHeaderHtml(bool bModified, string strField)
        {
            if (string.IsNullOrEmpty(strField) == true)
            {
                // return "\r\n<td class='content' colspan='3'></td>";
                return "<td class='fieldname'></td>"
    + "<td class='indicator'></td>"
    + "<td class='content'></td>";
            }

            // string strLineClass = "";
            string strFieldName = "";
            string strIndicatior = "";
            string strContent = "";

            // strLineClass = "header";
            strField = strField.Replace(' ', '_');

            strContent = MarcUtil.GetHtmlFieldContent(strField, false);

            string strTypeClass = "";
            if (bModified)
                strTypeClass = " modified";
            // 
            return "\r\n<td class='fieldname" + strTypeClass + "'>" + strFieldName + "</td>"
                + "<td class='indicator" + strTypeClass + "'>" + strIndicatior + "</td>"
                + "<td class='content" + strTypeClass + "'>" + strContent + "</td>";
        }





        #region 比较 OPAC 字段差异的



        // 创建一个字段的 HTML 局部 三个 <td>
        static string BuildOpacFieldHtml(
            ChangeType type,
            string strField)
        {
            if (string.IsNullOrEmpty(strField) == true)
                return "\r\n<td class='content' colspan='2'></td>";

            // string strLineClass = "";
            string strFieldName = "";
            string strContent = "";

            int nRet = strField.IndexOf(":");
            if (nRet == -1)
            {
                strFieldName = strField;
                strContent = "";
            }
            else
            {
                strFieldName = strField.Substring(0, nRet).Trim();
                strContent = strField.Substring(nRet + 1).Trim();
            }

            //    strLineClass = "datafield";

            string strTypeClass = " datafield";
            if (type == ChangeType.Modified)
                strTypeClass += " modified";
            else if (type == ChangeType.Inserted)
                strTypeClass += " inserted";
            else if (type == ChangeType.Deleted)
                strTypeClass += " deleted";
            else if (type == ChangeType.Imaginary)
                strTypeClass += " imaginary";
            else if (type == ChangeType.Unchanged)
                strTypeClass += " unchanged";

            // 
#if NO
            return "\r\n<td class='fieldname" + strTypeClass + "'>" + HttpUtility.HtmlEncode(strFieldName) + "</td>"
                + "<td class='indicator" + strTypeClass + "'>" + "" + "</td>"
                + "<td class='content" + strTypeClass + "'>" + HttpUtility.HtmlEncode(strContent) + "</td>";
#endif
            return "\r\n<td class='fieldname" + strTypeClass + "'>" + HttpUtility.HtmlEncode(strFieldName) + "</td>"
    + "<td class='content" + strTypeClass + "'>" + HttpUtility.HtmlEncode(strContent) + "</td>";

        }

        #endregion
    }

    public class OpacField
    {
        public string Name = "";
        public string Value = "";

        public OpacField()
        {
        }

        public OpacField(string strName, string strValue)
        {
            Name = strName;
            Value = strValue;
        }
    }

    public enum ChangeType
    {
        Unchanged,
        Deleted,
        Inserted,
        Imaginary,
        Modified
    }
}
