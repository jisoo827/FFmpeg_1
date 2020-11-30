using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JJCastDemo.FFmpeg.Statement;
using JJCastDemo.Common;

namespace JJCastDemo.FFmpeg
{
    public class DiagnosticsControl
    {
        private static string _FFMPEGPath = string.Empty;

        public int AudioRecorderProcess_ID = 0;
        public int CamRecorderProcess_ID = 0;
        private static Process processDesk = null;
        private static Process processCam = null;
        readonly FFmpegStatement ffmpegStatement = null;

        public DiagnosticsControl()
        {
            ffmpegStatement = new FFmpegStatement();
        }

        #region Event Method
        public List<Device> GetDeviceList()
        {
            List<Device> rtnList = new List<Device>();
            Process process = new Process();

            if (CommandExcute(ffmpegStatement.GetDeviceLisStmt(), process, true,false) == 1)
            {
                try
                {

                    string device = string.Empty;
                    process.StandardInput.WriteLine("exit");
                    foreach (string processOutput in Array.ConvertAll(process.StandardError.ReadToEnd().Split(Environment.NewLine.ToCharArray()), Convert.ToString))
                    {
                        if (processOutput.IndexOf("DirectShow video devices") > 0) device = "video";
                        if (processOutput.IndexOf("DirectShow audio devices") > 0) device = "audio";
                        if (device.Trim().Length > 0 && (processOutput.IndexOf("\"") > 0) && (processOutput.IndexOf("Alternative name") < 0))
                        {
                            Device dv = new Device
                            {
                                device = device,
                                name = processOutput.Substring(processOutput.IndexOf("\"") + 1, processOutput.Length - processOutput.IndexOf("\"") - 2)
                            };
                            rtnList.Add(dv);
                        }
                        Debug.WriteLine(processOutput);
                    }
                    process.WaitForExit();
                    process.Close();
                }
                catch (Exception e)
                {
                    process.Kill();
                }

            }
            return rtnList;

        }

        public int PartialRecord(string mic, Device monitor, string cam)
        {
            processDesk = new Process();
            processCam = new Process();
            
            try
            {
                string offset = "-offset_x " + monitor.point.X.ToString() + " -offset_y " + monitor.point.Y.ToString() + " -video_size " + monitor.size.Width.ToString() + "X" + monitor.size.Height.ToString();
                string argument = mic.Trim().Length == 0 ? ffmpegStatement.DesktopPartialRecordStmt(offset) : ffmpegStatement.DesktopPartialRecordStmt(offset, mic);
                
                if (cam.Trim().Length > 0)
                {
                    string argument2 = ffmpegStatement.CamRecordStmt(cam);
                    bool test = false;
                    if (test)
                    {
                        argument2 = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"cam.mp4\"";
                    }
                    CommandExcute(argument, processDesk, argument2, processCam);
                    CamRecorderProcess_ID = processCam.Id;
                }
                else
                {
                    CommandExcute(argument, processDesk,false,false);
                }

                AudioRecorderProcess_ID = processDesk.Id;
            }
            catch (Exception e)
            {
                Global.WriteLog("녹화 오류 : " + e.Message);
                return -1;
            }
            return 1;
        }

        public int ConcatVideo(string front, string back, string result)
        {
            return CommandExcute(ffmpegStatement.ConcatVideoStmt(front, back, result), new Process(), false, true);
        }

        public int OverLay(string rgbHex, string rdbCheck, Size monitorSize)
        {
            string pad = string.Empty;
            string crop = "[b]";
            string overlay = "(W - w):(H - h)";
            string size = "320:240";
            if (rgbHex.Trim().Length == 0) rgbHex = "00D800";
            switch (rdbCheck)
            {
                case "RIGHTOUT":
                    pad = "iw + (iw/3):ih";
                    crop = "[b]crop=iw/3:ih:iw/3:0[b2];[b2]";
                    overlay = "(W - w):0";
                    size = monitorSize.Width.ToString() + ":" + monitorSize.Height.ToString();
                    break;
                case "RIGHTIN":
                    pad = "iw:ih";
                    crop = "[b]crop=iw/3:ih:iw/3:0[b2];[b2]";
                    overlay = "(W - w):0";
                    size = monitorSize.Width.ToString() + ":" + monitorSize.Height.ToString();
                    break;
                case "RIGHTBOTTOMIN":
                default:
                    pad = "iw:ih";
                    break;
                case "RIGHTBOTTOMOUT":
                    pad = "iw+320:ih";
                    break;
                case "DIAGONALOUT":
                    pad = "iw+320:ih+240";
                    break;
                case "DIAGONALIN":
                    pad = "iw+160:ih+120";
                    break;
            }
            return CommandExcute(ffmpegStatement.OverlayVideoStmt(pad, crop, overlay, rgbHex, size), new Process(), false, true);
        }

