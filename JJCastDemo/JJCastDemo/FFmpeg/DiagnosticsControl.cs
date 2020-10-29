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

namespace JJCastDemo.FFmpeg
{
    public class DiagnosticsControl
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

        public int AudioRecorderProcess_ID = 0;
        public int CamRecorderProcess_ID = 0;
        Process processCam = new Process();

        // Enumerated type for the control messages sent to the handler routine
        enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);

        System.IO.StreamReader SR;
        System.IO.StreamWriter SW;

        string _FFMPEGPath = SystemInformation.ComputerName == "DESKTOP-2OGUI9T" ? @"E:\_Works\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe" : @"C:\Users\jisu827\Downloads\ffmpeg-4.3.1-win64-static\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe";

        /// <summary>
        /// Device List get
        /// </summary>
        /// <returns></returns>
        public List<Device> GetDeviceList()
        {
            List<Device> rtnList = new List<Device>();
            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
            process.StartInfo.FileName = _FFMPEGPath;
            //process.StartInfo.Arguments = @"-y -rtbufsize 100M -f gdigrab -framerate 30 -probesize 10M -draw_mouse 1 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p ""output.avi""";
            process.StartInfo.Arguments = @"ffmpeg -list_devices true -f dshow -i dummy";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            string processOutput = null;
            string device = string.Empty;
            while ((processOutput = process.StandardError.ReadLine()) != null)
            {
                if (processOutput.IndexOf("DirectShow video devices") > 0) device = "video";
                if (processOutput.IndexOf("DirectShow audio devices") > 0) device = "audio";
                if(device.Trim().Length > 0 && (processOutput.IndexOf("\"") > 0) && (processOutput.IndexOf("Alternative name") < 0))
                {
                    Device dv = new Device();
                    dv.device = device;
                    dv.name = processOutput.Substring(processOutput.IndexOf("\"") + 1, processOutput.Length - processOutput.IndexOf("\"") - 2);
                    rtnList.Add(dv);
                }
                Debug.WriteLine(processOutput);
            }
            process.WaitForExit();
            process.Close();
            return rtnList;
        }

        /// <summary>
        /// 사용자 화면 부분 녹화
        /// </summary>
        /// <param name="desktopPoint"></param>
        /// <param name="videoPoint"></param>
        /// <returns></returns>
        public int PartialRecord(Point desktopPoint, Point videoPoint, Process process , string mic, string monitor, string cam)
        {
            string offset = string.Empty;
            try
            {
                Point point = desktopPoint;
                point.X += videoPoint.X;
                point.Y += videoPoint.Y;
                ProcessStartInfo cmd = new ProcessStartInfo();
                ProcessStartInfo cmd_Cam = new ProcessStartInfo();

                string argument = string.Empty;
                string argument2 = string.Empty;
                if (monitor.Trim().ToUpper() != "FULL") offset = "-offset_x " + point.X.ToString() + " -offset_y " + point.Y.ToString();
                if (mic.Trim().Length == 0)
                {
                    argument = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -video_size 1280x720 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"output03_" + System.DateTime.Now.ToString("HHmmss") + ".mp4\"";
                }
                else
                {
                    argument = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -video_size 1280x720 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"output03_" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".mp4\" -f dshow -i audio=\"" + mic + "\"";
                }
                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.WorkingDirectory = @"C:\Users\pc\";

                if (cam.Trim().Length > 0)
                {
                    argument2 = "ffmpeg -y -f dshow -i video=\"" + cam + "\" -rtbufsize 100M -framerate 45 -video_size 298x144  -c:v libx264 -r 45 -preset ultrafast -tune zerolatency -crf 28 -pix_fmt yuv420p -c:a aac -strict -2 -ac 2 -b:a 128k \"cam.mp4\"";
                    cmd_Cam.FileName = "cmd";
                    cmd_Cam.RedirectStandardInput = true;
                    cmd_Cam.RedirectStandardOutput = true;
                    cmd_Cam.UseShellExecute = false;
                    cmd_Cam.CreateNoWindow = true;
                    cmd_Cam.WorkingDirectory = @"C:\Users\pc\";
                    processCam.StartInfo = cmd_Cam;
                    processCam.Start();
                    SW = processCam.StandardInput;
                    processCam.StandardInput.WriteLine(argument2);
                    //SW.WriteLine(argument2);
                }

                process.StartInfo = cmd; 
                process.Start(); 
                Thread.Sleep(200);
                process.StandardInput.WriteLine(argument);
                AudioRecorderProcess_ID = process.Id;
                CamRecorderProcess_ID = processCam.Id;
                
                //process.BeginErrorReadLine();

                //string processOutput = null;
                //while ((processOutput = process.StandardError.ReadLine()) != null)
                //{
                //    Debug.WriteLine(processOutput);
                //}

                //process.StandardInput.Close();
            }
            catch(Exception e)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// 사용자 화면 녹화 종료
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public int StopRecord(Process process)
        {
            if (AudioRecorderProcess_ID != 0)
            {
                //SetConsoleCtrlHandler(null, true);
                //GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);
                //FreeConsole();
                process.StandardInput.WriteLine("q");
                while (!process.WaitForExit(1000))
                {
                    Thread.Sleep(1000);
                }

                process.Dispose();
                AudioRecorderProcess_ID = 0;
            }
            if (CamRecorderProcess_ID != 0)
            {
                processCam.StandardInput.WriteLine("q");
                while (!processCam.WaitForExit(1000))
                {
                    Thread.Sleep(1000);
                }

                processCam.Dispose();
                CamRecorderProcess_ID = 0;
            }
            return 0;
        }

        /// <summary>
        /// 동영상 이어붙이기
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public int Merge(Process process)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            string argument = string.Empty;
            try
            {
                StreamWriter writer;
                writer = File.CreateText(@"C:\Users\pc\mergeVideo.txt");
                writer.WriteLine("file front.mp4");
                writer.WriteLine("file back.mp4");
                writer.Close();

                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.WorkingDirectory = @"C:\Users\pc\";

                argument = @"ffmpeg -y -i title_01_10초_minecraft.mp4 -acodec aac -vcodec libx264 -s hd720  -r 60 -qscale 0.1 -strict experimental front.mp4 && ffmpeg -y -i output03_201028214922.mp4 -acodec aac -vcodec libx264 -s hd720  -r 60 -qscale 0.1 -strict experimental back.mp4 &&  ffmpeg -y -f concat -i mergeVideo.txt -c copy output_merge.mp4 && exit";

                process.StartInfo = cmd;
                process.Start();
                Thread.Sleep(200);
                SR = process.StandardOutput;
                SW = process.StandardInput;
                SW.WriteLine(argument);
                while(!process.WaitForExit(100))
                {
                    Thread.Sleep(1000);
                }
                
                process.Dispose();
            }
            catch(Exception e)
            {
                return -1;
            }
            return 1;
        }
        public int OverLay()
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();
            string argument = string.Empty;
            try
            {

                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.WorkingDirectory = @"C:\Users\pc\";

                argument = "ffmpeg -y -i output_merge.mp4 -i 캠영상_test_1분_width240.mp4 -filter_complex \"[1:v]setpts = PTS + 10 / TB[a];[0:v][a]overlay = (W - w):(H - h):enable = gte(t\\, 10):eof_action = pass,format = yuv420p[out]\" -map \"[out]\" -map 0:a? -c:v libx264 -crf 18 -c:a copy output_overlay.mp4 && exit";

                process.StartInfo = cmd;
                process.Start();
                Thread.Sleep(200);
                SR = process.StandardOutput;
                SW = process.StandardInput;
                SW.WriteLine(argument);
                while (!process.WaitForExit(1000))
                {
                    Thread.Sleep(1000);
                }

                process.Dispose();
            }
            catch (Exception e)
            {
                return -1;
            }
            return 1;
        }
    }
    public class Device
    {
        public string device;
        public string name;
    }

    
}
