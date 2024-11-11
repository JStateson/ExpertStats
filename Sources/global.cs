using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;
using System.Windows.Forms.Automation;
using static ExpertStats.global;
using static ExpertStats.global.cKudoBase;

namespace ExpertStats
{
    public class global
    {
        //kudos/messagepage/board-id/
        public static string WhereExe = "C:\\Users\\josep\\source\\repos\\ExpertStats\\";
        public static DateTime Cutoff = DateTime.Now;
        public static string WhereSrc = "C:\\Users\\josep\\source\\repos\\ExpertStats\\";
        public static string sR = "kudos/messagepage/board-id/";// "https://h30434.www3.hp.com/t5/kudos/messagepage/board-id/";
        public static string WhereExpert = WhereExe + "HPexperts\\";
        public static string apLookupSolution = "lia-fa-solution";    // use to count solutions
        public static string apStartMessage = "class=\"lia-message-view-wrapper lia-js-data-messageUid-";
        public static string apLookupKudo = "class=\"lia-link-navigation lia-kudos-count-link\" aria-label=\"Kudos Count ";
        public static string apExpectPage  = "lia-link-navigation lia-custom-event\" aria-label=\"Page ";
        public static string apPageCountS = "<div class='MessageSubjectIcons '>";
        public static string apBaseLookup = "user/viewprofilepage/user-id/";
        public static string hpBase = "https://h30434.www3.hp.com/t5/";
        public static string[] sTerm = { "/ct-p/", "/bd-p/", "/tkb-p/", "/bg-p/" };

        //https://h30434.www3.hp.com/t5/user/viewprofilepage/user-id/178540/page/1


        public static cKudoRcvd KudoRcvd = new cKudoRcvd();
        public static List<cExpert> AuthorList = new List<cExpert>();

        /*
         finding which kudos are on a solution
        a = start of solution lookup string "lia-fa-solution"
        b = setart of kudo lookup string "MessageKudosCount lia-component-kudos-count lia-component-message-view-widget-kudos-count"
        x-there seems to be 111 bytes between them and the kudo follows the solution
        x-try 113 for clue they are matched
        if 20 enteries then 20 "<div class="MessageSubjectIcons\">"
        "local-date" and "local-time" are in pairs and repeat 20 times for a full page (total of 40)
        between the first local-date and the second local-date a paired sol + kudo can show up 
        1 count entries for page an allocate.  signal done if less than 20
        2 get the datetime and the subsequent datetime: locations i,j
        3 look for kudo and for solution between i,j
        4 repeat for entry
         * */


        public enum EAuthor
        {
            name,
            user_id,
            Spages,
            Spoints,
            Kpages,
            kpoints
        }

        public static string FormAllPostUrl(string id, int iPage)
        {
            return apBaseLookup + id + "/Page/" + iPage.ToString();
        }

        public class cKudoRcvd
        {
            private string user_id;
            private string filePath;
            private DateTime dtTemp = DateTime.Now;
            public bool bTaskBusy;
            public bool bTaskDone;
            private DateTime Cutoff;
            public int nUrls;
            public int nLimitUsers;
            public List<int>TotalWeight = new List<int>();
            public List<int>ExpertWeight = new List<int>();
            public class cOneKudo
            {
                public string iD;
                public int nWeight;
                public DateTime kDT;
            }

            public List<cOneKudo> RcvdList = new List<cOneKudo>();
            public List<List<cOneKudo>> AllRcvd = new List<List<cOneKudo>>();
            public List<string> UrlList = new List<string>();
            private int UserIndex;
            private List<string> PeerIDs;
            public bool Init(string userID, DateTime rCutoff)
            {
                user_id = userID;
                nUrls = 0;
                int i, j, k;
                bTaskBusy = true;
                bTaskDone = false;
                Cutoff = rCutoff;
                filePath = WhereExpert + user_id + "/KudoUrls.txt";
                if (!File.Exists(filePath)) return false;
                UrlList.Clear();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        i = line.IndexOf(sR);
                        if (i < 0) i = 0;
                        else i += sR.Length;                          
                        j = line.IndexOf(',', i);
                        if (j < 0) k = line.Length - i;
                        else k = j - i;
                        UrlList.Add(line.Substring(i, k));
                    }
                }
                return true;
            }
            public void InitHist(int nUsers)
            {
                AllRcvd.Clear();
                RcvdList.Clear();
                TotalWeight.Clear();
                ExpertWeight.Clear();
                PeerIDs = null;
                for(int i = 0; i < nUsers; i++)
                {
                    TotalWeight.Add(0);
                    ExpertWeight.Add(0);
                }
                nLimitUsers = nUsers;
                UserIndex = -1;
            }

