﻿namespace CashInTerminal
{
    partial class FormEncashment
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
            this.lblEncashmentTotal = new System.Windows.Forms.Label();
            this.btnEncashmentFinish = new System.Windows.Forms.Button();
            this.btnEncashmentPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblEncashmentTotal
            // 
            this.lblEncashmentTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblEncashmentTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblEncashmentTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEncashmentTotal.Location = new System.Drawing.Point(105, 93);
            this.lblEncashmentTotal.Name = "lblEncashmentTotal";
            this.lblEncashmentTotal.Size = new System.Drawing.Size(713, 24);
            this.lblEncashmentTotal.TabIndex = 11;
            this.lblEncashmentTotal.Text = "Режим Инкассации";
            this.lblEncashmentTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEncashmentFinish
            // 
            this.btnEncashmentFinish.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEncashmentFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnEncashmentFinish.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEncashmentFinish.Location = new System.Drawing.Point(330, 428);
            this.btnEncashmentFinish.Name = "btnEncashmentFinish";
            this.btnEncashmentFinish.Size = new System.Drawing.Size(260, 50);
            this.btnEncashmentFinish.TabIndex = 10;
            this.btnEncashmentFinish.Text = "Завершить";
            this.btnEncashmentFinish.UseVisualStyleBackColor = true;
            // 
            // btnEncashmentPrint
            // 
            this.btnEncashmentPrint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEncashmentPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnEncashmentPrint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEncashmentPrint.Location = new System.Drawing.Point(330, 156);
            this.btnEncashmentPrint.Name = "btnEncashmentPrint";
            this.btnEncashmentPrint.Size = new System.Drawing.Size(260, 50);
            this.btnEncashmentPrint.TabIndex = 9;
            this.btnEncashmentPrint.Text = "Печать чека";
            this.btnEncashmentPrint.UseVisualStyleBackColor = true;
            // 
            // FormEncashment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 490);
            this.Controls.Add(this.lblEncashmentTotal);
            this.Controls.Add(this.btnEncashmentFinish);
            this.Controls.Add(this.btnEncashmentPrint);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormEncashment";
            this.Text = "FormEncashment";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEncashmentTotal;
        private System.Windows.Forms.Button btnEncashmentFinish;
        private System.Windows.Forms.Button btnEncashmentPrint;

    }
}