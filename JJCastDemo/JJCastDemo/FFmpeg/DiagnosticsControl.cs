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

namespace JJCastDemo.FFmpeg
{
    public class DiagnosticsControl
    {
        private static string _FFMPEGPath = string.Empty;

        System.IO.StreamReader SR;
        System.IO.StreamWriter SW;
        public int AudioRecorderProcess_ID = 0;
        public int CamRecorderProcess_ID = 0;
        Process processDesk = null;
        Process processCam = null;
        FFmpegStatement ffmpegStatement = null;

        public DiagnosticsControl()
        {
            processDesk = new Process();
            processCam = new Process();
            ffmpegStatement = new FFmpegStatement();
        }

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
            process.StartInfo.Arguments = ffmpegStatement.GetDeviceLisStmt();

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

        public int PartialRecord(Point desktopPoint, Point videoPoint,  string mic, string monitor, string cam)
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
                    argument = ffmpegStatement.DesktopPartialRecordStmt(offset);
                }
                else
                {
                    argument = ffmpegStatement.DesktopPartialRecordStmt(offset,mic);
                }
                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.WorkingDirectory = Application.StartupPath;

                if (cam.Trim().Length > 0)
                {
                    //argument2 = ffmpegStatement.CamRecordStmt(cam);
                    argument2 = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 -offset_x 255 -offset_y 305 -video_size 1280x720 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"cam.mp4\"";
                    cmd_Cam.FileName = "cmd";
                    cmd_Cam.RedirectStandardInput = true;
                    cmd_Cam.RedirectStandardOutput = true;
                    cmd_Cam.UseShellExecute = false;
                    cmd_Cam.CreateNoWindow = true;
                    cmd_Cam.WorkingDirectory = Application.StartupPath;
                    processCam.StartInfo = cmd_Cam;
                    processCam.Start();
                    SW = processCam.StandardInput;
                    processCam.StandardInput.WriteLine(argument2);
                    //SW.WriteLine(argument2);
                }

                processDesk.StartInfo = cmd;
                processDesk.Start(); 
                Thread.Sleep(200);
                processDesk.StandardInput.WriteLine(argument);
                AudioRecorderProcess_ID = processDesk.Id;
                CamRecorderProcess_ID = cam.Trim().Length > 0 ? processCam.Id : 0;
                
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

        public int ConcatVideo()
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            string argument = string.Empty;
            Process process = new Process();
            try
            {
                StreamWriter writer;
                writer = File.CreateText(Application.StartupPath + "\\concatVideo.txt");
                writer.WriteLine("file front.mp4");
                writer.WriteLine("file back.mp4");
                writer.Close();

                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.WorkingDirectory = Application.StartupPath;

                argument = ffmpegStatement.ConcatVideoStmt("title_01_minecraft.mp4", "output_overLay.mp4");

                process.StartInfo = cmd;
                process.Start();
                Thread.Sleep(200);
                SR = process.StandardOutput;
                SW = process.StandardInput;
                SW.WriteLine(argument);

                int cnt = 0;
                while(!process.WaitForExit(100))
                {
                    Thread.Sleep(1000);
                    if(cnt++ == 60) SW.WriteLine("exit");
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
                cmd.WorkingDirectory = Application.StartupPath;

                argument = ffmpegStatement.OverlayVideoStmt();

                process.StartInfo = cmd;
                process.Start();
                Thread.Sleep(200);
                SR = process.StandardOutput;
                SW = process.StandardInput;
                SW.WriteLine(argument);

                int cnt = 0;
                while (!process.WaitForExit(100))
                {
                    Thread.Sleep(1000);
                    if (cnt++ == 60) SW.WriteLine("exit");
                }

                process.Dispose();
            }
            catch (Exception e)
            {
                return -1;
            }
            return 1;            
        }

        public int StopRecord()
        {
            if (AudioRecorderProcess_ID != 0)
            {
                //SetConsoleCtrlHandler(null, true);
                //GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);
                //FreeConsole();
                processDesk.StandardInput.WriteLine("q");
                processDesk.WaitForExit(1000);
                Thread.Sleep(1000);
                processDesk.StandardInput.WriteLine("exit");
                //process.WaitForExit(1000);
                while (!processDesk.WaitForExit(100))
                {
                    Thread.Sleep(1000);
                }
                processDesk.Dispose();
                AudioRecorderProcess_ID = 0;
            }
            if (CamRecorderProcess_ID != 0)
            {
                processCam.StandardInput.WriteLine("q");
                processCam.WaitForExit(1000);
                Thread.Sleep(1000);
                processCam.StandardInput.WriteLine("exit");
                //processCam.WaitForExit(1000);
                while (!processCam.WaitForExit(100))
                {
                    Thread.Sleep(1000);
                }
                processCam.Dispose();
                CamRecorderProcess_ID = 0;
            }
            return 0;
        }

        private static int CommandExcute(string argument)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();
            try
            {
                cmd.FileName = "cmd";
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.StandardOutputEncoding = System.Text.Encoding.UTF8;
                cmd.StandardErrorEncoding = System.Text.Encoding.UTF8;
                cmd.WorkingDirectory = Application.StartupPath;

                process.StartInfo = cmd;
                process.Start();
                Thread.Sleep(200);
                process.StandardInput.WriteLine(argument);
                int cnt = 0;
                while (!process.WaitForExit(100))
                {
                    Thread.Sleep(1000);
                    if (cnt++ == 60) process.StandardInput.WriteLine("exit");
                }

                process.Dispose();
            }
            catch (Exception e)
            {
                return -1;
            }
            return 1;
        }

        private static int CommandExcute(string argument, Process process)
        {
            return 1;
        }

        public static string FFMPEGPath
        {
            get { return _FFMPEGPath; }
            set { _FFMPEGPath = value; }
        }
    }
    public class Device
    {
        public string device;
        public string name;
    }

    
}
