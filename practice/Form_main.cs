using common;
using DigitalPlatform;
using DigitalPlatform.LibraryRestClient;
using DigitalPlatform.Marc;
using practice.test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice
{
    public partial class Form_main : Form
    {
        // 通道池
        RestChannelPool _channelPool = new RestChannelPool();

        public Form_main()
        {
            InitializeComponent();
        }

        public string ServerUrl
        {
            get
            {
                return this.Server_textBox_url.Text;
            }
        }

        public string UserName
        {
            get
            {
                return this.Login_textBox_userName.Text;
            }
        }

        public string Password
        {
            get
            {
                return this.Login_textBox_password.Text;
            }
        }

        public string Parameters
        {
            get
            {
                return this.Login_textBox_parameters.Text;
            }
        }

        public RestChannel GetChannel()
        {
            if (this.ServerUrl == "" || this.UserName == "")
            {
                throw new Exception("尚未设置dp2library url或用户名");
            }

            RestChannel channel= this._channelPool.GetChannel(this.ServerUrl, this.UserName);

            return channel;
        }

        void channelPool_BeforeLogin(object sender, BeforeLoginEventArgs e)
        {
            if (string.IsNullOrEmpty(this.ServerUrl))
            {
                e.Cancel = true;
                e.ErrorInfo = "dp2library URL为空";
            }

            e.LibraryServerUrl = this.ServerUrl;
            e.UserName = this.UserName;
            e.Password = this.Password;
            e.Parameters = this.Parameters;//"type=worker,client=dp2analysis|0.01";
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this._channelPool.BeforeLogin -= new BeforeLoginEventHandle(channelPool_BeforeLogin);
            this._channelPool.BeforeLogin += new BeforeLoginEventHandle(channelPool_BeforeLogin);


            this.Server_textBox_url.Text = Properties.Settings.Default.global_url;
            this.Login_textBox_userName.Text = Properties.Settings.Default.login_userName;
            this.Login_textBox_password.Text = Properties.Settings.Default.login_password;
            this.Login_textBox_parameters.Text = Properties.Settings.Default.login_parameters;

            this.SearchBiblio_textBox_BiblioDbNames.Text = Properties.Settings.Default.searchBiblio_biblioDbNames;
            this.SearchBiblio_textBox_QueryWord.Text = Properties.Settings.Default.searchBiblio_queryWord;
            this.SearchBiblio_textBox_PerMax.Text = Properties.Settings.Default.searchBiblio_perMax;
            this.SearchBiblio_textBox_FromStyle.Text = Properties.Settings.Default.searchBiblio_fromStyle;
            this.SearchBiblio_comboBox_MatchStyle.Text = Properties.Settings.Default.searchBiblio_matchStyle;
            this.SearchBiblio_textBox_ResultSetName.Text = Properties.Settings.Default.searchBiblio_resultSetName;
            this.SearchBiblio_textBox_SearchStyle.Text = Properties.Settings.Default.searchBiblio_searchStyle;

            this.GetSearchResult_textBox_ResultSetName.Text = Properties.Settings.Default.getSearchResult_resultsetName;
            this.GetSearchResult_textBox_Start.Text = Properties.Settings.Default.getSearchResult_start;
            this.GetSearchResult_textBox_Count.Text = Properties.Settings.Default.getSearchResult_count;
            this.GetSearchResult_textBox_BrowseInfoStyle.Text = Properties.Settings.Default.getSearchResult_browseInfoStyle;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._channelPool.CleanChannel();

            Properties.Settings.Default.global_url = this.Server_textBox_url.Text;

            Properties.Settings.Default.login_userName = this.Login_textBox_userName.Text;
            Properties.Settings.Default.login_password = this.Login_textBox_password.Text;
            Properties.Settings.Default.login_parameters = this.Login_textBox_parameters.Text;


            Properties.Settings.Default.searchBiblio_biblioDbNames=this.SearchBiblio_textBox_BiblioDbNames.Text;
            Properties.Settings.Default.searchBiblio_queryWord=this.SearchBiblio_textBox_QueryWord.Text;
            Properties.Settings.Default.searchBiblio_perMax=this.SearchBiblio_textBox_PerMax.Text;
            Properties.Settings.Default.searchBiblio_fromStyle=this.SearchBiblio_textBox_FromStyle.Text;
            Properties.Settings.Default.searchBiblio_matchStyle= this.SearchBiblio_comboBox_MatchStyle.Text;
            Properties.Settings.Default.searchBiblio_resultSetName=this.SearchBiblio_textBox_ResultSetName.Text;
            Properties.Settings.Default.searchBiblio_searchStyle=this.SearchBiblio_textBox_SearchStyle.Text;

            Properties.Settings.Default.getSearchResult_resultsetName=this.GetSearchResult_textBox_ResultSetName.Text;
            Properties.Settings.Default.getSearchResult_start=this.GetSearchResult_textBox_Start.Text;
            Properties.Settings.Default.getSearchResult_count=this.GetSearchResult_textBox_Count.Text;
            Properties.Settings.Default.getSearchResult_browseInfoStyle=this.GetSearchResult_textBox_BrowseInfoStyle.Text ;


            // 一定要调save函数才能把信息保存下来
            Properties.Settings.Default.Save();
        }

        private void 通用练习题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_c dlg = new Form_c();
            dlg.ShowDialog(this);
        }

        #region 登录

        private void button_getVersion_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                GetVersionResponse response= channel.GetVersion();
                if (response.GetVersionResult.Value == -1)
                {
                    this.textBox_result.Text = "获取版本出错：" + response.GetVersionResult.ErrorInfo;
                    return;
                }
                this.textBox_result.Text = response.GetVersionResult.ErrorInfo;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                string userName = this.Login_textBox_userName.Text.Trim();
                string password = this.Login_textBox_password.Text.Trim();
                string parameters = this.Login_textBox_parameters.Text.Trim();//"type=worker,client=resttest|0.01";
                if (userName == "")
                {
                    MessageBox.Show(this, "用户名不能为空");
                    return;
                }
                // 登录接口
                /// <para>-1:   出错</para>
                /// <para>0:    登录未成功</para>
                /// <para>1:    登录成功</para>
                LoginResponse response = channel.Login(userName,
                    password,
                    parameters);
                if (response.LoginResult.Value == 1)
                {
                    this.textBox_result.Text = "登录成功\r\n";
                }
                else
                {
                    this.textBox_result.Text = "登录失败\r\n";
                }

                this.textBox_result.Text += "Result:" 
                    + response.LoginResult.ErrorCode 
                    + response.LoginResult.ErrorInfo + "\r\n"
                    + "Rights:" + response.strRights + "\r\n"
                    + "UserName:" + response.strOutputUserName;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
               LogoutResponse response = channel.Logout();
                this.textBox_result.Text = "Result:" 
                    + response.LogoutResult.ErrorCode 
                    + response.LogoutResult.ErrorInfo;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        #endregion


        private void button_SearchBiblio_Click(object sender, EventArgs e)
        {

            RestChannel channel = this.GetChannel();
            try
            {
                int nPerMax = 0;
                if (this.SearchBiblio_textBox_PerMax.Text == "")
                    nPerMax = -1;
                else
                    nPerMax = Convert.ToInt32(this.SearchBiblio_textBox_PerMax.Text);


                //SearchBiblioResponse response 
                long lRet= channel.SearchBiblio(this.SearchBiblio_textBox_BiblioDbNames.Text,
                    this.SearchBiblio_textBox_QueryWord.Text,
                    nPerMax,
                    this.SearchBiblio_textBox_FromStyle.Text,
                    this.SearchBiblio_comboBox_MatchStyle.Text,
                    this.SearchBiblio_textBox_ResultSetName.Text,
                    SearchBiblio_textBox_OutputStyle.Text,// this.SearchBiblio_textBox_SearchStyle.Text,
                    this.SearchBiblio_textBox_filter.Text,
                    out string strQueryXml,
            out string strError);

                if (lRet == -1)
                {
                    this.textBox_result.Text = "error:" + strError;
                }
                else
                {
                    this.textBox_result.Text = "count:" + lRet + "\r\n"
                        + strQueryXml;
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetSearchResult_Click(object sender, EventArgs e)
        {
            this.textBox_result.Text = "";

            RestChannel channel = this.GetChannel();
            try
            {
                //GetSearchResultResponse response
                long lRet = channel.GetSearchResult(this.GetSearchResult_textBox_ResultSetName.Text,
                    Convert.ToInt64(this.GetSearchResult_textBox_Start.Text),
                    Convert.ToInt64(this.GetSearchResult_textBox_Count.Text),
                    this.GetSearchResult_textBox_BrowseInfoStyle.Text,
                    "",
                    out Record[] records,
                    out string strError);

                if (lRet == -1)
                {
                    this.textBox_result.Text = "errorInfo:" + strError;
                }

                if (records !=null && records.Length > 0)
                {
                    StringBuilder browse = new StringBuilder();
                    foreach (Record record in records)
                    {
                        browse.AppendLine(record.Path);

                        if (record.Cols !=null && record.Cols.Length>0)
                            browse.AppendLine( string.Join(",", record.Cols));
                        if (record.RecordBody !=null && string.IsNullOrEmpty(record.RecordBody.Xml)==false)
                             browse.AppendLine(record.RecordBody.Xml);
                    }
                    this.textBox_result.Text += browse.ToString();
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetBiblioInfo_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                GetBiblioInfoResponse response = channel.GetBiblioInfo(this.GetBiblioInfo_textBox_BiblioRecPath.Text,
                    this.GetBiblioInfo_textBox_BiblioType.Text);

                this.textBox_result.Text = "Result:" + response.GetBiblioInfoResult.ErrorCode
                    + response.GetBiblioInfoResult.ErrorInfo + "\r\n"
                    + response.strBiblio + "\r\n";
                    //+ response.
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetBiblioInfos_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {

                string strformats = GetBiblioInfos_textBox_Formats.Text;
                string[] formats = strformats.Split(new char[] { ','});

                GetBiblioInfosResponse response = channel.GetBiblioInfos(this.GetBiblioInfos_textBox_BiblioRecPath.Text,
                    formats);

                string strResult = "result:\r\n";
                if (response.results != null && response.results.Length > 0)
                {
                    foreach (string s in response.results)
                    {
                        strResult += s + "\r\n";
                    }
                }

                string strTimestamp = ByteArray.GetHexTimeStampString(response.baTimestamp);

                this.textBox_result.Text = "Result:" + response.GetBiblioInfosResult.ErrorCode
                    + response.GetBiblioInfosResult.ErrorInfo + "\r\n"
                    + "timestamp:" + strTimestamp + "\r\n"
                + strResult;
                    
                //+ response.
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }



        private void button__Reservation_start_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                ReservationResponse response = channel.Reservation(
                    this.comboBox_Reservation_action.Text,
                    this.textBox__Reservation_readerBarcode.Text,
                    this.textBox_Reservation_itemBarcodeList.Text
                    );

                this.textBox_result.Text = "Result:"
                    + "ErrorCode:" + response.ReservationResult.ErrorCode
                    + "\r\n" + "ErrorInfo:" + response.ReservationResult.ErrorInfo;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }


  


        // 创建书目
        private void button_setBiblioInfo_Click(object sender, EventArgs e)
        {
            string strError = "";
            string strAction = this.textBox_action.Text;
            string biblioRecPath = this.textBox_biblioPath.Text;
            string biblioType = this.textBox_biblioType.Text;
            string biblio = this.textBox_biblio.Text;

            string timestamp=this.textBox_timestamp.Text;
            byte[] baTimestamp = ByteArray.GetTimeStampByteArray(timestamp);
            string strComment = "";
            string strStyle = this.textBox_style.Text;

            RestChannel channel = this.GetChannel();
            try
            {


                byte[] baNewTimestamp = null;
                string strOutputPath = "";

                long lRet = channel.SetBiblioInfo(
                    strAction,
                    biblioRecPath,
                    biblioType,
                    biblio,
                    baTimestamp,
                    strComment,
                    strStyle,
                    out strOutputPath,
                    out baNewTimestamp,
                    out strError);
                if (lRet == -1)
                {
                    this.textBox_result.Text = strError;
                    MessageBox.Show(this, strError);
                    return;
                }

                this.textBox_result.Text = "strOutputPath=" + strOutputPath + "\r\n"
                    + "baNewTimestamp=" + ByteArray.GetHexTimeStampString(baNewTimestamp) +"\r\n"
                    + "strError=" + strError
                    + "lRet=" + lRet.ToString();

            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }




        private void button_getField_Click(object sender, EventArgs e)
        {
            string strFieldMap = this.textBox_map.Text.Trim();
            if (string.IsNullOrEmpty(strFieldMap) == true)
            {
                MessageBox.Show(this, "尚未配置字段提取规则。");
                return;
            }

            //
            string strMarc = this.textBox_biblio.Text.Trim();
            if (string.IsNullOrEmpty(strMarc) == true)
            {
                MessageBox.Show(this, "尚未设置marc数据。");
                return;
            }

            // 显示在界面上
            this.textBox_result.Text =MarcHelper.GetFields(strMarc, strFieldMap);
            
        }

        private void button_setField_Click(object sender, EventArgs e)
        {
            string strFieldMap = this.textBox_map.Text.Trim();
            if (string.IsNullOrEmpty(strFieldMap) == true)
            {
                MessageBox.Show(this, "尚未配置字段提取规则。");
                return;
            }
            List<FieldItem> fieldList = null;
            try
            {
                fieldList = MarcHelper.ParseFieldMap(strFieldMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }

            //
            string strMarc = this.textBox_biblio.Text.Trim();
            if (string.IsNullOrEmpty(strMarc) == true)
            {
                MessageBox.Show(this, "尚未设置marc数据。");
                return;
            }

            string marc =MarcHelper.SetFields(strMarc, fieldList);

            this.textBox_result.Text = marc;


        }

        private void button_searchItem_Click(object sender, EventArgs e)
        {
            
            RestChannel channel = this.GetChannel();
            try
            {
                int nPerMax = 0;
                if (this.searchItem_nPerMax.Text == "")
                    nPerMax = -1;
                else
                    nPerMax = Convert.ToInt32(this.searchItem_nPerMax.Text);
                /*
                 * 
public long SearchItem(string strItemDbName,
    string strQueryWord,
    int nPerMax,
    string strFrom,
    string strMatchStyle,
    string strResultSetName,
    string strSearchStyle,
     string strOutputStyle,
    out string strError)
                 */
                //SearchBiblioResponse response 
                long lRet = channel.SearchItem(this.searchItem_strItemDbName.Text,
                    this.searchItem_strQueryWord.Text,
                    nPerMax,
                    this.searchItem_strFrom.Text,
                    this.searchItem_strMatchStyle.Text,
                    this.searchItem_strResultSetName.Text,
                    this.searchItem_strSearchStyle.Text,
                    this.searchItem_strOutputStyle.Text,
                    out string strError);
                if (lRet == -1)
                {
                    this.textBox_result.Text = "error:" + strError;
                }
                else
                {
                    this.textBox_result.Text = "count:" + lRet;
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }
    }


}
