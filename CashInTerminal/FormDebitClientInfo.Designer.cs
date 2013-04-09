namespace CashInTerminal
{
    partial class FormDebitClientInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDebitClientInfo));
            this.btnDebitInfoBack = new System.Windows.Forms.Button();
            this.btnDebitInfoNext = new System.Windows.Forms.Button();
            this.groupBox1 = new CashInTerminal.Controls.MyGroupBox();
            this.lbl5 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDebitInfoBack
            // 
            resources.ApplyResources(this.btnDebitInfoBack, "btnDebitInfoBack");
            this.btnDebitInfoBack.BackColor = System.Drawing.Color.Transparent;
            this.btnDebitInfoBack.Name = "btnDebitInfoBack";
            this.btnDebitInfoBack.UseVisualStyleBackColor = false;
            this.btnDebitInfoBack.Click += new System.EventHandler(this.BtnDebitInfoBackClick);
            // 
            // btnDebitInfoNext
            // 
            resources.ApplyResources(this.btnDebitInfoNext, "btnDebitInfoNext");
            this.btnDebitInfoNext.BackColor = System.Drawing.Color.Transparent;
            this.btnDebitInfoNext.Name = "btnDebitInfoNext";
            this.btnDebitInfoNext.UseVisualStyleBackColor = false;
            this.btnDebitInfoNext.Click += new System.EventHandler(this.BtnDebitInfoNextClick);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.groupBox1.BorderColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.groupBox1.BorderRadius = 5;
            this.groupBox1.Controls.Add(this.lbl5);
            this.groupBox1.Controls.Add(this.lbl4);
            this.groupBox1.Controls.Add(this.lbl3);
            this.groupBox1.Controls.Add(this.lbl2);
            this.groupBox1.Controls.Add(this.lbl1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lbl5
            // 
            resources.ApplyResources(this.lbl5, "lbl5");
            this.lbl5.BackColor = System.Drawing.Color.Transparent;
            this.lbl5.Name = "lbl5";
            // 
            // lbl4
            // 
            resources.ApplyResources(this.lbl4, "lbl4");
            this.lbl4.BackColor = System.Drawing.Color.Transparent;
            this.lbl4.Name = "lbl4";
            // 
            // lbl3
            // 
            resources.ApplyResources(this.lbl3, "lbl3");
            this.lbl3.BackColor = System.Drawing.Color.Transparent;
            this.lbl3.Name = "lbl3";
            // 
            // lbl2
            // 
            resources.ApplyResources(this.lbl2, "lbl2");
            this.lbl2.BackColor = System.Drawing.Color.Transparent;
            this.lbl2.Name = "lbl2";
            // 
            // lbl1
            // 
            resources.ApplyResources(this.lbl1, "lbl1");
            this.lbl1.BackColor = System.Drawing.Color.Transparent;
            this.lbl1.Name = "lbl1";
            // 
            // FormDebitClientInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDebitInfoBack);
            this.Controls.Add(this.btnDebitInfoNext);
            this.Name = "FormDebitClientInfo";
            this.Load += new System.EventHandler(this.FormDebitClientInfoLoad);
            this.Controls.SetChildIndex(this.btnDebitInfoNext, 0);
            this.Controls.SetChildIndex(this.btnDebitInfoBack, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDebitInfoBack;
        private System.Windows.Forms.Button btnDebitInfoNext;
        private Controls.MyGroupBox groupBox1;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;

    }
}