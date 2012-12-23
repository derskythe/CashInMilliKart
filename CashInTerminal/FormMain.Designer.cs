namespace CashInTerminal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.pnlLanguage = new System.Windows.Forms.Panel();
            this.pnlProducts = new System.Windows.Forms.Panel();
            this.pnlClientCode = new System.Windows.Forms.Panel();
            this.pnlClientCodeInfo = new System.Windows.Forms.Panel();
            this.pnlMoney = new System.Windows.Forms.Panel();
            this.pnlPaySuccess = new System.Windows.Forms.Panel();
            this.pnlTestMode = new System.Windows.Forms.Panel();
            this.pnlEncashment = new System.Windows.Forms.Panel();
            this.btnAzeri = new System.Windows.Forms.Button();
            this.btnRussian = new System.Windows.Forms.Button();
            this.btnEnglish = new System.Windows.Forms.Button();
            this.pnlLanguage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLanguage
            // 
            this.pnlLanguage.Controls.Add(this.btnEnglish);
            this.pnlLanguage.Controls.Add(this.btnRussian);
            this.pnlLanguage.Controls.Add(this.btnAzeri);
            resources.ApplyResources(this.pnlLanguage, "pnlLanguage");
            this.pnlLanguage.Name = "pnlLanguage";
            // 
            // pnlProducts
            // 
            resources.ApplyResources(this.pnlProducts, "pnlProducts");
            this.pnlProducts.Name = "pnlProducts";
            // 
            // pnlClientCode
            // 
            resources.ApplyResources(this.pnlClientCode, "pnlClientCode");
            this.pnlClientCode.Name = "pnlClientCode";
            // 
            // pnlClientCodeInfo
            // 
            resources.ApplyResources(this.pnlClientCodeInfo, "pnlClientCodeInfo");
            this.pnlClientCodeInfo.Name = "pnlClientCodeInfo";
            // 
            // pnlMoney
            // 
            resources.ApplyResources(this.pnlMoney, "pnlMoney");
            this.pnlMoney.Name = "pnlMoney";
            // 
            // pnlPaySuccess
            // 
            resources.ApplyResources(this.pnlPaySuccess, "pnlPaySuccess");
            this.pnlPaySuccess.Name = "pnlPaySuccess";
            // 
            // pnlTestMode
            // 
            resources.ApplyResources(this.pnlTestMode, "pnlTestMode");
            this.pnlTestMode.Name = "pnlTestMode";
            // 
            // pnlEncashment
            // 
            resources.ApplyResources(this.pnlEncashment, "pnlEncashment");
            this.pnlEncashment.Name = "pnlEncashment";
            // 
            // btnAzeri
            // 
            resources.ApplyResources(this.btnAzeri, "btnAzeri");
            this.btnAzeri.Name = "btnAzeri";
            this.btnAzeri.UseVisualStyleBackColor = true;
            this.btnAzeri.Click += new System.EventHandler(this.btnAzeri_Click);
            // 
            // btnRussian
            // 
            resources.ApplyResources(this.btnRussian, "btnRussian");
            this.btnRussian.Name = "btnRussian";
            this.btnRussian.UseVisualStyleBackColor = true;
            this.btnRussian.Click += new System.EventHandler(this.btnRussian_Click);
            // 
            // btnEnglish
            // 
            resources.ApplyResources(this.btnEnglish, "btnEnglish");
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.UseVisualStyleBackColor = true;
            this.btnEnglish.Click += new System.EventHandler(this.btnEnglish_Click);
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.pnlEncashment);
            this.Controls.Add(this.pnlTestMode);
            this.Controls.Add(this.pnlPaySuccess);
            this.Controls.Add(this.pnlMoney);
            this.Controls.Add(this.pnlClientCodeInfo);
            this.Controls.Add(this.pnlClientCode);
            this.Controls.Add(this.pnlProducts);
            this.Controls.Add(this.pnlLanguage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMainLoad);
            this.pnlLanguage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLanguage;
        private System.Windows.Forms.Panel pnlProducts;
        private System.Windows.Forms.Panel pnlClientCode;
        private System.Windows.Forms.Panel pnlClientCodeInfo;
        private System.Windows.Forms.Panel pnlMoney;
        private System.Windows.Forms.Panel pnlPaySuccess;
        private System.Windows.Forms.Panel pnlTestMode;
        private System.Windows.Forms.Panel pnlEncashment;
        private System.Windows.Forms.Button btnEnglish;
        private System.Windows.Forms.Button btnRussian;
        private System.Windows.Forms.Button btnAzeri;
    }
}

