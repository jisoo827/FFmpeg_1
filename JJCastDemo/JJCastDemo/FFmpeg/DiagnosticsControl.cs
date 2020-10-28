using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

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

        string _FFMPEGPath = @"C:\Users\jisu827\Downloads\ffmpeg-4.3.1-win64-static\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe";
        
        /// <summary>
        /// Device List get
        /// </summary>
        /// <returns></returns>
        public List<Device> GetDeviceList(Process process)
        {
            List<Device> rtnList = new List<Device>();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
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
                if(device.Trim().Length > 0 && (processOutput.IndexOf("\"") > 0))
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
        public int PartialRecord(Point desktopPoint, Point videoPoint, Process process)
        {
            try
            {
                Point point = desktopPoint;
                point.X += videoPoint.X;
                point.Y += videoPoint.Y;
                ProcessStartInfo cmd = new ProcessStartInfo();
                
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = _FFMPEGPath;
                //process.StartInfo.Arguments = @"-y -rtbufsize 100M -f gdigrab -framerate 30 -probesize 10M -draw_mouse 1 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p ""output.avi""";
                process.StartInfo.Arguments = "-y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 -offset_x " + point.X.ToString() + " -offset_y " + point.Y.ToString() + " -video_size 720x404 -show_region 1 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"output03.mp4\"";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

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
            try
            {
                if (AttachConsole((uint)process.Id))
                {
                    // Disable Ctrl-C handling for our program
                    SetConsoleCtrlHandler(null, true);
                    GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);

                    //Moved this command up on suggestion from Timothy Jannace (see comments below)
                    FreeConsole();

                    // Must wait here. If we don't and re-enable Ctrl-C
                    // handling below too fast, we might terminate ourselves.
                    process.WaitForExit(2000);

                    //Re-enable Ctrl-C handling or any subsequently started
                    //programs will inherit the disabled state.
                    SetConsoleCtrlHandler(null, false);
                }
            }
            catch(Exception e)
            {
                return -1;
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
            try
            {
                StreamWriter writer;
                writer = File.CreateText("mergeVideo.txt");
                writer.WriteLine("file title_01_minecraft.mp4");
                writer.WriteLine("file All of Me (Jon Schmidt) - The Piano Guys.mp4");
                writer.Close();

                string filename = Path.GetTempFileName();

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = _FFMPEGPath;
                process.StartInfo.Arguments = @"-f concat -i mergeVideo.txt -c copy output_merge.mp4";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                process.Close();
            }
            catch(Exception e)
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
