using System;
using System.Diagnostics;
using System.IO;

namespace JJLab.Android
{
    public static class fhloader
    {
        public static bool isOk;

        public static string process(string cmd)
        {
            ProcessStartInfo info1 = new ProcessStartInfo();
            info1.UseShellExecute = false;
            info1.CreateNoWindow = true;
            info1.FileName = @"bin\fh_loader.exe";
            info1.Arguments = cmd;
            info1.RedirectStandardOutput = true;
            Process process1 = new Process();
            process1.StartInfo = info1;
            Process process = process1;
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        public static void Reboot(string port)
        {
            isOk = false;
            if (process(@"--port=\\.\" + port + " --reset --noprompt --showpercentagecomplete --memoryname=emmc").Contains("{All Finished Successfully}"))
            {
                isOk = true;
            }
        }

        public static void WriteByAddress(string port, string image, string[] StartSize)
        {
            isOk = false;
            string[] textArray1 = new string[9];
            textArray1[0] = @"--port=\\.\";
            textArray1[1] = port;
            textArray1[2] = " --sendimage=\"";
            textArray1[3] = image;
            textArray1[4] = "\" --start_sector=";
            textArray1[5] = StartSize[0];
            textArray1[6] = " --num_sectors=";
            textArray1[7] = StartSize[1];
            textArray1[8] = " --showpercentagecomplete --memoryname=emmc";
            if (process(string.Concat(textArray1)).Contains("{All Finished Successfully}"))
            {
                isOk = true;
            }
        }
    }
}
