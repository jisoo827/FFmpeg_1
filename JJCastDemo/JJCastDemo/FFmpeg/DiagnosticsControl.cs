using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace JJCastDemo.FFmpeg
{
    public class DiagnosticsControl
    {
        string _FFMPEGPath = @"C:\Users\jisu827\Downloads\ffmpeg-4.3.1-win64-static\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe";
        
        /// <summary>
        /// 윈도우 부분 녹화
        /// </summary>
        /// <param name="desktopPoint"></param>
        /// <param name="videoPoint"></param>
        /// <returns></returns>
        public int RecordPartial(Point desktopPoint, Point videoPoint)
        {
            try
            {
                Point point = desktopPoint;
                point.X += videoPoint.X;
                point.Y += videoPoint.Y;
                Process process = new Process();
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = _FFMPEGPath;
                //process.StartInfo.Arguments = @"-y -rtbufsize 100M -f gdigrab -framerate 30 -probesize 10M -draw_mouse 1 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p ""output.avi""";
                process.StartInfo.Arguments = @"-f gdigrab -framerate 30 -offset_x " + point.X.ToString() + " -offset_y " + point.Y.ToString() + " -video_size 640x480 -show_region 1 -i desktop output1.mp4";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
            }
            catch(Exception e)
            {
                return -1;
            }
            return 1;
        }
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
    }
    public class Device
    {
        public string device;
        public string name;
    }

    
}
