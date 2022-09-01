using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
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
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            hide_status();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header_einstellungen");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            textvorlagen_txt.Text = loader.GetString("Header_textvorlagen");
            einsenden_txt.Text = loader.GetString("ruckmeldung_einsenden");

            Allgemein.Header = loader.GetString("setting_option");
            Designswitch.Header = loader.GetString("toogle_button_desing");
            Designswitch.OnContent = loader.GetString("Designswitch_on");
            Designswitch.OffContent = loader.GetString("Designswitch_off");

            Info.Header = loader.GetString("header_info");
            AppName.Text = loader.GetString("Header");

            //DATENSCHUTZ TXT
            Verantwortlicher_Betreiber_txt1.Text = loader.GetString("verantwortlicher_header_f");
            Verantwortlicher_Betreiber_txt2.Text = loader.GetString("verantwortlicher_header_s");

            Verantwortlicher_Betreiber_txt.Text = loader.GetString("﻿Verantwortlicher_Betreiber");

            Allgemeiner_Datenschutz_txt.Text = loader.GetString("Allgemeiner_Datenschutz");
            Allgemeiner_Datenschutz_inhalt_txt.Text = loader.GetString("Allgemeiner_Datenschutz_txt");

            Allgemeiner_Datenschutz_txt.Text = loader.GetString("Allgemeiner_Datenschutz");
            Allgemeiner_Datenschutz_txt.Text = loader.GetString("Allgemeiner_Datenschutz");

            twitter_txt.Text = loader.GetString("Nutzung_von_Twitter");
            twitter_inhalt_txt.Text = loader.GetString("Nutzung_von_Twitter_txt");

            adduplex_txt.Text = loader.GetString("adduplex_header");
            adduplex_inhalt_txt.Text = loader.GetString("adduplex_txt");

            Kontaktformular_txt.Text = loader.GetString("Kontaktformular");
            Kontaktformular_inhalt_txt.Text = loader.GetString("Kontaktformular_txt");

            Allgemeiner_Datenschutz_txt.Text = loader.GetString("Allgemeiner_Datenschutz");
            Allgemeiner_Datenschutz_txt.Text = loader.GetString("Allgemeiner_Datenschutz");

            auskunft_txt.Text = loader.GetString("Auskunft");
            auskunft_inhalt_txt.Text = loader.GetString("Auskunft_txt");

            urheber_txt.Text = loader.GetString("Urheberrecht_app");
            urheber_inhalt_txt.Text = loader.GetString("Urheberrecht_app_txt");

            urheber_wb_txt.Text = loader.GetString("Urheberrecht_web");
            urheber_wb_inhalt_txt.Text = loader.GetString("Urheberrecht_web_txt");

            haft_txt.Text = loader.GetString("Haftungsausschluss");
            haft_inhalt_txt.Text = loader.GetString("Haftungsausschluss_txt");

            faq_txt.Text = loader.GetString("FAQ");

            mail_txt.Text = loader.GetString("Mail_Unternehmen");
            mail_inhalt_txt.Text = loader.GetString("Mail_Unternehmen_txt");

            ruck_txt.Text = loader.GetString("Ruckmeldung");
            ruck_inhalt_txt.Text = loader.GetString("Ruckmeldung_txt");

            var AppV = Package.Current.Id.Version;
            Versionnummer.Text = $"Version {AppV.Major}.{AppV.Minor}.{AppV.Build}.{AppV.Revision}";

            CheckAndUpdateTheme();
        }

        private void CheckAndUpdateTheme()
        {
            Designswitch.IsOn = Convert.ToBoolean(AppSettings.Setting.Values["ThemeSwitchPos"]);

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try
            {
                switch (AppSettings.Setting.Values["Theme"].ToString())
                {
                    case "Light":
                        RequestedTheme = ElementTheme.Light;
                        Designswitch.Header = loader.GetString("system_design_dunkel");
                        return;

                    case "Dark":
                        RequestedTheme = ElementTheme.Dark;
                        Designswitch.Header = loader.GetString("system_design_dunkel");

                        return;
                }
            }

            catch
            {
                AppSettings.Setting.Values["Theme"] = "Light";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string parrameter = e.Parameter.ToString();
            if (parrameter == "info")
                pivot_settings.SelectedIndex = 1;
            bool isHardwareButtonsAPIPresent = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
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
            else if (einsenden.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung_Einsenden));
            }
            else if (ruckmeldungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung));
            }
            else if (textvorlagen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Textvorlagen));
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

        private void Designswitch_Toggled(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            Designswitch.OnContent = loader.GetString("Designswitch_on");
            Designswitch.OffContent = loader.GetString("Designswitch_off");
            switch (Designswitch.IsOn)
            {
                case false:
                    AppSettings.Setting.Values["Theme"] = "Light";
                    AppSettings.Setting.Values["ThemeSwitchPos"] = Designswitch.IsOn;
                    Designswitch.Header = loader.GetString("system_design_dunkel");
                    CheckAndUpdateTheme();
                    return;

                case true:
                    AppSettings.Setting.Values["Theme"] = "Dark";
                    AppSettings.Setting.Values["ThemeSwitchPos"] = Designswitch.IsOn;
                    Designswitch.Header = loader.GetString("system_design_dunkel");
                    CheckAndUpdateTheme();
                    return;
            }
        }

        //END
    }
}
