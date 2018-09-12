using System;
using System.Collections.Generic;
using System.Linq;
using PCSC;
using PCSC.Iso7816;
using PCSC.Monitoring;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem.Managers
{
    class CardReader
    {
        string reader = string.Empty;
        SCardContext context = new SCardContext();
        public static string uID = " ";

        private bool readerClosed = true;

        public string tes = " ";

        public SCardMonitor sCardMonitor;

        private static MainWindow parrent;

        public CardReader(MainWindow parrentWindow)
        {
            parrent = parrentWindow;
            if (IsClosed() && AnyReadersAvailable())
            {
                ReaderOne();
                FindReader();
                sCardMonitor = OpenCardReader();
            }
        }

        public void ReaderOne()
        {
            FindReader();
            context.GetReaderStatus(reader);
        }

        public bool AnyReadersAvailable()
        {
            context.Establish(SCardScope.System);
            string[] readerNames = context.GetReaders();
            return readerNames.Length > 0;
        }

        public void FindReader()
        {
            //Context is the object to find the nfc reader in the system.
            //Context can use both the scardScope.system for local and globel for remote conncetions.
            context.Establish(SCardScope.System);
            string[] readerNames = context.GetReaders();

            //This is used to find a reader and use that for reading cards.
            //rigth now it just use the last one it finds.
            foreach (string item in readerNames)
            {
                reader = item;
                //Console.WriteLine(item);
            }
        }

        public SCardMonitor OpenCardReader()
        {
            ContextFactory contextFac = new ContextFactory();
            SCardMonitor scardmon = new SCardMonitor(contextFac, SCardScope.System);
            //If monitor exception occurs, close the monitor and set the reader to closed.
            //scardmon.MonitorException += (sender, exception) =>
            //{
            //    Console.WriteLine("Closing Monitor");
            //    scardmon.Cancel();
            //    scardmon.Dispose();
            //    readerClosed = true;
            //};
            scardmon.CardInserted += (sender, EventArgs) => ReadCard(context, reader, ref tes);
            scardmon.Start(reader);
            readerClosed = false;

            //scardmon.Cancel();
            return scardmon;
        }

        public bool IsClosed()
        {
            return readerClosed;
        }

        public static void ReadCard(SCardContext context, string reader, ref string tes)
        {
            IsoReader isoReader = new IsoReader(context, reader, SCardShareMode.Shared, SCardProtocol.Any);

            var apdu = new CommandApdu(IsoCase.Case2Short, isoReader.ActiveProtocol)
            {
                CLA = 0xff, // Class.
                INS = 0xCA, // what instrution you are using.
                P1 = 0x00, // Parameter 1.
                P2 = 0x00, // Parameter 2.
                Le = 0x4 // Length of the return value.                    
            };

            Response test = isoReader.Transmit(apdu);
            uID = BitConverter.ToString(test.GetData());
            string[] a = uID.Split('-');
            Array.Reverse(a);
            //parrent.CardInserted(string.Join("", a));
        }
    }
}
