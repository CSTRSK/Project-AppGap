using Project_AppGap.Helper;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Phone.Devices.Notification;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_AppGap.Seiten.Texte
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Appfehlt : Page
    {
        //Random Texte
        public Random a = new Random();
        int MyNumber;
        //+1 Cap für randomiser benötigt
        int cap = 4;

        public Appfehlt()
        {
            this.InitializeComponent();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header_textvorlagen");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            textvorlage_txt.Text = loader.GetString("Header_textvorlagen");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            kontakt_txt.Text = loader.GetString("Header_kontakt");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            //randomizer?

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            Object value = roamingSettings.Values["randomizer_project_appgap"];

            if (value == null)
            {
            }
            else
            {
                // Access data in value
                string random_on_off = roamingSettings.Values["randomizer_project_appgap"].ToString();
                if (random_on_off == "0")
                {
                    randomTextVorlage();
                }
            }
           
            piv_1.Header = loader.GetString("v1");
            piv_2.Header = loader.GetString("v2");
            piv_3.Header = loader.GetString("v3");

            string n_s1 = loader.GetString("appfehlt_vorlage_1");
            n_s1 = n_s1.Replace("@%", "");
            V1.Text = n_s1;

            string n_s2 = loader.GetString("appfehlt_vorlage_2");
            n_s2 = n_s2.Replace("@%", "");
            V2.Text = n_s2;

            string n_s3 = loader.GetString("appfehlt_vorlage_3");
            n_s3 = n_s3.Replace("@%", "");
            V3.Text = n_s3;

            CopyButton.Label = loader.GetString("copy_btn");
            MailButton.Label = loader.GetString("send_btn");
            hide_status();
            CheckAndUpdateTheme();
        }

        private void CheckAndUpdateTheme()
        {

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try
            {
                switch (AppSettings.Setting.Values["Theme"].ToString())
                {
                    case "Light":
                        RequestedTheme = ElementTheme.Light;
                        return;

                    case "Dark":
                        RequestedTheme = ElementTheme.Dark;
                        return;
                }
            }

            catch
            {
                AppSettings.Setting.Values["Theme"] = "Light";
            }
        }

        private void randomTextVorlage()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var nums = Enumerable.Range(1, cap).ToArray();
            var rnd = new Random();

            MyNumber = a.Next(1, cap);
            //wenn unerwartet cap dann setze auf 2
            if (MyNumber == 4)
                MyNumber = 2;

            if (MyNumber == 1)
                Piv.SelectedIndex = 0;            
            else if (MyNumber == 2)
                Piv.SelectedIndex = 1;
            else if (MyNumber == 3)
                Piv.SelectedIndex = 2;

        }

        private async void hide_status()
        {
            try
            {
                var api = "Windows.Phone.UI.Input.HardwareButtons";
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
                {
                    MySplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                    MySplitView.IsPaneOpen = false;
                }
            }
            catch { return; }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startseite.IsSelected)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (textvorlage.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Textvorlagen));
            }
            else if (ruckmeldungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung));
            }
            else if (uber_uns.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.UberUns));
            }
            else if (einstellungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.SettingPage), "m");
            }
            else if (kontakt.IsSelected)
            {
                this.Frame.Navigate(typeof(Kontakt.Main_Kontakt));
            }

        }

        private void HamburgerHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void MailButton_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            if (Piv.SelectedIndex == 0)
            {
                string n_s = loader.GetString("appfehlt_vorlage_1");
                n_s = n_s.Replace("@%", "%0d%0A%0d%0A");
                var success = await Launcher.LaunchUriAsync(new Uri("mailto:?Subject=" + "Ihre App fehlt" + "&Body=" + n_s, UriKind.Absolute));
           }

            if (Piv.SelectedIndex == 1)
            {
                string n_s = loader.GetString("appfehlt_vorlage_2");
                n_s = n_s.Replace("@%", "%0d%0A%0d%0A");
                var success = await Launcher.LaunchUriAsync(new Uri("mailto:?Subject=" + "Ihre App fehlt" + "&Body=" + n_s, UriKind.Absolute));
            }

            if (Piv.SelectedIndex == 2)
            {
                string n_s = loader.GetString("appfehlt_vorlage_3");
                n_s = n_s.Replace("@%", "%0d%0A%0d%0A");
                var success = await Launcher.LaunchUriAsync(new Uri("mailto:?Subject=" + "Ihre App fehlt" + "&Body="  +n_s, UriKind.Absolute));
            }


        }


        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            Object value = roamingSettings.Values["vibrate_project_appgap"];

            if (value == null)
            {
                // No data 
                Windows.Phone.Devices.Notification.VibrationDevice v = VibrationDevice.GetDefault();
                v.Vibrate(TimeSpan.FromMilliseconds(50));
            }
            else
            {
                // Access data in value
                string vibrate_on_off = roamingSettings.Values["vibrate_project_appgap"].ToString();
                if (vibrate_on_off == "0")
                {
                    Windows.Phone.Devices.Notification.VibrationDevice v = VibrationDevice.GetDefault();
                    v.Vibrate(TimeSpan.FromMilliseconds(50));
                }
            }



            var dataPackage = new DataPackage();

            if (Piv.SelectedIndex == 0)
            {
                string Clipboardtext = V1.Text;
                dataPackage.SetText(Clipboardtext);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }

            if (Piv.SelectedIndex == 1)
            {
                string Clipboardtext = V2.Text;
                dataPackage.SetText(Clipboardtext);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }

            if (Piv.SelectedIndex == 2)
            {
                string Clipboardtext = V3.Text;
                dataPackage.SetText(Clipboardtext);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }
        }


    } //END
}



