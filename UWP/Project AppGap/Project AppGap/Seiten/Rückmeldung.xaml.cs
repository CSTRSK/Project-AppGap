using Newtonsoft.Json;
using Project_AppGap.Helper;
using Project_AppGap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_AppGap.Seiten
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Rückmeldung : Page
    {
        public ObservableCollection<RootObject> MemoryItems { get; set; }

        string url_date_newest = String.Format("https://cstrsk.de/project-appgap/ruckmeldung/test.json");
        string old_text_before_translate;

        List<string> items = new List<string>();
        int i;
        int selectet_item;

        DispatcherTimer timer;
        int elapsed_time;

        bool searching;
        bool watching;

        bool isLoadingNewsestDB;
        bool noData;

        private readonly string _apiKey = "GITHUB NO KEY";

        public Rückmeldung()
        {
            this.InitializeComponent();
            CheckAndUpdateTheme();
            hide_status();
            MemoryItems = new ObservableCollection<RootObject>();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            header_txt.Text = loader.GetString("Header_Rückmeldung");
            startseite_txt.Text = loader.GetString("Startseite_txt");
            ruckmeldungen_txt.Text = loader.GetString("Header_Rückmeldung");
            einstellungen_txt.Text = loader.GetString("Header_einstellungen");
            uber_uns_txt.Text = loader.GetString("Header_uberuns");
            textvorlagen_txt.Text = loader.GetString("Header_textvorlagen");
            refresh_button.Label = loader.GetString("aktualisieren");
            translate_button.Visibility = Visibility.Collapsed;
            undo_button.Visibility = Visibility.Collapsed;
            translate_button.Label = loader.GetString("translate_btn");
            undo_button.Label = loader.GetString("translate_back_btn");
            einsenden_txt.Text = loader.GetString("ruckmeldung_einsenden");
        }

        private void CheckAndUpdateTheme()
        {
            searching = false;
            watching = false;
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            searching = false;
            watching = false;
            var api = "Windows.Phone.UI.Input.HardwareButtons";
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;

                var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                var height_wb1 = Window.Current.Bounds.Width - 50;
                MyAutoSuggsetBox.Width = height_wb1;
                BackHeaderButton.Visibility = Visibility.Collapsed;
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            e.Handled = true;
            if (watching || searching || !searching || !watching)
            {
                if (searching && !watching)
                {
                    var api = "Windows.Phone.UI.Input.HardwareButtons";
                    if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
                    {
                        MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
                        searching = false;
                        header_txt.Visibility = Visibility.Visible;
                        HamburgerHeaderButton.Visibility = Visibility.Visible;
                        refresh_button.Visibility = Visibility.Visible;
                        search_btn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
                        searching = false;
                        header_txt.Visibility = Visibility.Visible;
                        HamburgerHeaderButton.Visibility = Visibility.Visible;
                        BackHeaderButton.Visibility = Visibility.Collapsed;
                        refresh_button.Visibility = Visibility.Visible;
                        search_btn.Visibility = Visibility.Visible;
                    }
                }
                else if (watching && !searching || fullview.Visibility == Visibility.Visible)
                {
                    MasterListView.Visibility = Visibility.Visible;
                    search_btn.Visibility = Visibility.Visible;
                    HamburgerHeaderButton.Visibility = Visibility.Visible;
                    refresh_button.Visibility = Visibility.Visible;
                    search_btn.Visibility = Visibility.Visible;
                    fullview.Visibility = Visibility.Collapsed;
                    BackHeaderButton.Visibility = Visibility.Collapsed;
                    MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
                    translate_button.Visibility = Visibility.Collapsed;
                    MyAutoSuggsetBox.Text = "";
                    watching = false;
                }
                else if (fullview.Visibility == Visibility.Collapsed && MasterListView.Visibility == Visibility.Visible && !searching && !watching)
                {
                    if (rootFrame != null && rootFrame.CanGoBack)
                    {
                        this.Frame.Navigate(typeof(MainPage));
                        this.Frame.BackStack.Remove(this.Frame.BackStack.Last());
                        searching = false;
                        watching = false;
                    }
                }
            }
            else if (fullview.Visibility == Visibility.Visible)
            {
                MasterListView.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                HamburgerHeaderButton.Visibility = Visibility.Visible;
                refresh_button.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                fullview.Visibility = Visibility.Collapsed;
                BackHeaderButton.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
                translate_button.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Text = "";
                watching = false;
                searching = false;
            }
        }

        private void BackHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            if (searching)
            {
                var api = "Windows.Phone.UI.Input.HardwareButtons";
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
                {
                    MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
                    searching = false;
                    header_txt.Visibility = Visibility.Visible;
                    HamburgerHeaderButton.Visibility = Visibility.Visible;
                    refresh_button.Visibility = Visibility.Visible;
                    search_btn.Visibility = Visibility.Visible;
                }
                else
                {
                    MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
                    searching = false;
                    header_txt.Visibility = Visibility.Visible;
                    HamburgerHeaderButton.Visibility = Visibility.Visible;
                    BackHeaderButton.Visibility = Visibility.Collapsed;
                    refresh_button.Visibility = Visibility.Visible;
                    search_btn.Visibility = Visibility.Visible;
                }
            }
            else if (!searching && fullview.Visibility == Visibility.Visible)
            {
                MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
                searching = false;
                MasterListView.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                HamburgerHeaderButton.Visibility = Visibility.Visible;
                MasterListView.Visibility = Visibility.Visible;
                refresh_button.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                fullview.Visibility = Visibility.Collapsed;
                BackHeaderButton.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
                translate_button.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Text = "";
            }
            else
            {
                MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
                searching = false;
                MasterListView.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                HamburgerHeaderButton.Visibility = Visibility.Visible;
                MasterListView.Visibility = Visibility.Visible;
                refresh_button.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                fullview.Visibility = Visibility.Collapsed;
                BackHeaderButton.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
                translate_button.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Text = "";
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            try
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                MasterListView.Visibility = Visibility.Visible;
                fullview.Visibility = Visibility.Collapsed;
                MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
                MyProgressRing.Visibility = Visibility.Visible;
                BackHeaderButton.Visibility = Visibility.Collapsed;
                HamburgerHeaderButton.Visibility = Visibility.Visible;
                searching = false;
                watching = false;
                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

                if (internet == true)
                {
                    while (MemoryItems.Count < 10)
                    {
                        Task t = PopulateCharactersAsync(MemoryItems, url_date_newest);
                        await t;
                    }
                    isLoadingNewsestDB = true;
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

        public async Task PopulateCharactersAsync(ObservableCollection<RootObject> MemoryItems, string url)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            try
            {
                // Call out to Marvel
                HttpClient http = new HttpClient();

                var response = await http.GetAsync(url);
                var jsonMessage = await response.Content.ReadAsStringAsync();

                // Response -> string / json -> deserialize
                var collection = JsonConvert.DeserializeObject<ObservableCollection<RootObject>>(jsonMessage);

                if (collection.Count != 0)
                {

                    foreach (var item in collection)
                    {
                        var sb = new StringBuilder();

                        sb.Append(item.appname);
                        var fileString = sb.ToString();
                        if (fileString.ToString() == "none" && noData == false)
                        {
                            noData = true;
                            MessageDialog dialog = new MessageDialog(loader.GetString("keineDaten_txt"), loader.GetString("keineDaten_header"));
                            await dialog.ShowAsync();
                            this.Frame.Navigate(typeof(MainPage));
                            break;
                        }
                        var sb1 = new StringBuilder();

                        sb1.Append(item.date);
                        var fileString_date = sb1.ToString();

                        var sb2 = new StringBuilder();

                        sb2.Append(item.response);
                        var fileString_response = sb2.ToString();

                        var sb3 = new StringBuilder();

                        sb3.Append(item.state);
                        var fileString_state = sb3.ToString();

                        string appname_s = fileString.ToString().Replace("Ã¼", "ü").Replace("ã¼", "Ü").Replace("Ã¤", "ä").Replace("ã¤", "Ä").Replace("Ã¶", "ö").Replace("ã", "Ö").Replace("Ã", "ß");
                        string response_s = fileString_response.ToString().Replace("Ã¼", "ü").Replace("ã¼", "Ü").Replace("Ã¤", "ä").Replace("ã¤", "Ä").Replace("Ã¶", "ö").Replace("ã", "Ö").Replace("Ã", "ß");
                        i++;
                        var memoryItem = new RootObject
                        {

                            appname = appname_s,
                            date = fileString_date,
                            response = response_s,
                            state = "ms-appx:///Assets/DatenbankStatusAnzeigen/" + fileString_state + ".png"
                        };
                        if (MemoryItems.Any(p => p.appname == fileString) == false)
                        {
                            items.Add(appname_s);
                            MemoryItems.Add(memoryItem);
                            MyProgressRing.IsActive = false;
                            MyProgressRing.Visibility = Visibility.Collapsed;
                        }

                    }
                }
            }
            catch { return; }
        }

        private async void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            HamburgerHeaderButton.Visibility = Visibility.Collapsed;
            MasterListView.Visibility = Visibility.Collapsed;
            refresh_button.Visibility = Visibility.Collapsed;
            fullview.Visibility = Visibility.Visible;
            var api = "Windows.Phone.UI.Input.HardwareButtons";
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
            {
                BackHeaderButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackHeaderButton.Visibility = Visibility.Visible;
            }
            MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
            search_btn.Visibility = Visibility.Collapsed;
            MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(""); });
            MyAutoSuggsetBox.Text = "";



            watching = true;
            searching = false;

            var selectedCharacter = (RootObject)e.ClickedItem;

            app_name_txt.Text = selectedCharacter.appname;
            app_datum_txt.Text = selectedCharacter.date;
            app_antwort_txt.Text = selectedCharacter.response;
            BitmapImage status_img = new BitmapImage(new Uri(selectedCharacter.state));
            app_status_img.Source = status_img;
            translate_button.Visibility = Visibility.Visible;
        }

        private async void MyAutoSuggsetBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            HamburgerHeaderButton.Visibility = Visibility.Collapsed;
            header_txt.Visibility = Visibility.Visible;
            refresh_button.Visibility = Visibility.Collapsed;
            searching = true;
            string a = (args.SelectedItem).ToString();
            MasterListView.ItemsSource = this.MemoryItems.Where((item) => { return item.appname.Contains(a/*MyAutoSuggsetBox.Text*/); });
            var api = "Windows.Phone.UI.Input.HardwareButtons";
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
            {
            }
            else
            {
                BackHeaderButton.Visibility = Visibility.Visible;
            }

            await System.Threading.Tasks.Task.Delay(200);
            search_btn.Visibility = Visibility.Collapsed;
            MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
            MyAutoSuggsetBox.Text = "";
        }

        private void MyAutoSuggsetBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            timer.Stop();
            elapsed_time = 0;
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += Timer_Tick;
            timer.Start();

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
				var autoSuggestBox = (AutoSuggestBox)sender;
                var filtered = items.Where(p => p.StartsWith(MyAutoSuggsetBox.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                sender.ItemsSource = filtered;
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            elapsed_time++;
            if (elapsed_time == 15 || elapsed_time >= 15)
            {
                timer.Stop();

                MyAutoSuggsetBox.Visibility = Visibility.Collapsed;
                header_txt.Visibility = Visibility.Visible;
                search_btn.Visibility = Visibility.Visible;
                elapsed_time = 0;
            }
        }

        private void search_btn_tap(object sender, RoutedEventArgs e)
        {
            elapsed_time = 0;
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += Timer_Tick;
            timer.Start();

            MyAutoSuggsetBox.Visibility = Visibility.Visible;
            header_txt.Visibility = Visibility.Collapsed;
            search_btn.Visibility = Visibility.Collapsed;
            MyAutoSuggsetBox.Focus(FocusState.Programmatic);
        }

        private async void sort_abz_click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();


            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;


            refresh_button.Label = loader.GetString("aktualisieren");
            if (isLoadingNewsestDB == false)
            {
                a_z_sort_button.Label = loader.GetString("z_to_a");
                await PopulateCharactersAsync(MemoryItems, url_date_newest);
                isLoadingNewsestDB = true;
                MasterListView.ItemsSource = MemoryItems;
            }
            else
            {

                var SortResult = MemoryItems.OrderBy(a => a.appname);
                MasterListView.ItemsSource = SortResult;
                a_z_sort_button.Label = loader.GetString("alteste");
                isLoadingNewsestDB = false;
            }

            MyProgressRing.IsActive = false;
           MyProgressRing.Visibility = Visibility.Collapsed;
        }

        private async void refresh_click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            MemoryItems.Clear();
            MasterListView.ItemsSource = null;
            MasterListView.Items.Clear();

            refresh_button.Label = loader.GetString("aktualisiere");
            if (isLoadingNewsestDB == true)
            {
                await PopulateCharactersAsync(MemoryItems, url_date_newest);
            }

            refresh_button.Label = loader.GetString("aktualisieren");

            MasterListView.ItemsSource = MemoryItems;

            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
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
            else if (uber_uns.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.UberUns));
            }
            else if (textvorlagen.IsSelected)
            {
                this.Frame.Navigate(typeof(Seiten.Textvorlagen));
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

        private void translate_click(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            string txt_tranz;
            old_text_before_translate = app_antwort_txt.Text;
            string currentLang = CultureInfo.CurrentUICulture.ToString();
            string str = currentLang.Substring(0, 2);
            if (str == "de")
            {
                app_antwort_txt.Text = app_antwort_txt.Text.Replace("&", "und");
            }
            else if (str == "en")
            {
                app_antwort_txt.Text = app_antwort_txt.Text.Replace("&", "and");
            }
            translateItNow(app_antwort_txt.Text, str);
            translate_button.Visibility = Visibility.Collapsed;
            undo_button.Visibility = Visibility.Visible;
        }

        private async void translateItNow(string to_translate, string translate_to)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var requestString =
                  String.Format("https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&text={1}&lang={2}&format={3}",
                  _apiKey, to_translate, translate_to, "plain");
            try
            {
                using (var client = new HttpClient())
                {

                    var response = client.GetAsync(requestString).Result;
                    if (response.IsSuccessStatusCode)
                    { // by calling .Result you are performing a synchronous call
                        var responseContent = response.Content;

                        // by calling .Result you are synchronously reading the result
                        string responseString = responseContent.ReadAsStringAsync().Result;


                        Regex r_out = new Regex("text(.+?)]}");
                        MatchCollection mc_out = r_out.Matches(responseString);
                        //ID11 Extractet
                        string r_outS = mc_out[0].Groups[1].Value.ToString();
                        string replace_quotationmark = r_outS.Replace("\"", "");
                        string replace_bracket = replace_quotationmark.Replace("[", "");
                        string replace_colon = replace_bracket.Replace(":", "");
                        app_antwort_txt.Text = replace_colon;
                        MyProgressRing.IsActive = false;
                        MyProgressRing.Visibility = Visibility.Visible;
                    }
                }
            }
            catch { }
        }

        private void undo_click(object sender, RoutedEventArgs e)
        {
            translate_button.Visibility = Visibility.Visible;
            undo_button.Visibility = Visibility.Collapsed;
            app_antwort_txt.Text = old_text_before_translate;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            searching = false;
            watching = false;
        }
        //END
    }
}