            public void SetPeerIDs(ref List<string> peers)
            {
                PeerIDs = peers;
            }

            private bool bPeerLookup(string id)
            {
                foreach(string s in PeerIDs)
                {
                    if(id ==  s) return true;
                }
                return false;
            }

            public bool ReadRcvdList(string id)
            {                
                filePath = WhereExpert + id + "/KudosRcvdList.txt";
                bool bUsePeer = false;
                if(PeerIDs != null)
                    bUsePeer = PeerIDs.Count > 0;
                if (File.Exists(filePath))
                {
                    DateTime now = DateTime.Now;
                    RcvdList = new List<cOneKudo>();
                    string[] sIn = File.ReadAllLines(filePath);
                    UserIndex++;
                    foreach (string line in sIn)
                    {
                        string[] aLine = line.Split(',');
                        cOneKudo oK = new cOneKudo();
                        oK.iD = aLine[0];
                        oK.nWeight = Convert.ToInt32(aLine[1]);
                        if(bUsePeer)
                        {
                            bool bPeer = bPeerLookup(oK.iD);
                            if (bPeer || oK.nWeight > 1) ExpertWeight[UserIndex] += oK.nWeight;
                            else
                            {
                                TotalWeight[UserIndex] += oK.nWeight;
                            }
                        }
                        else
                        {
                            
                            if (oK.nWeight > 1)
                                ExpertWeight[UserIndex] += oK.nWeight;
                            else
                                TotalWeight[UserIndex] += oK.nWeight;
                        }
                        bool b = dtHPtime(aLine[2], ref dtTemp);
                        if (!b)
                        {
                            return false;
                        }
                        oK.kDT = dtTemp;
                        RcvdList.Add(oK);
                    }
                }
                AllRcvd.Add(RcvdList);
                return true;
            }
            public void SaveKudoList()
            {
                int i = 0;
                int n = RcvdList.Count;
                int[] SI = new int[n];
                filePath = WhereExpert + user_id + "/KudosRcvdList.txt";
                DateTime[] dtS = new DateTime[n];
                foreach (cOneKudo oK in RcvdList)
                {
                    dtS[i] = oK.kDT;
                    i++;
                }
                SI = dtS
.Select((value, index) => new { Value = value, Index = index })
.OrderByDescending(item => item.Value)
.Select(item => item.Index)
.ToArray();
                string sOut = "";
                i=1;
                for(int j = 0; j < n; j++)
                {
                    cOneKudo oK = RcvdList[SI[j]];
                    sOut += oK.iD + ",";
                    sOut += oK.nWeight.ToString() + ",";
                    sOut += sHPdt(oK.kDT) + ((i < n) ? Environment.NewLine : "");
                    i++;
                }
                File.WriteAllText(filePath,sOut);
            }
            public bool Add(string iD,  string nWeight, string kDT)
            {
                
                bool b = dtHPtime(kDT,ref dtTemp);
                if(!b)
                {
                    return false;
                }
                cOneKudo oK1 = new cOneKudo();
                oK1.iD = iD;
                oK1.nWeight = Convert.ToInt32(nWeight);
                oK1.kDT = dtTemp;
                RcvdList.Add(oK1);
                return true;
                /*
                foreach(cOneKudo oK in RcvdList)
                {
                    if (oK.iD != iD) continue;
                    if (oK.kDT != dtTemp) continue;
                    cOneKudo oK1 = new cOneKudo();
                    oK1.iD = iD;
                    oK1.nWeight = Convert.ToInt32(nWeight);
                    oK1.kDT = dtTemp;
                    RcvdList.Add(oK1);
                    return true;
                }
                return true;
                */
            }

