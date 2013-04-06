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
            this.btnBack = new System.Windows.Forms.Button();
            this.lblComission = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMoneyCurrency
            // 
            this.lblMoneyCurrency.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMoneyCurrency.BackColor = System.Drawing.Color.Transparent;
            this.lblMoneyCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoneyCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMoneyCurrency.Location = new System.Drawing.Point(435, 288);
            this.lblMoneyCurrency.Name = "lblMoneyCurrency";
            this.lblMoneyCurrency.Size = new System.Drawing.Size(324, 112);
            this.lblMoneyCurrency.TabIndex = 10;
            this.lblMoneyCurrency.Text = "AZN";
            this.lblMoneyCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMoneyTotal
            // 
            this.lblMoneyTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMoneyTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblMoneyTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoneyTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMoneyTotal.Location = new System.Drawing.Point(39, 288);
            this.lblMoneyTotal.Name = "lblMoneyTotal";
            this.lblMoneyTotal.Size = new System.Drawing.Size(414, 112);
            this.lblMoneyTotal.TabIndex = 12;
            this.lblMoneyTotal.Text = "0";
            this.lblMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(91, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(839, 124);
            this.label2.TabIndex = 11;
            this.label2.Text = "Получено";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMoneyNext
            // 
            this.btnMoneyNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoneyNext.BackColor = System.Drawing.Color.Transparent;
            this.btnMoneyNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnMoneyNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMoneyNext.Location = new System.Drawing.Point(687, 658);
            this.btnMoneyNext.Name = "btnMoneyNext";
            this.btnMoneyNext.Size = new System.Drawing.Size(321, 76);
            this.btnMoneyNext.TabIndex = 9;
            this.btnMoneyNext.Text = "Оплатить";
            this.btnMoneyNext.UseVisualStyleBackColor = false;
            this.btnMoneyNext.Click += new System.EventHandler(this.BtnMoneyNextClick);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBack.Location = new System.Drawing.Point(12, 658);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(321, 76);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // lblComission
            // 
            this.lblComission.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblComission.BackColor = System.Drawing.Color.Transparent;
            this.lblComission.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComission.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblComission.Location = new System.Drawing.Point(12, 548);
            this.lblComission.Name = "lblComission";
            this.lblComission.Size = new System.Drawing.Size(996, 40);
            this.lblComission.TabIndex = 22;
            this.lblComission.Text = "Комиссия за операцию 0,4%";
            this.lblComission.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblComission.Visible = false;
            // 
            // FormMoneyInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.Controls.Add(this.lblComission);
            this.Controls.Add(this.lblMoneyCurrency);
            this.Controls.Add(this.lblMoneyTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnMoneyNext);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormMoneyInput";
            this.Text = "FormMoneyInput";
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