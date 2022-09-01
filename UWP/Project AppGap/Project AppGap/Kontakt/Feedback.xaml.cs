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
    public sealed partial class Feedback : Page
    {
        bool e_mail_bool;
        bool feedback_bool;

        public Feedback()
        {
            this.InitializeComponent();
            Sende_btn.IsEnabled = false;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            e_mail_txt.PlaceholderText = loader.GetString("your_mail");
            feedback_txt.PlaceholderText = loader.GetString("your_feedback");
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
        private void feedback_txt_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            feedback_txt.SelectionStart = feedback_txt.Text.Length + 1;
            feedback_txt.SelectionLength = 0;
            if (feedback_txt.Text.Length >= 3)
            {
                feedback_bool = true;
                feedback_txt.Header = "";
                feedback_txt.BorderBrush = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                Sende_btn.IsEnabled = false;
                feedback_bool = false;
                feedback_txt.Header = loader.GetString("betreff_error");
                feedback_txt.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (e_mail_bool && feedback_bool)
            {
                Sende_btn.IsEnabled = true;
            }
            else
            {
                Sende_btn.IsEnabled = false;
            }

        }

        private void e_mail_txt_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (e_mail_txt.Text.Length >= 6)
            {
                try
                {
                    string email = e_mail_txt.Text;
                    if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                    {
                        Sende_btn.IsEnabled = false;
                        e_mail_bool = false;
                        e_mail_txt.Header = loader.GetString("email_error_validation");
                        e_mail_txt.BorderBrush = new SolidColorBrush(Colors.Red);
                        return;
                    }
                    else
                    {
                        e_mail_bool = true;
                        e_mail_txt.Header = "";
                        e_mail_txt.BorderBrush = new SolidColorBrush(Colors.LightGreen);
                    }
                }
                catch { return; }
            }
            else
            {
                e_mail_bool = false;
                e_mail_txt.Header = loader.GetString("email_error_lengt");
                e_mail_txt.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (e_mail_bool && feedback_bool)
            {
                Sende_btn.IsEnabled = true;
            }
            else
            {
                Sende_btn.IsEnabled = false;
            }

        }

        private async void Sende_btn_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            Sende_btn.IsEnabled = false;
            e_mail_txt.IsEnabled = false;
            feedback_txt.IsEnabled = false;
            await Send_Email();
            Sende_btn.IsEnabled = true;
            e_mail_txt.IsEnabled = true;
            feedback_txt.IsEnabled = true;
            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
            e_mail_txt.Text = "";
            feedback_txt.Text = "";
            e_mail_txt.PlaceholderText = loader.GetString("your_mail");
            feedback_txt.PlaceholderText = loader.GetString("your_feedback");
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
                oMail.To.Add(new MailAddress(e_mail_txt.Text));

                // Set email subject and email body text
                oMail.Subject = "Feedback";
                oMail.TextBody = feedback_txt.Text;


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

        //END
    }
}