        public int Cut(string url, string start, string end, string max)
        {
            if(CommandExcute(ffmpegStatement.CutVideoStmt(url,"0", start, 1), new Process(), false, true) == 1)
                return CommandExcute(ffmpegStatement.CutVideoStmt(url,end, max, 2), new Process(), false, true);
            else return -1;
        }

        public int StopRecord()
        {
            if (AudioRecorderProcess_ID != 0)
            {
                RecordProcessDispose(processDesk);
                AudioRecorderProcess_ID = 0;
            }
            if (CamRecorderProcess_ID != 0)
            {
                RecordProcessDispose(processCam);
                CamRecorderProcess_ID = 0;
            }
            return 0;
        }
        #endregion

        #region Command Method

        private static int CommandExcute(string argument, Process process, bool isReturnOuput = false, bool isExit = false)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            try
            {
                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.WorkingDirectory = Application.StartupPath;
                if (isReturnOuput)
                {
                    cmd.RedirectStandardError = true;
                    cmd.StandardOutputEncoding = System.Text.Encoding.UTF8;
                    cmd.StandardErrorEncoding = System.Text.Encoding.UTF8;
                }
                process.StartInfo = cmd;
                process.Start();
                Thread.Sleep(200);
                process.StandardInput.WriteLine(argument);
                Debug.WriteLine("process id : " + process.Id);

                if(isExit)
                {
                    int cnt = 0;
                    while (!process.WaitForExit(100))
                    {
                        Thread.Sleep(1000);
                        if (cnt++ == 3600) process.StandardInput.WriteLine("exit");
                    }

                    process.Dispose();
                }
            }
            catch (Exception e)
            {
                Global.WriteLog("에러발생 : " + e.Message);
                return -1;
            }
            return 1;
        }

        private static void CommandExcute(string argument1, Process process1, string argument2, Process process2)
        {
            ProcessStartInfo cmd1 = new ProcessStartInfo();
            ProcessStartInfo cmd2 = new ProcessStartInfo();

            cmd1.FileName = cmd2.FileName = "cmd";
            cmd1.RedirectStandardInput = cmd2.RedirectStandardInput = true;
            cmd1.RedirectStandardOutput = cmd2.RedirectStandardOutput = true;
            cmd1.UseShellExecute = cmd2.UseShellExecute = false;
            cmd1.CreateNoWindow = cmd2.CreateNoWindow = true;
            cmd1.WorkingDirectory = cmd2.WorkingDirectory = Application.StartupPath;

            process2.StartInfo = cmd2;
            process2.Start();
            Thread.Sleep(200);
            process2.StandardInput.WriteLine(argument2);

            process1.StartInfo = cmd1;
            process1.Start();
            Thread.Sleep(200);
            process1.StandardInput.WriteLine(argument1);
            Debug.WriteLine("process1 id : " + process1.Id);
            Debug.WriteLine("process2 id : " + process2.Id);
        }

        private static void RecordProcessDispose(Process process)
        {
            process.StandardInput.WriteLine("q");
            process.WaitForExit(1000);
            Thread.Sleep(1000);
            process.StandardInput.WriteLine("exit");
            while (!process.WaitForExit(100))
            {
                Thread.Sleep(1000);
            }
            process.Dispose();
        }
        #endregion

        #region Variable
        public static string FFMPEGPath
        {
            get { return _FFMPEGPath; }
            set { _FFMPEGPath = value; }
        }
        #endregion
    }
    public class Device
    {
        public string device { get; set; }
        public string name { get; set; }
        public Size size { get; set; }
        public Point point { get; set; }
    }


}