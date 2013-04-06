using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CashInTerminal.BaseForms;

namespace CashInTerminal
{
    public partial class FormTestMode : FormMdiChild
    {
        public FormTestMode()
        {
            InitializeComponent();
        }

        private void FormTestModeLoad(object sender, EventArgs e)
        {
            HomeButton = false;
        }
    }
}
