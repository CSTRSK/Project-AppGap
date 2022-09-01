using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Project_AppGap
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int count_click;
        Uri wb_uri = new Uri("http://plfa.bplaced.com/pag/spenden.html");

        public MainPage()
        {
            this.InitializeComponent();
            hide_status();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            //   kontakt_txt.Text = loader.GetString("Header_kontakt");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            textvorlagen_txt.Text = loader.GetString("Header_textvorlagen");
            textvorlagen_button.Content = loader.GetString("Header_textvorlagen");
            einsenden_txt.Text = loader.GetString("ruckmeldung_einsenden");

            GreetingTextBlock.Text = loader.GetString("GreetingTextBlock");
            ruckmeldung_button.Content = loader.GetString("startseite_rückmeldung_btn");
            wunschApp_button.Content = loader.GetString("wunsch_app_btn");
            ruckmeldung_senden_button.Content = loader.GetString("ruckmeldung_einsenden");
            //   Spenden_button.Content = loader.GetString("unterstutze_projekt");
            count_click = 0;
            tileUpdateManager();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var frame = this.Frame;
            if (frame != null)
            {
                var cacheSize = ((frame)).CacheSize;
                ((frame)).CacheSize = 0;
                ((frame)).CacheSize = cacheSize;
            }

            CheckAndUpdateTheme();
            IPropertySet roamingProperties = ApplicationData.Current.RoamingSettings.Values;
            if (roamingProperties.ContainsKey("nutzungsbedingungen"))
            {
                // The normal case
            }
            else
            {

                var dialog = new MessageDialog(loader.GetString("nutzungsbedingungen_txt"));
                dialog.Title = loader.GetString("nutzungsbedingungen_head");
                dialog.Commands.Add(new UICommand { Label = loader.GetString("nutzungsbedingungen_erfahren"), Id = 0 });
                dialog.Commands.Add(new UICommand { Label = loader.GetString("nutzungsbedingungen_verabschieden"), Id = 1 });
                var res = await dialog.ShowAsync();

                if ((int)res.Id == 0)
                {
                    this.Frame.Navigate(typeof(Seiten.SettingPage), "info");

                }
                if ((int)res.Id == 1)
                {

                };
                roamingProperties["nutzungsbedingungen"] = bool.TrueString;
            }

            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

            if (internet == true)
            {
                InterstitialAd.ShowInterstitialAd();
            }
            else
            {
            }
        }

        private void CheckAndUpdateTheme()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Color.FromArgb(255, 0, 120, 215);
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Color.FromArgb(255, 0, 120, 215);
                    titleBar.ForegroundColor = Colors.White;
                }
            }

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
                    StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    await statusBar.HideAsync();
                    MySplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                    MySplitView.IsPaneOpen = false;

                    var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    var width_var = Window.Current.Bounds.Width;
                    int width_int = Convert.ToInt32(width_var);
                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView") && width_int > 1280)
                    {
                        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                        if (titleBar != null)
                        {
                            MySplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                            MySplitView.IsPaneOpen = true;
                            titleBar.ButtonBackgroundColor = Color.FromArgb(255, 0, 120, 215);
                            titleBar.ButtonForegroundColor = Colors.White;
                            titleBar.BackgroundColor = Color.FromArgb(255, 0, 120, 215);
                            titleBar.ForegroundColor = Colors.White;
                        }
                    }
                }
            }
            catch { return; }
        }

        private void HamburgerHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startseite.IsSelected)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (textvorlagen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Textvorlagen));
            }
            else if (ruckmeldungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung), count_click);
            }
            else if (uber_uns.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.UberUns));
            }
            else if (einsenden.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Rückmeldung_Einsenden));
            }
            else if (einstellungen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.SettingPage), "m");
            }
            count_click++;
        }

        private void ShButt_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            DataTransferManager.ShowShareUI();
        }


        private void MainPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var req = args.Request;
            req.Data.Properties.Title = "Ich Empfehle";
            req.Data.SetText("Ich empfehle dir die App Project AppGap von CSTRSK: https://www.microsoft.com/store/apps/9nblggh5f9x0");
        }

        private void ruckmeldung_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Seiten.Rückmeldung), count_click);
            count_click++;
        }

        private void wunschApp_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Kontakt.Main_Kontakt), "wa");
        }
        private void ruckmeldung_senden_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Seiten.Rückmeldung_Einsenden));
        }

        private async void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            try
            {
                var api = "Windows.Phone.UI.Input.HardwareButtons";
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
                {
                    StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    await statusBar.HideAsync();
                    MySplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                    MySplitView.IsPaneOpen = false;

                    var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    var width_var = Window.Current.Bounds.Width;
                    int width_int = Convert.ToInt32(width_var);
                    if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api) && ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView") && width_int > 1280)
                    {
                        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                        if (titleBar != null)
                        {
                            MySplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                            MySplitView.IsPaneOpen = true;
                            titleBar.ButtonBackgroundColor = Color.FromArgb(255, 0, 120, 215);
                            titleBar.ButtonForegroundColor = Colors.White;
                            titleBar.BackgroundColor = Color.FromArgb(255, 0, 120, 215);
                            titleBar.ForegroundColor = Colors.White;
                        }
                    }
                }
            }
            catch { return; }
        }

        private void AppShowBox_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Seiten.showbox));
        }

        private void Spenden_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Seiten.UberUns), "spenden");
        }

        private void vote_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Seiten.VoteBox));
        }

        private void tileUpdateManager()
        {
            try
            {
                var tileContent = new Uri("http://plfa.bplaced.net/pag/tile.xml");
                var requestedInterval = PeriodicUpdateRecurrence.SixHours;
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.StartPeriodicUpdate(tileContent, requestedInterval);
            }
            catch { return; }
        }

        private void Textvorlagen_button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Seiten.Textvorlagen));
        }

        //END
    }
}