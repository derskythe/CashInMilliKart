using CashInTerminal.Controls;

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
            this.btnPayDebit = new CashInTerminal.Controls.MainButton();
            this.btnPayCredit = new CashInTerminal.Controls.MainButton();
            this.SuspendLayout();
            // 
            // btnPayDebit
            // 
            this.btnPayDebit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayDebit.BackColor = System.Drawing.Color.Transparent;
            this.btnPayDebit.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayDebit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPayDebit.Location = new System.Drawing.Point(12, 139);
            this.btnPayDebit.Name = "btnPayDebit";
            this.btnPayDebit.Size = new System.Drawing.Size(996, 105);
            this.btnPayDebit.TabIndex = 5;
            this.btnPayDebit.Text = "Пополнение счета";
            this.btnPayDebit.UseVisualStyleBackColor = false;
            this.btnPayDebit.Click += new System.EventHandler(this.BtnPayDebitClick);
            // 
            // btnPayCredit
            // 
            this.btnPayCredit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayCredit.BackColor = System.Drawing.Color.Transparent;
            this.btnPayCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayCredit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPayCredit.Location = new System.Drawing.Point(12, 25);
            this.btnPayCredit.Name = "btnPayCredit";
            this.btnPayCredit.Size = new System.Drawing.Size(996, 105);
            this.btnPayCredit.TabIndex = 4;
            this.btnPayCredit.Text = "Оплата кредита";
            this.btnPayCredit.UseVisualStyleBackColor = false;
            this.btnPayCredit.Click += new System.EventHandler(this.BtnPayCreditClick);
            // 
            // FormProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.Controls.Add(this.btnPayDebit);
            this.Controls.Add(this.btnPayCredit);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormProducts";
            this.Text = "FormProducts";
            this.Load += new System.EventHandler(this.FormProductsLoad);
            this.Controls.SetChildIndex(this.btnPayCredit, 0);
            this.Controls.SetChildIndex(this.btnPayDebit, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private MainButton btnPayDebit;
        private MainButton btnPayCredit;

    }
}