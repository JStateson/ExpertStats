using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExpertStats.global;
using static ExpertStats.global.cKudoBase;
using Microsoft.Web.WebView2.Core;
using System.Net;
using System.Reflection;

//This app and source code copyrighted Joseph Stateson November 9, 2024

namespace ExpertStats
{
    public partial class ExpertInfo : Form
    {
        int numAyears = 1;

        private static bool bAsyncDone = false;

        static string sA = "kudos/leaderboardpage/timerange/one_year/tab/authors/page/";
        static string sU = "forums/recentpostspage/post-type/message/is-solution/true/user-id/"; //xxxxxxx/page/";
        static string sK = "kudos/userpage/user-id/"; //4187452/tab/my-kudoed-messages/page/1
        static string hrefT = "href=\"/t5/user/viewprofilepage/user-id/";
        static string SolLast = "<li class=\"lia-paging-page-last lia-js-data-pageNum-";
        //<li class="lia-paging-page-last lia-js-data-pageNum-107">
        static string skout = "";
        static int skcnt = 0;
        static bool WritingContacts = false;
        static bool Counting_SK = false;    //counting kudos or solutions

        static string sPagesTotal = "";
        static string kPagesTotal = "";


        static string sContactList = "UsernameID.txt";
        static string sExpertList = "ExpertList.txt";
        static int WhichPB = 0;

        static string sol_id;
        static int sol_nPageMax;
        static string sol_sG;
        static int sol_rtnval;
        static int kud_rtnval;
        static int sol_progress;
        static List<DateTime> sol_Solutions = new List<DateTime>();
        static List<long> sol_LSolutions = new List<long>();
        private bool iStartUp = true;
        private static bool StopAsync = false;

        static string spanT0 = "data-lia-message-uid=";
        static string spanT1 = "<span class=\"DateTime\">";
        static string spanT2 = "<span class=\"local-date\">";
        static string spanT3 = "<span class=\"local-time\">";
        static string spanTe = "</span>";

        private cSortTools SortTools = new cSortTools();

        private static string NameCounted = "";
        private static string NumCounted = "";

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri(hpBase),
        };

        private void SaveSettings()
        {
            Properties.Settings.Default.DateToUse = tbYear.Text;
            Properties.Settings.Default.Save();
        }

        public ExpertInfo()
        {
            InitializeComponent();
            SetCutoffDate();
            CreateNeededStuff();
            //Start();
        }

        private void CreateNeededStuff()
        {
            bool isDebug = Debugger.IsAttached;
            if (!isDebug)
            {
                WhereExe = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + "\\";
                WhereExpert = WhereExe + "HPexperts\\";
            }

            lbWhereExe.Text = WhereExpert;
            if (!Directory.Exists(WhereExpert))
            {
                Directory.CreateDirectory(WhereExpert);
            }

            KudoBaseTool = new cKudoBase();
            sContactList = WhereExpert + sContactList;
            sExpertList = WhereExpert + sExpertList;
            KudoBaseTool.FillBase();
            if (!File.Exists(sContactList))
            {
                WritingContacts = true;
                WhichPB = 4;
                GroupsEnable(false);
                TimerAcnt.Enabled = true;
                pbAuthors.Maximum = 4;
                pbAuthors.Value = 0;
                object value = GetAsync(sharedClient);
            }
            else Start();
        }

/*
        How to create a folder at the executable location so that the release app can run on my PC:
        mklink /D "C:\Users\josep\source\repos\ExpertStats\bin\x64\Release\HPexperts" "C:\Users\josep\source\repos\ExpertStats\HP
        use rmdir to remove 
*/
        private void SetCutoffDate()
        {
            tbYear.Text = Properties.Settings.Default.DateToUse;
            string dts = tbYear.Text + " " + tbCutTime.Text;
            Cutoff = DateTime.Now;
            bool b = dtHPtime(dts, ref Cutoff);
            if (!b)
            {
                MessageBox.Show("date time bad " + dts + "\r\nChanging to 01-01-2024");
                tbYear.Text = "01-01-2024";
                SaveSettings();
                dtHPtime(tbYear.Text, ref Cutoff);
            }
        }

        private void Start()
        {
            if (File.Exists(sContactList))
            {
                ReadContacts();
                if (File.Exists(sExpertList))
                {
                    ReadExperts();
                }
            }
            

        }

