﻿namespace CashInTerminal.BaseForms
{
    partial class FormMdiChild
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
            this.lblApplicationVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblApplicationVersion
            // 
            this.lblApplicationVersion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblApplicationVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationVersion.Location = new System.Drawing.Point(12, 9);
            this.lblApplicationVersion.Name = "lblApplicationVersion";
            this.lblApplicationVersion.Size = new System.Drawing.Size(178, 17);
            this.lblApplicationVersion.TabIndex = 7;
            // 
            // FormMdiChild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.ControlBox = false;
            this.Controls.Add(this.lblApplicationVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMdiChild";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormChildLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMdiChildPaint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblApplicationVersion;
    }
}