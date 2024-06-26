﻿using System;
using CashInTerminal.BaseForms;
using Containers.Enums;

namespace CashInTerminal
{
    public partial class FormCreditTypeSelect : FormMdiChild
    {
        public FormCreditTypeSelect()
        {
            InitializeComponent();
        }        

        private void BtnClientNumberClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.CreditPaymentByClientCode;
            ChangeView(typeof(FormCreditByClientCode));
        }

        private void BtnCreditNumberAndPasportClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.CreditPaymentByPassportAndAccount;
            ChangeView(typeof(FormCreditByPassport1));
        }

        private void BtnBolCardClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.CreditPaymentBolcard;
            ChangeView(typeof(FormCreditByBolcard));
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }

        private void FormCreditTypeSelectLoad(object sender, EventArgs e)
        {
            HomeButton = false;
        }
    }
}
