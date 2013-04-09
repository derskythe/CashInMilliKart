namespace CashInTerminal
{
    partial class FormEncashment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEncashment));
            this.lblEncashmentTotal = new System.Windows.Forms.Label();
            this.btnEncashmentFinish = new System.Windows.Forms.Button();
            this.btnEncashmentPrint = new System.Windows.Forms.Button();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.SuspendLayout();
            // 
            // lblEncashmentTotal
            // 
            resources.ApplyResources(this.lblEncashmentTotal, "lblEncashmentTotal");
            this.lblEncashmentTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblEncashmentTotal.Name = "lblEncashmentTotal";
            // 
            // btnEncashmentFinish
            // 
            resources.ApplyResources(this.btnEncashmentFinish, "btnEncashmentFinish");
            this.btnEncashmentFinish.BackColor = System.Drawing.Color.Transparent;
            this.btnEncashmentFinish.Name = "btnEncashmentFinish";
            this.btnEncashmentFinish.UseVisualStyleBackColor = false;
            this.btnEncashmentFinish.Click += new System.EventHandler(this.BtnEncashmentFinishClick);
            // 
            // btnEncashmentPrint
            // 
            resources.ApplyResources(this.btnEncashmentPrint, "btnEncashmentPrint");
            this.btnEncashmentPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnEncashmentPrint.Name = "btnEncashmentPrint";
            this.btnEncashmentPrint.UseVisualStyleBackColor = false;
            this.btnEncashmentPrint.Click += new System.EventHandler(this.BtnEncashmentPrintClick);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocumentPrintPage);
            // 
            // FormEncashment
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblEncashmentTotal);
            this.Controls.Add(this.btnEncashmentFinish);
            this.Controls.Add(this.btnEncashmentPrint);
            this.Name = "FormEncashment";
            this.Load += new System.EventHandler(this.FormEncashmentLoad);
            this.Controls.SetChildIndex(this.btnEncashmentPrint, 0);
            this.Controls.SetChildIndex(this.btnEncashmentFinish, 0);
            this.Controls.SetChildIndex(this.lblEncashmentTotal, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEncashmentTotal;
        private System.Windows.Forms.Button btnEncashmentFinish;
        private System.Windows.Forms.Button btnEncashmentPrint;
        private System.Drawing.Printing.PrintDocument printDocument;

    }
}