using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class MigrationController
    { //Denne del af koden er UDELUKKENDE til mmigration så udstyret bliver afleveret i den gamle DB!!!
        public MigrationController()
        {
        }
        private static MigrationController instance;

        public static MigrationController Instance
        {
            get
            {
                if (instance == null)
                { instance = new MigrationController(); }
                return instance;
            }
        }

        public bool CheckIfItemIsLendedInOldDB(string itemMifare)
        {
            return DALMigration.Instance.CheckIfItemIsLendedInOldDB(itemMifare);
        }

        public bool ReturnItemInOldDB(string itemMifareToReturnInOldDB)
        {
            return DALMigration.Instance.ReturnItemInOldDB(itemMifareToReturnInOldDB);
        }
    }
}
