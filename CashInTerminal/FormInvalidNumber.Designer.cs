namespace CashInTerminal
{
    partial class FormInvalidNumber
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
            this.label10 = new System.Windows.Forms.Label();
            this.btbBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(105, 134);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(713, 24);
            this.label10.TabIndex = 10;
            this.label10.Text = "Не правильный номер. ";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btbBack
            // 
            this.btbBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btbBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btbBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btbBack.Location = new System.Drawing.Point(331, 428);
            this.btbBack.Name = "btbBack";
            this.btbBack.Size = new System.Drawing.Size(260, 50);
            this.btbBack.TabIndex = 9;
            this.btbBack.Text = "Назад";
            this.btbBack.UseVisualStyleBackColor = true;
            this.btbBack.Click += new System.EventHandler(this.btbBack_Click);
            // 
            // FormInvalidNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 490);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btbBack);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormInvalidNumber";
            this.Text = "FormInvalidNumber";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btbBack;

    }
}