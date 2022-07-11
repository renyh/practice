using DigitalPlatform.LibraryRestClient;
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
                //SearchBiblioResponse response 
                long lRet= channel.SearchBiblio(this.SearchBiblio_textBox_BiblioDbNames.Text,
                    this.SearchBiblio_textBox_QueryWord.Text,
                    Convert.ToInt32(this.SearchBiblio_textBox_PerMax.Text),
                    this.SearchBiblio_textBox_FromStyle.Text,
                    this.SearchBiblio_comboBox_MatchStyle.Text,
                    this.SearchBiblio_textBox_ResultSetName.Text,
                    this.SearchBiblio_textBox_SearchStyle.Text,
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
                        browse.AppendLine( string.Join(",", record.Cols));
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
                    + response.strBiblio;

               
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetBiblioInfos_Click(object sender, EventArgs e)
        {
            //todo 结构还没写
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
    }
}
