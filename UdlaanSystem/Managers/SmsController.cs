using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic;
using UdlaanSystem.DataAccess;

namespace UdlaanSystem.Managers
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
            if (Settings1.Default.SmsService == true)
            {
                Random rnd = new Random();
                int code = rnd.Next(100000, 1000000);
                string msg = "Koden er '" + code + "'.";
                DALSms.Instance.SendSms(phoneNumber, msg);

                for (int i = 0; i < 3; i++)
                {
                    int TreeWayBool = CheckSmsCode(code);
                    if (TreeWayBool == 0)
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
                    else if (TreeWayBool == 1)
                    {
                        return true;
                    }
                    else if (TreeWayBool == 2)
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        //åbner en dialog hvor Ubuy skal intaste den kode personen har fået tilsendt på sms og tjekker at koderne er ens.
        //Dette gøres for at sikre at vi har personens rigtige tlfnummer.
        public int CheckSmsCode(int code)
        {
            UISmsInput UISmsInput = new UISmsInput();
            UISmsInput.ShowDialog();
            int inputCode = UISmsInput.inputCode;

            if (inputCode == 79131379)
            {
                return 2;
            }
            else if (inputCode == code)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void GenerateLendReceipt(UserObject userObject, List<LendObject> lendObjects)
        {
            string location = "";
            if (Settings1.Default.LocationNæstved == true)
            {
                location = "Næstved";
            }
            else if (Settings1.Default.LocationRingsted == true)
            {
                location = "Ringsted";
            }
            else if (Settings1.Default.LocationRoskilde == true)
            {
                location = "Roskilde";
            }
            else if (Settings1.Default.LocationVordingborg == true)
            {
                location = "Vordingborg";
            }

            if (Settings1.Default.SmsService == true)
            {
                string itemsMsg = "";
                DateTime returnDate = new DateTime();

                foreach (LendObject lendObject in lendObjects)
                {
                    returnDate = lendObject.ReturnDate;
                    itemsMsg += lendObject.ItemObject.Type + " " + lendObject.ItemObject.Manufacturer + " " + lendObject.ItemObject.Model + " " + lendObject.ItemObject.Id + Environment.NewLine;
                }

                string msg = "Hej " + userObject.ZbcName + Environment.NewLine + Environment.NewLine + "Du har den " + DateTime.Now + " lånt følgende udstyr:" + Environment.NewLine + Environment.NewLine + itemsMsg + Environment.NewLine + "Dette udstyr skal være afleveret den " + returnDate + " senest!" + Environment.NewLine + Environment.NewLine + "Med Venlig Hilsen" + Environment.NewLine + "-Ubuy " + location; //Ubuy Rinsted kan ændres så man vælger location i config filen
                DALSms.Instance.SendSms(userObject.PhoneNumber, msg);
            }
        }

        public void GenerateReturnReceipt(UserObject userObject, List<LendObject> lendObjects)
        {
            string location = "";
            if (Settings1.Default.LocationNæstved == true)
            {
                location = "Næstved";
            }
            else if (Settings1.Default.LocationRingsted == true)
            {
                location = "Ringsted";
            }
            else if (Settings1.Default.LocationRoskilde == true)
            {
                location = "Roskilde";
            }
            else if (Settings1.Default.LocationVordingborg == true)
            {
                location = "Vordingborg";
            }

            if (Settings1.Default.SmsService == true)
            {
                string itemsMsg = "";
                foreach (LendObject lendObject in lendObjects)
                {
                    itemsMsg += lendObject.ItemObject.Type + " " + lendObject.ItemObject.Manufacturer + " " + lendObject.ItemObject.Model + " " + lendObject.ItemObject.Id + Environment.NewLine;
                }
                string msg = "Hej " + userObject.ZbcName + Environment.NewLine + Environment.NewLine + "Du har den " + DateTime.Now + " afleveret følgende udstyr:" + Environment.NewLine + Environment.NewLine + itemsMsg + Environment.NewLine +  "Med Venlig Hilsen" + Environment.NewLine + "-Ubuy " + location; //Ubuy Rinsted kan ændres så man vælger location i config filen
                DALSms.Instance.SendSms(userObject.PhoneNumber, msg);
            }
        }
    }
}