        private void SetHeaders()
        {
            int[] iSizes = new int[] { 32,128,64,64,64,64,64,128};
            dgvExpert.Columns[0].HeaderText = "N";
            dgvExpert.Columns[1].HeaderText = "Name";
            dgvExpert.Columns[2].HeaderText = "User ID";
            dgvExpert.Columns[3].HeaderText = "Solution\r\nPages";
            dgvExpert.Columns[4].HeaderText = "Solution\r\nPoints";
            dgvExpert.Columns[5].HeaderText = "Kudo\r\nPages";
            dgvExpert.Columns[6].HeaderText = "Kudo\r\nUrls";
            dgvExpert.Columns[7].HeaderText = "Solution\r\nDate Range";
            for (int i = 0; i < 7; i++)
            {
                dgvExpert.Columns[i].Width = iSizes[i];
            }
            dgvExpert.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            pbAuthors.Maximum = AuthorList.Count;
            pbAuthors.Value = 0;
            WhichPB = 0;
            GroupsEnable(false);
            TimerAcnt.Enabled = true;
            Counting_SK = true;
            StopAsync = false;
            object value = GetAsyncS(sharedClient);
        }


        private void ReadExperts()
        {
            cExpert ce;
            ce = new cExpert();
            foreach (string line in File.ReadLines(sExpertList))
            {
                string[] sS = line.Split(',');
                ce.name = sS[0];
                ce.user_id = sS[1];
                ce.Spages = sS[2];
                ce.Spoints = NumSolPoints(ce.user_id);
                if (ce.Spoints == "") ce.Spoints = "0";
                ce.Kpages = sS[3];
                if (sS.Length > 4)
                    ce.Kpoints = sS[4];
                else ce.Kpoints = "0";
                if (sS.Length > 5)
                    ce.DatePosted = sS[5];
                else ce.DatePosted = "";
                for (int i = 0; i < AuthorList.Count; i++)
                {
                    if(ce.user_id == AuthorList[i].user_id)
                    {
                        cExpert cd = AuthorList[i];
                        cd.Spages = ce.Spages;
                        cd.Kpages = ce.Kpages;
                        cd.Spoints = ce.Spoints;
                        cd.Kpoints = ce.Kpoints;
                        cd.DatePosted = ce.DatePosted;
                        cd.TotalPosts = ce.TotalPosts;
                        cd.TotalSolutions = ce.TotalSolutions;
                        cd.Kudoed = ce.Kudoed;
                        break;
                    }
                }
            }
            dgvExpert.DataSource = AuthorList;
        }

