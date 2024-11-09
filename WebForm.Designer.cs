namespace ExpertStats
{
    partial class WebForm
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
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.TimerWalking = new System.Windows.Forms.Timer(this.components);
            this.pbAll = new System.Windows.Forms.ProgressBar();
            this.lbDaysWanted = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.tbPage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.SuspendLayout();
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView21.Location = new System.Drawing.Point(12, 12);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(118, 108);
            this.webView21.TabIndex = 0;
            this.webView21.ZoomFactor = 1D;
            this.webView21.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webView21_NavigationCompleted);
            // 
            // TimerWalking
            // 
            this.TimerWalking.Interval = 1000;
            this.TimerWalking.Tick += new System.EventHandler(this.TimerWalking_Tick);
            // 
            // pbAll
            // 
            this.pbAll.Location = new System.Drawing.Point(283, 61);
            this.pbAll.Name = "pbAll";
            this.pbAll.Size = new System.Drawing.Size(277, 23);
            this.pbAll.Step = 1;
            this.pbAll.TabIndex = 1;
            // 
            // lbDaysWanted
            // 
            this.lbDaysWanted.AutoSize = true;
            this.lbDaysWanted.Location = new System.Drawing.Point(280, 28);
            this.lbDaysWanted.Name = "lbDaysWanted";
            this.lbDaysWanted.Size = new System.Drawing.Size(99, 13);
            this.lbDaysWanted.TabIndex = 2;
            this.lbDaysWanted.Text = "300 Days expected";
            // 
            // btnStop
            // 
            this.btnStop.ForeColor = System.Drawing.Color.Red;
            this.btnStop.Location = new System.Drawing.Point(283, 134);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tbPage
            // 
            this.tbPage.Location = new System.Drawing.Point(371, 194);
            this.tbPage.Name = "tbPage";
            this.tbPage.Size = new System.Drawing.Size(49, 20);
            this.tbPage.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Pages read";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(280, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "User Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(371, 239);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(166, 20);
            this.tbName.TabIndex = 7;
            // 
            // WebForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 309);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPage);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lbDaysWanted);
            this.Controls.Add(this.pbAll);
            this.Controls.Add(this.webView21);
            this.Name = "WebForm";
            this.Text = "WebForm";
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Timer TimerWalking;
        private System.Windows.Forms.ProgressBar pbAll;
        private System.Windows.Forms.Label lbDaysWanted;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox tbPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
    }
}