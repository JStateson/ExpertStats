using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static ExpertStats.global;
using DateTimeIntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType;
using SeriesChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType;
using AxisEnabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled;
using ChartArea = System.Windows.Forms.DataVisualization.Charting.ChartArea;
using ChartValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType;
using ElementPosition = System.Windows.Forms.DataVisualization.Charting.ElementPosition;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;
using TextAnnotation = System.Windows.Forms.DataVisualization.Charting.TextAnnotation;
using Legend = System.Windows.Forms.DataVisualization.Charting.Legend;

namespace ExpertStats
{
    public partial class ScatterForm : Form
    {
        private List<cWorkInfo> workList = new List<cWorkInfo>();
        private int nAuthors;
        private BindingSource MyBindingSource = new BindingSource();
        private int NumCheckedMacros = 0;
        private DateTime Cutoff;
        private int nMaxCnt=0;
        private int PlotType;
        public ScatterForm( List<bool> rselectedRow, DateTime rCutoff)
        {
            InitializeComponent();
            int DayWorked = 0;
            nAuthors = AuthorList.Count;
            Cutoff = rCutoff;
            for (int i = 0; i < nAuthors; i++)
            {
                cWorkInfo wi = new cWorkInfo();
                wi.Show = rselectedRow[i];
                if (wi.Show) NumCheckedMacros++;
                wi.name = AuthorList[i].name;
                wi.Solutions = ReadDTLFromSOL(AuthorList[i].user_id, ref DayWorked);
                if (wi.Solutions == null)
                    wi.Solutions = new List<DateTime>();
                wi.DaysWorked = DayWorked;
                wi.bEmpty = wi.Solutions.Count == 0;
                workList.Add(wi);
            }
            MyBindingSource.DataSource = workList;
            dgvCSel.DataSource = MyBindingSource;
            MyBindingSource.ResetBindings(false);
            dgvCSel.Columns[2].Width = 48;
            dgvCSel.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCSel.Columns[1].ReadOnly = true;
            dgvCSel.Columns[0].Width = 32;
            for (int i = 0; i < AuthorList.Count; i++)
                dgvCSel.Rows[i].Cells[2].Value = "";
            ShowInitial();
        }




        private int[] YAxisInt(int value, int size)
        {
            return Enumerable.Repeat(value, size).ToArray();
        }


        private void ClearChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
            chart1.Annotations.Clear();
        }

        private int GetYheight(int i)
        {
            double m = NumCheckedMacros-1;
            if (m > 0)
            {
                double a = (100.0 * i) / m;
                return 100 - Convert.ToInt32(a);
            }
            return 50;
        }

        private void ShowInitial()
        {
            ChartArea chartArea = new ChartArea();            
            List<int>WhereUsed = new List<int>();
            PlotType = 0;
            chart1.ChartAreas.Add(chartArea);
            if (NumCheckedMacros == 0) return;
            DateTime today = DateTime.Today;
            Legend legend = new Legend();
            chart1.Legends.Add(legend);
            chart1.Legends[0].Title = "Solutions shown in ()";
            chart1.ChartAreas[0].AxisY.IsReversed = false;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisY.Interval = 10;
            chart1.ChartAreas[0].AxisX.Crossing = today.ToOADate();
            chart1.ChartAreas[0].AxisX.IsReversed = true;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days; // Interval type
            chart1.ChartAreas[0].AxisX.Interval = 30; // Set interval as needed (e.g., every hour)
            chart1.ChartAreas[0].AxisX.Minimum = today.AddDays(-365).ToOADate(); // Start from 6 months ago
            chart1.ChartAreas[0].AxisX.Maximum = today.ToOADate(); // End at today
            

            int jInc = 100 / NumCheckedMacros;
            int k;
            int jCnt = 0;
            if (jInc <= 0) jInc = 1;
            if(jInc > 40)
                jInc = 10;
            int nMax = 0;
            for (int i = 0; i < nAuthors; i++)
            {               
                if (!workList[i].Show) continue;
                cExpert author = AuthorList[i];
                cWorkInfo wi = workList[i];
                nMax = Math.Max(nMax, wi.Solutions.Count);
                string seriesname = "(" + wi.Solutions.Count.ToString() + ") "+author.name;
                chart1.Series.Add(seriesname);
                chart1.Series[seriesname].EmptyPointStyle.Color = Color.Transparent;
                chart1.Series[seriesname].ChartType = SeriesChartType.Point;
                k = GetYheight(jCnt);
                chart1.Series[seriesname].Points.DataBindXY(wi.Solutions,YAxisInt(k,wi.Solutions.Count));
                jCnt++;
            }
            for (int i = 0; i < nAuthors; i++)
            {
                if (!workList[i].Show) continue;
                double a = workList[i].Solutions.Count * 100.0 / nMax;
                int t = Convert.ToInt32(a);
                dgvCSel.Rows[i].Cells[2].Value = t.ToString();
            }
        }

