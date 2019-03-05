using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UdlaanSystem.Managers;
using UdlaanSystem.Properties;

namespace UdlaanSystem
{
    public partial class UIConfigPanelDetail : Window
    {
        public UIConfigPanelDetail()
        {
            InitializeComponent();

            LoadSettings();
            
        }

        private bool changesHasBeenMade = false;
        private bool startupFri = true;
        private bool startupMonThur = true;

        private void LoadSettings()
        {
            if (Settings.Default.LocationNæstved)
            {
                LocationCombo.Text = "Næstved";
            }
            else if (Settings.Default.LocationRingsted)
            {
                LocationCombo.Text = "Ringsted";
            }
            else if (Settings.Default.LocationRoskilde)
            {
                LocationCombo.Text = "Roskilde";
            }
            else if (Settings.Default.LocationVordingborg)
            {
                LocationCombo.Text = "Vordingbord";
            }

            if (Settings.Default.SmsService)
            {
                SMSService.Background = Brushes.LightSeaGreen;
                SMSService.Content = "TIL";
            }
            else
            {
                SMSService.Background = Brushes.IndianRed;
                SMSService.Content = "FRA";
            }

            if (Settings.Default.PartSmsService)
            {
                LendSmsService.Background = Brushes.LightSeaGreen;
                LendSmsService.Content = "TIL";
            }
            else
            {
                LendSmsService.Background = Brushes.IndianRed;
                LendSmsService.Content = "FRA";
            }

            TimeReturnFri.Value = Settings.Default.TimeForReturnFriday;
            TimeReturnMonThur.Value = Settings.Default.TimeForReturnMonToThur;
        }

        private void SMSService_Click(Object sender, RoutedEventArgs e)
        {
            if (Settings.Default.SmsService)
            {
                Settings.Default.SmsService = false;
                SMSService.Background = Brushes.IndianRed;
                SMSService.Content = "FRA";
            }
            else
            {
                Settings.Default.SmsService = true;
                SMSService.Background = Brushes.LightSeaGreen;
                SMSService.Content = "TIL";
            }
            changesHasBeenMade = true;
        }

        private void LendSmsService_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.PartSmsService)
            {
                Settings.Default.PartSmsService = false;
                LendSmsService.Background = Brushes.IndianRed;
                LendSmsService.Content = "FRA";
            }
            else
            {
                Settings.Default.PartSmsService = true;
                LendSmsService.Background = Brushes.LightSeaGreen;
                LendSmsService.Content = "TIL";
            }
            changesHasBeenMade = true;
        }

        private void TimeReturnFri_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (startupFri)
            {
                startupFri = false;
            }
            else
            {
                Settings.Default.TimeForReturnFriday = (TimeSpan)TimeReturnFri.Value;
                changesHasBeenMade = true;
            }            
        }

        private void TimeReturnMonThur_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (startupMonThur)
            {
                startupMonThur = false;
            }
            else
            {
                Settings.Default.TimeForReturnMonToThur = (TimeSpan)TimeReturnMonThur.Value;
                changesHasBeenMade = true;
            }
        }

        private void SaveChanges_Click (Object sender, RoutedEventArgs e)
        {
            if (changesHasBeenMade)
            {
                Settings.Default.Save();
                MessageBox.Show("Du har lavet en ændring. Programmet genstarter nu så de nye ændringer træder i kraft");
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else
            {
                this.Close();
            }
        }

        
    }
}
