namespace CashInTerminal
{
    partial class FormCreditTypeSelect
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
            this.btnCreditNumberAndPasport = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnBolCard = new System.Windows.Forms.Button();
            this.btnClientNumber = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreditNumberAndPasport
            // 
            this.btnCreditNumberAndPasport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreditNumberAndPasport.BackColor = System.Drawing.Color.Transparent;
            this.btnCreditNumberAndPasport.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnCreditNumberAndPasport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCreditNumberAndPasport.Location = new System.Drawing.Point(12, 129);
            this.btnCreditNumberAndPasport.Name = "btnCreditNumberAndPasport";
            this.btnCreditNumberAndPasport.Size = new System.Drawing.Size(996, 105);
            this.btnCreditNumberAndPasport.TabIndex = 11;
            this.btnCreditNumberAndPasport.Text = "По номеру кредита и паспорту";
            this.btnCreditNumberAndPasport.UseVisualStyleBackColor = false;
            this.btnCreditNumberAndPasport.Click += new System.EventHandler(this.BtnCreditNumberAndPasportClick);
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
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // btnBolCard
            // 
            this.btnBolCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBolCard.BackColor = System.Drawing.Color.Transparent;
            this.btnBolCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnBolCard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBolCard.Location = new System.Drawing.Point(12, 240);
            this.btnBolCard.Name = "btnBolCard";
            this.btnBolCard.Size = new System.Drawing.Size(996, 105);
            this.btnBolCard.TabIndex = 13;
            this.btnBolCard.Text = "Болькард";
            this.btnBolCard.UseVisualStyleBackColor = false;
            this.btnBolCard.Click += new System.EventHandler(this.BtnBolCardClick);
            // 
            // btnClientNumber
            // 
            this.btnClientNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClientNumber.BackColor = System.Drawing.Color.Transparent;
            this.btnClientNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnClientNumber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClientNumber.Location = new System.Drawing.Point(12, 18);
            this.btnClientNumber.Name = "btnClientNumber";
            this.btnClientNumber.Size = new System.Drawing.Size(996, 105);
            this.btnClientNumber.TabIndex = 14;
            this.btnClientNumber.Text = "По клиентскому коду";
            this.btnClientNumber.UseVisualStyleBackColor = false;
            this.btnClientNumber.Click += new System.EventHandler(this.BtnClientNumberClick);
            // 
            // FormCreditTypeSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.Controls.Add(this.btnClientNumber);
            this.Controls.Add(this.btnBolCard);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCreditNumberAndPasport);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormCreditTypeSelect";
            this.Text = "FormCreditTypeSelect";
            this.Load += new System.EventHandler(this.FormCreditTypeSelectLoad);
            this.Controls.SetChildIndex(this.btnCreditNumberAndPasport, 0);
            this.Controls.SetChildIndex(this.btnBack, 0);
            this.Controls.SetChildIndex(this.btnBolCard, 0);
            this.Controls.SetChildIndex(this.btnClientNumber, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreditNumberAndPasport;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnBolCard;
        private System.Windows.Forms.Button btnClientNumber;
    }
}