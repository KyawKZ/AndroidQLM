using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace JJLab.Android
{
    public static class fastboot
    {
        public static string process(string cmd)
        {
            ProcessStartInfo p = new ProcessStartInfo
            {
                FileName = "fastboot.exe",
                WorkingDirectory = @"C:\adb",
                Arguments = cmd,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            };
            Process fb = new Process();
            fb.StartInfo = p;
            fb.Start();
            return fb.StandardOutput.ReadToEnd().ToLower();
        }
        public static bool isConnected()
        {            
            if (process("devices").Contains("fastboot"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool flash(string partition,string image)
        {            
            if(process("flash "+partition+" \"" + image+"\"").Contains("OKAY"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool erase(string partition)
        {
            if(process("erase " + partition).Contains("OKAY"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool EDL()
        {
            if(process("oem edl").Contains("Finished"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
