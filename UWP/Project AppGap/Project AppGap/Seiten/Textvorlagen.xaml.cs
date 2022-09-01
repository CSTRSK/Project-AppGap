using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_AppGap.Seiten
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Textvorlagen : Page
    {
        public Textvorlagen()
        {
            this.InitializeComponent();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header_textvorlagen");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            textvorlage_txt.Text = loader.GetString("Header_textvorlagen");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            einsenden_txt.Text = loader.GetString("ruckmeldung_einsenden");

            button.Content = loader.GetString("spiel_veraltet");
            button1.Content = loader.GetString("spiel_fehlt");
            button2.Content = loader.GetString("app_veraltet");
            button3.Content = loader.GetString("app_fehlt");


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
            else if (einsenden.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung_Einsenden));
            }
            else if (uber_uns.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.UberUns));
            }
            else if (einstellungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.SettingPage), "m");
            }

        }

        private void HamburgerHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button).Tag;

            int t = Convert.ToInt16(tag);
            switch (t)
            {
                case 1:
                    this.Frame.Navigate(typeof(Seiten.Texte.Appfehlt));
                    break;
                case 2:
                    this.Frame.Navigate(typeof(Seiten.Texte.Appveraltet));
                    break;
                case 3:
                    this.Frame.Navigate(typeof(Seiten.Texte.Spielfehlt));
                    break;
                case 4:
                    this.Frame.Navigate(typeof(Seiten.Texte.Spielveraltet));
                    break;

                default:
                    break;
            }
        }
        //END
    }
}
