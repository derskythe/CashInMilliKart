namespace CashInTerminal
{
    partial class FormMoneyInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMoneyInput));
            this.lblMoneyCurrency = new System.Windows.Forms.Label();
            this.lblMoneyTotal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMoneyNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblComission = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMoneyCurrency
            // 
            resources.ApplyResources(this.lblMoneyCurrency, "lblMoneyCurrency");
            this.lblMoneyCurrency.BackColor = System.Drawing.Color.Transparent;
            this.lblMoneyCurrency.Name = "lblMoneyCurrency";
            // 
            // lblMoneyTotal
            // 
            resources.ApplyResources(this.lblMoneyTotal, "lblMoneyTotal");
            this.lblMoneyTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblMoneyTotal.Name = "lblMoneyTotal";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // btnMoneyNext
            // 
            resources.ApplyResources(this.btnMoneyNext, "btnMoneyNext");
            this.btnMoneyNext.BackColor = System.Drawing.Color.Transparent;
            this.btnMoneyNext.Name = "btnMoneyNext";
            this.btnMoneyNext.UseVisualStyleBackColor = false;
            this.btnMoneyNext.Click += new System.EventHandler(this.BtnMoneyNextClick);
            // 
            // btnBack
            // 
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // lblComission
            // 
            resources.ApplyResources(this.lblComission, "lblComission");
            this.lblComission.BackColor = System.Drawing.Color.Transparent;
            this.lblComission.Name = "lblComission";
            // 
            // FormMoneyInput
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblComission);
            this.Controls.Add(this.lblMoneyCurrency);
            this.Controls.Add(this.lblMoneyTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnMoneyNext);
            this.Name = "FormMoneyInput";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMoneyInputFormClosing);
            this.Load += new System.EventHandler(this.FormMoneyInputLoad);
            this.Controls.SetChildIndex(this.btnMoneyNext, 0);
            this.Controls.SetChildIndex(this.btnBack, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lblMoneyTotal, 0);
            this.Controls.SetChildIndex(this.lblMoneyCurrency, 0);
            this.Controls.SetChildIndex(this.lblComission, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMoneyCurrency;
        private System.Windows.Forms.Label lblMoneyTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMoneyNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblComission;

    }
}