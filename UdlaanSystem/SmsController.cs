using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic;

namespace UdlaanSystem
{
    class SmsController
    {
        public SmsController()
        {
        }
        private static SmsController instance;

        public static SmsController Instance
        {
            get
            {
                if (instance == null)
                { instance = new SmsController(); }
                return instance;
            }
        }

        //Generere en 6 cifret kode og sender den til det nummer den får med ned.
        public bool GenerateSmsCode(int phoneNumber)
        {
            Random rnd = new Random();
            int code = rnd.Next(100000, 1000000);
            DALSms.Instance.SendVerificationSms(phoneNumber, code);
            return CheckSmsCode(code);
        }

        //åbner en dialog hvor Ubuy skal intaste den kode personen har fået tilsendt på sms og tjekker at koderne er ens.
        //Dette gøres for at sikre at vi har personens rigtige tlfnummer.
        public bool CheckSmsCode(int code)
        {
            UISmsInput UISmsInput = new UISmsInput();
            UISmsInput.ShowDialog();
            int inputCode = UISmsInput.inputCode;

            if (inputCode == code)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Koden er forkert");
                return false;
            }
        }
    }
}