        private string NumSolPoints(string id)
        {
            string filePath = WhereExpert + id + "\\Solutions.txt";
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists) return "";
            long fileSizeInBytes = fileInfo.Length;
            fileSizeInBytes /= 29;
            return fileSizeInBytes.ToString(); ;
        }

        private void ReadContacts()
        {
            cExpert ce;
            AuthorList.Clear();
            int n = 0;
            foreach (string line in File.ReadLines(sContactList))
            {
                n++;
                ce = new cExpert();
                string[] sS = line.Split(',');
                ce.name = sS[0];
                ce.user_id = sS[1];
                ce.Spages = "-1";
                ce.Kpages = "-1";
                ce.Spoints = "0";
                ce.Kpoints = "0";
                ce.N = AuthorList.Count+1;
                AuthorList.Add(ce);
            }
            dgvExpert.DataSource = AuthorList;
            SetHeaders();
        }

        static int add_id_name(ref string s, ref string sOut)
        {
            string stemp;
            int n = s.IndexOf(hrefT);
            if (n == -1) return 0;
            int a = n + hrefT.Length;
            int id1 = s.IndexOf('\"', a);
            string id = s.Substring(a, id1 - a);
            int n1 = s.IndexOf("title=\"", id1);
            a = n1 + 7;
            int n2 = s.IndexOf('\"', a);
            string nam = s.Substring(a, n2 - a);
            stemp = nam + "," + id;
            if (!sOut.Contains(stemp))
                sOut += stemp + Environment.NewLine;
            string t = s.Substring(n2);
            return 1 + add_id_name(ref t, ref sOut);
        }

        static async Task GetAsync(HttpClient httpClient)
        {
            int n = 0;
            string jsonResponse="";
            for (int i = 1; i < 4; i++)
            {
                skcnt = i;
                try
                {
                    using HttpResponseMessage response = await httpClient.GetAsync(sA + i.ToString() + "/tab/authors");
                    jsonResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return;
                }
                add_id_name(ref jsonResponse, ref KudoBaseTool.sOutTemp);
            }
            WritingContacts = false;
        }

        static async Task GetAsyncS(HttpClient httpClient)
        {
            string nPages;
            string jsonResponse;
            skout = "";
            skcnt = 0;
            int iID;
            string sID;
            sPagesTotal = "";
            WritingContacts = true;
            foreach (cExpert ce in AuthorList)
            {
                string id = ce.user_id;
                NameCounted = ce.name;
                iID = skcnt + 1;
                sID = "s" + iID.ToString();
                if (ce.Spages != "-1")
                {
                    skcnt++;
                    continue;
                }
                try
                {
                    using HttpResponseMessage response = await httpClient.GetAsync(sU + id + "/page/1");
                    jsonResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return;
                }
                int i = jsonResponse.LastIndexOf(SolLast);
                if (i == -1)
                {
                    //if (jsonResponse.Contains("No Posts Found"))nPages = "";
                    //else nPages = "0";
                    nPages = "0";
                }
                else
                {
                    i += SolLast.Length;
                    int j = jsonResponse.IndexOf("\">", i);
                    Debug.Assert(j != -1);
                    nPages = jsonResponse.Substring(i, j - i);
                }
                NumCounted = nPages;
                ce.Spages = nPages;
                skcnt++;
                if (StopAsync) break;
            }
            skcnt = AuthorList.Count;
            WritingContacts = false;
        }

        static async Task GetAsyncK(HttpClient httpClient)
        {
            string nPages;
            string jsonResponse;
            skout = "";
            skcnt = 0;
            sPagesTotal = "";
            kud_rtnval = 0;
            foreach (cExpert ce in AuthorList)
            {
                NameCounted = ce.name;
                string id = ce.user_id;
                if (ce.Kpages != "-1")
                {
                    continue;
                }
                try
                {
                    string sTemp = sK + id + "/tab/my-kudoed-messages/page/1";
                    using HttpResponseMessage response = await httpClient.GetAsync(sTemp);
                    jsonResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return;
                }
                int i = jsonResponse.IndexOf(SolLast);
                if (i == -1)
                {
                    nPages = "0";
                }
                else
                {
                    i += SolLast.Length;
                    int j = jsonResponse.IndexOf("\">", i);
                    Debug.Assert(j != -1);
                    nPages = jsonResponse.Substring(i, j - i);
                }
                ce.Kpages = nPages;
                NumCounted = nPages;
                if (StopAsync) break;
            }
            skcnt = AuthorList.Count;
            kud_rtnval = 1;
        }


        private string FormSolutionUrl(string id, int iPage)
        {
            return sU + id + "/page/" + iPage.ToString();
        }

        private string FormKudoUrl(string id, int iPage)
        {
            return sK + id + "/tab/my-kudoed-messages/page/" + iPage.ToString();
        }

        private static void ShowEdge(string sUrl)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "msedge";
            process.StartInfo.Arguments = sUrl;
            process.Start();
        }

        private void UpdateDGVlist()
        {
            File.Delete(sExpertList);
            string sOut = "";
            string s;
            string kpts = "";
            foreach (cExpert ce in AuthorList)
            {
                kpts = ce.Kpoints.Trim(); ;
                if (kpts == "") kpts = "0";
                s = ce.name + "," + ce.user_id + "," + ce.Spages + "," + ce.Kpages +"," + kpts +","+ ce.DatePosted + Environment.NewLine;
                sOut += s;
            }
            File.WriteAllText(sExpertList, sOut);
        }

        private void dgvExpert_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;
            if (e.ColumnIndex < 3)
            {
                string s = dgvExpert.Rows[e.RowIndex].Cells[2].Value.ToString();
                ShowEdge(hpBase + FormAllPostUrl(s, 1));
            }
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                string s = dgvExpert.Rows[e.RowIndex].Cells[2].Value.ToString();
                ShowEdge(hpBase + FormSolutionUrl(s, 1));
            }
            if (e.ColumnIndex == 5 || e.ColumnIndex == 6)
            {
                string s = dgvExpert.Rows[e.RowIndex].Cells[2].Value.ToString();
                ShowEdge(hpBase + FormKudoUrl(s, 1));
            }
        }


        /*
         *          if(skcnt > pbAuthors.Maximum || skcnt < 0)
                    {
                        MessageBox.Show("pb value wrong " + skcnt.ToString() + " " + pbAuthors.Maximum.ToString());
                        int ii = 0;
                    }
        */
        private void TimerAcnt_Tick(object sender, EventArgs e)
        {            
            switch (WhichPB)
            {
                case 0: // COUNT SOLUTIONS

                    pbAuthors.Value = skcnt;
                    
                    lbCurrentVol.Text = NameCounted;
                    lbCnt.Text = NumCounted;
                    if (!WritingContacts)
                    {
                        TimerAcnt.Stop();
                        //TimerAcnt.Dispose();
                        pbAuthors.Value = 0;
                        dgvExpert.Invalidate(false);
                        UpdateDGVlist();
                        GroupsEnable(true);
                        Counting_SK = false;
                    }
                    break;
                case 1: // COUNTS KUDOS
                    pbAuthors.Value = skcnt;
                    lbCurrentVol.Text = NameCounted;
                    lbCnt.Text = NumCounted;
                    if (kud_rtnval != 0)
                    {
                        TimerAcnt.Stop();
                        //TimerAcnt.Dispose();
                        pbAuthors.Value = 0;
                        dgvExpert.Invalidate(false);
                        UpdateDGVlist();
                        GroupsEnable(true);
                        Counting_SK = false;
                    }
                    break;
                case 2: // GET SOLUTIONS
                    pbGetSol.Value = sol_progress;
                    lbCnt.Text = sol_progress.ToString();
                    if (sol_rtnval != 0)
                    {
                        TimerAcnt.Stop();
                        //TimerAcnt.Dispose();
                        SaveSolutions(sol_id, ref sol_Solutions);
                        UpdateRawSolCnt(sol_id, sol_Solutions.Count);
                        pbGetSol.Value = 0;
                        bFinishTaskBusy = false;
                        TaskFinished();
                        StopAsync = true;
                    }
                    break;
                case 3: // GET KUDOS
                    pbGetSol.Value = KudoBaseTool.BusyPage;
                    lbCnt.Text = sol_progress.ToString();
                    if(!KudoBaseTool.Busy || sol_rtnval != 0)
                    {
                        TimerAcnt.Stop();
                        //TimerAcnt.Dispose();
                        SaveKudoUrls(sol_id);
                        UpdateRawKudoCnt(sol_id, KudoBaseTool.nKudosTotal);
                        pbGetSol.Value = 0;
                        bFinishTaskBusy = false;
                        TaskFinished();
                        StopAsync = true;
                    }
                    break;
                 case 4:
                    pbAuthors.Value = skcnt;                    
                    if (!WritingContacts)
                    {
                        TimerAcnt.Stop();
                        pbAuthors.Value = 0;
                        File.WriteAllText(sContactList,  KudoBaseTool.sOutTemp);
                        ReadContacts();
                        if (File.Exists(sExpertList))
                        {
                            ReadExperts();
                        }
                        GroupsEnable(true);
                    }
                    break;
                case 5: // KUDO REFERRALS
                    pbGetSol.Value = KudoRcvd.nUrls;
                    lbCnt.Text = sol_progress.ToString();
                    if (!KudoRcvd.bTaskBusy)
                    {
                        TimerAcnt.Stop();
                        //TimerAcnt.Dispose();
                        KudoRcvd.SaveKudoList();
                        KudoRcvd.bTaskDone = true;
                        bFinishTaskBusy = false;
                        pbGetSol.Value = 0;
                        TaskFinished();
                        StopAsync = true;
                    }
                    break;
            }
        }

        private void UpdateRawKudoCnt(string id, int cnt)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgvExpert.Rows)
            {
                if (row.Cells[2].Value.ToString() == id)
                {
                    row.Cells[6].Value = cnt.ToString();
                    AuthorList[i].Kpoints = cnt.ToString();
                    UpdateDGVlist();
                    dgvExpert.Invalidate(false);
                    break;
                }
                i++;
            }
        }

        private void UpdateRawSolCnt(string id, int cnt)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgvExpert.Rows)
            {
                if (row.Cells[2].Value.ToString() == id)
                {
                    row.Cells[4].Value = cnt.ToString();
                    AuthorList[i].Spoints = cnt.ToString();
                    UpdateDGVlist();
                    dgvExpert.Invalidate(false);
                    break;
                }
                i++;
            }
        }

        private void SaveKudoUrls(string id)
        {
            string pathFolders = WhereExpert;
            if (!Directory.Exists(pathFolders))
            {
                Directory.CreateDirectory(pathFolders);
            }
            pathFolders += id;
            if (!Directory.Exists(pathFolders))
            {
                Directory.CreateDirectory(pathFolders);
            }
            string filePath = pathFolders + "/KudoUrls.txt";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (cKitems ck in KudoBaseTool.kitems)
                {
                    writer.WriteLine(ck.sUrl);
                }
            }
        }

        private void btnCountKudos_Click(object sender, EventArgs e)
        {
            pbAuthors.Value = 0;
            pbAuthors.Maximum = AuthorList.Count;
            WhichPB = 1;
            GroupsEnable(false);
            TimerAcnt.Enabled = true;
            Counting_SK = true;
            StopAsync = false;
            object value = GetAsyncK(sharedClient);
        }

        private void SaveSolutions(string id, ref List<DateTime> dates)
        {
            string pathFolders = WhereExpert;
            if (!Directory.Exists(pathFolders))
            {
                Directory.CreateDirectory(pathFolders);
            }
            pathFolders += id;
            if (!Directory.Exists(pathFolders))
            {
                Directory.CreateDirectory(pathFolders);
            }
            WriteDTLToFile(ref dates, pathFolders + "/solutions.txt");
        }
        

        private void WriteDTLToFile(ref List<DateTime> dateTimeList, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DateTime dateTime in dateTimeList)
                {
                    writer.WriteLine(dateTime.ToString("o")); // "o" for ISO 8601 format
                }
            }

        }

        static async Task GetAsyncKUD(HttpClient httpClient)
        {
            skcnt = 0;
            string jsonResponse;

            KudoBaseTool.ParseInit();
            for (int i = 0; i < sol_nPageMax; i++)
            {
                bool b = true;
                int j = 0;
                sol_progress = i + 1; 
                try
                {
                    using HttpResponseMessage response = await httpClient.GetAsync(sol_sG + (i+1).ToString());
                    jsonResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Request error: " + e.Message);
                    return;
                }
                while (b)
                {
                    sol_rtnval = KudoBaseTool.ParseKudoPage(ref jsonResponse);
                    if (sol_rtnval == 0) break;
                    if (sol_rtnval == 1) return;
                    if (sol_rtnval < 0) return;
                }
                if (i == sol_nPageMax - 1)
                {
                    sol_rtnval = 1;
                    KudoBaseTool.Busy = false;
                }
                if (StopAsync) return;
            }
        }




        static async Task GetAsyncSOL(HttpClient httpClient)
        {
            skcnt = 0;
            string jsonResponse;
            int n = 0;
            for (int i = 0; i < sol_nPageMax; i++)
            {
                bool b = true;
                int j = 0;
                sol_progress = i;
                try
                {
                    using HttpResponseMessage response = await httpClient.GetAsync(sol_sG + (i+1).ToString());
                    jsonResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return;
                }

                while (b)
                {
                    int NewPage = 0;
                    sol_rtnval = ExtractDT(ref jsonResponse, j, ref NewPage);
                    n = sol_Solutions.Count;
                    if (sol_rtnval == 0) break;
                    if (sol_rtnval == 1) return;
                    if (sol_rtnval < 0) return;
                }
                if(i == sol_nPageMax -1)
                {
                    sol_rtnval = 1;
                }
                if (StopAsync) return;
            }
        }




        // 0 out of data
        // 1 out of time
        static int ExtractDT(ref string s, int iStart, ref int NewPage)
        {
            int i;
            string dtString = "";
            if (iStart >= s.Length) return 0;

            i = s.IndexOf(spanT0, iStart);  // find this first
            if (i < 0) return 0;
            iStart = i + spanT0.Length; ;

            i = s.IndexOf(spanT1, iStart);
            if (i < 0) return 0;


            iStart = i + spanT1.Length;
            i = s.IndexOf(spanT2, iStart);
            if (i < 0) return 0;
            iStart = i + spanT2.Length;
            int j = s.IndexOf(spanTe, iStart);
            if (j < 0) return 0;
            string dtS = s.Substring(iStart, j - iStart) + " ";


            iStart = j + spanTe.Length;
            i = s.IndexOf(spanT3, iStart);
            if (i < 0) return 0;
            iStart = i + spanT3.Length;
            j = s.IndexOf(spanTe, iStart);
            if (j < 0) return 0;
            string dtT = s.Substring(iStart, j - iStart);
            dtString = dtS.Trim() + " " + dtT.Trim();
            DateTime dateTime = DateTime.Now;
            bool b = dtHPtime(dtString, ref dateTime);
            if (!b) return -1;

            sol_Solutions.Add(dateTime);
            iStart = j + spanTe.Length;

            if (dateTime < Cutoff)
            {
                NewPage++;
                if (NewPage > 10)
                    return 1;
            }
            return ExtractDT(ref s, iStart, ref NewPage);
        }

        private void RunGetSol()
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            bAsyncDone = false;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            int dgvE_row = selectedRow.Index;
            string sE = dgvExpert.Rows[dgvE_row].Cells[4].Value.ToString();
            if (!(sE == "0" | sE == "") &&  !cbSolCalculate.Checked) return;
            string id = dgvExpert.Rows[dgvE_row].Cells[2].Value.ToString();
            lbCurrentVol.Text = dgvExpert.Rows[dgvE_row].Cells[1].Value.ToString();
            int nPageMax = Convert.ToInt32(dgvExpert.Rows[dgvE_row].Cells[3].Value);
            if (nPageMax > 500) nPageMax = 500;
            if (nPageMax <= 0) return;
            string sGetSolPage0 = sU + id + "/page/";
            WhichPB = 2;
            pbGetSol.Maximum = nPageMax;
            pbGetSol.Value = 0;
            StartingTasks();
            sol_id = id;
            sol_nPageMax = nPageMax;
            sol_sG = sGetSolPage0;
            sol_rtnval = 0;
            sol_Solutions.Clear();
            TimerAcnt.Enabled = true;
            StopAsync = false;
            object value = GetAsyncSOL(sharedClient);
        }

        private void btnGetSol_Click(object sender, EventArgs e)
        {
           RunGetSol();
        }

        private void RunGetKudo()
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            bAsyncDone = false;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            int dgvE_row = selectedRow.Index;
            string sE = dgvExpert.Rows[dgvE_row].Cells[6].Value.ToString();
            if (!(sE == "0" | sE == "") && !cbKudoCalculate.Checked) return;
            string id = dgvExpert.Rows[dgvE_row].Cells[2].Value.ToString();
            lbCurrentVol.Text = dgvExpert.Rows[dgvE_row].Cells[1].Value.ToString();
            //List<DateTime> OLDdateTimeList = ReadDTLFromFile(id);
            DateTime Cutoff = DateTime.Now;
            int nPageMax = Convert.ToInt32(dgvExpert.Rows[dgvE_row].Cells[5].Value);
            if (nPageMax > 500) nPageMax = 500;
            if (nPageMax <= 0) return;
            string sGetKudPage0 = sK + id + "/tab/my-kudoed-messages/page/";
            pbGetSol.Maximum = nPageMax;
            pbGetSol.Value = 0;
            StartingTasks();
            sol_id = id;
            sol_nPageMax = nPageMax;
            sol_sG = sGetKudPage0;
            sol_rtnval = 0;
            sol_Solutions.Clear();
            WhichPB = 3;
            TimerAcnt.Enabled = true;
            StopAsync = false;
            object value = GetAsyncKUD(sharedClient);
        }

        private void btnGetKudo_Click(object sender, EventArgs e)
        {
            RunGetKudo();
        }

        private void MakeKUtbl_Click(object sender, EventArgs e)
        {
            string sOut = "";
            bool bFound;  

            string rawSrc = WhereSrc + "..\\HPSiteMap\\SiteMapRaw.txt";
            if(File.Exists(rawSrc))
            {
                StreamReader sr = new StreamReader(rawSrc);
                string line = sr.ReadLine();
                while (line != null)
                {
                    while (line == "")
                    {
                        line = sr.ReadLine();
                        if (line == null) break;
                    }
                    if (line == null) break;

                    bFound = true;
                    for(int j = 0; j < sTerm.Length; j++)
                    {
                        int m = sTerm[j].Length;
                        int i = line.IndexOf(sTerm[j]);
                        if (i < 0)
                        {
                            bFound = false;
                            continue;
                        }
                        string T5 = line.Substring(0, i);
                        string idBoard = line.Substring(i + m);
                        sOut += T5 + "," + idBoard + Environment.NewLine;
                        line = sr.ReadLine();
                        break;
                    }
                    if(!bFound)
                        Debug.Assert(false);
                }
                sr.Close();
                string pathKUtable = WhereExpert + "T5Folders.txt";
                File.WriteAllText(pathKUtable, sOut);
            }            

        }

        private void btnDelContacts_Click(object sender, EventArgs e)
        {
            File.Delete(sContactList);
            Application.DoEvents();
            Application.Exit();
        }

        private void RunKudoRef()
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            bAsyncDone = false;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            int dgvE_row = selectedRow.Index;
            string id = dgvExpert.Rows[dgvE_row].Cells[2].Value.ToString();
            lbCurrentVol.Text = dgvExpert.Rows[dgvE_row].Cells[1].Value.ToString();
            KudoRcvd.Init(id, Cutoff);
            WhichPB = 5;
            StartingTasks();
            FormTaskKR(id, Cutoff, sR);
        }

        private void FormTaskKR(string id, DateTime Cutoff, string sR)
        {
            TimerAcnt.Enabled = true;            
            pbGetSol.Maximum = KudoRcvd.UrlList.Count+1;
            object value = GetAsyncKR(sharedClient);
        }

        static async Task GetAsyncKR(HttpClient httpClient)
        {
            string jsonResponse;
            KudoRcvd.bTaskBusy = true;
            KudoRcvd.bTaskDone = false;
            KudoRcvd.RcvdList.Clear();
            sol_progress = 0;
            foreach (string sUrl in KudoRcvd.UrlList)
            {
                sol_progress++;
                try
                {
                    using HttpResponseMessage response = await httpClient.GetAsync(sR + sUrl);
                    jsonResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return;
                }
                sol_rtnval = KudoRcvd.KudoParse(ref jsonResponse);
                if (StopAsync) return;
            }
            KudoRcvd.bTaskBusy = false;
        }

        private void btnKudoRef_Click(object sender, EventArgs e)
        {
            StopAsync = false;
            RunKudoRef();
        }

        private void btnDelBlanks_Click(object sender, EventArgs e)
        {
            int n = 0;
            List<int> RemoveList = new List<int>();
            int i = 0;
            foreach (cExpert ce in AuthorList)
            {
                //if ((ce.Spages == "" || ce.Spages == "-1") && (ce.Kpages == "" || ce.Kpages == "-1"))
                if ((ce.Spages == "0") && (ce.Kpages == "0"))
                    RemoveList.Add(i);
                i++;
            }
            n = RemoveList.Count;
            if (n > 0 )
            {
                dgvExpert.DataSource = null;
                for (i = n - 1; i >= 0; i--)
                {
                    AuthorList.RemoveAt(RemoveList[i]);
                }
                dgvExpert.DataSource = AuthorList;
                UpdateDGVlist();
                WriteContactsFromAuthor();
                for(i = 0; i < AuthorList.Count; i++)
                {
                    AuthorList[i].N = i + 1;
                    dgvExpert.Rows[i].Cells[0].Value = (i + 1).ToString();
                }
            }
        }
        private void WriteContactsFromAuthor()
        {
            string sOut = "";
            foreach(cExpert ce in AuthorList)
            {
                string s = ce.name + "," + ce.user_id + Environment.NewLine;
                sOut += s;
            }
            File.WriteAllText(sContactList, sOut);
        }

        private void btnGetRange_Click(object sender, EventArgs e)
        {
            int nValidSols = 0;
            string pathName = "";
            foreach (cExpert ce in AuthorList)
            {
                string id = ce.user_id;
                pathName = WhereExpert + id + "/solutions.txt";
                List<DateTime> dates = ReadDTLFromSOL(id, ref nValidSols);
                if (dates == null)
                {
                    ce.DatePosted = "none in interval";
                    continue;
                }
                if(dates.Count > 0)
                {
                    List<DateTime> sortedDates = dates.OrderByDescending(d => d).ToList();
                    int n = sortedDates.Count - 1;
                    string s = sortedDates[0].ToString("MM-dd-yyyy") + " to " + sortedDates[n].ToString("MM-dd-yyyy");
                    ce.DatePosted = s;
                    WriteDTLToFile(ref sortedDates,pathName);
                }
                else File.Delete(pathName);
            }
            dgvExpert.Invalidate(false);
            UpdateDGVlist();
        }


        private int StartFinishTask = -1;
        private bool bFinishTaskBusy = false;
        private void FinishTimer_Tick(object sender, EventArgs e)
        {
            if(bAsyncDone)
            {
                bAsyncDone = false;
                bFinishTaskBusy = true;
                switch (FinishTimer.Tag)
                {
                    case "SOL":
                        RunGetSol();
                        break;
                    case "KUDO":
                        RunGetKudo();
                        break;
                    case "REF":
                        RunKudoRef();
                        break;
                    case "ALL":
                        GetAllPosts();
                        break;
                }
            }
            else
            {
                if(!bFinishTaskBusy)
                {
                    StartFinishTask++;
                    if(StartFinishTask == AuthorList.Count)
                    {
                        FinishTimer.Enabled = false;
                        TaskFinished();
                        //FinishTimer.Dispose();
                        GroupsEnable(true);
                    }
                    else
                    {
                        dgvExpert.ClearSelection();
                        dgvExpert.Rows[StartFinishTask].Selected = true;
                        bAsyncDone = true;
                    }
                }
            }
        }

        private void btnFinishSol_Click(object sender, EventArgs e)
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            StartFinishTask = selectedRow.Index;
            bAsyncDone = true;
            FinishTimer.Tag = "SOL";
            StartingTasks();
            FinishTimer.Enabled = true; // must come after startingtasks()
        }

        private void btnFinishRef_Click(object sender, EventArgs e)
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            StartFinishTask = selectedRow.Index;
            bAsyncDone = true;
            FinishTimer.Tag = "REF";
            FinishTimer.Enabled = true;
        }

        private void btnFinishKudo_Click(object sender, EventArgs e)
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            StartFinishTask = selectedRow.Index;
            bAsyncDone = true;
            FinishTimer.Tag = "KUDO";
            StartingTasks();
            FinishTimer.Enabled = true;
        }

        static bool[] ColSortDirection = new bool[6] { false, false, false, false, false, false }; // true is descending false is ascending
        private void SortTable(int c, bool b)
        {
            ColSortDirection[c] = b;
            SortTable(c);
        }
        private void SortTable(int c)
        {
            if (c > 5) return;
            bool b;
            b = ColSortDirection[c];
            ColSortDirection[c] = !b;
            SortTools.SortAuthorField((EAuthor)c, b, ref AuthorList);
            dgvExpert.Invalidate(false);
        }

        private void dgvExpert_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortTable(e.ColumnIndex-1);
        }

        private void btnScatPlot_Click(object sender, EventArgs e)
        {
            SortTable(5,false);
            List<bool> selectedRow = new List<bool>();
            int i, n = AuthorList.Count;
            for (i = 0; i < n; i++)
                selectedRow.Add(false);

            n = dgvExpert.SelectedRows.Count;
            if (n > 0)
            {
                for(i = 0; i < n; i++)
                {
                    int j = dgvExpert.SelectedRows[i].Index;
                    selectedRow[j] = true;
                }
            }
            else
            {
                i = 0;
                foreach (DataGridViewRow row in dgvExpert.Rows)
                {
                    int j = Convert.ToInt32(ExpertStats.global.AuthorList[i].Spoints);
                    selectedRow[i] =  j > 9;
                    i++;
                }
            }
            ScatterForm PlotScatter = new ScatterForm( selectedRow, Cutoff);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }

        private int FetchAllPostsInfo()
        {
            int cnt = 0;
            foreach(cExpert ce in AuthorList)
            {
                string filePath = WhereExpert + ce.user_id + "/AllPosts.txt";
                if (File.Exists(filePath))
                {
                    string s = File.ReadAllText(filePath);
                    string[] sS = s.Split(',');
                    ce.TotalPosts = sS[0];
                    ce.TotalSolutions = sS[1];
                    ce.Kudoed = sS[2];
                    ce.ktt_days = Convert.ToInt32(sS[3]);
                    cnt++;
                }
                else ce.ktt_days = 0;
            }
            return cnt;
        }

        private void btnHist_Click(object sender, EventArgs e)
        {
            List<bool> selectedRow = new List<bool>();
            int i = 0;
            SortTable(5,false);
            if(dgvExpert.SelectedRows.Count == 0)
            {
                foreach (DataGridViewRow row in dgvExpert.Rows)
                {
                    bool r =  AuthorList[i].Kpoints.Length > 1;
                    selectedRow.Add(r);
                    i++;
                }
            }
            else
            {
                i = 0;
                foreach (DataGridViewRow row in dgvExpert.Rows)
                {
                    selectedRow.Add((bool)row.Selected);
                }
            }
            if(FetchAllPostsInfo() > 0)
            {
                TPChart HistChart = new TPChart(selectedRow, tbYear.Text);
                HistChart.ShowDialog();
                HistChart.Dispose();
            }
        }

        private void btnDelExpert_Click(object sender, EventArgs e)
        {
            File.Delete(sExpertList);
        }

        private void btnGetAllPosts_Click(object sender, EventArgs e)
        {
            GetAllPosts();
        }

        private void GetAllPosts()
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            bAsyncDone = false;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            int dgvE_row = selectedRow.Index;            
            string id = dgvExpert.Rows[dgvE_row].Cells[2].Value.ToString();
            string filePath = WhereExpert + id + "/AllPosts.txt";
            //if (File.Exists(filePath)) return;
            WebForm wb = new WebForm(dgvE_row, Cutoff); 
            wb.ShowDialog();
            wb.Dispose();
            bFinishTaskBusy = false;
        }

        private void btnAutoPost_Click(object sender, EventArgs e)
        {
            if (dgvExpert.SelectedRows.Count <= 0) return;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            StartFinishTask = selectedRow.Index;
            bAsyncDone = true;
            GroupsEnable(false);
            FinishTimer.Tag = "ALL";
            FinishTimer.Enabled = true;
        }

        private bool bStillRunning;
        private void StartingTasks()
        {
            bStillRunning = true;
            if (FinishTimer.Enabled) return;
            GroupsEnable(false);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            FinishTimer.Stop();
            FinishTimer.Enabled = false;
            StopAsync = true;
            pbAuthors.Value = 0;
            TimerAcnt.Stop();
            TimerAcnt.Enabled = false;
            Application.DoEvents();
            if (Counting_SK)
            {
                UpdateDGVlist();
                dgvExpert.Invalidate(false);
            }
            GroupsEnable(true);
        }

        private void GroupsEnable(bool b)
        {
            groupBox1.Enabled = b;
            groupBox2.Enabled = b;
            groupBox3.Enabled = b;
            groupBox4.Enabled = b;
            //btnStop.Enabled =   !b;
            // the below does not work with windows timers, need the threading timer
            // or system.timers.system 
            //dgvExpert.Enabled = !b;
        }
        private void TaskFinished()
        {
            if (FinishTimer.Enabled) return;
            bool b = true;
            bStillRunning = false;
            groupBox1.Enabled = b;
            groupBox2.Enabled = b;
            groupBox3.Enabled = b;
            groupBox4.Enabled = b;
        }

        private void ExpertInfo_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            help MainHelp = new help();
            MainHelp.ShowDialog();
            MainHelp.Dispose();
        }

        private void tbYear_TextChanged(object sender, EventArgs e)
        {
            if(iStartUp)
            {
                iStartUp = false;
                return;
            }
            btnUseDate.Enabled = true;
            lbRestart.Visible = true;
        }

        private void btnUseDate_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Application.Exit();
        }

        private void btnClearCts_Click(object sender, EventArgs e)
        {
            List<int> ClearedRows = new List<int>();
            int i = 0;
            if (dgvExpert.SelectedRows.Count == 0)
            {
                foreach(DataGridViewRow Row in dgvExpert.Rows)
                {
                    Row.Cells[3].Value = "-1";
                    Row.Cells[5].Value = "-1";
                    ClearedRows.Add(i);
                }
            }
            else
            {
                foreach(DataGridViewRow Row in dgvExpert.SelectedRows)
                {
                    Row.Cells[3].Value = "-1";
                    Row.Cells[5].Value = "-1";
                    ClearedRows.Add(Row.Index);
                }    
                dgvExpert.ClearSelection();
            }
            foreach(int j in ClearedRows)
            {
                AuthorList[j].Spages = "-1";
                AuthorList[j].Kpages = "-1";
            }
            UpdateDGVlist();
        }
    }
}
