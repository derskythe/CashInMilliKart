﻿using System;
using System.Windows.Forms;
using NLog;

namespace CashInTerminal
{
    public partial class FormMdiChild : Form
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        protected FormMdiMain FormMain
        {
            get
            {
                if (ParentForm != null)
                {
                    return ((FormMdiMain)ParentForm);
                }

                return null;
            }   
        }

        public FormMdiChild()
        {
            InitializeComponent();
        }

        protected void FormLanguage_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        protected void ChangeView(Form form)
        {
            if (ParentForm != null)
            {
                FormMain.OpenForm(form);
                Close();
            }
        }
    }
}