using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace vkAuthorize
{
    using Params = NameValueCollection;
    public partial class frmMain : Form
    {
        //settings
        private int appId = YOUR_APP_ID;
        private VKScopeList myScope = VKScopeList.Wall;
        //init after authrization
        string accessToken = "";
        int userId = 0;


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var authString = String.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", appId, (int)myScope);
            authBrowser.Navigate(authString);
        }

        private void authBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().IndexOf("access_token") != -1)
            {
                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(e.Url.ToString()))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                      accessToken = m.Groups["value"].Value;
                      Console.WriteLine(accessToken);
                      this.Close();
                    }
                    else if (m.Groups["name"].Value == "user_id")
                    {
                      userId = Convert.ToInt32(m.Groups["value"].Value);
                    }
                }
                //MessageBox.Show(String.Format("Ключ доступа: {0}\nUserID: {1}", accessToken, userId));
                authBrowser.Visible = false;
            }
        }
    }

    public enum VKScopeList
    {
        Notify = 1,
        Friends = 2,
        Photos = 4,
        Audio = 8,
        Video = 16,
        Offers = 32,
        Questions = 64,
        Pages = 128,
        Link = 256,
        Notes = 2048,
        Messages = 4096,
        Wall = 8192,
        Docs = 131072,
        All = Notify | Friends | Photos | Audio | Video | Questions | Pages | Link | Notes | Messages | Wall | Docs,
    }
}
