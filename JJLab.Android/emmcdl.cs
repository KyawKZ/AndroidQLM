using System;
using System.Diagnostics;
using System.IO;

namespace JJLab.Android
{
    public class emmcdl
    {
        public static string PortName;
        public static string process(string cmd)
        {
            ProcessStartInfo info1 = new ProcessStartInfo();
            info1.UseShellExecute = false;
            info1.CreateNoWindow = true;
            info1.FileName = @"bin\emmcdl.exe";
            info1.Arguments = cmd;
            info1.RedirectStandardOutput = true;
            Process process1 = new Process();
            process1.StartInfo = info1;
            Process process = process1;
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }
        public static bool Detect()
        {
            PortName = "";
            bool flag = false;
            string s = process("-l");
            if (s.Contains("Qualcomm"))
            {
                using (StringReader reader = new StringReader(s))
                {
                    while (true)
                    {
                        if (reader.Peek() == -1)
                        {
                            break;
                        }
                        string str2 = reader.ReadLine();
                        if (str2.StartsWith("Qualcomm"))
                        {
                            string str3 = str2.Substring(0, str2.IndexOf("("));
                            PortName = str2.Substring(str2.IndexOf("(") + 1).Replace(")", "");
                            flag = true;
                        }
                    }
                }
            }
            return flag;
        }
        public static bool SendLoader(string port, string loader)
        {            
            if (process($"-p "+port+" -f "+"\""+loader+"\"").Contains("The operation completed successfully"))
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        public static bool SendXML(string port, string loader, string xmlpath)
        {
            if (process(" -p " + port + " -f " + "\"" + loader + "\" -x " + xmlpath).Contains("The operation completed successfully"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string[] PartitionAddress(string port, string loader, string partition, string gptList)
        {
            string[] strArray = new string[] { "", "" };
            using (StringReader reader = new StringReader(gptList))
            {
                while (true)
                {
                    if (reader.Peek() != -1)
                    {
                        string str = reader.ReadLine();
                        if (!str.Contains("Partition Name: " + partition))
                        {
                            continue;
                        }
                        char[] separator = new char[] { ':' };
                        string[] strArray2 = str.Split(separator);
                        string str2 = strArray2[2].Replace("Size in LBA", "").Replace(" ", "");
                        string str3 = strArray2[3].Replace(" ", "");
                        strArray = new string[] { str2, str3 };
                    }
                    break;
                }
            }
            return strArray;
        }
        public static string[] GetAddress(string port, string loader, string partition)
        {
            string[] strArray = new string[] { "", "" };
            string gPT = GetGPT(port, loader);
            return PartitionAddress(port, loader, partition, gPT);
        }

        private static string GetGPT(string port, string loader) =>
            process($"-p {port} -f \"{loader}\" -gpt");
    }
}
