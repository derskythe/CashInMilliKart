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
            this.btnByCardFull = new System.Windows.Forms.Button();
            this.btnByCardAccountNumber = new System.Windows.Forms.Button();
            this.btnCurrent = new System.Windows.Forms.Button();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBack.Location = new System.Drawing.Point(12, 487);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(260, 50);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // btnByCardFull
            // 
            this.btnByCardFull.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnByCardFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnByCardFull.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnByCardFull.Location = new System.Drawing.Point(356, 24);
            this.btnByCardFull.Name = "btnByCardFull";
            this.btnByCardFull.Size = new System.Drawing.Size(389, 50);
            this.btnByCardFull.TabIndex = 11;
            this.btnByCardFull.Text = "Карточный. Полный номер карты";
            this.btnByCardFull.UseVisualStyleBackColor = true;
            this.btnByCardFull.Click += new System.EventHandler(this.BtnByCardFullClick);
            // 
            // btnByCardAccountNumber
            // 
            this.btnByCardAccountNumber.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnByCardAccountNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnByCardAccountNumber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnByCardAccountNumber.Location = new System.Drawing.Point(356, 97);
            this.btnByCardAccountNumber.Name = "btnByCardAccountNumber";
            this.btnByCardAccountNumber.Size = new System.Drawing.Size(389, 50);
            this.btnByCardAccountNumber.TabIndex = 12;
            this.btnByCardAccountNumber.Text = "Карточный. Номер счета";
            this.btnByCardAccountNumber.UseVisualStyleBackColor = true;
            this.btnByCardAccountNumber.Click += new System.EventHandler(this.BtnByCardAccountNumberClick);
            // 
            // btnCurrent
            // 
            this.btnCurrent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnCurrent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCurrent.Location = new System.Drawing.Point(356, 170);
            this.btnCurrent.Name = "btnCurrent";
            this.btnCurrent.Size = new System.Drawing.Size(389, 50);
            this.btnCurrent.TabIndex = 12;
            this.btnCurrent.Text = "Текущий";
            this.btnCurrent.UseVisualStyleBackColor = true;
            this.btnCurrent.Click += new System.EventHandler(this.BtnCurrentClick);
            // 
            // btnDeposit
            // 
            this.btnDeposit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeposit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnDeposit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeposit.Location = new System.Drawing.Point(358, 246);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(389, 50);
            this.btnDeposit.TabIndex = 12;
            this.btnDeposit.Text = "Депозитный";
            this.btnDeposit.UseVisualStyleBackColor = true;
            this.btnDeposit.Click += new System.EventHandler(this.BtnDepositClick);
            // 
            // FormDebitPayType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 549);
            this.Controls.Add(this.btnDeposit);
            this.Controls.Add(this.btnCurrent);
            this.Controls.Add(this.btnByCardAccountNumber);
            this.Controls.Add(this.btnByCardFull);
            this.Controls.Add(this.btnBack);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormDebitPayType";
            this.Text = "FormDebitPayType";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnByCardFull;
        private System.Windows.Forms.Button btnByCardAccountNumber;
        private System.Windows.Forms.Button btnCurrent;
        private System.Windows.Forms.Button btnDeposit;
    }
}