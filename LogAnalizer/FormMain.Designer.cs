namespace LogAnalizer
{
    partial class FormMain
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtTrasnsactions = new System.Windows.Forms.RichTextBox();
            this.txtWarning = new System.Windows.Forms.RichTextBox();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtWarning2 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(13, 13);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open file";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpenClick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.txt";
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Text Files|*.txt|Log files|*.log|All files|*.*";
            // 
            // txtTrasnsactions
            // 
            this.txtTrasnsactions.Location = new System.Drawing.Point(12, 42);
            this.txtTrasnsactions.Name = "txtTrasnsactions";
            this.txtTrasnsactions.Size = new System.Drawing.Size(482, 480);
            this.txtTrasnsactions.TabIndex = 1;
            this.txtTrasnsactions.Text = "";
            // 
            // txtWarning
            // 
            this.txtWarning.Location = new System.Drawing.Point(500, 42);
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.Size = new System.Drawing.Size(314, 480);
            this.txtWarning.TabIndex = 1;
            this.txtWarning.Text = "";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(348, 16);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(145, 20);
            this.txtTotal.TabIndex = 2;
            // 
            // txtWarning2
            // 
            this.txtWarning2.Location = new System.Drawing.Point(820, 42);
            this.txtWarning2.Name = "txtWarning2";
            this.txtWarning2.Size = new System.Drawing.Size(319, 480);
            this.txtWarning2.TabIndex = 3;
            this.txtWarning2.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 534);
            this.Controls.Add(this.txtWarning2);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.txtWarning);
            this.Controls.Add(this.txtTrasnsactions);
            this.Controls.Add(this.btnOpen);
            this.Name = "FormMain";
            this.Text = "Log Analizer";
            this.Load += new System.EventHandler(this.FormMainLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RichTextBox txtTrasnsactions;
        private System.Windows.Forms.RichTextBox txtWarning;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.RichTextBox txtWarning2;
    }
}

