namespace CashInTerminal
{
    partial class FormPaySuccess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaySuccess));
            this.lblSuccessTotalAmount = new System.Windows.Forms.Label();
            this.btnSuccessNext = new System.Windows.Forms.Button();
            this.lblSuccessWelcome = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.lblComission = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSuccessTotalAmount
            // 
            resources.ApplyResources(this.lblSuccessTotalAmount, "lblSuccessTotalAmount");
            this.lblSuccessTotalAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessTotalAmount.Name = "lblSuccessTotalAmount";
            // 
            // btnSuccessNext
            // 
            resources.ApplyResources(this.btnSuccessNext, "btnSuccessNext");
            this.btnSuccessNext.BackColor = System.Drawing.Color.Transparent;
            this.btnSuccessNext.Name = "btnSuccessNext";
            this.btnSuccessNext.UseVisualStyleBackColor = false;
            this.btnSuccessNext.Click += new System.EventHandler(this.BtnSuccessNextClick);
            // 
            // lblSuccessWelcome
            // 
            resources.ApplyResources(this.lblSuccessWelcome, "lblSuccessWelcome");
            this.lblSuccessWelcome.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessWelcome.Name = "lblSuccessWelcome";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocumentPrintPage);
            // 
            // lblComission
            // 
            resources.ApplyResources(this.lblComission, "lblComission");
            this.lblComission.BackColor = System.Drawing.Color.Transparent;
            this.lblComission.Name = "lblComission";
            // 
            // FormPaySuccess
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblComission);
            this.Controls.Add(this.lblSuccessTotalAmount);
            this.Controls.Add(this.btnSuccessNext);
            this.Controls.Add(this.lblSuccessWelcome);
            this.Controls.Add(this.label3);
            this.Name = "FormPaySuccess";
            this.Load += new System.EventHandler(this.FormPaySuccessLoad);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblSuccessWelcome, 0);
            this.Controls.SetChildIndex(this.btnSuccessNext, 0);
            this.Controls.SetChildIndex(this.lblSuccessTotalAmount, 0);
            this.Controls.SetChildIndex(this.lblComission, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSuccessTotalAmount;
        private System.Windows.Forms.Button btnSuccessNext;
        private System.Windows.Forms.Label lblSuccessWelcome;
        private System.Windows.Forms.Label label3;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.Label lblComission;

    }
}