        private void btnDrawSel_Click(object sender, EventArgs e)
        {
            bool isChecked;
            NumCheckedMacros = 0;
            for (int i = 0; i < AuthorList.Count; i++)
                dgvCSel.Rows[i].Cells[2].Value = "";
            for (int i = 0; i < dgvCSel.Rows.Count; i++)
            {
                isChecked = (bool)dgvCSel.Rows[i].Cells[0].Value;
                if(isChecked && workList[i].bEmpty)
                {
                    dgvCSel.Rows[i].Cells[0].Value = false;
                    isChecked = false;
                }
                workList[i].Show = isChecked;
                NumCheckedMacros += isChecked ? 1 : 0;
            }
            if (NumCheckedMacros < 0) return;
            ClearChart();
            if (rbDaily.Checked)
                ShowInitial();
            else ShowHist();
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

        private void SetChecks(bool b)
        {
            foreach (DataGridViewRow row in dgvCSel.Rows)
            {
                row.Cells[0].Value = b;
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            SetChecks(true);
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            SetChecks(false);
        }

        private int SolutionsPerMonth(int days, int solutions)
        {
            double m = days;
            double h = solutions * 30.0 / m;
            if (solutions == 1) return 1;
            return Convert.ToInt32(h);
        }

        int NextMultipleOf100(int n)
        {
            return ((n + 99) / 100) * 100;
        }

        private void ShowHist()
        {
            int i, j = 0, nMaxValue = 0 ;
            int n = dgvCSel.Rows.Count;
            int[] Perf = new int[NumCheckedMacros];
            string[] Users = new string[NumCheckedMacros];
            for(i = 0; i < n; i++)
            {
                if ((bool)dgvCSel.Rows[i].Cells[0].Value)
                {
                    Perf[j] = SolutionsPerMonth(workList[i].DaysWorked, workList[i].Solutions.Count);
                    nMaxValue = Math.Max(Perf[j], nMaxValue);
                    Users[j] = workList[i].name;
                    j++;
                }
            }
            j = 0;
            for(i = 0; i < n; i++)
            {
                if ((bool)dgvCSel.Rows[i].Cells[0].Value)
                {
                    double a = Perf[j] * 100.0 / nMaxValue;
                    int t = Convert.ToInt32(a);
                    dgvCSel.Rows[j].Cells[2].Value = t.ToString();
                    j++;
                }

            }

            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);
            chart1.Width = 800;
            if (NumCheckedMacros > 25)
            {
                chart1.Width = 32 * NumCheckedMacros;
            }
            Series series = new Series()
            {
                ChartType = SeriesChartType.StackedColumn,
                Color = Color.Red,
                IsValueShownAsLabel = false,
                XValueType = ChartValueType.String
            };

            for (j = 0; j < NumCheckedMacros; j++)
            {
                series.Points.AddXY(Users[j], Perf[j]);                
            }

            chart1.Series.Add(series);
            chart1.Series[0]["PointWidth"] = "0.2";
            chartArea.AxisX.Title = "User Name";
            chartArea.AxisY.Title = "Solutions per Month";
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.LabelStyle.Angle = 45;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.Maximum = NumCheckedMacros;
            chartArea.AxisX.Maximum = NumCheckedMacros;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = NextMultipleOf100(nMaxValue);
            //chartArea.AxisY.Interval = 100;
            chart1.Legends.Clear();
            // Hide the major and minor tick marks on the Y-axis
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.Enabled = AxisEnabled.True;
        }
    }
}

