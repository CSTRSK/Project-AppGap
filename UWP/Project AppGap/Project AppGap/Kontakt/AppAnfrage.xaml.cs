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
    public sealed partial class AppAnfrage : Page
    {
        bool e_mail_bool;
        bool app_name_bool;
        public AppAnfrage()
        {
            this.InitializeComponent();
            Sende_btn.IsEnabled = false;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            Entwickler_txt.PlaceholderText = loader.GetString("email_unternehmen");
            AppName_txt.PlaceholderText = loader.GetString("kontakt_app_name");
            Sende_btn.Content = loader.GetString("sende_mail");


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
            await Send_Email();
            Sende_btn.IsEnabled = true;
            Entwickler_txt.IsEnabled = true;
            AppName_txt.IsEnabled = true;
            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
            Entwickler_txt.Text = "";
            AppName_txt.Text = "";
            Entwickler_txt.PlaceholderText = loader.GetString("email_unternehmen");
            AppName_txt.PlaceholderText = loader.GetString("kontakt_app_name");
            Sende_btn.IsEnabled = false;

        }

        private async Task Send_Email()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            String Result = "";
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Set sender email address, please change it to yours
                oMail.From = new MailAddress("project-appgap@outlook.com");

                // Add recipient email address, please change it to yours
                oMail.To.Add(new MailAddress(Entwickler_txt.Text));

                if (languale_switch.IsOn == true)
                {
                    // Set email subject and email body text
                    oMail.Subject ="We miss your App"+" "+ AppName_txt.Text + " " + "for Windows 10 and Windows 10S";
                    oMail.TextBody = "Ladies and gentlemen,\n\nI wonder always again why actually the app" + " " + AppName_txt.Text + " " + "you for Windows 10 devices (PCs, notebooks, tablets, smart phones, Xbox and Microsoft HoloLens) there is no. Windows 10 is the far most common system with world-wide access to a large app store. With Windows 10S, the demand for your app is even greater. Because users can only download programs from the Windows App Store. I have a feeling that that is not yet aware of you.\n\nYou can keep your iOS app with a kind of 'Bridge' by Microsoft, called 'Project Islandwood' (to do so they can learn here httts://dev.windows.com/en-us/bridges/ios and here https://blogs.windows.com/buildingapps/2016/01/20/building-a-simple-app-with-the-windows-bridge-for-ios/) directly and in a few steps on the 10 app platform porting Windows. The converted app is available on all other devices from smartphones over tablets up to the PC, then at the same time because Windows 10 and Windows 10S on all these devices used the same base code.\n\nIt takes very little time and is very cost-effective, use this tool to create an app for Windows 10 platform. It would certainly look forward me and more than 500 million other Windows users, if you hear about and can confirm me to develop an app for Windows 10.\n\nI would be grateful if you could look at the thing itself and thus give me a feedback can.\n\nWith friendly greetings\nYour CSTRSK -Project AppGap Team\nRene\nhttps://cstrsk.de/\nhttp://project-appgap.cstrsk.de/";
                }
                else
                {
                    // Set email subject and email body text
                    oMail.Subject = "Wir vermissen Ihre App"+ " " + AppName_txt.Text + " " + "für Windows 10 & Windows 10S";
                    oMail.TextBody = "Sehr geehrte Damen und Herren,\n\nIch frage mich immer wieder warum es eigentlich die App" + " " + AppName_txt.Text + " " + "von Ihnen nicht im Windows Store gibt. Windows 10 ist das weit verbreitetste System mit weltweiter Anbindung an einen großen App-Store. Mit Windows 10S wird die Nachfragen Ihrer App noch größer. Da Nutzer Programme nur noch vom Windows App Store downloaden können. Ich habe das Gefühl, dass Ihnen das noch nicht ganz bewusst ist.\n\nDabei können Sie mit einer Art 'Brücke' von Microsoft, namens 'Project Islandwood'(dazu können sie hier https://dev.windows.com/en-us/bridges/ios und hier  https://blogs.windows.com/buildingapps/2016/01/20/building-a-simple-app-with-the-windows-bridge-for-ios/  mehr erfahren) Ihre iOS-App direkt und in wenigen Schritten auf die Windows 10 App-Plattform portieren. Die umgewandelte App ist dann zugleich auf allen anderen Geräten von Smartphones über Tablets bis hin zum PC verfügbar, weil Windows 10 und Windows 10S auf all diesen Geräten den gleichen Basis-Code verwendet.\n\nEs nimmt nur sehr wenig Zeit ein und ist sehr kostengünstig, mit diesem Tool eine App für die Windows 10 Plattform zu erstellen.Es würde mich und mehr als 500 Millionen andere Windows - Nutzer sicherlich freuen, wenn Sie von sich hören lassen und mir bestätigen können eine App für Windows 10 zu entwickeln.\n\nIch wäre Ihnen dankbar, wenn Sie sich die Sache mal anschauen könnten und mir dementsprechend eine Rückmeldung geben können.\nGerne könnten wir von CSTRSK eine Windows 10 App für Sie erstellen oder vielleicht währen Sie auch damit einverstanden, wenn ein privat Entwickler eine Inoffizielle" + " " + AppName_txt.Text + " " + "App erstellt. Wir könnten ihnen bei einer Verbindungsaufnahme behilflich sein.\n\nMit freundliche Grüßen\n\ndein CSTRSK -Project AppGap Team\nRene\nhttps://cstrsk.de/\nhttp://project-appgap.cstrsk.de/";
                }

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
                Result = loader.GetString("emailsend_suggses");
            }
            catch (Exception ep)
            {
                Result = String.Format(loader.GetString("emailsend_fail") + "{0}", ep.Message);
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

            if (e_mail_bool && app_name_bool)
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
                        Sende_btn.IsEnabled = false;
                        e_mail_bool = false;
                        Entwickler_txt.Header = loader.GetString("email_error_validation");
                        Entwickler_txt.BorderBrush = new SolidColorBrush(Colors.Red);
                        return;
                    }
                    else
                    {
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
            if (e_mail_bool && app_name_bool)
            {
                Sende_btn.IsEnabled = true;
            }else
            {
                Sende_btn.IsEnabled = false;
            }
        }


        //end
    }
}
