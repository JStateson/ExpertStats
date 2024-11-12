using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExpertStats.global;
/*
 https://h30434.www3.hp.com/t5/forums/recentpostspage/post-type/message/user-id/18563/page/
want to capture solved / unsolved statistics for user
base  https://h30434.www3.hp.com/t5/
forum forums/recentpostspage/post-type/message/user-id/
userpage 18563/page/

 */


namespace ExpertStats
{
    public partial class WebForm : Form
    {
        private int iAuthor;
        private bool TaskBusy;
        private bool TaskDone;
        private int WalkingPtr = 0;  // 0:init  1:continue
        private int WalkCount = 0;
        private int DaysWanted = 300;
        private int AvailablePages = 0;
        private int ParseCode = 0;
        DateTime StartDate = DateTime.Now;
        DateTime SmallestDate;  // the largest of the 20 or so date on a page so we know when to stop
        private string BaseUrl = "https://h30434.www3.hp.com/t5/forums/recentpostspage/post-type/message/user-id/";
        private string AddToBaseUrl = "forums/recentpostspage/post-type/message/user-id/";
        private string user_id;
        private int CurrentPage = 0;
        private int StartPage;
        private Uri CurrentUri;
        private string htmlContent;
        private bool bError = false;

        private cAllPostInfo AllPostInfo = new cAllPostInfo();

        private string FormUrl()
        {
            CurrentPage++;
            return BaseUrl + user_id + "/page/" + CurrentPage.ToString();
        }
        public WebForm(int rAuthor, DateTime Cutoff)
        {
            InitializeComponent();
            lbDaysWanted.Text = "Days wanted " + DaysWanted.ToString();
            iAuthor = rAuthor;
            user_id = AuthorList[rAuthor].user_id;
            tbName.Text = AuthorList[rAuthor].name;
            webView21.Visible = false;
            webView21.Size = new System.Drawing.Size(0, 0);
            AllPostInfo.Init(Cutoff);
            StartPage = CurrentPage + 1;
            InitializeAsync();
            StartWalking();
        }

        private int DaysElapsed(DateTime dt, ref bool bDone)
        {
            TimeSpan d = StartDate - dt;
            bDone = (d.Days >= DaysWanted);
            return d.Days;
        }

        private async void InitializeAsync()
        {
            TaskBusy = true;
            TaskDone = false;
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate(FormUrl());
        }

        private void StopWalking()
        {
            TimerWalking.Enabled = false;
            AllPostInfo.WriteUserStats(user_id);
            this.Close();
        }

        private void StartWalking()
        {
            TimerWalking.Enabled = true;
        }

        private int GetProgress()
        {
            TimeSpan t = DateTime.Now - AllPostInfo.LastDate;
            double a = t.Days * 100.0 / DaysWanted;
            if (a > 99.0)
            {
                StopWalking();
                return 0;
            }

            tbPage.Text = CurrentPage.ToString();
            return Convert.ToInt32(a);
        }

        private void TimerWalking_Tick(object sender, EventArgs e)
        {
            WalkCount++;
            pbAll.Value = GetProgress();
            if (ParseCode != 0 || bError)
            {
                StopWalking();
                return;
            }
            if (WalkingPtr == 0)
            {
                if (TaskBusy) return;
                WalkCount = 0;
                WalkingPtr = 1;
                TaskBusy = true;
                webView21.CoreWebView2.Navigate(FormUrl());
                return;
            }
            if (WalkingPtr == 1)
            {
                if (TaskBusy) return;
                WalkCount = 0;
                TaskBusy = true;
                webView21.CoreWebView2.Navigate(FormUrl());
            }

        }

        private async void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {

                htmlContent = await webView21.CoreWebView2.ExecuteScriptAsync("document.documentElement.querySelector('body').innerHTML");
                htmlContent = htmlContent.Trim('"').Replace("\\u003C", "<").Replace("\\u003E", ">").Replace("\\\"", "\"");
                if (CurrentPage == StartPage)
                {
                    AvailablePages = AllPostInfo.GetExpectedPages(ref htmlContent);
                    if (AvailablePages <= 0)
                    {
                        bError = true;
                        TaskBusy = false;
                        return;
                    }
                }
                ParseCode = AllPostInfo.AllPostParse(ref htmlContent);
                TaskBusy = false;
            }
            else
            {
                bError = true;
                TaskBusy = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopWalking();
        }

        private void WebForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webView21 != null)
            {
                webView21.Dispose();
            }
        }
    }
}
