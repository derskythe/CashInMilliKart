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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLanguage));
            this.btnEnglish = new System.Windows.Forms.Button();
            this.btnRussian = new System.Windows.Forms.Button();
            this.btnAzeri = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEnglish
            // 
            resources.ApplyResources(this.btnEnglish, "btnEnglish");
            this.btnEnglish.BackColor = System.Drawing.Color.Transparent;
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.UseVisualStyleBackColor = false;
            this.btnEnglish.Click += new System.EventHandler(this.BtnEnglishClick);
            // 
            // btnRussian
            // 
            resources.ApplyResources(this.btnRussian, "btnRussian");
            this.btnRussian.BackColor = System.Drawing.Color.Transparent;
            this.btnRussian.Name = "btnRussian";
            this.btnRussian.UseVisualStyleBackColor = false;
            this.btnRussian.Click += new System.EventHandler(this.BtnRussianClick);
            // 
            // btnAzeri
            // 
            resources.ApplyResources(this.btnAzeri, "btnAzeri");
            this.btnAzeri.BackColor = System.Drawing.Color.Transparent;
            this.btnAzeri.Name = "btnAzeri";
            this.btnAzeri.UseVisualStyleBackColor = false;
            this.btnAzeri.Click += new System.EventHandler(this.BtnAzeriClick);
            // 
            // FormLanguage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnEnglish);
            this.Controls.Add(this.btnRussian);
            this.Controls.Add(this.btnAzeri);
            this.Name = "FormLanguage";
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