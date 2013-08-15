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
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtTrasnsactions = new System.Windows.Forms.RichTextBox();
            this.txtWarning = new System.Windows.Forms.RichTextBox();
            this.txtWarning2 = new System.Windows.Forms.RichTextBox();
            this.cmbTransactions = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
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
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(237, 18);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(145, 20);
            this.txtTotal.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.txtWarning2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtWarning, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTrasnsactions, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 42);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1122, 445);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // txtTrasnsactions
            // 
            this.txtTrasnsactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTrasnsactions.Location = new System.Drawing.Point(3, 3);
            this.txtTrasnsactions.Name = "txtTrasnsactions";
            this.txtTrasnsactions.Size = new System.Drawing.Size(367, 439);
            this.txtTrasnsactions.TabIndex = 2;
            this.txtTrasnsactions.Text = "";
            // 
            // txtWarning
            // 
            this.txtWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWarning.Location = new System.Drawing.Point(376, 3);
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.Size = new System.Drawing.Size(367, 439);
            this.txtWarning.TabIndex = 3;
            this.txtWarning.Text = "";
            // 
            // txtWarning2
            // 
            this.txtWarning2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWarning2.Location = new System.Drawing.Point(749, 3);
            this.txtWarning2.Name = "txtWarning2";
            this.txtWarning2.Size = new System.Drawing.Size(370, 439);
            this.txtWarning2.TabIndex = 4;
            this.txtWarning2.Text = "";
            // 
            // cmbTransactions
            // 
            this.cmbTransactions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransactions.FormattingEnabled = true;
            this.cmbTransactions.Items.AddRange(new object[] {
            "Only AZN transactions",
            "Only USD transactions"});
            this.cmbTransactions.Location = new System.Drawing.Point(761, 18);
            this.cmbTransactions.Name = "cmbTransactions";
            this.cmbTransactions.Size = new System.Drawing.Size(255, 21);
            this.cmbTransactions.TabIndex = 5;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 499);
            this.Controls.Add(this.cmbTransactions);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.btnOpen);
            this.Name = "FormMain";
            this.Text = "Log Analizer";
            this.Load += new System.EventHandler(this.FormMainLoad);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox txtWarning2;
        private System.Windows.Forms.RichTextBox txtWarning;
        private System.Windows.Forms.RichTextBox txtTrasnsactions;
        private System.Windows.Forms.ComboBox cmbTransactions;
    }
}

