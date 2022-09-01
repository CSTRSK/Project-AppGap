using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_AppGap.Seiten.Texte
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary> 
    public sealed partial class Spielveraltet : Page
    {
        public Spielveraltet()
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


            string n_s1 = loader.GetString("spielveraltet_vorlage_1");
            n_s1 = n_s1.Replace("@%", "");
            V.Text = n_s1;

            piv_1.Header = loader.GetString("v1");
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
                string Clipboardtext = V.Text;
                dataPackage.SetText(Clipboardtext);
                try
                {
                    Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);

                }

                catch
                {
                    return;
                }
            }
        }

        private async void MailButton_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            if (Piv.SelectedIndex == 0)
            {
                string n_s = loader.GetString("spielveraltet_vorlage_1");
                n_s = n_s.Replace("@%", "%0d%0A%0d%0A");
                var success = await Launcher.LaunchUriAsync(new Uri("mailto:?Subject=" + "Ihre Spiel ist Veraltet" + "&Body=" + n_s, UriKind.Absolute));
            }
        }

        //END
    }
}
