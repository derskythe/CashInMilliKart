﻿namespace CashInTerminal
{
    partial class FormPaySuccess
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
            this.lblSuccessTotalAmount = new System.Windows.Forms.Label();
            this.btnSuccessNext = new System.Windows.Forms.Button();
            this.lblSuccessWelcome = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.SuspendLayout();
            // 
            // lblSuccessTotalAmount
            // 
            this.lblSuccessTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSuccessTotalAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.lblSuccessTotalAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSuccessTotalAmount.Location = new System.Drawing.Point(154, 280);
            this.lblSuccessTotalAmount.Name = "lblSuccessTotalAmount";
            this.lblSuccessTotalAmount.Size = new System.Drawing.Size(713, 62);
            this.lblSuccessTotalAmount.TabIndex = 12;
            this.lblSuccessTotalAmount.Text = "AZN";
            this.lblSuccessTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSuccessNext
            // 
            this.btnSuccessNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSuccessNext.BackColor = System.Drawing.Color.Transparent;
            this.btnSuccessNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnSuccessNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSuccessNext.Location = new System.Drawing.Point(350, 658);
            this.btnSuccessNext.Name = "btnSuccessNext";
            this.btnSuccessNext.Size = new System.Drawing.Size(321, 76);
            this.btnSuccessNext.TabIndex = 13;
            this.btnSuccessNext.Text = "Дальше";
            this.btnSuccessNext.UseVisualStyleBackColor = false;
            this.btnSuccessNext.Click += new System.EventHandler(this.BtnSuccessNextClick);
            // 
            // lblSuccessWelcome
            // 
            this.lblSuccessWelcome.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSuccessWelcome.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.lblSuccessWelcome.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSuccessWelcome.Location = new System.Drawing.Point(154, 370);
            this.lblSuccessWelcome.Name = "lblSuccessWelcome";
            this.lblSuccessWelcome.Size = new System.Drawing.Size(713, 62);
            this.lblSuccessWelcome.TabIndex = 10;
            this.lblSuccessWelcome.Text = "Благодарим!";
            this.lblSuccessWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(154, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(713, 62);
            this.label3.TabIndex = 11;
            this.label3.Text = "Получено";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocumentPrintPage);
            // 
            // FormPaySuccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.Controls.Add(this.lblSuccessTotalAmount);
            this.Controls.Add(this.btnSuccessNext);
            this.Controls.Add(this.lblSuccessWelcome);
            this.Controls.Add(this.label3);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormPaySuccess";
            this.Text = "FormPaySuccess";
            this.Load += new System.EventHandler(this.FormPaySuccessLoad);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblSuccessWelcome, 0);
            this.Controls.SetChildIndex(this.btnSuccessNext, 0);
            this.Controls.SetChildIndex(this.lblSuccessTotalAmount, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSuccessTotalAmount;
        private System.Windows.Forms.Button btnSuccessNext;
        private System.Windows.Forms.Label lblSuccessWelcome;
        private System.Windows.Forms.Label label3;
        private System.Drawing.Printing.PrintDocument printDocument;

    }
}