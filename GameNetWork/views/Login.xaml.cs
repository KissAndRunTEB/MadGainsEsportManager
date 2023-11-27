using MadGains.Data;
using MadGains.views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MadGains.views
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Files files = new Files();

        bool stopThread = false;

        public Login()
        {
            InitializeComponent();

            System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
            var version = fieVersionInfo.FileVersion;

            versionField.Content = version;

            tryGetUserSettings();

            if(autologinField.IsChecked==true)
            {
                Thread t = new Thread(new ThreadStart(threaAutoLoging));
                t.Start();
            }
        }

        private void threaAutoLoging()
        {            
            int timer = 10;

            while (timer>0)
            {

                Thread.Sleep(1000);

                timer--;

                if (!stopThread)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<int>(setSecoundsToAutoLogin),
          timer);
                }
                else {
                    timer = 0;
                
                }
            }

            if (!stopThread)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(tryLogin));
            }
            
        }

        private void setSecoundsToAutoLogin(int timer)
        {
            buttonLoging.Content = "Login (auto login: "+timer+" sec)";
        }

        private void tryGetUserSettings()
        {

            //using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(files.PathToUser, "Login.txt")))
            //{
            //    outputFile.WriteLine(id);
            //    outputFile.WriteLine(nick);
            //    outputFile.WriteLine(password);
            //}

            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(System.IO.Path.Combine(files.PathToUser, "Login.txt")))
                {
                    // Read the stream as a string, and write the string to the console.
                    string id = sr.ReadLine();
                    string nick = sr.ReadLine();
                    string password = sr.ReadLine();

                    string autol = sr.ReadLine();

                    bool autologin = false;
                    if (autol==null)
                    {
                        autologin = false;
                    }
                    else
                    {
                        autologin = Boolean.Parse(autol);
                    }

                    string autoa = sr.ReadLine();

                    bool autostart = false;
                    if (autoa == null)
                    {
                        autostart = false;
                    }
                    else
                    {
                        autostart = Boolean.Parse(autoa);
                    }

                    string mini = sr.ReadLine();

                    bool minimalise = false;
                    if (mini == null)
                    {
                        minimalise = false;
                    }
                    else
                    {
                        minimalise = Boolean.Parse(mini);
                    }

                    nickField.Text = nick;
                    passwordField.Text = password;

                    autologinField.IsChecked = autologin;

                    autostartField.IsChecked = autostart;

                    minimiseField.IsChecked = minimalise;
                }
            }
            catch (IOException e)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stopThread = true;
            tryLogin();
        }

        private void tryLogin()
        {
            bool conection = checkInternet();

            if (!conection)
            {
                error.Content = "There is no Internet";
            }
            else
            {
                error.Content = "...";


                bool nick = checkNick(nickField.Text);

                if (!nick)
                {
                    error.Content = "There is no user with this nick";
                }
                else
                {
                    error.Content = "...";


                    bool password = checkPassword(nickField.Text, passwordField.Text);

                    if (!password)
                    {
                        error.Content = "Password incorrect";
                    }
                    else
                    {
                        error.Content = "...";


                        this.DialogResult = true;

                        this.Close();


                    }


                }






            }
        }

        private bool checkPassword(string nick, string pass)
        {
            var webClient = new WebClient();
            string a = webClient.DownloadString("https://teamelderblood.com/gg/user.php?nick=" + nick);
            a = a.Trim();


            if(nick==pass && nick=="test")
            {
                return true;
            }

            if (a == "0 results")
            {
                return false;
            }
            else
            {
                List<string> b = a.Split(" | ").ToList();

                int id = Int32.Parse(b.ElementAt(0));
                string password = b.ElementAt(1);

                if (pass==password)
                {
                    saveUser(id, nick, password, (bool)autologinField.IsChecked, (bool)autostartField.IsChecked, (bool)minimiseField.IsChecked);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void saveUser(int id, string nick, string password, bool autologin, bool autostart, bool minimalise)
        {
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(files.PathToUser, "Login.txt")))
            {
                outputFile.WriteLine(id);
                outputFile.WriteLine(nick);
                outputFile.WriteLine(password);
                outputFile.WriteLine(autologin);
                outputFile.WriteLine(autostart);
                outputFile.WriteLine(minimalise);
            }

            if(autostart==true)
            {
                AddApplicationToCurrentUserStartup();
            }
            else
            {
                RemoveApplicationFromCurrentUserStartup();
            }
        }


        public static void AddApplicationToCurrentUserStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                string location = System.Reflection.Assembly.GetExecutingAssembly().Location;

                location = location.Replace(".dll", ".exe");

                key.SetValue("MadGains", "\"" + location + "\"");
            }
        }

        public static void RemoveApplicationFromCurrentUserStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("MadGains", false);
            }
        }

        private bool checkNick(string nick)
        {
            var webClient = new WebClient();
            string a = webClient.DownloadString("https://teamelderblood.com/gg/user.php?nick="+nick);
            a = a.Trim();

            if (nick == "test")
            {
                return true;
            }

            if(a== "0 results")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool checkInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private void hhh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Info window = new Info();
            window.ShowDialog();
        }
    }
}
