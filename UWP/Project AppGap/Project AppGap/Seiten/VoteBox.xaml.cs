using Newtonsoft.Json;
using Project_AppGap.Helper;
using Project_AppGap.VoteBoxModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_AppGap.Seiten
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VoteBox : Page
    {
        public ObservableCollection<votebox_jsonClass> MemoryItems { get; set; }

        string url_date_newest = String.Format("http://plfa.bplaced.net/pag/voteJson.json");

        IList<string> items = new List<string>();
        bool noData;

        public VoteBox()
        {
            this.InitializeComponent();
            CheckAndUpdateTheme();
            hide_status();
            MemoryItems = new ObservableCollection<votebox_jsonClass>();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            header_txt.Text = loader.GetString("VoteBox_titel");
            startseite_txt.Text = loader.GetString("Startseite_txt");

            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            kontakt_txt.Text = loader.GetString("Header_kontakt");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            showbox_txt.Text = loader.GetString("app_showbox_title");
            votebox_txt.Text = loader.GetString("VoteBox_titel");
            votebox_disclamer.Text = loader.GetString("VoteBox_disclamer");
            noData = false;
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

        private void hide_status()
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            try
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                MasterListView.Visibility = Visibility.Visible;
                MyProgressRing.Visibility = Visibility.Visible;
                BackHeaderButton.Visibility = Visibility.Collapsed;
                HamburgerHeaderButton.Visibility = Visibility.Visible;

                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

                if (internet == true)
                {
                    while (MemoryItems.Count < 10)
                    {
                        Task t = PopulateCharactersAsync(MemoryItems, url_date_newest);
                        await t;
                    }


                }
                else
                {
                    MyProgressRing.IsActive = false;
                    MyProgressRing.Visibility = Visibility.Collapsed;
                    //errorInternet
                    MessageDialog dialog = new MessageDialog(loader.GetString("keineInternetVerbindung"), loader.GetString("keinInternetHeader"));
                    await dialog.ShowAsync();
                }
            }
            catch { }

            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
        }

        public async Task PopulateCharactersAsync(ObservableCollection<votebox_jsonClass> MemoryItems, string url)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            try
            {
                // Call out to Marvel
                HttpClient http = new HttpClient();

                var response = await http.GetAsync(url);
                var jsonMessage = await response.Content.ReadAsStringAsync();

                // Response -> string / json -> deserialize
                var collection = JsonConvert.DeserializeObject<ObservableCollection<votebox_jsonClass>>(jsonMessage);


                if (collection.Count != 0)
                {

                    foreach (var item in collection)
                    {
                        var sb = new StringBuilder();

                        sb.Append(item.name);
                        var fileString = sb.ToString();

                        if(fileString.ToString() == "none" && noData == false)
                        {
                            noData = true;
                            MessageDialog dialog = new MessageDialog(loader.GetString("keineDaten_txt"), loader.GetString("keineDaten_header"));
                            await dialog.ShowAsync();
                            this.Frame.Navigate(typeof(MainPage));
                            break;
                        }
                        var sb1 = new StringBuilder();

                        sb1.Append(item.datum);
                        var fileString_img = sb1.ToString();

                        var sb2 = new StringBuilder();

                        sb2.Append(item.link);
                        var fileString_link = sb2.ToString();

                        var sb3 = new StringBuilder();

                        sb3.Append(item.firma);
                        var fileString_response = sb3.ToString();

                        var memoryItem = new votebox_jsonClass
                        {
                            name = fileString,
                            datum = fileString_img,
                            link = fileString_link,
                            firma = fileString_response,
                        };
                        if (MemoryItems.Any(p => p.name == fileString) == false)
                        {
                            items.Add(fileString);
                            MemoryItems.Add(memoryItem);
                            MyProgressRing.IsActive = false;
                            MyProgressRing.Visibility = Visibility.Collapsed;
                        }
                    }
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
            else if (uber_uns.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.UberUns));
            }
            else if (kontakt.IsSelected)
            {
                this.Frame.Navigate(typeof(Kontakt.Main_Kontakt));
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

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedCharacter = (votebox_jsonClass)e.ClickedItem;
            Windows.System.Launcher.LaunchUriAsync(new Uri(selectedCharacter.link));
        }

        private void BackHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        //END
    }
}
