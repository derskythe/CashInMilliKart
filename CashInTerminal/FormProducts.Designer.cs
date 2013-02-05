namespace CashInTerminal
{
    partial class FormProducts
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
            this.btnPayDebit = new System.Windows.Forms.Button();
            this.btnPayCredit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPayDebit
            // 
            this.btnPayDebit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPayDebit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnPayDebit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPayDebit.Location = new System.Drawing.Point(337, 282);
            this.btnPayDebit.Name = "btnPayDebit";
            this.btnPayDebit.Size = new System.Drawing.Size(260, 50);
            this.btnPayDebit.TabIndex = 5;
            this.btnPayDebit.Text = "Пополнение счета";
            this.btnPayDebit.UseVisualStyleBackColor = true;
            this.btnPayDebit.Click += new System.EventHandler(this.BtnPayDebitClick);
            // 
            // btnPayCredit
            // 
            this.btnPayCredit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPayCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnPayCredit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPayCredit.Location = new System.Drawing.Point(337, 191);
            this.btnPayCredit.Name = "btnPayCredit";
            this.btnPayCredit.Size = new System.Drawing.Size(260, 50);
            this.btnPayCredit.TabIndex = 4;
            this.btnPayCredit.Text = "Оплата кредита";
            this.btnPayCredit.UseVisualStyleBackColor = true;
            this.btnPayCredit.Click += new System.EventHandler(this.BtnPayCreditClick);
            // 
            // FormProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 523);
            this.Controls.Add(this.btnPayDebit);
            this.Controls.Add(this.btnPayCredit);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormProducts";
            this.Text = "FormProducts";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPayDebit;
        private System.Windows.Forms.Button btnPayCredit;

    }
}