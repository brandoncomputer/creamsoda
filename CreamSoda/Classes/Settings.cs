using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Drawing;

namespace CoHLauncher
{
    class Settings
    {

        public static bool SetupNeeded {
            get {
                return GamePath == "" || !File.Exists(Path.Combine(GamePath, "CoHLauncher.exe"));
            }
        }

        public static string GamePath
        {
            get
            {
                FixRegistryPolution();
                return CoHLauncherRegistry.GetValue("CoHPath", "").ToString();
            }
            set
            {
                CoHLauncherRegistry.SetValue("CoHPath", value);
            }
        }

        public static bool QuitOnLaunch
        {
            get
            {

                return CoHLauncherRegistry.GetValue("QuitOnLaunch", "FALSE").ToString().ToUpper() == "TRUE";
            }
            set
            {
                if(value) 
                    CoHLauncherRegistry.SetValue("QuitOnLaunch", "TRUE");
                else
                    CoHLauncherRegistry.SetValue("QuitOnLaunch", "FALSE");
            }
        }

        public static string GameParams
        {
            get
            {
                return CoHLauncherRegistry.GetValue("Parameters", "").ToString();
            }
            set
            {
                CoHLauncherRegistry.SetValue("Parameters", value);
            }
        }

        public static Color BGColor
        {
            get
            {
                bool success = int.TryParse(CoHLauncherRegistry.GetValue("BGColor", SystemColors.Control.ToArgb()).ToString(), out int color);
                if (success) return Color.FromArgb(color);
                else return Color.Black;
            }
            set
            {
                CoHLauncherRegistry.SetValue("BGColor", value.ToArgb());
            }
        }

        public static Color TextColor
        {
            get
            {
                bool success = int.TryParse(CoHLauncherRegistry.GetValue("TextColor", SystemColors.ControlText.ToArgb()).ToString(), out int color);
                if (success) return Color.FromArgb(color);
                else return Color.Black;
            }
            set
            {
                CoHLauncherRegistry.SetValue("TextColor", value.ToArgb());
            }
        }

        public static Color ListColor
        {
            get
            {
                bool success = int.TryParse(CoHLauncherRegistry.GetValue("ListColor", SystemColors.Window.ToArgb()).ToString(), out int color);
                if (success) return Color.FromArgb(color);
                else return Color.Black;
            }
            set
            {
                CoHLauncherRegistry.SetValue("TextColor", value.ToArgb());
            }
        }

        public static Color ListTextColor
        {
            get
            {
                bool success = int.TryParse(CoHLauncherRegistry.GetValue("ListTextColor", SystemColors.WindowText.ToArgb()).ToString(), out int color);
                if (success) return Color.FromArgb(color);
                else return Color.Black;
            }
            set
            {
                CoHLauncherRegistry.SetValue("ListTextColor", value.ToArgb());
            }
        }

        public static List<string> Manifests
        {
            get
            {
                char[] splitChars = {'\n'};
                return CoHLauncherRegistry.GetValue("Manifests", "").ToString().Split(splitChars, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }
            set
            {
                string strManifests = "";
                foreach (string Manifest in value)
                {
                    strManifests += Manifest.Trim() + "\n";
                }

                if (strManifests.EndsWith("\n")) strManifests = strManifests.Substring(0, strManifests.Length - 1);
                CoHLauncherRegistry.SetValue("Manifests", strManifests);
            }
        }

        public static string LastManifest
        {
            get
            {
                return CoHLauncherRegistry.GetValue("LastManifest", "").ToString();
            }
            set
            {
                CoHLauncherRegistry.SetValue("LastManifest", value);
            }
        }

        public static void Reset() {
            CoHLauncherRegistry.DeleteValue("CoHPath");
        }

        private static RegistryKey CoHLauncherRegistry {
            get
            {
                RegistryKey r = Registry.CurrentUser.OpenSubKey(@"Software\CoHLauncher\Settings", true);

                if (r == null)
                {
                    r = Registry.CurrentUser.CreateSubKey(@"Software\CoHLauncher\Settings");
                }

                return r;
            }
        }

        private static void FixRegistryPolution()
        {
            try
            {
                string s = Registry.CurrentUser.GetValue("CoHPath", "").ToString();
                if (s != "")
                {
                    GamePath = s;
                    Registry.CurrentUser.DeleteValue("CoHPath");
                }
            }
            catch (Exception) { }
        }



    }
}
