using CashInTerminal.Controls;

namespace CashInTerminal
{
    partial class FormCreditClientInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditClientInfo));
            this.lbl7 = new System.Windows.Forms.Label();
            this.btnCreditInfoBack = new System.Windows.Forms.Button();
            this.btnCreditInfoNext = new System.Windows.Forms.Button();
            this.lbl8 = new System.Windows.Forms.Label();
            this.groupBox1 = new CashInTerminal.Controls.MyGroupBox();
            this.lbl6 = new System.Windows.Forms.Label();
            this.lbl5 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl7
            // 
            resources.ApplyResources(this.lbl7, "lbl7");
            this.lbl7.BackColor = System.Drawing.Color.Transparent;
            this.lbl7.Name = "lbl7";
            // 
            // btnCreditInfoBack
            // 
            resources.ApplyResources(this.btnCreditInfoBack, "btnCreditInfoBack");
            this.btnCreditInfoBack.BackColor = System.Drawing.Color.Transparent;
            this.btnCreditInfoBack.Name = "btnCreditInfoBack";
            this.btnCreditInfoBack.UseVisualStyleBackColor = false;
            this.btnCreditInfoBack.Click += new System.EventHandler(this.BtnCreditInfoBackClick);
            // 
            // btnCreditInfoNext
            // 
            resources.ApplyResources(this.btnCreditInfoNext, "btnCreditInfoNext");
            this.btnCreditInfoNext.BackColor = System.Drawing.Color.Transparent;
            this.btnCreditInfoNext.Name = "btnCreditInfoNext";
            this.btnCreditInfoNext.UseVisualStyleBackColor = false;
            this.btnCreditInfoNext.Click += new System.EventHandler(this.BtnCreditInfoNextClick);
            // 
            // lbl8
            // 
            resources.ApplyResources(this.lbl8, "lbl8");
            this.lbl8.BackColor = System.Drawing.Color.Transparent;
            this.lbl8.Name = "lbl8";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.groupBox1.BorderColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.groupBox1.BorderRadius = 5;
            this.groupBox1.Controls.Add(this.lbl6);
            this.groupBox1.Controls.Add(this.lbl5);
            this.groupBox1.Controls.Add(this.lbl4);
            this.groupBox1.Controls.Add(this.lbl3);
            this.groupBox1.Controls.Add(this.lbl2);
            this.groupBox1.Controls.Add(this.lbl1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lbl6
            // 
            resources.ApplyResources(this.lbl6, "lbl6");
            this.lbl6.BackColor = System.Drawing.Color.Transparent;
            this.lbl6.Name = "lbl6";
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
            // FormCreditClientInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl8);
            this.Controls.Add(this.lbl7);
            this.Controls.Add(this.btnCreditInfoBack);
            this.Controls.Add(this.btnCreditInfoNext);
            this.Name = "FormCreditClientInfo";
            this.Load += new System.EventHandler(this.FormCreditClientInfoLoad);
            this.Controls.SetChildIndex(this.btnCreditInfoNext, 0);
            this.Controls.SetChildIndex(this.btnCreditInfoBack, 0);
            this.Controls.SetChildIndex(this.lbl7, 0);
            this.Controls.SetChildIndex(this.lbl8, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl7;
        private System.Windows.Forms.Button btnCreditInfoBack;
        private System.Windows.Forms.Button btnCreditInfoNext;
        private System.Windows.Forms.Label lbl8;
        private MyGroupBox groupBox1;
        private System.Windows.Forms.Label lbl6;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;

    }
}