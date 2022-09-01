using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_AppGap.Seiten
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UberUns : Page
    {
        public UberUns()
        {
            this.InitializeComponent();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            header_txt.Text = loader.GetString("Header_uberuns");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            textvorlagen_txt.Text = loader.GetString("Header_textvorlagen");

            AllgemeinText.Text = loader.GetString("überuns_allgemein_text");
            allgemein_header.Header = loader.GetString("header_allgemein");
            einsenden_txt.Text = loader.GetString("ruckmeldung_einsenden");
            team_txt.Header = loader.GetString("team_header");
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
            else if (textvorlagen.IsSelected)
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
            string parrameter = e.Parameter.ToString();
            if (parrameter == "spenden")
                Pivot_container.SelectedIndex = 2;
            }
            catch { return; }

        }

        private void HamburgerHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void spenden_099_btn_Click(object sender, RoutedEventArgs e)
        {
            PurchaseProduct99();
        }

        private void spenden_199_btn_Click(object sender, RoutedEventArgs e)
        {
            PurchaseProduct199();
        }

        private void spenden_499_btn_Click(object sender, RoutedEventArgs e)
        {
            PurchaseProduct499();
        }

        public static async Task<ProductPurchaseStatus> PurchaseProduct99()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            PurchaseResults results = null;

            results = await CurrentApp.RequestProductPurchaseAsync("ProjectAppGapBeta099");

            if (results.Status == ProductPurchaseStatus.Succeeded)
            {
                MessageDialog md = new MessageDialog(loader.GetString("spenden_sucsess"));
                await md.ShowAsync();
            }
            else
            {
                MessageDialog md = new MessageDialog(loader.GetString("spenden_fail"), loader.GetString("kontakt_versendet_head_error"));
                await md.ShowAsync();
            }
            return results.Status;

        }

        public static async Task<ProductPurchaseStatus> PurchaseProduct199()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            PurchaseResults results = null;

            results = await CurrentApp.RequestProductPurchaseAsync("ProjectAppGapBeta199");

            if (results.Status == ProductPurchaseStatus.Succeeded)
            {
                MessageDialog md = new MessageDialog(loader.GetString("spenden_sucsess"));
                await md.ShowAsync();
            }
            else
            {
                MessageDialog md = new MessageDialog(loader.GetString("spenden_fail"), loader.GetString("kontakt_versendet_head_error"));
                await md.ShowAsync();
            }
            return results.Status;
        }

        public static async Task<ProductPurchaseStatus> PurchaseProduct499()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            PurchaseResults results = null;

            results = await CurrentApp.RequestProductPurchaseAsync("ProjectAppGapBeta499");

            if (results.Status == ProductPurchaseStatus.Succeeded)
            {
                MessageDialog md = new MessageDialog(loader.GetString("spenden_sucsess"));
                await md.ShowAsync();
            }
            else
            {
                MessageDialog md = new MessageDialog(loader.GetString("spenden_fail"), loader.GetString("kontakt_versendet_head_error"));
                await md.ShowAsync();
            }
            return results.Status;

        }

        private void patreon_btn_Click(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.patreon.com/CSTRSK"));
        }



        //END
    }
}
