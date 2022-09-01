using EASendMailRT;
using Project_AppGap.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class ruckmeldung : Page
    {
        bool e_mail_bool;
        bool app_name_bool;
        bool antwort_bool;
        string status_string;

        public ruckmeldung()
        {
            this.InitializeComponent();
            Sende_btn.IsEnabled = false;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            Entwickler_txt.PlaceholderText = loader.GetString("email_unternehmen");
            AppName_txt.PlaceholderText = loader.GetString("kontakt_app_name");
            Sende_btn.Content = loader.GetString("sende_mail");
            antwort_txt.PlaceholderText = loader.GetString("antwort_von");
            state_box.PlaceholderText = loader.GetString("state");

            CheckAndUpdateTheme();
            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
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

        private async void Sende_btn_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            Sende_btn.IsEnabled = false;
            Entwickler_txt.IsEnabled = false;
            AppName_txt.IsEnabled = false;
            Sende_btn.IsEnabled = false;
            antwort_txt.IsEnabled = false;
            state_box.IsEnabled = false;
            await Send_Email();
            Sende_btn.IsEnabled = true;
            Entwickler_txt.IsEnabled = true;
            AppName_txt.IsEnabled = true;
            Sende_btn.IsEnabled = true;
            antwort_txt.IsEnabled = true;
            state_box.IsEnabled = true;
            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
            Entwickler_txt.Text = "";
            AppName_txt.Text = "";
            antwort_txt.Text = "";
            Entwickler_txt.Text = "";
            Entwickler_txt.PlaceholderText = loader.GetString("email_unternehmen");
            AppName_txt.PlaceholderText = loader.GetString("kontakt_app_name");
            antwort_txt.PlaceholderText = loader.GetString("antwort_von");
            state_box.PlaceholderText = loader.GetString("state");
            Sende_btn.IsEnabled = false;
        }

        private async Task Send_Email()
        {
            String Result = "";
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Set sender email address, please change it to yours
                oMail.From = new MailAddress("project-appgap@outlook.com");

                // Add recipient email address, please change it to yours
                oMail.To.Add(new MailAddress("project-appgap@outlook.com"));

                // Set email subject and email body text
                oMail.Subject = "Rückmeldung Einsendung";
                oMail.TextBody = "{" +
                    "appname" + ":" + "\"" + AppName_txt.Text + "\"" + "," +
                    "date" + ":" + "\"" + DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString() + "\"" + "," +
                    "state" + ":" + "\"" + status_string + "\"" +
                    "response" + ":" + "\"" + antwort_txt.Text + "\"" +
                    "},";

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer("mail.smtp2go.com");


                // If your SMTP server requires TLS connection on 25 port, please add this line
                // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                // If your SMTP server requires SSL connection on 465 port, please add this line
                oServer.Port = 2525;
                oServer.ConnectType = SmtpConnectType.ConnectSTARTTLS;


                // User and password for SMTP authentication
                oServer.User = "GITHUB NO USER";
                oServer.Password = "GITHUB NO PW";

                await oSmtp.SendMailAsync(oServer, oMail);
                Result = "Email wurde erfolgreich verschickt!";
            }
            catch (Exception ep)
            {
                Result = String.Format("Das senden der E-Mail ist wegen dem folgenden Fehler fehlgeschlagen: {0}", ep.Message);
            }

            // Display Result by Diaglog box
            Windows.UI.Popups.MessageDialog dlg = new
                Windows.UI.Popups.MessageDialog(Result);

            await dlg.ShowAsync();
        }

        private void AppName_txt_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            AppName_txt.SelectionStart = AppName_txt.Text.Length + 1;
            AppName_txt.SelectionLength = 0;
            if (AppName_txt.Text.Length >= 3)
            {
                app_name_bool = true;
                AppName_txt.Header = "";
                AppName_txt.BorderBrush = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                Sende_btn.IsEnabled = false;
                app_name_bool = false;
                AppName_txt.Header = loader.GetString("betreff_error");
                AppName_txt.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (e_mail_bool && app_name_bool && antwort_bool && state_box.SelectedIndex != null)
            {
                Sende_btn.IsEnabled = true;
            }
            else
            {
                Sende_btn.IsEnabled = false;
            }
        }

        private void Entwickler_txt_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (Entwickler_txt.Text.Length >= 6)
            {
                try
                {
                    string email = Entwickler_txt.Text;
                    if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                    {
                        e_mail_bool = false;
                        Entwickler_txt.Header = loader.GetString("email_error_validation");
                        Entwickler_txt.BorderBrush = new SolidColorBrush(Colors.Red);
                        return;
                    }
                    else
                    {
                        Sende_btn.IsEnabled = false;
                        e_mail_bool = true;
                        Entwickler_txt.Header = "";
                        Entwickler_txt.BorderBrush = new SolidColorBrush(Colors.LightGreen);
                    }
                }
                catch { return; }
            }
            else
            {
                e_mail_bool = false;
                Entwickler_txt.Header = loader.GetString("email_error_lengt");
                Entwickler_txt.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (e_mail_bool && app_name_bool && antwort_bool && state_box.SelectedIndex != null)
            {
                Sende_btn.IsEnabled = true;
            }
            else
            {
                Sende_btn.IsEnabled = false;
            }
        }

        private void antwort_txt_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            antwort_txt.SelectionStart = antwort_txt.Text.Length + 1;
            antwort_txt.SelectionLength = 0;
            if (antwort_txt.Text.Length >= 3)
            {
                antwort_bool = true;
                antwort_txt.Header = "";
                antwort_txt.BorderBrush = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                Sende_btn.IsEnabled = false;
                antwort_bool = false;
                antwort_txt.Header = loader.GetString("betreff_error");
                antwort_txt.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (e_mail_bool && app_name_bool && antwort_bool && state_box.SelectedIndex != null)
            {
                Sende_btn.IsEnabled = true;
            }
            else
            {
                Sende_btn.IsEnabled = false;
            }
        }

        private void state_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBoxItem = e.AddedItems[0] as ComboBoxItem;
            if (comboBoxItem == null) return;
            var content = comboBoxItem.Content as string;

            if(content != null && content == "verfügbar")
            {
                status_string = "available";
            }
            else if ( content == "in Arbeit")
            {
                status_string = "inwork";
            }
            else if (content == "wird überprüft")
            {
                status_string = "checking";
            }
            else if (content == "abgelehnt")
            {
                status_string = "rejected";
            }

            if (e_mail_bool && app_name_bool && antwort_bool && state_box.SelectedIndex != null)
            {
                Sende_btn.IsEnabled = true;
            }
            else
            {
                Sende_btn.IsEnabled = false;
            }
        }


        //end
    }
}
