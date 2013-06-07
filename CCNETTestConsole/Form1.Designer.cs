namespace CCNETTestConsole
{
    partial class Form1
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
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnStartPool = new System.Windows.Forms.Button();
            this.btnStopPool = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnPool = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGetBills = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(45, 10);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 0;
            this.txtPort.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(12, 45);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(260, 23);
            this.btnInit.TabIndex = 2;
            this.btnInit.Text = "Init";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.BtnInitClick);
            // 
            // btnStartPool
            // 
            this.btnStartPool.Location = new System.Drawing.Point(12, 74);
            this.btnStartPool.Name = "btnStartPool";
            this.btnStartPool.Size = new System.Drawing.Size(260, 23);
            this.btnStartPool.TabIndex = 2;
            this.btnStartPool.Text = "Start Pool";
            this.btnStartPool.UseVisualStyleBackColor = true;
            this.btnStartPool.Click += new System.EventHandler(this.BtnStartPoolClick);
            // 
            // btnStopPool
            // 
            this.btnStopPool.Location = new System.Drawing.Point(12, 103);
            this.btnStopPool.Name = "btnStopPool";
            this.btnStopPool.Size = new System.Drawing.Size(260, 23);
            this.btnStopPool.TabIndex = 2;
            this.btnStopPool.Text = "Stop pool";
            this.btnStopPool.UseVisualStyleBackColor = true;
            this.btnStopPool.Click += new System.EventHandler(this.BtnStopPoolClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(278, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(595, 222);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // btnPool
            // 
            this.btnPool.Location = new System.Drawing.Point(12, 133);
            this.btnPool.Name = "btnPool";
            this.btnPool.Size = new System.Drawing.Size(260, 23);
            this.btnPool.TabIndex = 4;
            this.btnPool.Text = "Pool";
            this.btnPool.UseVisualStyleBackColor = true;
            this.btnPool.Click += new System.EventHandler(this.BtnPoolClick);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(151, 7);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpenClick);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 162);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(260, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnResetClick);
            // 
            // btnGetBills
            // 
            this.btnGetBills.Location = new System.Drawing.Point(12, 191);
            this.btnGetBills.Name = "btnGetBills";
            this.btnGetBills.Size = new System.Drawing.Size(260, 23);
            this.btnGetBills.TabIndex = 2;
            this.btnGetBills.Text = "Get Bills";
            this.btnGetBills.UseVisualStyleBackColor = true;
            this.btnGetBills.Click += new System.EventHandler(this.BtnGetBillsClick);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.Location = new System.Drawing.Point(278, 238);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(595, 247);
            this.richTextBox2.TabIndex = 6;
            this.richTextBox2.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 488);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnPool);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnStopPool);
            this.Controls.Add(this.btnStartPool);
            this.Controls.Add(this.btnGetBills);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Name = "Form1";
            this.Text = "Test console";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1FormClosed);
            this.Load += new System.EventHandler(this.Form1Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnStartPool;
        private System.Windows.Forms.Button btnStopPool;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnPool;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGetBills;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}

