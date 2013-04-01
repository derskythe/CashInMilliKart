using System;

namespace CashInTerminal
{
    partial class FormLanguage
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
            this.btnEnglish = new System.Windows.Forms.Button();
            this.btnRussian = new System.Windows.Forms.Button();
            this.btnAzeri = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEnglish
            // 
            this.btnEnglish.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEnglish.BackColor = System.Drawing.Color.Transparent;
            this.btnEnglish.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnEnglish.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEnglish.Location = new System.Drawing.Point(204, 456);
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.Size = new System.Drawing.Size(613, 79);
            this.btnEnglish.TabIndex = 5;
            this.btnEnglish.Text = "English";
            this.btnEnglish.UseVisualStyleBackColor = false;
            this.btnEnglish.Click += new System.EventHandler(this.BtnEnglishClick);
            // 
            // btnRussian
            // 
            this.btnRussian.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRussian.BackColor = System.Drawing.Color.Transparent;
            this.btnRussian.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnRussian.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRussian.Location = new System.Drawing.Point(204, 337);
            this.btnRussian.Name = "btnRussian";
            this.btnRussian.Size = new System.Drawing.Size(613, 79);
            this.btnRussian.TabIndex = 4;
            this.btnRussian.Text = "Russian";
            this.btnRussian.UseVisualStyleBackColor = false;
            this.btnRussian.Click += new System.EventHandler(this.BtnRussianClick);
            // 
            // btnAzeri
            // 
            this.btnAzeri.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAzeri.BackColor = System.Drawing.Color.Transparent;
            this.btnAzeri.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold);
            this.btnAzeri.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAzeri.Location = new System.Drawing.Point(204, 212);
            this.btnAzeri.Name = "btnAzeri";
            this.btnAzeri.Size = new System.Drawing.Size(613, 79);
            this.btnAzeri.TabIndex = 3;
            this.btnAzeri.Text = "Azeri";
            this.btnAzeri.UseVisualStyleBackColor = false;
            this.btnAzeri.Click += new System.EventHandler(this.BtnAzeriClick);
            // 
            // FormLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 746);
            this.Controls.Add(this.btnEnglish);
            this.Controls.Add(this.btnRussian);
            this.Controls.Add(this.btnAzeri);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormLanguage";
            this.Text = "FormLanguage";
            this.Load += new System.EventHandler(this.FormLanguage_Load);
            this.Controls.SetChildIndex(this.btnAzeri, 0);
            this.Controls.SetChildIndex(this.btnRussian, 0);
            this.Controls.SetChildIndex(this.btnEnglish, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEnglish;
        private System.Windows.Forms.Button btnRussian;
        private System.Windows.Forms.Button btnAzeri;        
    }
}