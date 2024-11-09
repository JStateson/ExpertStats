using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static ExpertStats.global;
using AxisEnabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled;
using ChartArea = System.Windows.Forms.DataVisualization.Charting.ChartArea;
using ChartValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType;
using ElementPosition = System.Windows.Forms.DataVisualization.Charting.ElementPosition;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;
using SeriesChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType;
using TextAnnotation = System.Windows.Forms.DataVisualization.Charting.TextAnnotation;

namespace ExpertStats
{
    public partial class TPChart : Form
    {
        private int PlotType = 0;

        private List<bool> selectedRow;
        private int iBinCnt = 7;// one week = 7 da
        private List<cWorkInfo> workList = new List<cWorkInfo>();
        private int nAuthors;
        private BindingSource MyBindingSource = new BindingSource();
        private int NumCheckedMacros = 0;
        public List<string> PeerIDs = new List<string>();
        private int nMaxValue = 0;
        private DateTime Cutoff;
        private string dt_first;
        private List<cEachUserVal> UserPosts = new List<cEachUserVal>();
        public TPChart( ref List<bool> rselectedRow, string rCutoff)
        {
            InitializeComponent();
            int i = 0;
            dt_first = rCutoff;
            selectedRow = rselectedRow;

            nAuthors = AuthorList.Count;
            int LargestDayCnt = 0;
            for (i = 0; i < nAuthors; i++)
            {
                cExpert ce = AuthorList[i];
                PeerIDs.Add(ce.user_id);
                cWorkInfo wi = new cWorkInfo();
                wi.Show = rselectedRow[i];
                if (wi.Show) NumCheckedMacros++;
                wi.name = ce.name;
                workList.Add(wi);
                cEachUserVal eu = new cEachUserVal();
                int j = Convert.ToInt32(ce.TotalPosts);
                if (ce.ktt_days == 0 || j == 0)
                {
                    selectedRow[i] = false;
                    eu.days = 0;
                    UserPosts.Add(eu);
                    continue;
                }
                eu.Kudoed = Convert.ToInt32(ce.Kudoed);
                eu.Solved = Convert.ToInt32(ce.TotalSolutions);
                eu.Kudoed = Convert.ToInt32(ce.Kudoed);
                eu.days = ce.ktt_days;
                LargestDayCnt = Math.Max(LargestDayCnt, eu.days);
                eu.Unsolved = j - eu.Kudoed - eu.Solved;
                UserPosts.Add(eu);
            }
            double m = LargestDayCnt;
            foreach (cEachUserVal eu in UserPosts)
            {
                if (eu.days == 0) continue;
                eu.m = m / eu.days;
                eu.Solved = Convert.ToInt32(eu.Solved * eu.m);
                eu.Unsolved = Convert.ToInt32(eu.Unsolved * eu.m);
                eu.Kudoed = Convert.ToInt32(eu.Kudoed * eu.m);
            }

            KudoRcvd.InitHist(AuthorList.Count);
            i = 0;
            KudoRcvd.SetPeerIDs(ref PeerIDs);
            foreach (cExpert ce in AuthorList)
            {
                KudoRcvd.ReadRcvdList(ce.user_id);
            }
            MyBindingSource.DataSource = workList;
            dgvCSel.DataSource = MyBindingSource;
            dgvCSel.Columns[2].Width = 48;
            dgvCSel.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCSel.Columns[0].Width = 32;
            MyBindingSource.ResetBindings(false);
            ChartAll();
        }

        private int Scale100(int n, int m)
        {
            double a = n * 100.0 / m;
            return Convert.ToInt32(a);
        }

