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
            this.lblMoneyCurrency = new System.Windows.Forms.Label();
            this.lblMoneyTotal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMoneyNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMoneyCurrency
            // 
            this.lblMoneyCurrency.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMoneyCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblMoneyCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMoneyCurrency.Location = new System.Drawing.Point(477, 155);
            this.lblMoneyCurrency.Name = "lblMoneyCurrency";
            this.lblMoneyCurrency.Size = new System.Drawing.Size(132, 23);
            this.lblMoneyCurrency.TabIndex = 10;
            this.lblMoneyCurrency.Text = "AZN";
            this.lblMoneyCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMoneyTotal
            // 
            this.lblMoneyTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMoneyTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblMoneyTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMoneyTotal.Location = new System.Drawing.Point(316, 155);
            this.lblMoneyTotal.Name = "lblMoneyTotal";
            this.lblMoneyTotal.Size = new System.Drawing.Size(132, 23);
            this.lblMoneyTotal.TabIndex = 12;
            this.lblMoneyTotal.Text = "0";
            this.lblMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(42, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(839, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Получено";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMoneyNext
            // 
            this.btnMoneyNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoneyNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnMoneyNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMoneyNext.Location = new System.Drawing.Point(651, 428);
            this.btnMoneyNext.Name = "btnMoneyNext";
            this.btnMoneyNext.Size = new System.Drawing.Size(260, 50);
            this.btnMoneyNext.TabIndex = 9;
            this.btnMoneyNext.Text = "Дальше";
            this.btnMoneyNext.UseVisualStyleBackColor = true;
            this.btnMoneyNext.Click += new System.EventHandler(this.btnMoneyNext_Click);
            // 
            // FormMoneyInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 490);
            this.Controls.Add(this.lblMoneyCurrency);
            this.Controls.Add(this.lblMoneyTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMoneyNext);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormMoneyInput";
            this.Text = "FormMoneyInput";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMoneyCurrency;
        private System.Windows.Forms.Label lblMoneyTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMoneyNext;

    }
}