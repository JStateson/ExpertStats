namespace ExpertStats
{
    partial class ExpertInfo
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
            this.components = new System.ComponentModel.Container();
            this.lbWhereExe = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClearCts = new System.Windows.Forms.Button();
            this.btnAutoPost = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnFinishKudo = new System.Windows.Forms.Button();
            this.cbKudoCalculate = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnFinishSol = new System.Windows.Forms.Button();
            this.cbSolCalculate = new System.Windows.Forms.CheckBox();
            this.btnDelExpert = new System.Windows.Forms.Button();
            this.btnFinishRef = new System.Windows.Forms.Button();
            this.btnGetRange = new System.Windows.Forms.Button();
            this.btnDelBlanks = new System.Windows.Forms.Button();
            this.btnDelContacts = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGetAllPosts = new System.Windows.Forms.Button();
            this.btnKudoRef = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbRestart = new System.Windows.Forms.Label();
            this.btnUseDate = new System.Windows.Forms.Button();
            this.tbCutTime = new System.Windows.Forms.TextBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.pbGetSol = new System.Windows.Forms.ProgressBar();
            this.btnGetKudo = new System.Windows.Forms.Button();
            this.btnGetSol = new System.Windows.Forms.Button();
            this.lbCnt = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnHist = new System.Windows.Forms.Button();
            this.btnScatPlot = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MakeKUtbl = new System.Windows.Forms.Button();
            this.pbAuthors = new System.Windows.Forms.ProgressBar();
            this.btnCountKudos = new System.Windows.Forms.Button();
            this.btnCount = new System.Windows.Forms.Button();
            this.dgvExpert = new System.Windows.Forms.DataGridView();
            this.TimerAcnt = new System.Windows.Forms.Timer(this.components);
            this.FinishTimer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.lbCurrentVol = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpert)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbWhereExe
            // 
            this.lbWhereExe.AutoSize = true;
            this.lbWhereExe.Location = new System.Drawing.Point(324, 697);
            this.lbWhereExe.Name = "lbWhereExe";
            this.lbWhereExe.Size = new System.Drawing.Size(35, 13);
            this.lbWhereExe.TabIndex = 9;
            this.lbWhereExe.Text = "label1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnClearCts);
            this.groupBox3.Controls.Add(this.btnAutoPost);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.btnDelExpert);
            this.groupBox3.Controls.Add(this.btnFinishRef);
            this.groupBox3.Controls.Add(this.btnGetRange);
            this.groupBox3.Controls.Add(this.btnDelBlanks);
            this.groupBox3.Controls.Add(this.btnDelContacts);
            this.groupBox3.Location = new System.Drawing.Point(993, 268);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(376, 412);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Diagnostics and Tools";
            // 
            // btnClearCts
            // 
            this.btnClearCts.ForeColor = System.Drawing.Color.Red;
            this.btnClearCts.Location = new System.Drawing.Point(219, 266);
            this.btnClearCts.Name = "btnClearCts";
            this.btnClearCts.Size = new System.Drawing.Size(95, 23);
            this.btnClearCts.TabIndex = 18;
            this.btnClearCts.Text = "Clear Users";
            this.toolTip1.SetToolTip(this.btnClearCts, "Sets either all user solutiions\r\n to -1 or just the selected users\r\nNo users are " +
        "removed and you\r\nmust run the COUNT tools");
            this.btnClearCts.UseVisualStyleBackColor = true;
            this.btnClearCts.Click += new System.EventHandler(this.btnClearCts_Click);
            // 
            // btnAutoPost
            // 
            this.btnAutoPost.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoPost.Location = new System.Drawing.Point(26, 290);
            this.btnAutoPost.Name = "btnAutoPost";
            this.btnAutoPost.Size = new System.Drawing.Size(113, 41);
            this.btnAutoPost.TabIndex = 17;
            this.btnAutoPost.Text = "Automate All Posts\r\nStats from selected ";
            this.btnAutoPost.UseVisualStyleBackColor = true;
            this.btnAutoPost.Click += new System.EventHandler(this.btnAutoPost_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnFinishKudo);
            this.groupBox7.Controls.Add(this.cbKudoCalculate);
            this.groupBox7.Location = new System.Drawing.Point(26, 144);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(265, 53);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Auto: Kudos";
            // 
            // btnFinishKudo
            // 
            this.btnFinishKudo.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnFinishKudo.Location = new System.Drawing.Point(18, 18);
            this.btnFinishKudo.Name = "btnFinishKudo";
            this.btnFinishKudo.Size = new System.Drawing.Size(95, 23);
            this.btnFinishKudo.TabIndex = 9;
            this.btnFinishKudo.Text = "Start at selection";
            this.btnFinishKudo.UseVisualStyleBackColor = true;
            this.btnFinishKudo.Click += new System.EventHandler(this.btnFinishKudo_Click);
            // 
            // cbKudoCalculate
            // 
            this.cbKudoCalculate.AutoSize = true;
            this.cbKudoCalculate.Location = new System.Drawing.Point(140, 18);
            this.cbKudoCalculate.Name = "cbKudoCalculate";
            this.cbKudoCalculate.Size = new System.Drawing.Size(94, 17);
            this.cbKudoCalculate.TabIndex = 11;
            this.cbKudoCalculate.Text = "Re-Fetch data";
            this.cbKudoCalculate.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnFinishSol);
            this.groupBox6.Controls.Add(this.cbSolCalculate);
            this.groupBox6.Location = new System.Drawing.Point(26, 85);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(265, 53);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Auto: Solutions";
            // 
            // btnFinishSol
            // 
            this.btnFinishSol.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnFinishSol.Location = new System.Drawing.Point(18, 19);
            this.btnFinishSol.Name = "btnFinishSol";
            this.btnFinishSol.Size = new System.Drawing.Size(95, 23);
            this.btnFinishSol.TabIndex = 10;
            this.btnFinishSol.Text = "Start at selection";
            this.btnFinishSol.UseVisualStyleBackColor = true;
            this.btnFinishSol.Click += new System.EventHandler(this.btnFinishSol_Click);
            // 
            // cbSolCalculate
            // 
            this.cbSolCalculate.AutoSize = true;
            this.cbSolCalculate.Location = new System.Drawing.Point(140, 23);
            this.cbSolCalculate.Name = "cbSolCalculate";
            this.cbSolCalculate.Size = new System.Drawing.Size(91, 17);
            this.cbSolCalculate.TabIndex = 12;
            this.cbSolCalculate.Text = "Re-fetch data";
            this.cbSolCalculate.UseVisualStyleBackColor = true;
            // 
            // btnDelExpert
            // 
            this.btnDelExpert.ForeColor = System.Drawing.Color.Red;
            this.btnDelExpert.Location = new System.Drawing.Point(219, 331);
            this.btnDelExpert.Name = "btnDelExpert";
            this.btnDelExpert.Size = new System.Drawing.Size(95, 23);
            this.btnDelExpert.TabIndex = 14;
            this.btnDelExpert.Text = "Delete Experts";
            this.btnDelExpert.UseVisualStyleBackColor = true;
            this.btnDelExpert.Click += new System.EventHandler(this.btnDelExpert_Click);
            // 
            // btnFinishRef
            // 
            this.btnFinishRef.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnFinishRef.Location = new System.Drawing.Point(26, 220);
            this.btnFinishRef.Name = "btnFinishRef";
            this.btnFinishRef.Size = new System.Drawing.Size(113, 41);
            this.btnFinishRef.TabIndex = 13;
            this.btnFinishRef.Text = "Automate Referrals\r\nfrom selected row";
            this.btnFinishRef.UseVisualStyleBackColor = true;
            this.btnFinishRef.Click += new System.EventHandler(this.btnFinishRef_Click);
            // 
            // btnGetRange
            // 
            this.btnGetRange.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnGetRange.Location = new System.Drawing.Point(15, 33);
            this.btnGetRange.Name = "btnGetRange";
            this.btnGetRange.Size = new System.Drawing.Size(95, 35);
            this.btnGetRange.TabIndex = 8;
            this.btnGetRange.Text = "Get Sorted Date\r\nRange solutions";
            this.btnGetRange.UseVisualStyleBackColor = true;
            this.btnGetRange.Click += new System.EventHandler(this.btnGetRange_Click);
            // 
            // btnDelBlanks
            // 
            this.btnDelBlanks.ForeColor = System.Drawing.Color.Red;
            this.btnDelBlanks.Location = new System.Drawing.Point(219, 302);
            this.btnDelBlanks.Name = "btnDelBlanks";
            this.btnDelBlanks.Size = new System.Drawing.Size(95, 23);
            this.btnDelBlanks.TabIndex = 5;
            this.btnDelBlanks.Text = "Del Empty Users";
            this.btnDelBlanks.UseVisualStyleBackColor = true;
            this.btnDelBlanks.Click += new System.EventHandler(this.btnDelBlanks_Click);
            // 
            // btnDelContacts
            // 
            this.btnDelContacts.ForeColor = System.Drawing.Color.Red;
            this.btnDelContacts.Location = new System.Drawing.Point(219, 232);
            this.btnDelContacts.Name = "btnDelContacts";
            this.btnDelContacts.Size = new System.Drawing.Size(95, 23);
            this.btnDelContacts.TabIndex = 4;
            this.btnDelContacts.Text = "Delete Users";
            this.toolTip1.SetToolTip(this.btnDelContacts, "Deletes the contact file it will be\r\nrebuilt when the program restarts\r\nYou must " +
        "run the count tools and\r\ndecide which users to delete");
            this.btnDelContacts.UseVisualStyleBackColor = true;
            this.btnDelContacts.Click += new System.EventHandler(this.btnDelContacts_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGetAllPosts);
            this.groupBox2.Controls.Add(this.btnKudoRef);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.pbGetSol);
            this.groupBox2.Controls.Add(this.btnGetKudo);
            this.groupBox2.Controls.Add(this.btnGetSol);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.groupBox2.Location = new System.Drawing.Point(16, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 420);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Obtain Statistics";
            // 
            // btnGetAllPosts
            // 
            this.btnGetAllPosts.Location = new System.Drawing.Point(19, 328);
            this.btnGetAllPosts.Name = "btnGetAllPosts";
            this.btnGetAllPosts.Size = new System.Drawing.Size(111, 23);
            this.btnGetAllPosts.TabIndex = 9;
            this.btnGetAllPosts.Text = "Get All Posts";
            this.btnGetAllPosts.UseVisualStyleBackColor = true;
            this.btnGetAllPosts.Click += new System.EventHandler(this.btnGetAllPosts_Click);
            // 
            // btnKudoRef
            // 
            this.btnKudoRef.Location = new System.Drawing.Point(17, 286);
            this.btnKudoRef.Name = "btnKudoRef";
            this.btnKudoRef.Size = new System.Drawing.Size(111, 23);
            this.btnKudoRef.TabIndex = 7;
            this.btnKudoRef.Text = "Get Kudo Referrals";
            this.btnKudoRef.UseVisualStyleBackColor = true;
            this.btnKudoRef.Click += new System.EventHandler(this.btnKudoRef_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbRestart);
            this.groupBox5.Controls.Add(this.btnUseDate);
            this.groupBox5.Controls.Add(this.tbCutTime);
            this.groupBox5.Controls.Add(this.tbYear);
            this.groupBox5.Location = new System.Drawing.Point(20, 147);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(156, 118);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Start Date";
            // 
            // lbRestart
            // 
            this.lbRestart.AutoSize = true;
            this.lbRestart.ForeColor = System.Drawing.Color.Red;
            this.lbRestart.Location = new System.Drawing.Point(10, 82);
            this.lbRestart.Name = "lbRestart";
            this.lbRestart.Size = new System.Drawing.Size(82, 13);
            this.lbRestart.TabIndex = 3;
            this.lbRestart.Text = "Program will exit";
            this.lbRestart.Visible = false;
            // 
            // btnUseDate
            // 
            this.btnUseDate.Enabled = false;
            this.btnUseDate.Location = new System.Drawing.Point(9, 47);
            this.btnUseDate.Name = "btnUseDate";
            this.btnUseDate.Size = new System.Drawing.Size(101, 23);
            this.btnUseDate.TabIndex = 2;
            this.btnUseDate.Text = "Use This Date";
            this.toolTip1.SetToolTip(this.btnUseDate, "Program will exit");
            this.btnUseDate.UseVisualStyleBackColor = true;
            this.btnUseDate.Click += new System.EventHandler(this.btnUseDate_Click);
            // 
            // tbCutTime
            // 
            this.tbCutTime.Location = new System.Drawing.Point(6, 92);
            this.tbCutTime.Name = "tbCutTime";
            this.tbCutTime.ReadOnly = true;
            this.tbCutTime.Size = new System.Drawing.Size(65, 20);
            this.tbCutTime.TabIndex = 1;
            this.tbCutTime.Text = "12:00 AM";
            this.tbCutTime.Visible = false;
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(8, 19);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(76, 20);
            this.tbYear.TabIndex = 0;
            this.tbYear.Text = "01-01-2024";
            this.tbYear.TextChanged += new System.EventHandler(this.tbYear_TextChanged);
            // 
            // pbGetSol
            // 
            this.pbGetSol.Location = new System.Drawing.Point(20, 110);
            this.pbGetSol.Name = "pbGetSol";
            this.pbGetSol.Size = new System.Drawing.Size(156, 19);
            this.pbGetSol.TabIndex = 5;
            // 
            // btnGetKudo
            // 
            this.btnGetKudo.Location = new System.Drawing.Point(20, 67);
            this.btnGetKudo.Name = "btnGetKudo";
            this.btnGetKudo.Size = new System.Drawing.Size(95, 23);
            this.btnGetKudo.TabIndex = 4;
            this.btnGetKudo.Text = "Get Kudos";
            this.btnGetKudo.UseVisualStyleBackColor = true;
            this.btnGetKudo.Click += new System.EventHandler(this.btnGetKudo_Click);
            // 
            // btnGetSol
            // 
            this.btnGetSol.Location = new System.Drawing.Point(20, 33);
            this.btnGetSol.Name = "btnGetSol";
            this.btnGetSol.Size = new System.Drawing.Size(95, 23);
            this.btnGetSol.TabIndex = 3;
            this.btnGetSol.Text = "Get Solutions";
            this.btnGetSol.UseVisualStyleBackColor = true;
            this.btnGetSol.Click += new System.EventHandler(this.btnGetSol_Click);
            // 
            // lbCnt
            // 
            this.lbCnt.AutoSize = true;
            this.lbCnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCnt.ForeColor = System.Drawing.Color.Red;
            this.lbCnt.Location = new System.Drawing.Point(6, 43);
            this.lbCnt.Name = "lbCnt";
            this.lbCnt.Size = new System.Drawing.Size(31, 16);
            this.lbCnt.TabIndex = 8;
            this.lbCnt.Text = "#cnt";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnHist);
            this.groupBox4.Controls.Add(this.btnScatPlot);
            this.groupBox4.Location = new System.Drawing.Point(993, 35);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(175, 149);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Solution Graphs";
            // 
            // btnHist
            // 
            this.btnHist.Location = new System.Drawing.Point(15, 75);
            this.btnHist.Name = "btnHist";
            this.btnHist.Size = new System.Drawing.Size(124, 23);
            this.btnHist.TabIndex = 2;
            this.btnHist.Text = "Kudo  Plot";
            this.btnHist.UseVisualStyleBackColor = true;
            this.btnHist.Click += new System.EventHandler(this.btnHist_Click);
            // 
            // btnScatPlot
            // 
            this.btnScatPlot.Location = new System.Drawing.Point(15, 35);
            this.btnScatPlot.Name = "btnScatPlot";
            this.btnScatPlot.Size = new System.Drawing.Size(124, 23);
            this.btnScatPlot.TabIndex = 1;
            this.btnScatPlot.Text = "Solution Graphs";
            this.btnScatPlot.UseVisualStyleBackColor = true;
            this.btnScatPlot.Click += new System.EventHandler(this.btnScatPlot_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MakeKUtbl);
            this.groupBox1.Controls.Add(this.pbAuthors);
            this.groupBox1.Controls.Add(this.btnCountKudos);
            this.groupBox1.Controls.Add(this.btnCount);
            this.groupBox1.Location = new System.Drawing.Point(16, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 220);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initial Build";
            // 
            // MakeKUtbl
            // 
            this.MakeKUtbl.Enabled = false;
            this.MakeKUtbl.Location = new System.Drawing.Point(9, 35);
            this.MakeKUtbl.Name = "MakeKUtbl";
            this.MakeKUtbl.Size = new System.Drawing.Size(111, 23);
            this.MakeKUtbl.TabIndex = 3;
            this.MakeKUtbl.Text = "Make Kudo Lookup";
            this.MakeKUtbl.UseVisualStyleBackColor = true;
            this.MakeKUtbl.Click += new System.EventHandler(this.MakeKUtbl_Click);
            // 
            // pbAuthors
            // 
            this.pbAuthors.Location = new System.Drawing.Point(9, 162);
            this.pbAuthors.Name = "pbAuthors";
            this.pbAuthors.Size = new System.Drawing.Size(156, 19);
            this.pbAuthors.TabIndex = 2;
            // 
            // btnCountKudos
            // 
            this.btnCountKudos.Location = new System.Drawing.Point(9, 119);
            this.btnCountKudos.Name = "btnCountKudos";
            this.btnCountKudos.Size = new System.Drawing.Size(95, 23);
            this.btnCountKudos.TabIndex = 1;
            this.btnCountKudos.Text = "Count Kudos";
            this.toolTip1.SetToolTip(this.btnCountKudos, "this only counts kudo pages");
            this.btnCountKudos.UseVisualStyleBackColor = true;
            this.btnCountKudos.Click += new System.EventHandler(this.btnCountKudos_Click);
            // 
            // btnCount
            // 
            this.btnCount.Location = new System.Drawing.Point(9, 85);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(95, 23);
            this.btnCount.TabIndex = 0;
            this.btnCount.Text = "Count Solutions";
            this.toolTip1.SetToolTip(this.btnCount, "Just gets number pages");
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // dgvExpert
            // 
            this.dgvExpert.AllowUserToAddRows = false;
            this.dgvExpert.AllowUserToDeleteRows = false;
            this.dgvExpert.AllowUserToResizeColumns = false;
            this.dgvExpert.AllowUserToResizeRows = false;
            this.dgvExpert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExpert.Location = new System.Drawing.Point(249, 14);
            this.dgvExpert.Name = "dgvExpert";
            this.dgvExpert.Size = new System.Drawing.Size(707, 680);
            this.dgvExpert.TabIndex = 6;
            this.dgvExpert.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExpert_CellDoubleClick);
            this.dgvExpert.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvExpert_ColumnHeaderMouseClick);
            // 
            // TimerAcnt
            // 
            this.TimerAcnt.Interval = 1000;
            this.TimerAcnt.Tick += new System.EventHandler(this.TimerAcnt_Tick);
            // 
            // FinishTimer
            // 
            this.FinishTimer.Interval = 5000;
            this.FinishTimer.Tick += new System.EventHandler(this.FinishTimer_Tick);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.Red;
            this.btnStop.Location = new System.Drawing.Point(1232, 35);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 44);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.UseWaitCursor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lbCurrentVol
            // 
            this.lbCurrentVol.AutoSize = true;
            this.lbCurrentVol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCurrentVol.ForeColor = System.Drawing.Color.Red;
            this.lbCurrentVol.Location = new System.Drawing.Point(6, 20);
            this.lbCurrentVol.Name = "lbCurrentVol";
            this.lbCurrentVol.Size = new System.Drawing.Size(44, 16);
            this.lbCurrentVol.TabIndex = 11;
            this.lbCurrentVol.Text = "Name";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lbCurrentVol);
            this.groupBox8.Controls.Add(this.lbCnt);
            this.groupBox8.Location = new System.Drawing.Point(1189, 105);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(180, 79);
            this.groupBox8.TabIndex = 11;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Tasking Info";
            // 
            // ExpertInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 725);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lbWhereExe);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvExpert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExpertInfo";
            this.Text = "ExpertStats";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.ExpertInfo_HelpButtonClicked);
            this.groupBox3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpert)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbWhereExe;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnFinishKudo;
        private System.Windows.Forms.CheckBox cbKudoCalculate;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnFinishSol;
        private System.Windows.Forms.CheckBox cbSolCalculate;
        private System.Windows.Forms.Button btnDelExpert;
        private System.Windows.Forms.Button btnFinishRef;
        private System.Windows.Forms.Button btnGetRange;
        private System.Windows.Forms.Button btnDelBlanks;
        private System.Windows.Forms.Button btnDelContacts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnGetAllPosts;
        private System.Windows.Forms.Label lbCnt;
        private System.Windows.Forms.Button btnKudoRef;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tbCutTime;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.ProgressBar pbGetSol;
        private System.Windows.Forms.Button btnGetKudo;
        private System.Windows.Forms.Button btnGetSol;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnHist;
        private System.Windows.Forms.Button btnScatPlot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button MakeKUtbl;
        private System.Windows.Forms.ProgressBar pbAuthors;
        private System.Windows.Forms.Button btnCountKudos;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.DataGridView dgvExpert;
        private System.Windows.Forms.Timer TimerAcnt;
        private System.Windows.Forms.Timer FinishTimer;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAutoPost;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lbCurrentVol;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnUseDate;
        private System.Windows.Forms.Label lbRestart;
        private System.Windows.Forms.Button btnClearCts;
    }
}