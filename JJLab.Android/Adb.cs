using System;
using System.IO;
using System.Diagnostics;

namespace JJLab.Android
{
    public static class Adb
    {
        public static string Device;
        public static string process(string cmd)
        {
            ProcessStartInfo p = new ProcessStartInfo()
            {
                FileName = "adb.exe",
                WorkingDirectory = @"C:\adb",
                Arguments = cmd,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            Process p1 = new Process();
            p1.StartInfo = p;
            p1.Start();
            return p1.StandardOutput.ReadToEnd();
        }
        public static bool HasConnectedDevice()
        {
            Device = "";
            bool flag = false;
            string p = process("devices");
            if (p.Length > 29)
            {
                using (StringReader s = new StringReader(p))
                {
                    string line;
                    while (s.Peek() != -1)
                    {
                        line = s.ReadLine();

                        if (line.StartsWith("List") || line.StartsWith("\r\n") || line.Trim() == "")
                            continue;

                        if (line.IndexOf('\t') != -1)
                        {
                            line = line.Substring(0, line.IndexOf('\t'));
                            flag = true;
                        }
                    }
                }
            }
            return flag;            
        }
        public static bool HasRoot()
        {            
            if(process("shell su -c whoami").Contains("root"))
            {
                return true;
            }
            return false;
        }
        public static void MiAccountSignInSignOut(string apkpath)
        {
            process("push " + apkpath + " /data/local/tmp/a.apk");
            process("shell su -c mount -o rw,remount -t auto /system");
            process("shell su -c mount -o rw,remount -t auto /");
            process("shell mv /data/local/tmp/a.apk /system/priv-app/Finddevice/Finddevice.apk");
            process("shell su -c chmod 644 /system/priv-app/Finddevice/Finddevice.apk");
        }
        public static void Reboot(string Mode)
        {
            process("reboot " + Mode);
        }                
        public static bool system_as_root()
        {
            if(!process("shell ls /system_root").Contains("No such file or directory"))
            {
                return false;   
            }
            else
            {
                return true;
            }
        }
        public static void EUBypass()
        {
            if (system_as_root())
            {
                process("shell mv /system_root/system/app/CloudService/CloudService.apk /system_root/system/app/CloudService.bak");
                process("shell mv /system_root/system/priv-app/FindDevice/FindDevice.apk /system_root/system/priv-app/FindDevice/FindDevice.bak");                
            }
            else
            {
                process("shell mv /system/app/CloudService/CloudService.apk /system/app/CloudService.bak");
                process("shell mv /system/priv-app/FindDevice/FindDevice.apk /system/priv-app/FindDevice/FindDevice.bak");
            }
        }
    }
}
