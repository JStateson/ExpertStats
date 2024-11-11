namespace ExpertStats
{
    partial class TPChart
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
            this.KudoInfo = new System.Windows.Forms.Label();
            this.dgvCSel = new System.Windows.Forms.DataGridView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cboxUseLog = new System.Windows.Forms.CheckBox();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnDrawSel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbKudos = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCSel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // KudoInfo
            // 
            this.KudoInfo.AutoSize = true;
            this.KudoInfo.BackColor = System.Drawing.SystemColors.Info;
            this.KudoInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.KudoInfo.Location = new System.Drawing.Point(7, 99);
            this.KudoInfo.Name = "KudoInfo";
            this.KudoInfo.Size = new System.Drawing.Size(156, 78);
            this.KudoInfo.TabIndex = 26;
            this.KudoInfo.Text = "Kudos from the inner circle\r\nare not included\r\n\r\nYou can select user names\r\nand t" +
    "hen press the enter key\r\nto check or uncheck selections";
            // 
            // dgvCSel
            // 
            this.dgvCSel.AllowUserToAddRows = false;
            this.dgvCSel.AllowUserToDeleteRows = false;
            this.dgvCSel.AllowUserToResizeColumns = false;
            this.dgvCSel.AllowUserToResizeRows = false;
            this.dgvCSel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCSel.Location = new System.Drawing.Point(1178, 117);
            this.dgvCSel.Name = "dgvCSel";
            this.dgvCSel.RowHeadersVisible = false;
            this.dgvCSel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCSel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCSel.Size = new System.Drawing.Size(212, 498);
            this.dgvCSel.TabIndex = 21;
            this.dgvCSel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCSel_KeyDown);
            // 
            // chart1
            // 
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 50);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(922, 600);
            this.chart1.TabIndex = 20;
            this.chart1.Text = "chart";
            // 
            // cboxUseLog
            // 
            this.cboxUseLog.AutoSize = true;
            this.cboxUseLog.Location = new System.Drawing.Point(10, 32);
            this.cboxUseLog.Name = "cboxUseLog";
            this.cboxUseLog.Size = new System.Drawing.Size(108, 17);
            this.cboxUseLog.TabIndex = 25;
            this.cboxUseLog.Text = "Make Y log scale";
            this.cboxUseLog.UseVisualStyleBackColor = true;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(1315, 68);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 22);
            this.btnClearAll.TabIndex = 24;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(1178, 67);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelAll.TabIndex = 23;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnDrawSel
            // 
            this.btnDrawSel.Location = new System.Drawing.Point(1178, 17);
            this.btnDrawSel.Name = "btnDrawSel";
            this.btnDrawSel.Size = new System.Drawing.Size(101, 23);
            this.btnDrawSel.TabIndex = 22;
            this.btnDrawSel.Text = "Draw Checked";
            this.btnDrawSel.UseVisualStyleBackColor = true;
            this.btnDrawSel.Click += new System.EventHandler(this.btnDrawSel_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Location = new System.Drawing.Point(183, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(962, 643);
            this.panel1.TabIndex = 27;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Controls.Add(this.rbKudos);
            this.groupBox1.Location = new System.Drawing.Point(15, 226);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 165);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(20, 76);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(68, 17);
            this.rbAll.TabIndex = 1;
            this.rbAll.Text = "All  Posts";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbKudos
            // 
            this.rbKudos.AutoSize = true;
            this.rbKudos.Checked = true;
            this.rbKudos.Location = new System.Drawing.Point(20, 42);
            this.rbKudos.Name = "rbKudos";
            this.rbKudos.Size = new System.Drawing.Size(55, 17);
            this.rbKudos.TabIndex = 0;
            this.rbKudos.TabStop = true;
            this.rbKudos.Text = "Kudos";
            this.rbKudos.UseVisualStyleBackColor = true;
            // 
            // TPChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1414, 694);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.KudoInfo);
            this.Controls.Add(this.dgvCSel);
            this.Controls.Add(this.cboxUseLog);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnDrawSel);
            this.Name = "TPChart";
            this.Text = "TPChart";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCSel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label KudoInfo;
        private System.Windows.Forms.DataGridView dgvCSel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox cboxUseLog;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnDrawSel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbKudos;
        private System.Windows.Forms.RadioButton rbAll;
    }
}