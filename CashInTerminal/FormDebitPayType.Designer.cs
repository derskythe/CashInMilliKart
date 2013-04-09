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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDebitPayType));
            this.btnBack = new System.Windows.Forms.Button();
            this.btnByClientCode = new System.Windows.Forms.Button();
            this.btnByPassportAndAccount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // btnByClientCode
            // 
            resources.ApplyResources(this.btnByClientCode, "btnByClientCode");
            this.btnByClientCode.BackColor = System.Drawing.Color.Transparent;
            this.btnByClientCode.Name = "btnByClientCode";
            this.btnByClientCode.UseVisualStyleBackColor = false;
            this.btnByClientCode.Click += new System.EventHandler(this.BtnByCardFullClick);
            // 
            // btnByPassportAndAccount
            // 
            resources.ApplyResources(this.btnByPassportAndAccount, "btnByPassportAndAccount");
            this.btnByPassportAndAccount.BackColor = System.Drawing.Color.Transparent;
            this.btnByPassportAndAccount.Name = "btnByPassportAndAccount";
            this.btnByPassportAndAccount.UseVisualStyleBackColor = false;
            this.btnByPassportAndAccount.Click += new System.EventHandler(this.BtnByCardAccountNumberClick);
            // 
            // FormDebitPayType
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnByPassportAndAccount);
            this.Controls.Add(this.btnByClientCode);
            this.Controls.Add(this.btnBack);
            this.Name = "FormDebitPayType";
            this.Load += new System.EventHandler(this.FormDebitPayTypeLoad);
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