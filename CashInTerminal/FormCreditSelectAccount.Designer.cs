namespace CashInTerminal
{
    partial class FormCreditSelectAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditSelectAccount));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnBack = new System.Windows.Forms.Button();
            this.dataGridSelect = new System.Windows.Forms.DataGridView();
            this.tdAccountNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdCurrency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // dataGridSelect
            // 
            resources.ApplyResources(this.dataGridSelect, "dataGridSelect");
            this.dataGridSelect.AllowUserToAddRows = false;
            this.dataGridSelect.AllowUserToDeleteRows = false;
            this.dataGridSelect.AllowUserToResizeColumns = false;
            this.dataGridSelect.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dataGridSelect.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridSelect.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridSelect.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridSelect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.dataGridSelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridSelect.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSelect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tdAccountNumber,
            this.tdType,
            this.tdDate,
            this.tdAmount,
            this.tdCurrency});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridSelect.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridSelect.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.dataGridSelect.MultiSelect = false;
            this.dataGridSelect.Name = "dataGridSelect";
            this.dataGridSelect.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridSelect.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridSelect.RowHeadersVisible = false;
            this.dataGridSelect.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridSelect.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridSelect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // tdAccountNumber
            // 
            this.tdAccountNumber.FillWeight = 140F;
            resources.ApplyResources(this.tdAccountNumber, "tdAccountNumber");
            this.tdAccountNumber.Name = "tdAccountNumber";
            this.tdAccountNumber.ReadOnly = true;
            // 
            // tdType
            // 
            resources.ApplyResources(this.tdType, "tdType");
            this.tdType.Name = "tdType";
            this.tdType.ReadOnly = true;
            // 
            // tdDate
            // 
            resources.ApplyResources(this.tdDate, "tdDate");
            this.tdDate.Name = "tdDate";
            this.tdDate.ReadOnly = true;
            // 
            // tdAmount
            // 
            this.tdAmount.FillWeight = 70F;
            resources.ApplyResources(this.tdAmount, "tdAmount");
            this.tdAmount.Name = "tdAmount";
            this.tdAmount.ReadOnly = true;
            // 
            // tdCurrency
            // 
            this.tdCurrency.FillWeight = 70F;
            resources.ApplyResources(this.tdCurrency, "tdCurrency");
            this.tdCurrency.Name = "tdCurrency";
            this.tdCurrency.ReadOnly = true;
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // FormCreditSelectAccount
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.dataGridSelect);
            this.Name = "FormCreditSelectAccount";
            this.Load += new System.EventHandler(this.FormCreditSelectAccountLoad);
            this.Controls.SetChildIndex(this.dataGridSelect, 0);
            this.Controls.SetChildIndex(this.btnBack, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridSelect;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdAccountNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdType;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdCurrency;
    }
}