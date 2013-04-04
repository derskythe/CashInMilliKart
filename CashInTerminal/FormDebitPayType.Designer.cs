namespace CashInTerminal
{
    partial class FormDebitPayType
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnByClientCode = new System.Windows.Forms.Button();
            this.btnByPassportAndAccount = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // btnByClientCode
            // 
            this.btnByClientCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnByClientCode.BackColor = System.Drawing.Color.Transparent;
            this.btnByClientCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnByClientCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnByClientCode.Location = new System.Drawing.Point(12, 63);
            this.btnByClientCode.Name = "btnByClientCode";
            this.btnByClientCode.Size = new System.Drawing.Size(996, 105);
            this.btnByClientCode.TabIndex = 11;
            this.btnByClientCode.Text = "По клиентскому коду";
            this.btnByClientCode.UseVisualStyleBackColor = false;
            this.btnByClientCode.Click += new System.EventHandler(this.BtnByCardFullClick);
            // 
            // btnByPassportAndAccount
            // 
            this.btnByPassportAndAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnByPassportAndAccount.BackColor = System.Drawing.Color.Transparent;
            this.btnByPassportAndAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnByPassportAndAccount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnByPassportAndAccount.Location = new System.Drawing.Point(12, 174);
            this.btnByPassportAndAccount.Name = "btnByPassportAndAccount";
            this.btnByPassportAndAccount.Size = new System.Drawing.Size(996, 105);
            this.btnByPassportAndAccount.TabIndex = 12;
            this.btnByPassportAndAccount.Text = "По номеру кредита и паспорту";
            this.btnByPassportAndAccount.UseVisualStyleBackColor = false;
            this.btnByPassportAndAccount.Click += new System.EventHandler(this.BtnByCardAccountNumberClick);
            // 
            // FormDebitPayType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.Controls.Add(this.btnByPassportAndAccount);
            this.Controls.Add(this.btnByClientCode);
            this.Controls.Add(this.btnBack);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormDebitPayType";
            this.Text = "FormDebitPayType";
            this.Controls.SetChildIndex(this.btnBack, 0);
            this.Controls.SetChildIndex(this.btnByClientCode, 0);
            this.Controls.SetChildIndex(this.btnByPassportAndAccount, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnByClientCode;
        private System.Windows.Forms.Button btnByPassportAndAccount;
    }
}