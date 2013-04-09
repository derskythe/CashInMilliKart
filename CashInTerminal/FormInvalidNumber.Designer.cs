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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInvalidNumber));
            this.label10 = new System.Windows.Forms.Label();
            this.btbBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Name = "label10";
            // 
            // btbBack
            // 
            resources.ApplyResources(this.btbBack, "btbBack");
            this.btbBack.BackColor = System.Drawing.Color.Transparent;
            this.btbBack.Name = "btbBack";
            this.btbBack.UseVisualStyleBackColor = false;
            this.btbBack.Click += new System.EventHandler(this.BtbBackClick);
            // 
            // FormInvalidNumber
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btbBack);
            this.Name = "FormInvalidNumber";
            this.Controls.SetChildIndex(this.btbBack, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btbBack;

    }
}