        private void ChartAll()
        {
            int i = 0, j = 0, n = 0;
            int value = 0;
            KudoInfo.Visible = rbKudos.Checked;
            for (i = 0; i < AuthorList.Count; i++)
                dgvCSel.Rows[i].Cells[2].Value = "";
            i = 0;
            List<int> WhereSelected = new List<int>();
            n = AuthorList.Count;
            int[] barUser = new int[n];
            int[] barPeer = new int[n];
            int[] barExpe = new int[n];
            string[] xAx = new string[n];
            nMaxValue = 0;
            n = 0;
            for(int q = 0; q < selectedRow.Count; q++)
            {
                bool b = selectedRow[q];
                if (b)
                {
                    if(rbAll.Checked)
                    {
                        cEachUserVal eu = UserPosts[j];
                        if (eu.days == 0)
                        {
                            b = false;
                            selectedRow[j] = b;
                            NumCheckedMacros--;
                            continue;
                        }
                        WhereSelected.Add(q);
                        n++;
                        barUser[i] = eu.Solved;
                        barPeer[i] = eu.Kudoed;
                        barExpe[i] = eu.Unsolved;
                        nMaxValue = Math.Max(nMaxValue, eu.Solved + eu.Kudoed + eu.Unsolved);
                    }
                    if(rbKudos.Checked)
                    {
                        WhereSelected.Add(q);
                        n++;
                        barUser[i] = KudoRcvd.TotalWeight[j];
                        barPeer[i] = KudoRcvd.ExpertWeight[j];
                        nMaxValue = Math.Max(nMaxValue, barUser[i] + barPeer[i]);
                    }
                    xAx[i] = AuthorList[j].name;
                    i++;
                }
                j++;
            }

            if(rbAll.Checked) // rescale to 100
            {
                
                for(j = 0; j < i; j++)
                {
                    barExpe[j] = Scale100(barExpe[j], nMaxValue);
                    barPeer[j] = Scale100(barPeer[j], nMaxValue);
                    barUser[j] = Scale100(barUser[j], nMaxValue);
                    int t = barExpe[j] + barPeer[j] + barUser[j];
                    dgvCSel.Rows[WhereSelected[j]].Cells[2].Value = t;
                }
                nMaxValue = 100;
            }
            if(rbKudos.Checked)
            {
                for (j = 0; j < i; j++)
                {
                    int t = barPeer[j] + barUser[j];
                    t = 100 * t / nMaxValue;
                    dgvCSel.Rows[WhereSelected[j]].Cells[2].Value = t;
                }
            }

            // Add a ChartArea to the Chart
            ChartArea chartArea = new ChartArea();

            chart1.ChartAreas.Add(chartArea);
            // 25 can fit in to 800
            chart1.Width = 800;
            if (NumCheckedMacros > 25)
            {
                chart1.Width = 32 * NumCheckedMacros;
            }


            Color[] segmentColors = { Color.Red, Color.Green, Color.Blue };

            // Create a separate series for each segment
            for (i = 0; i < 3; i++)// segmentColors.Length-1; i++)
            {
                if (i == 2 && rbKudos.Checked) continue;
                Series series = new Series()
                {
                    ChartType = SeriesChartType.StackedColumn,
                    Color = segmentColors[i],
                    IsValueShownAsLabel = false, // Show values on segments
                    XValueType = ChartValueType.String
                };

                // Add points for each bar in this segment
                for (j = 0; j < n; j++)
                {
                    if(rbKudos.Checked)
                        value = (i == 0) ? barUser[j] : barPeer[j];
                    if(rbAll.Checked)
                    {
                        if (i == 0) value = barUser[j];
                        if (i == 1) value = barPeer[j];
                        if (i == 2) value = barExpe[j];
                    }
                    series.Points.AddXY(xAx[j], value);
                }
                chart1.Series.Add(series);
                chart1.Series[i]["PointWidth"] = "0.2";
            }

            // Customize the chart appearance
            chartArea.AxisX.Title = "User Name";
            if (rbKudos.Checked) chartArea.AxisY.Title = "Kudos";
            else chartArea.AxisY.Title = "Response by OP";
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.LabelStyle.Angle = 45;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.Maximum = n;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisY.Minimum = (cboxUseLog.Checked) ? 1 : 0;
            chartArea.AxisY.Maximum = NextMultipleOf100(nMaxValue);
            //chartArea.AxisY.Interval = 100;
            chartArea.AxisY.IsLogarithmic = cboxUseLog.Checked;
            if (rbKudos.Checked) chart1.Titles.Add("Kudos received in HP support forum since " + dt_first + " excluding inner circle");

            else chart1.Titles.Add("Posts scaled to 100% and days normalized");
            // Hide the major and minor tick marks on the Y-axis
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.Enabled = AxisEnabled.True;

            // Optionally hide the Y-axis labels
            //chartArea.AxisY.LabelStyle.Enabled = false;
            chart1.Legends.Clear();
            AddSeriesNamesToLeft();
        }

