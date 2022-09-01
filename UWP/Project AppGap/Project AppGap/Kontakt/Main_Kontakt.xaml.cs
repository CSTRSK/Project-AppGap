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

namespace Project_AppGap.Kontakt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main_Kontakt : Page
    {
        public Main_Kontakt()
        {
            this.InitializeComponent();
            hide_status();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header_kontakt");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            showbox_txt.Text = loader.GetString("app_showbox_title");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            kontakt_txt.Text = loader.GetString("Header_kontakt");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            votebox_txt.Text = loader.GetString("VoteBox_titel");

            Anfrage_an_entwickler.Content = loader.GetString("wunsch_app_btn");
            Ruckmeldung_einsenden.Content = loader.GetString("ruckmeldung_einsenden");
            feedback_entwickler.Content = loader.GetString("send_feedback");
            allgemein_entwickler.Content = loader.GetString("allgemeine_questions");
            CheckAndUpdateTheme();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                string parrameter = e.Parameter.ToString();
                if (parrameter == "wa")
                {
                    kontaktFrame.Navigate(typeof(Kontakt.AppAnfrage));
                }
                if (parrameter == "rs")
                {
                    kontaktFrame.Navigate(typeof(Kontakt.ruckmeldung));
                }
            }
            catch { return; }
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
            else if (showboxs.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.showbox));
            }
            else if (ruckmeldungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung));
            }
            else if (voteboxs.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.VoteBox));
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

        private void Ruckmeldung_einsenden_Click(object sender, RoutedEventArgs e)
        {
            kontaktFrame.Navigate(typeof(Kontakt.ruckmeldung));
        }

        private void Anfrage_an_entwickler_Click(object sender, RoutedEventArgs e)
        {
            kontaktFrame.Navigate(typeof(Kontakt.AppAnfrage));
        }

        private void feedback_entwickler_Click(object sender, RoutedEventArgs e)
        {
            kontaktFrame.Navigate(typeof(Kontakt.Feedback));
        }

        private void allgemein_entwickler_Click(object sender, RoutedEventArgs e)
        {
            kontaktFrame.Navigate(typeof(Kontakt.Allgemeine_Frage));
        }


        //END
    }
}
