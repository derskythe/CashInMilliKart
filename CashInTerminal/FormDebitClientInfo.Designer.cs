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
            this.lblDebitInfoDate = new System.Windows.Forms.Label();
            this.lblDebitInfoPassport = new System.Windows.Forms.Label();
            this.lblDebitInfoAccount = new System.Windows.Forms.Label();
            this.lblDebitInfoFullname = new System.Windows.Forms.Label();
            this.btnDebitInfoBack = new System.Windows.Forms.Button();
            this.btnDebitInfoNext = new System.Windows.Forms.Button();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDebitInfoDate
            // 
            this.lblDebitInfoDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDebitInfoDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblDebitInfoDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDebitInfoDate.Location = new System.Drawing.Point(105, 206);
            this.lblDebitInfoDate.Name = "lblDebitInfoDate";
            this.lblDebitInfoDate.Size = new System.Drawing.Size(713, 24);
            this.lblDebitInfoDate.TabIndex = 13;
            this.lblDebitInfoDate.Text = ".";
            this.lblDebitInfoDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDebitInfoPassport
            // 
            this.lblDebitInfoPassport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDebitInfoPassport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblDebitInfoPassport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDebitInfoPassport.Location = new System.Drawing.Point(105, 169);
            this.lblDebitInfoPassport.Name = "lblDebitInfoPassport";
            this.lblDebitInfoPassport.Size = new System.Drawing.Size(713, 24);
            this.lblDebitInfoPassport.TabIndex = 11;
            this.lblDebitInfoPassport.Text = ".";
            this.lblDebitInfoPassport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDebitInfoAccount
            // 
            this.lblDebitInfoAccount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDebitInfoAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblDebitInfoAccount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDebitInfoAccount.Location = new System.Drawing.Point(105, 132);
            this.lblDebitInfoAccount.Name = "lblDebitInfoAccount";
            this.lblDebitInfoAccount.Size = new System.Drawing.Size(713, 24);
            this.lblDebitInfoAccount.TabIndex = 12;
            this.lblDebitInfoAccount.Text = ".";
            this.lblDebitInfoAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDebitInfoFullname
            // 
            this.lblDebitInfoFullname.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDebitInfoFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblDebitInfoFullname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDebitInfoFullname.Location = new System.Drawing.Point(105, 96);
            this.lblDebitInfoFullname.Name = "lblDebitInfoFullname";
            this.lblDebitInfoFullname.Size = new System.Drawing.Size(713, 23);
            this.lblDebitInfoFullname.TabIndex = 10;
            this.lblDebitInfoFullname.Text = ".";
            this.lblDebitInfoFullname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDebitInfoBack
            // 
            this.btnDebitInfoBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDebitInfoBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnDebitInfoBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDebitInfoBack.Location = new System.Drawing.Point(12, 428);
            this.btnDebitInfoBack.Name = "btnDebitInfoBack";
            this.btnDebitInfoBack.Size = new System.Drawing.Size(260, 50);
            this.btnDebitInfoBack.TabIndex = 9;
            this.btnDebitInfoBack.Text = "Назад";
            this.btnDebitInfoBack.UseVisualStyleBackColor = true;
            this.btnDebitInfoBack.Click += new System.EventHandler(this.BtnDebitInfoBackClick);
            // 
            // btnDebitInfoNext
            // 
            this.btnDebitInfoNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebitInfoNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnDebitInfoNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDebitInfoNext.Location = new System.Drawing.Point(651, 428);
            this.btnDebitInfoNext.Name = "btnDebitInfoNext";
            this.btnDebitInfoNext.Size = new System.Drawing.Size(260, 50);
            this.btnDebitInfoNext.TabIndex = 8;
            this.btnDebitInfoNext.Text = "Дальше";
            this.btnDebitInfoNext.UseVisualStyleBackColor = true;
            this.btnDebitInfoNext.Click += new System.EventHandler(this.BtnDebitInfoNextClick);
            // 
            // lblCurrency
            // 
            this.lblCurrency.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrency.Location = new System.Drawing.Point(105, 243);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(713, 24);
            this.lblCurrency.TabIndex = 14;
            this.lblCurrency.Text = ".";
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormDebitClientInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 490);
            this.Controls.Add(this.lblCurrency);
            this.Controls.Add(this.lblDebitInfoDate);
            this.Controls.Add(this.lblDebitInfoPassport);
            this.Controls.Add(this.lblDebitInfoAccount);
            this.Controls.Add(this.lblDebitInfoFullname);
            this.Controls.Add(this.btnDebitInfoBack);
            this.Controls.Add(this.btnDebitInfoNext);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormDebitClientInfo";
            this.Text = "FormDebitClientInfo";
            this.Load += new System.EventHandler(this.FormDebitClientInfoLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDebitInfoDate;
        private System.Windows.Forms.Label lblDebitInfoPassport;
        private System.Windows.Forms.Label lblDebitInfoAccount;
        private System.Windows.Forms.Label lblDebitInfoFullname;
        private System.Windows.Forms.Button btnDebitInfoBack;
        private System.Windows.Forms.Button btnDebitInfoNext;
        private System.Windows.Forms.Label lblCurrency;

    }
}