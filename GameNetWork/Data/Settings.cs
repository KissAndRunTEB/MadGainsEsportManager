using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MadGains.Data
{
    public class Settings
    {
        int idUser;
        string nick;
        string password;

        bool autologin;
        bool autostart;
        
        bool minimalise;

        public Settings()
        {
            Files files = new Files();

            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(System.IO.Path.Combine(files.PathToUser, "Login.txt")))
                {
                    // Read the stream as a string, and write the string to the console.
                    this.IdUser = Int32.Parse(sr.ReadLine());
                    this.Nick = sr.ReadLine();
                    this.Password = sr.ReadLine();

                    this.Autologin = Boolean.Parse(sr.ReadLine());
                    this.Autostart = Boolean.Parse(sr.ReadLine());

                    this.Minimalise = Boolean.Parse(sr.ReadLine());
                }
            }
            catch (IOException e)
            {

            }
        }

        public int IdUser { get => idUser; set => idUser = value; }
        public string Nick { get => nick; set => nick = value; }
        public string Password { get => password; set => password = value; }
        public bool Minimalise { get => minimalise; set => minimalise = value; }
        public bool Autologin { get => autologin; set => autologin = value; }
        public bool Autostart { get => autostart; set => autostart = value; }
    }
}
