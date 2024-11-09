namespace ExpertStats
{
    partial class ScatterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvCSel = new System.Windows.Forms.DataGridView();
            this.btnDrawSel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSolMonth = new System.Windows.Forms.RadioButton();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCSel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(15, 12);
            this.chart1.Name = "chart1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(850, 588);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // dgvCSel
            // 
            this.dgvCSel.AllowUserToAddRows = false;
            this.dgvCSel.AllowUserToDeleteRows = false;
            this.dgvCSel.AllowUserToResizeColumns = false;
            this.dgvCSel.AllowUserToResizeRows = false;
            this.dgvCSel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCSel.Location = new System.Drawing.Point(1121, 102);
            this.dgvCSel.Name = "dgvCSel";
            this.dgvCSel.RowHeadersVisible = false;
            this.dgvCSel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCSel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCSel.Size = new System.Drawing.Size(155, 527);
            this.dgvCSel.TabIndex = 1;
            this.dgvCSel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCSel_KeyDown);
            // 
            // btnDrawSel
            // 
            this.btnDrawSel.Location = new System.Drawing.Point(1136, 52);
            this.btnDrawSel.Name = "btnDrawSel";
            this.btnDrawSel.Size = new System.Drawing.Size(101, 23);
            this.btnDrawSel.TabIndex = 2;
            this.btnDrawSel.Text = "Draw Checked";
            this.btnDrawSel.UseVisualStyleBackColor = true;
            this.btnDrawSel.Click += new System.EventHandler(this.btnDrawSel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(948, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 65);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select rows and press  the\r\nenter key to check/uncheck\r\n\r\nUsers with only 1 solut" +
    "ion are\r\nshown as single dot in histogram";
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(978, 24);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelAll.TabIndex = 4;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(978, 53);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 22);
            this.btnClearAll.TabIndex = 5;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSolMonth);
            this.groupBox1.Controls.Add(this.rbDaily);
            this.groupBox1.Location = new System.Drawing.Point(951, 194);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 129);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph Type";
            // 
            // rbSolMonth
            // 
            this.rbSolMonth.AutoSize = true;
            this.rbSolMonth.ForeColor = System.Drawing.SystemColors.Highlight;
            this.rbSolMonth.Location = new System.Drawing.Point(14, 73);
            this.rbSolMonth.Name = "rbSolMonth";
            this.rbSolMonth.Size = new System.Drawing.Size(118, 17);
            this.rbSolMonth.TabIndex = 1;
            this.rbSolMonth.Text = "Solutions per month";
            this.rbSolMonth.UseVisualStyleBackColor = true;
            // 
            // rbDaily
            // 
            this.rbDaily.AutoSize = true;
            this.rbDaily.Checked = true;
            this.rbDaily.ForeColor = System.Drawing.SystemColors.Highlight;
            this.rbDaily.Location = new System.Drawing.Point(14, 34);
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.Size = new System.Drawing.Size(91, 17);
            this.rbDaily.TabIndex = 0;
            this.rbDaily.TabStop = true;
            this.rbDaily.Text = "Daily progress";
            this.rbDaily.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(902, 618);
            this.panel1.TabIndex = 7;
            // 
            // ScatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 642);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDrawSel);
            this.Controls.Add(this.dgvCSel);
            this.Name = "ScatterForm";
            this.Text = "Work schedule and Kudo graphs";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCSel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataGridView dgvCSel;
        private System.Windows.Forms.Button btnDrawSel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSolMonth;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.Panel panel1;
    }
}