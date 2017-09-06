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
        public bool GenerateVerificationSms(int phoneNumber)
        {
            Random rnd = new Random();
            int code = rnd.Next(100000, 1000000);
            string msg = "Koden er '" + code + "'.";
            DALSms.Instance.SendSms(phoneNumber, msg);

            for (int i = 0; i < 3; i++)
            {
                if (CheckSmsCode(code) == false)
                {
                    if (2 - i == 0)
                    {
                        MessageBox.Show("Koden er forkert.  Godkendelse misllykkedes");
                    }
                    else
                    {
                        MessageBox.Show("Koden er forkert. " + Convert.ToString(2 - i) + " forsøg tilbage!");
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
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
                return false;
            }
        }

        /*public void GenerateLendReceipt(int phoneNumber, LendedObject lendedObject)
        {
            string msg = "Hej " + [zbcnavn] + Environment.NewLine + Environment.NewLine + "Du har den " + [udlaantdato] + " klokken " + [udlaanttid] + " lånt følgende udstyr:" + Environment.NewLine + Environment.NewLine + [listeafudstyr] + Environment.NewLine + Environment.NewLine + "Dette udstyr skal være afleveret den " + [afleveringsdato] + " klokken " + [afleveringstid] + "senest!" + Environment.NewLine + Environment.NewLine + "Med Venlig Hilsen" + Environment.NewLine + "-Ubuy Ringsted"; //Ubuy Rinsted kan ændres så man vælger location i config filen
            DALSms.Instance.SendSms(phoneNumber, msg);
        }

        public void GenerateReturnReceipt(int phoneNumber)
        {
            string msg = "Hej " + [zbcnavn] + Environment.NewLine + Environment.NewLine + "Du har den " + [afleveretdato] + " klokken " + [afleverettid] + " afleveret følgende udstyr:" + Environment.NewLine + Environment.NewLine + [listeafudstyr] + Environment.NewLine + Environment.NewLine +  "Med Venlig Hilsen" + Environment.NewLine + "-Ubuy Ringsted"; //Ubuy Rinsted kan ændres så man vælger location i config filen
            DALSms.Instance.SendSms(phoneNumber, msg);
        }*/
    }
}