            public int KudoParse(ref string s)
            {
                int iS = 0;
                nUrls++;
                return Parse(ref s, iS);
            }
            private string kP1 = "href=\"https://h30434.www3.hp.com/t5/user/viewprofilepage/user-id/";
            private string kP1e = "\">";
            private string kP2 = "<span class=\"base-count-number\">";
            private string kP2e = "</span>";
            //string WTF = "<span class=\"local-date\">‎";
            private string kP3 = "<span class=\"local-date\">";
            private string kP3e = "</span>";
            private cOneKudo myOK;
            private int Parse(ref string s, int iS)
            {
                int i=0, j;
                if (iS >= s.Length) return 0;
                i = s.IndexOf(kP1, iS);
                if (i < 0) return 0;
                iS = i + kP1.Length;
                j = s.IndexOf(kP1e, iS);
                if (j < 0) return -1;
                string sID = s.Substring(iS, j - iS);

                iS = j + kP1e.Length;
                i = s.IndexOf(kP2, iS);
                if(i < 0) return -1;
                iS = i + kP2.Length;
                j = s.IndexOf(kP2e, iS);
                if(j < 0) return -1;
                string sW = s.Substring(iS, j - iS);

                iS = j + kP2e.Length;
                i = s.IndexOf(kP3, iS);
                if (i < 0)return -1;
                iS = i + kP3.Length;
                j = s.IndexOf(kP3e, iS);
                if (j < 0) return -1;
                string sD = s.Substring(iS, j - iS).Trim(); // this has no time, only date
                sD += " 12:00 AM";
                bool b = dtHPtime(sD, ref dtTemp);
                if(!b)return -1;
                Add(sID, sW, sD);
                iS = j + kP3e.Length;
                return Parse(ref s, iS);
            }
        }

        public class cKudoBase
        {
            private List<string> KudoBase = new List<string>();  
            private List<string> ReplBase = new List<string>();
            public string sOutTemp = "";
            public class cKitems
            {
                public int nKudos;
                public string sUrl;
                public string Pid;  // post id
                public string Kid;  // kudo id
                public DateTime dtKudo;
            }

            public List<cKitems> kitems = new List<cKitems>();
            public bool Busy; // if false then we are done
            public int BusyPage;    // starts page 0 .. number of kudo pages
            public int nKudosTotal;
           

            // for extracting KudoLink, number, date , time
            private string kU0 = "<a class=\"page-link lia-link-navigation lia-custom-event\"";
            private string kU1 = " href=\"/t5/";
            private string kU1e = "/";

            private string kP = "/m-p/";
            private string kPe = "\">";

            private string kU2 = "class=\"MessageKudosCount\">";
            private string kU2e = "</span>";
            private string kD0 = "<span class=\"local-date\">‎";
            private string kD0e = "</span>";
            private string kT0 = "<span class=\"local-time\">";
            private string kT0e = "</span>";

            private DateTime RecentInPage;  // most recent in a page as the date of an older is not updated
            // this is a kluge but it would be unusual for 12 old kudos to be recenty updated all sequentially
            private int NumberMostRecent; // if get to 12 then must be old page
            private DateTime dtTemp = DateTime.Now;
            private static DateTime EventStart = DateTime.Now;
            public bool FindKudo(string sKudo1, ref string KudoLoc)
            {
                string sKudo = "https://h30434.www3.hp.com/t5/" + sKudo1;
                int i = KudoBase.IndexOf(sKudo);
                if (i == -1) return false;
                KudoLoc = // "https://h30434.www3.hp.com/t5/kudos/messagepage/board-id/" +
                    ReplBase[i] + "/message-id/";
                return true;
            }
            private void AddBase(string sK, string sR)
            {
                KudoBase.Add(sK);
                ReplBase.Add(sR);
            }

            public bool FillBase()
            {
                string pathKUtable = WhereExpert + "T5Folders.txt";
                if(!File.Exists(pathKUtable))
                {
                    sOutTemp = "";
                    MessageBox.Show("You must have the VS2022 project HPSiteMap\r\nAn old one will be created at " + pathKUtable);
                    string sOld = "https://h30434.www3.hp.com/t5/Notebooks,Notebook\r\nhttps://h30434.www3.hp.com/t5/Notebook-Video-Display-and-Touch,Display\r\nhttps://h30434.www3.hp.com/t5/Business-Notebooks,General\r\nhttps://h30434.www3.hp.com/t5/Notebook-Hardware-and-Upgrade-Questions,Hardware\r\nhttps://h30434.www3.hp.com/t5/Notebook-Wireless-and-Networking,Internet\r\nhttps://h30434.www3.hp.com/t5/Notebook-Operating-System-and-Recovery,OS\r\nhttps://h30434.www3.hp.com/t5/Notebook-Audio,Sound\r\nhttps://h30434.www3.hp.com/t5/Notebook-Boot-and-Lockup,Laptop-Freeze\r\nhttps://h30434.www3.hp.com/t5/Notebook-Software-and-How-To-Questions,Recovery\r\nhttps://h30434.www3.hp.com/t5/Printers,InkJet\r\nhttps://h30434.www3.hp.com/t5/Mobile-Printing-Cloud-Printing,Photo\r\nhttps://h30434.www3.hp.com/t5/Scanning-Faxing-Copying,Scan\r\nhttps://h30434.www3.hp.com/t5/Printer-Setup-Software-Drivers,Software\r\nhttps://h30434.www3.hp.com/t5/Printing-Errors-or-Lights-Stuck-Print-Jobs,PostPrint\r\nhttps://h30434.www3.hp.com/t5/DesignJet-Large-Format-Printers-Digital-Press,DesignJet-LargeFormat-DigitalPress\r\nhttps://h30434.www3.hp.com/t5/Sprocket,sprocket\r\nhttps://h30434.www3.hp.com/t5/Samsung,Samsung\r\nhttps://h30434.www3.hp.com/t5/Printers-Knowledge-Base,printers-knowledge-base\r\nhttps://h30434.www3.hp.com/t5/Printer-Wireless-Networking-Internet,Printer-Wireless-Networking-and-Internet\r\nhttps://h30434.www3.hp.com/t5/Printer-Ink-Cartridges-Print-Quality,Printer-Ink-Cartridges-and-Print-Quality\r\nhttps://h30434.www3.hp.com/t5/Printer-Paper-Jams-Feed-Issues,Printer-Paper-Jams-and-Feed-Issues\r\nhttps://h30434.www3.hp.com/t5/Desktops,DesktopPC\r\nhttps://h30434.www3.hp.com/t5/Desktop-Software-and-How-To-Questions,DesktopQA\r\nhttps://h30434.www3.hp.com/t5/Desktop-Hardware-and-Upgrade-Questions,HardwareDPC\r\nhttps://h30434.www3.hp.com/t5/Desktop-Audio,Monitor\r\nhttps://h30434.www3.hp.com/t5/Desktop-Operating-Systems-and-Recovery,OSandSW\r\nhttps://h30434.www3.hp.com/t5/Desktop-Video-Display-and-Touch,TouchSmartPC\r\nhttps://h30434.www3.hp.com/t5/Desktop-Boot-and-Lockup,lockups\r\nhttps://h30434.www3.hp.com/t5/Sprout-by-HP,Sprout\r\nhttps://h30434.www3.hp.com/t5/Business-PCs-Workstations-and-Point-of-Sale-Systems,Business-PC-Workstation-POS\r\nhttps://h30434.www3.hp.com/t5/Desktop-Wireless-and-Networking,DesktopWirelessNetworking\r\nhttps://h30434.www3.hp.com/t5/Desktop-Knowledge-Base,desktop-knowledge-base\r\nhttps://h30434.www3.hp.com/t5/Gaming,Gaming\r\nhttps://h30434.www3.hp.com/t5/Gaming-Notebooks,GamingNotebooks\r\nhttps://h30434.www3.hp.com/t5/Gaming-Desktops,GamingDesktops\r\nhttps://h30434.www3.hp.com/t5/Gaming-Accessories,GamingAccessories\r\nhttps://h30434.www3.hp.com/t5/Gaming-Knowledge-Base,gaming-knowledge-base\r\nhttps://h30434.www3.hp.com/t5/Apps-Services-Software,Apps-and-Services\r\nhttps://h30434.www3.hp.com/t5/HP-ZCentral-Remote-Boost-and-Connect,RGS\r\nhttps://h30434.www3.hp.com/t5/HP-Instant-Ink,instantink\r\nhttps://h30434.www3.hp.com/t5/Poly-Services,Poly_Services\r\nhttps://h30434.www3.hp.com/t5/Poly-Software,Poly_Software\r\nhttps://h30434.www3.hp.com/t5/Poly-Headsets,Headsets\r\nhttps://h30434.www3.hp.com/t5/Accessories,Headset_Accessories\r\nhttps://h30434.www3.hp.com/t5/Aviation-Specials,Aviation_and_Specials\r\nhttps://h30434.www3.hp.com/t5/Bluetooth-Headsets-Earbuds,Bluetooth_Headsets\r\nhttps://h30434.www3.hp.com/t5/Contact-Center-Headsets,Call_Center_Headsets\r\nhttps://h30434.www3.hp.com/t5/DECT-Wireless-Headsets,DECT_Wireless_Headsets\r\nhttps://h30434.www3.hp.com/t5/USB-Headsets,USB_Headsets\r\nhttps://h30434.www3.hp.com/t5/Poly-Phones,Phones\r\nhttps://h30434.www3.hp.com/t5/Accessories,Phone_Accessories\r\nhttps://h30434.www3.hp.com/t5/UC-Platforms,UC_Platforms\r\nhttps://h30434.www3.hp.com/t5/Desk-and-IP-Conference-Phones,Desk_IP_Conference_Phones\r\nhttps://h30434.www3.hp.com/t5/ATA-VoIP-Adapters,ATA_VoIP_Adapters\r\nhttps://h30434.www3.hp.com/t5/USB-Bluetooth-Speakerphones,USB_Bluetooth_Speakerphones\r\nhttps://h30434.www3.hp.com/t5/PSTN,PSTN\r\nhttps://h30434.www3.hp.com/t5/Wireless-Phone-Solutions,Wireless_Phone_Solutions\r\nhttps://h30434.www3.hp.com/t5/Poly-Video-Conferencing,Video_Conferencing\r\nhttps://h30434.www3.hp.com/t5/Accessories-Cameras,Video_Conferencing_Accessories\r\nhttps://h30434.www3.hp.com/t5/Meeting-Room-Solutions,Meeting_Room_Solutions\r\nhttps://h30434.www3.hp.com/t5/Webcams,Webcams\r\nhttps://h30434.www3.hp.com/t5/Collaboration-Conferencing-Platforms,Collaboration_Conferencing_Platforms\r\nhttps://h30434.www3.hp.com/t5/Poly-International,Poly_International\r\nhttps://h30434.www3.hp.com/t5/Chinese-%E4%B8%AD%E6%96%87,Chinese\r\nhttps://h30434.www3.hp.com/t5/HP-Expert-Center,expert\r\nhttps://h30434.www3.hp.com/t5/Inner-Circle,VIP\r\nhttps://h30434.www3.hp.com/t5/Meet-the-Experts,meetexperts\r\nhttps://h30434.www3.hp.com/t5/Insider-News-and-Events,expertevents\r\nhttps://h30434.www3.hp.com/t5/Technical-Briefings-and-Training-Material,Training_and_Tech_Briefings\r\nhttps://h30434.www3.hp.com/t5/Ideas-and-suggestions,ideas\r\nhttps://h30434.www3.hp.com/t5/Meet-HP-Team,meetteam\r\nhttps://h30434.www3.hp.com/t5/Guidelines-and-Best-Practices,guidelines\r\nhttps://h30434.www3.hp.com/t5/Expert-Portal,expertportal\r\nhttps://h30434.www3.hp.com/t5/Welcome-to-the-Community,about\r\nhttps://h30434.www3.hp.com/t5/First-Time-Here-Learn-How-to-Post-and-More,Hello\r\nhttps://h30434.www3.hp.com/t5/Community-Blog,Bestof\r\nhttps://h30434.www3.hp.com/t5/Private-Boards,Private\r\n";
                    File.WriteAllText(pathKUtable, sOld);
                }
                StreamReader sr = new StreamReader(pathKUtable);
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null) break;
                    if (line == "") break;
                    string[] sL = line.Split(',');
                    AddBase(sL[0], sL[1]);
                }
                return true;
            } 

            public void ParseInit()
            {
                kitems.Clear();
                Busy = true;
                BusyPage = 0;
                nKudosTotal = 0;
            }

            public int ParseKudoPage(ref string aPage)
            {
                int iStart = 0;
                BusyPage++;
                NumberMostRecent = 0;
                int RtnCod = ExtractKP(ref aPage, iStart);

                if (RtnCod == -1)
                {
                    Busy = false;
                }
                else if (Cutoff > RecentInPage && NumberMostRecent > 10)
                {
                    Busy = false;
                    RtnCod = 1;
                }
                return RtnCod;
            }
            private int ExtractKP(ref string s, int iS)
            {
                int i, j;
                bool b;
                if (iS > s.Length) return 0;
                i = s.IndexOf(kU0, iS);  // get to the correct area
                if (i < 0) return 0;
                iS = i + kU0.Length;
                i = s.IndexOf(kU1, iS); // start of url
                if (i < 0) return -1;
                iS = i + kU1.Length;
                j = s.IndexOf(kU1e, iS); // terminator
                if(j<0) return -1;
                string sTXT = s.Substring(iS, j - iS);
                string sUrl = "";
                b = FindKudo(sTXT, ref sUrl);
                if(!b)  // this can happen for archived posts: Printers-Archive-Read-Only
                {
                    return 0;
                }

                iS = j + kU1e.Length;
                i = s.IndexOf(kP, iS);
                if(i<0) return -1;
                iS = i + kP.Length;
                j = s.IndexOf(kPe, iS);
                if(j<0) return -1;
                string sCode = s.Substring(iS, j - iS);  //9226577#M211515
                string[] sC = sCode.Split(new[] { "#M" }, StringSplitOptions.RemoveEmptyEntries);

                iS = j + kPe.Length;
                i = s.IndexOf(kU2, iS);
                if (i < 0) return -1;
                iS = i + kU2.Length;
                j = s.IndexOf(kU2e, iS);
                if (j < 0) return -1;
                string sN = s.Substring(iS, j - iS).Trim();
                int nN = int.Parse(sN);

                iS = j + kU2e.Length;
                i = s.IndexOf(kD0, iS);
                if( i < 0) return -1;
                iS = i + kD0.Length;
                j = s.IndexOf(kD0e, iS);
                if(j < 0) return -1;
                string sD = s.Substring(iS, j - iS);

                iS = j + kD0e.Length;
                i = s.IndexOf(kT0, iS);
                iS = i + kT0.Length;
                j = s.IndexOf(kT0e, iS);
                if(j< 0) return -1;
                string sT = s.Substring(iS, j - iS);
                string dtString = sD.Trim() + " " + sT.Trim();
                iS = j + kT0e.Length;

                DateTime dateTime = dtTemp;
                b = dtHPtime(dtString, ref dateTime);
                if (!b) return -1;

                cKitems ck = new cKitems();
                ck.nKudos = nN;
                ck.dtKudo = dateTime;
                ck.Pid = sC[0].Trim();
                ck.Kid = sC[1].Trim();
                ck.sUrl = sUrl + ck.Kid;

                if (dateTime < Cutoff )
                {
                    NumberMostRecent++;
                    if (NumberMostRecent > 10) return 1;
                }
                nKudosTotal += nN;
                kitems.Add(ck);
                return  ExtractKP(ref s, iS);
            }
        }

        public static string HPformat = "MM-dd-yyyy h:mm tt";
        public static string sHPdt(DateTime dt)
        {
            return dt.ToString(HPformat);
        }

        //10-29-2024 07:29 AM 
        public static bool dtHPtime(string dts1, ref DateTime dtResponse)
        {
            
            string dts = Regex.Replace(dts1, @"[^\u0000-\u007F]+", string.Empty);
            char[] ch = dts.ToCharArray();
            if (DateTime.TryParseExact(
                dts, HPformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtResponse))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static cKudoBase KudoBaseTool;

        public class cExpert
        {
            public int N {  get; set; }
            public string name { get; set; }
            public string user_id { get; set; }
            public string Spages { get; set; }
            public string Spoints { get; set; }
            public string Kpages { get; set; }
            public string Kpoints { get; set; }
            public string DatePosted { get; set; }
            public string Kudoed;
            public string TotalPosts;
            public string TotalSolutions;
            public int ktt_days;    // duration for above three items
        }

        public class cHelpful
        {
            public DateTime KudoDT;
            public string sNumKudo;
            public List<string> KudoID = new List<string>();
        }

        public class cWorkInfo
        {
            public bool Show { get; set; }
            public string name { get; set; }
            public string Pct { get; set; }
            public int DaysWorked;
            public string user_id;
            public bool bEmpty;
            public int WhereUsed; // index into the selected row array
            public List<DateTime> Solutions = new List<DateTime>();
        }


        public static List<DateTime> ReadDTLFromSOL(string id, ref int DaysWorked)
        {
            TimeSpan d;
            string pathFolders = WhereExpert + id;
            if (!Directory.Exists(pathFolders)) return null;

            string filePath = pathFolders + "/solutions.txt";
            if (!File.Exists(filePath)) return null;

            List<DateTime> dateTimeList = new List<DateTime>();

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (DateTime.TryParseExact(line, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime dateTime))
                {
                    if (dateTime < Cutoff) break;
                    dateTimeList.Add(dateTime);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            int n = dateTimeList.Count;
            if(n < 2)
            {
                DaysWorked = (n==0) ? 0 : 1;
            }
            else
            {
                d = dateTimeList[0] - dateTimeList[n - 1];
                DaysWorked = d.Days;
            }
            return dateTimeList;
        }
        public class cSortTools
        {
            private static int[] SI; // SortInx;
            private static int[] US; // unsorted
            private static string[] sS;
            public void SortAuthorField(EAuthor f, bool Descending, ref List<cExpert> AuthorList)
            {
                int n = AuthorList.Count;
                SI = new int[n];
                sS = new string[n];
                switch (f)
                {
                    case EAuthor.name:
                        sS = AuthorList.Select(cExpert => cExpert.name).ToArray();
                        break;
                    case EAuthor.user_id:
                        sS = AuthorList.Select(cExpert => cExpert.user_id).ToArray();
                        break;
                    case EAuthor.Spages:
                        sS = AuthorList.Select(cExpert => cExpert.Spages).ToArray();
                        break;
                    case EAuthor.Spoints:
                        sS = AuthorList.Select(cExpert => cExpert.Spoints).ToArray();
                        break;
                    case EAuthor.Kpages:
                        sS = AuthorList.Select(cExpert => cExpert.Kpages).ToArray();
                        break;
                    case EAuthor.kpoints:
                        sS = AuthorList.Select(cExpert => cExpert.Kpoints).ToArray();
                        break;
                }
                if(f == EAuthor.name)
                {
                    SI = sS
.Select((value, index) => new { Value = value, Index = index })
.OrderBy(item => item.Value)
.Select(item => item.Index)
.ToArray();
                }
                else
                {
                    US = Array.ConvertAll(sS, int.Parse);
                    SI = US
.Select((value, index) => new { Value = value, Index = index })
.OrderBy(item => item.Value)
.Select(item => item.Index)
.ToArray();

                }

                if (!Descending)
                {
                    Array.Reverse(SI);
                }
                
                List<cExpert> list = new List<cExpert>(AuthorList);
                for(int i = 0; i < n; i++)
                {
                    AuthorList[i] = list[SI[i]];
                    AuthorList[i].N = i + 1;
                }
            }
        }

        public class cEachUserAll
        {
            public string Kudoed;
            public string TotalPosts;
            public string TotalSolutions;
            public int days;
        }
        public class cEachUserVal
        {
            public int Kudoed;
            public int Unsolved;
            public int Solved;
            public int days;
            public double m;
            public string name;
        }

        public class cAllPostInfo
        {
            private int Kudoed;
            public int TotalPosts;
            public DateTime LastDate = DateTime.Now;
            public DateTime FirstDate;
            private bool FirstDateSet = true;
            private int TotalSolutions;
            private bool bLastpage;

            private string sKudoed;
            private string sTotalPosts;
            private string sTotalSolutions;

            private string s1s = "<span class=\"local-date\">‎";
            private string s1S = "<span class='local-date'>‎";
            private string s1e = "</span>";
            private string s2s = "<span class=\"local-time\">";
            private string s2S = "<span class='local-time'>";
            private string s2e = "</span>";

            //public List<cEachUserAll> AllUserSols = new List<cEachUserAll>();

            private DateTime dtTemp;
            private DateTime Cutoff;
            private int ExpectedPages = 0;
            public void Init(DateTime tCutoff)
            {
                bLastpage = false;
                dtTemp = DateTime.Now;
                Cutoff = tCutoff;
                Kudoed = 0;
                TotalPosts = 0;
                TotalSolutions = 0;
            }

            public void WriteUserStats(string id)
            {
                string filePath = WhereExpert + id  + "/AllPosts.txt";
                sTotalPosts = TotalPosts.ToString();
                sTotalSolutions = TotalSolutions.ToString();
                sKudoed = Kudoed.ToString();
                TimeSpan t = FirstDate - LastDate;
                string sOut = sTotalPosts + "," + sTotalSolutions + "," + sKudoed + "," + t.Days.ToString();
                File.WriteAllText(filePath, sOut);
            }

            public int GetExpectedPages(ref string s)   // only want full pages
            {
                int i, j;
                i = s.LastIndexOf(apExpectPage);
                if (i < 0) return 0;
                i += apExpectPage.Length;
                j = s.IndexOf("\"", i);
                ExpectedPages = Convert.ToInt32(s.Substring(i,j - i)) - 1;
                return ExpectedPages;
            }

            // returns index of item following the searchTerm
            private static List<int> FindOccurrences(ref string text, string searchTerm)
            {
                List<int> positions = new List<int>();
                int index = text.IndexOf(searchTerm);
                while (index != -1)
                {
                    positions.Add(index + searchTerm.Length);
                    index = text.IndexOf(searchTerm, index + searchTerm.Length);
                }
                return positions;
            }


            public int AllPostParse(ref string aPage)
            {
                int i, j;
                bool bUseQ = true;
                int nExpect = 20; // per page
                int n = 0;
                int NumCutBelow=0;
                List<int>StartMessage = new List<int>();
                StartMessage = FindOccurrences(ref aPage, apStartMessage);
                // there are 40 of each but only 20 unique
                List<int> localdate = FindOccurrences(ref aPage, bUseQ ? s1s : s1S);
                List<int> localtime = FindOccurrences(ref aPage, bUseQ ? s2s : s2S);
                n = localdate.Count / 2;
                if (n != nExpect) return 1;
                for(i = 0; i < n; i++)
                {
                    j = aPage.IndexOf(s1e, localdate[i]);
                    string sDay = aPage.Substring(localdate[i], j - localdate[i]);
                    j = aPage.IndexOf(s1e, localtime[i]);
                    string sTim = aPage.Substring(localtime[i], j - localtime[i]);
                    string sDT = sDay + " " + sTim;
                    bool b = dtHPtime(sDT, ref dtTemp);
                    if (i == 0 && FirstDateSet)
                    {
                        FirstDate = dtTemp;
                        FirstDateSet = false;
                    }
                    if (!b) return -1;
                    if (dtTemp < Cutoff)
                        NumCutBelow++;
                }
                if (NumCutBelow == 20) return 1;
                LastDate = dtTemp;
                TotalPosts += 20;
                List<int> apSols = FindOccurrences(ref aPage, apLookupSolution);
                TotalSolutions += apSols.Count;
                List<int> apKudo = FindOccurrences(ref aPage, apLookupKudo);
                int KudosLeft = apKudo.Count;
                for (i = 0; i < n-1; i++)
                {
                    int ld1 = StartMessage[i];
                    int ld2 = StartMessage[i+1];
                    
                    foreach (int k in apKudo)
                    {
                        for (j = 0; j < apSols.Count; j++)
                        {
                            int s = apSols[j];
                            if (ld1 < s && s < ld2)
                            {
                                if (ld1 < k && k < ld2)
                                {
                                    KudosLeft--;
                                }
                            }
                        }
                    }
                }
                Kudoed += KudosLeft/2;
                return 0;
            }
            
        }

    }
}
