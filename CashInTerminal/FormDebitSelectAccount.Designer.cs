namespace CashInTerminal
{
    partial class FormDebitSelectAccount
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.dataGridSelect = new System.Windows.Forms.DataGridView();
            this.tdAccountNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tdCurrency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBack.Location = new System.Drawing.Point(12, 534);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(260, 50);
            this.btnBack.TabIndex = 15;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNext.Location = new System.Drawing.Point(772, 534);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(260, 50);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "Дальше";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
            // 
            // dataGridSelect
            // 
            this.dataGridSelect.AllowUserToAddRows = false;
            this.dataGridSelect.AllowUserToDeleteRows = false;
            this.dataGridSelect.AllowUserToResizeColumns = false;
            this.dataGridSelect.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.AliceBlue;
            this.dataGridSelect.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridSelect.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridSelect.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridSelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridSelect.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSelect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tdAccountNumber,
            this.tdType,
            this.tdDate,
            this.tdCurrency});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridSelect.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridSelect.Location = new System.Drawing.Point(12, 12);
            this.dataGridSelect.MultiSelect = false;
            this.dataGridSelect.Name = "dataGridSelect";
            this.dataGridSelect.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridSelect.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridSelect.RowHeadersVisible = false;
            this.dataGridSelect.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridSelect.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridSelect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridSelect.Size = new System.Drawing.Size(1020, 447);
            this.dataGridSelect.TabIndex = 13;
            this.dataGridSelect.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridSelectCellClick);
            // 
            // tdAccountNumber
            // 
            this.tdAccountNumber.HeaderText = "Номер счета";
            this.tdAccountNumber.Name = "tdAccountNumber";
            this.tdAccountNumber.ReadOnly = true;
            // 
            // tdType
            // 
            this.tdType.HeaderText = "Тип счета";
            this.tdType.Name = "tdType";
            this.tdType.ReadOnly = true;
            // 
            // tdDate
            // 
            this.tdDate.HeaderText = "Дата открытия";
            this.tdDate.Name = "tdDate";
            this.tdDate.ReadOnly = true;
            // 
            // tdCurrency
            // 
            this.tdCurrency.HeaderText = "Валюта";
            this.tdCurrency.Name = "tdCurrency";
            this.tdCurrency.ReadOnly = true;
            // 
            // FormDebitSelectAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 596);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.dataGridSelect);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormDebitSelectAccount";
            this.Text = "FormDebitSelectAccount";
            this.Load += new System.EventHandler(this.FormDebitSelectAccountLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridView dataGridSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdAccountNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdType;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn tdCurrency;
    }
}