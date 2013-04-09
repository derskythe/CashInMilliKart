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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditTypeSelect));
            this.btnCreditNumberAndPasport = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnBolCard = new System.Windows.Forms.Button();
            this.btnClientNumber = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreditNumberAndPasport
            // 
            resources.ApplyResources(this.btnCreditNumberAndPasport, "btnCreditNumberAndPasport");
            this.btnCreditNumberAndPasport.BackColor = System.Drawing.Color.Transparent;
            this.btnCreditNumberAndPasport.Name = "btnCreditNumberAndPasport";
            this.btnCreditNumberAndPasport.UseVisualStyleBackColor = false;
            this.btnCreditNumberAndPasport.Click += new System.EventHandler(this.BtnCreditNumberAndPasportClick);
            // 
            // btnBack
            // 
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // btnBolCard
            // 
            resources.ApplyResources(this.btnBolCard, "btnBolCard");
            this.btnBolCard.BackColor = System.Drawing.Color.Transparent;
            this.btnBolCard.Name = "btnBolCard";
            this.btnBolCard.UseVisualStyleBackColor = false;
            this.btnBolCard.Click += new System.EventHandler(this.BtnBolCardClick);
            // 
            // btnClientNumber
            // 
            resources.ApplyResources(this.btnClientNumber, "btnClientNumber");
            this.btnClientNumber.BackColor = System.Drawing.Color.Transparent;
            this.btnClientNumber.Name = "btnClientNumber";
            this.btnClientNumber.UseVisualStyleBackColor = false;
            this.btnClientNumber.Click += new System.EventHandler(this.BtnClientNumberClick);
            // 
            // FormCreditTypeSelect
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClientNumber);
            this.Controls.Add(this.btnBolCard);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCreditNumberAndPasport);
            this.Name = "FormCreditTypeSelect";
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