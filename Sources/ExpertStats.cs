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

        static string sPagesTotal = "";
        static string kPagesTotal = "";


        static string sWE = "";
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


        static string spanT0 = "data-lia-message-uid=";
        static string spanT1 = "<span class=\"DateTime\">";
        static string spanT2 = "<span class=\"local-date\">";
        static string spanT3 = "<span class=\"local-time\">";
        static string spanTe = "</span>";

        //private int dgvE_row = -1;
        //private int dgvE_col = -1;

        private cSortTools SortTools = new cSortTools();


        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri(hpBase),
        };


        public ExpertInfo()
        {
            InitializeComponent();
            string dts = tbYear.Text.Trim() + " " + tbCutTime.Text.Trim();
            Cutoff = DateTime.Now;
            bool b = dtHPtime(dts, ref Cutoff);
            if (!b)
            {
                MessageBox.Show("date time bad " + dts);
                return;
            }
            bool isDebug = Debugger.IsAttached;
            if (!isDebug)
            {
                //App is running in release mode
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                WhereExe = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + "\\";
                WhereExpert = WhereExe + "HPexperts\\";
                lbWhereExe.Text = WhereExpert;

/*
        How to create a folder at the executable location so that the release app can run on my PC:
        mklink /D "C:\Users\josep\source\repos\ExpertStats\bin\x64\Release\HPexperts" "C:\Users\josep\source\repos\ExpertStats\HP
        use rmdir to remove 
*/
            }
            KudoBaseTool = new cKudoBase();
            Start();
        }
        

        private void Start()
        {
            sWE = WhereExpert + sContactList;
            sContactList = sWE;
            sWE = WhereExpert + sExpertList;
            sExpertList = sWE;
            KudoBaseTool.FillBase();
            if (File.Exists(sContactList))
            {
                ReadContacts();
                if (File.Exists(sExpertList))
                {
                    ReadExperts();
                }
            }
            else
            {
                WritingContacts = true;
                WhichPB = 4;
                TimerAcnt.Enabled = true;
                object value = GetAsync(sharedClient);
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
            TimerAcnt.Enabled = true;
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
            string sOut = "";
            int n = 0;
            string jsonResponse="";
            File.Delete(sContactList);
            for (int i = 1; i < 4; i++)
            {
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
                add_id_name(ref jsonResponse, ref sOut);
            }
            File.WriteAllText(sContactList, sOut);
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
                iID = skcnt + 1;
                sID = "s" + iID.ToString();
                if (ce.Spages != "-1" && ce.Spages != "")
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
                    if (jsonResponse.Contains("No Posts Found"))
                        nPages = "";
                    else nPages = "1";
                }
                else
                {
                    i += SolLast.Length;
                    int j = jsonResponse.IndexOf("\">", i);
                    Debug.Assert(j != -1);
                    nPages = jsonResponse.Substring(i, j - i);
                }
                ce.Spages = nPages;
                skcnt++;
            }
            skcnt = AuthorList.Count;
            WritingContacts = false;
        }

        static async Task GetAsyncK(HttpClient httpClient)
        {
            string nPages;
            string jsonResponse;
            skout = "";
            skcnt = -1;
            sPagesTotal = "";
            kud_rtnval = 0;
            foreach (cExpert ce in AuthorList)
            {
                skcnt++;
                string id = ce.user_id;
                int iID = skcnt;
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
                    nPages = "";
                }
                else
                {
                    i += SolLast.Length;
                    int j = jsonResponse.IndexOf("\">", i);
                    Debug.Assert(j != -1);
                    nPages = jsonResponse.Substring(i, j - i);
                }
                ce.Kpages = nPages;
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

        private void TimerAcnt_Tick(object sender, EventArgs e)
        {
            switch (WhichPB)
            {
                case 0:
                    pbAuthors.Value = skcnt;
                    if (!WritingContacts)
                    {
                        TimerAcnt.Stop();
                        TimerAcnt.Dispose();
                        pbAuthors.Value = 0;
                        dgvExpert.Invalidate(false);
                        UpdateDGVlist();
                    }
                    break;
                case 1:
                    pbAuthors.Value = skcnt;
                    if (kud_rtnval != 0)
                    {
                        TimerAcnt.Stop();
                        TimerAcnt.Dispose();
                        pbAuthors.Value = 0;
                        dgvExpert.Invalidate(false);
                        UpdateDGVlist();
                    }
                    break;
                case 2:
                    pbGetSol.Value = sol_progress;
                    lbCnt.Text = sol_progress.ToString();
                    if (sol_rtnval != 0)
                    {
                        TimerAcnt.Stop();
                        TimerAcnt.Dispose();
                        SaveSolutions(sol_id, ref sol_Solutions);
                        UpdateRawSolCnt(sol_id, sol_Solutions.Count);
                        pbGetSol.Value = 0;
                        bFinishTaskBusy = false;
                    }
                    break;
                case 3:
                    pbGetSol.Value = KudoBaseTool.BusyPage;
                    lbCnt.Text = sol_progress.ToString();
                    if(!KudoBaseTool.Busy || sol_rtnval != 0)
                    {
                        TimerAcnt.Stop();
                        TimerAcnt.Dispose();
                        SaveKudoUrls(sol_id);
                        UpdateRawKudoCnt(sol_id, KudoBaseTool.nKudosTotal);
                        pbGetSol.Value = 0;
                        bFinishTaskBusy = false;
                    }
                    break;
                 case 4:
                    pbAuthors.Value = skcnt;
                    if (!WritingContacts)
                    {
                        TimerAcnt.Stop();
                        TimerAcnt.Dispose();
                        ReadContacts();
                        pbAuthors.Value = 0;
                        if (File.Exists(sExpertList))
                        {
                            ReadExperts();
                        }
                    }
                    break;
                case 5:
                    pbGetSol.Value = KudoRcvd.nUrls;
                    lbCnt.Text = sol_progress.ToString();
                    if (!KudoRcvd.bTaskBusy)
                    {
                        TimerAcnt.Stop();
                        TimerAcnt.Dispose();
                        KudoRcvd.SaveKudoList();
                        KudoRcvd.bTaskDone = true;
                        bFinishTaskBusy = false;
                        pbGetSol.Value = 0;
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
            WhichPB = 1;
            TimerAcnt.Enabled = true;
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



        private void FormTaskS(string id, int nPageMax, DateTime Cutoff, string sG)
        {
            sol_id = id;
            sol_nPageMax = nPageMax;
            sol_sG = sG;
            sol_rtnval = 0;
            sol_Solutions.Clear();
            TimerAcnt.Enabled = true;
            object value = GetAsyncSOL(sharedClient);
        }

        private void FormTaskK(string id, int nPageMax, DateTime Cutoff, string sG)
        {
            sol_id = id;
            sol_nPageMax = nPageMax;
            sol_sG = sG;
            sol_rtnval = 0;
            sol_Solutions.Clear();
            TimerAcnt.Enabled = true;
            object value = GetAsyncKUD(sharedClient);
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
                sol_progress = i + 1;
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
                    sol_rtnval = ExtractDT(ref jsonResponse, j);
                    n = sol_Solutions.Count;
                    if (sol_rtnval == 0) break;
                    if (sol_rtnval == 1) return;
                    if (sol_rtnval < 0) return;
                }
                if(i == sol_nPageMax -1)
                {
                    sol_rtnval = 1;
                }
            }
        }

        // 0 out of data
        // 1 out of time
        static int ExtractDT(ref string s, int iStart)
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
            //long l = ConvertToTicks(dtString);

            sol_Solutions.Add(dateTime);
            iStart = j + spanTe.Length;

            if (dateTime < Cutoff) return 1;
            return ExtractDT(ref s, iStart);
        }

        private void RunGetSol()
        {
            int nValidSols = 0;
            if (dgvExpert.SelectedRows.Count <= 0) return;
            bAsyncDone = false;
            DataGridViewRow selectedRow = dgvExpert.SelectedRows[0];
            int dgvE_row = selectedRow.Index;
            string sE = dgvExpert.Rows[dgvE_row].Cells[4].Value.ToString();
            if (!(sE == "0" | sE == "") &&  !cbSolCalculate.Checked) return;
            string id = dgvExpert.Rows[dgvE_row].Cells[2].Value.ToString();
            List<DateTime> OLDdateTimeList = ReadDTLFromSOL(id, ref nValidSols);
            int nPageMax = Convert.ToInt32(dgvExpert.Rows[dgvE_row].Cells[3].Value);
            if (nPageMax > 500) nPageMax = 500;
            string sGetSolPage0 = sU + id + "/page/";
            WhichPB = 2;
            pbGetSol.Maximum = nPageMax;
            pbGetSol.Value = 0;
            FormTaskS(id, nPageMax, Cutoff, sGetSolPage0);
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
            //List<DateTime> OLDdateTimeList = ReadDTLFromFile(id);
            DateTime Cutoff = DateTime.Now;
            int nPageMax = Convert.ToInt32(dgvExpert.Rows[dgvE_row].Cells[5].Value);
            if (nPageMax > 500) nPageMax = 500;
            string sGetKudPage0 = sK + id + "/tab/my-kudoed-messages/page/";
            pbGetSol.Maximum = nPageMax;
            pbGetSol.Value = 0;
            FormTaskK(id, nPageMax, Cutoff, sGetKudPage0);
        }

        private void btnGetKudo_Click(object sender, EventArgs e)
        {
            RunGetKudo();
        }

        private void MakeKUtbl_Click(object sender, EventArgs e)
        {
            string sOut = "";
            string[] sTerm = { "/ct-p/", "/bd-p/", "/tkb-p/", "/bg-p/" };
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

                    bFound = false;
                    for(int j = 0; j < sTerm.Length; j++)
                    {
                        int m = sTerm[j].Length;
                        int i = line.IndexOf(sTerm[j]);
                        if (i < 0) continue;
                        string T5 = line.Substring(0, i);
                        string idBoard = line.Substring(i + m);
                        sOut += T5 + "," + idBoard + Environment.NewLine;
                        line = sr.ReadLine();
                        bFound = true;
                        break;
                    }
                    if(!bFound)
                        Debug.Assert(false);
                }
                sr.Close();
            }
            string pathKUtable = WhereExpert + "T5Folders.txt";
            File.WriteAllText(pathKUtable, sOut);
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
            KudoRcvd.Init(id, Cutoff);
            WhichPB = 5;
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
            }
            KudoRcvd.bTaskBusy = false;
        }

        private void btnKudoRef_Click(object sender, EventArgs e)
        {
            RunKudoRef();
        }

        private void btnDelBlanks_Click(object sender, EventArgs e)
        {
            int n = 0;
            List<int> RemoveList = new List<int>();
            int i = 0;
            foreach (cExpert ce in AuthorList)
            {
                if ((ce.Spages == "" || ce.Spages == "-1") && (ce.Kpages == "" || ce.Kpages == "-1"))
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

            foreach (cExpert ce in AuthorList)
            {
                string id = ce.user_id;
                List<DateTime> dates = ReadDTLFromSOL(id, ref nValidSols);
                List<DateTime> sortedDates = dates.OrderByDescending(d => d).ToList();
                int n = sortedDates.Count-1;
                string s = sortedDates[0].ToString("MM-dd-yyyy") + " to " + sortedDates[n].ToString("MM-dd-yyyy");
                ce.DatePosted = s;
                WriteDTLToFile(ref sortedDates, WhereExpert + id + "/solutions.txt");
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
                        FinishTimer.Dispose();
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
            FinishTimer.Enabled = true;
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
                    int j = Convert.ToInt32(AuthorList[i].Spoints);
                    selectedRow[i] =  j > 9;
                    i++;
                }
            }
            ScatterForm PlotScatter = new ScatterForm( ref selectedRow, Cutoff);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }

        private void FetchAllPostsInfo()
        {
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
                }
                else ce.ktt_days = 0;
            }
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
                    row.Selected =  AuthorList[i].Spoints.Length > 1;
                    selectedRow.Add(row.Selected);
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
            FetchAllPostsInfo();
            TPChart HistChart = new TPChart(ref selectedRow, tbYear.Text);
            HistChart.ShowDialog();
            HistChart.Dispose();
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
            FinishTimer.Tag = "ALL";
            FinishTimer.Enabled = true;
        }
    }
}
