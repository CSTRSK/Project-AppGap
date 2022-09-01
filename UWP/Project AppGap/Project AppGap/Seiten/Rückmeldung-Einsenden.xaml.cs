using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
    public sealed partial class Rückmeldung_Einsenden : Page
    {
        public Rückmeldung_Einsenden()
        {
            this.InitializeComponent();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            textvorlagen_txt.Text = loader.GetString("Header_textvorlagen");
            einsenden_txt.Text = loader.GetString("ruckmeldung_einsenden");
            MyProgressRing.Visibility = Visibility.Collapsed;
            MyProgressRing.IsActive = false;
            loadURL();
            CheckAndUpdateTheme();
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

        private async void loadURL()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

            if (internet == true)
            {
                MyProgressRing.IsActive = true;
                MyProgressRing.Visibility = Visibility.Visible;
                   Uri targetUri = new Uri("https://cstrsk.de/project-appgap/ruckmeldung/");
                web_view.Navigate(targetUri);
            }
            else
            {
                MessageDialog dialog = new MessageDialog(loader.GetString("keineInternetVerbindung"), loader.GetString("keinInternetHeader"));
                await dialog.ShowAsync();
            }
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
                this.Frame.Navigate(typeof(Seiten.Rückmeldung));
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
        }

        private void HamburgerHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
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

        private void web_view_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            MyProgressRing.Visibility = Visibility.Collapsed;
            MyProgressRing.IsActive = false;
        }

        //END
    }
}