        private System.Drawing.Color custQ(bool b, int i)
        {
            System.Drawing.Color[] categories = { Color.Red, Color.Green, Color.Blue };
            System.Drawing.Color[] categoryUS = { Color.Red, Color.Green, Color.Blue };
            if (b)
            {
                return categories[i];
            }
            else
            {
                return categoryUS[i];
            }
        }
        private string custL(bool b, int i)
        {
            string[] categories = { "User Kudo", "Peer Kudo", "Inner Circle" };
            string[] categoryUS = { "Solved","Kudoed","No Response" };
            if(b)
            {
                return categories[i];
            }
            else
            {
                return categoryUS[i];
            }
        }

        private void AddSeriesNamesToLeft()
        {

            // Create a custom label for the first series
            TextAnnotation annotation1 = new TextAnnotation
            {
                Text = custL(rbKudos.Checked, 0),
                ForeColor = custQ(rbKudos.Checked, 0),
                Alignment = ContentAlignment.MiddleCenter,
                X = 0, // Position X to the left of the chart
                Y = 20    // Adjust Y position to place the label
            };
            chart1.Annotations.Add(annotation1);

            // Create a custom label for the second series
            TextAnnotation annotation2 = new TextAnnotation
            {
                Text = custL(rbKudos.Checked, 1),
                ForeColor = custQ(rbKudos.Checked, 1),
                Alignment = ContentAlignment.MiddleCenter,
                X = 0, // Position X to the left of the chart
                Y = 15    // Adjust Y position to place the label
            };
            chart1.Annotations.Add(annotation2);

            if(true)
            {
                TextAnnotation annotation3 = new TextAnnotation
                {
                    Text = custL(rbKudos.Checked, 2),
                    ForeColor = custQ(rbKudos.Checked, 2),
                    Alignment = ContentAlignment.MiddleCenter,
                    X = 0, // Position X to the left of the chart
                    Y = 10    // Adjust Y position to place the label
                };
                chart1.Annotations.Add(annotation3);
            }
        }



        private void SetChecks(bool b)
        {
            //foreach (DataGridViewRow row in dgvCSel.Rows)  row.Cells[0].Value = b;
            for (int i = 0; i < selectedRow.Count; i++)
            {
                selectedRow[i] = b;
                dgvCSel.Rows[i].Cells[0].Value = b;
            }
        }

        private void btnDrawSel_Click(object sender, EventArgs e)
        {
            bool isChecked;
            NumCheckedMacros = 0;
            for (int i = 0; i < dgvCSel.Rows.Count; i++)
            {
                isChecked = (bool)dgvCSel.Rows[i].Cells[0].Value;
                workList[i].Show = isChecked;
                selectedRow[i] = isChecked;
                NumCheckedMacros += isChecked ? 1 : 0;
            }
            ClearChart();
            ChartAll();
        }

        private void ClearChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
            chart1.Annotations.Clear();
        }

        int NextMultipleOf100(int n)
        {
            return ((n + 99) / 100) * 100;
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            SetChecks(true);
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            SetChecks(false);
        }

        private void dgvCSel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (dgvCSel.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvCSel.SelectedRows)
                    {
                        bool IsChecked = (bool)row.Cells[0].Value;
                        row.Cells[0].Value = !IsChecked;
                    }
                }
            }
        }
    }
}
