using System;
using System.Text;
using System.Windows.Forms;
using CashInTerminalWpf;
using CashInTerminalWpf.Enums;

namespace CCNETTestConsole
{
    public partial class Form1 : Form
    {
        readonly CCNETDevice _Device = new CCNETDevice();
        private delegate void WriteLogDelegate(object item);
        private delegate void WriteLogDelegate2(object item);

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnOpenClick(object sender, EventArgs e)
        {
            try
            {
                _Device.Open(Convert.ToInt32(txtPort.Text), CCNETPortSpeed.S9600);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void BtnInitClick(object sender, EventArgs e)
        {
            try
            {
                _Device.Init();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void BtnStartPoolClick(object sender, EventArgs e)
        {
            try
            {
                _Device.DeviceState.Currency = "AZN";
                _Device.CurrentCurrency = "AZN";
                _Device.StartPool = true;
                _Device.Poll();                
                _Device.EnableAll();                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void BtnStopPoolClick(object sender, EventArgs e)
        {
            try
            {
                _Device.Poll();
                _Device.StartPool = false;
                _Device.Disable();                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void BtnPoolClick(object sender, EventArgs e)
        {
            try
            {
                _Device.Poll();                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void Form1Load(object sender, EventArgs e)
        {
            _Device.ReadCommand += DeviceOnReadCommand;
            _Device.StartCompleted += DeviceOnStartCompleted;
            _Device.BillStacked += DeviceOnBillStacked;
            _Device.BillRejected += DeviceOnBillRejected;
            _Device.GetBills += DeviceOnGetBills;
        }

        private void DeviceOnGetBills(CCNETDeviceState ccnetDeviceState)
        {
            var str = new StringBuilder();
            str.Append("Currency list: \n");
            foreach (var item in ccnetDeviceState.AvailableCurrencies)
            {
                str.Append(item).Append("\n");
            }
            WriteLog(str.ToString());
        }

        private void DeviceOnBillRejected(CCNETDeviceState ccnetDeviceState)
        {
            WriteLog(ccnetDeviceState.DeviceStateDescription);
        }

        private void DeviceOnBillStacked(CCNETDeviceState ccnetDeviceState)
        {
            WriteLog2(ccnetDeviceState.Nominal + " " + ccnetDeviceState.Currency);
        }

        private void DeviceOnStartCompleted(CCNETDeviceState ccnetDeviceState)
        {
            var str = new StringBuilder();
            str.Append("Currency list: \n");
            foreach (var item in ccnetDeviceState.AvailableCurrencies)
            {
                str.Append(item).Append("\n");
            }
            WriteLog(str.ToString());
        }

        private void DeviceOnReadCommand(CCNETDeviceState ccnetDeviceState)
        {
            WriteLog(ccnetDeviceState.DeviceStateDescription);
        }

        private void WriteLog(Object value)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new WriteLogDelegate(WriteLog), value);
            }
            else
            {
                richTextBox1.Text += (String) value + "\n";
            }
        }

        private void WriteLog2(Object value)
        {
            if (richTextBox2.InvokeRequired)
            {
                richTextBox2.Invoke(new WriteLogDelegate2(WriteLog2), value);
            }
            else
            {
                richTextBox2.Text += (String)value + "\n";
            }
        }

        private void Form1FormClosed(object sender, FormClosedEventArgs e)
        {
            _Device.Close();
            _Device.Dispose();
        }

        private void BtnResetClick(object sender, EventArgs e)
        {
            try
            {
                _Device.Reset();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void BtnGetBillsClick(object sender, EventArgs e)
        {
            try
            {
                _Device.GetBillTable();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}
