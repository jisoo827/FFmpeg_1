using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FFmpeg.AutoGen;
using System.Drawing;
using System.IO;
using JJCastDemo.FFmpeg;
using JJCastDemo.FFmpeg.Decoder;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

namespace JJCastDemo
{
    public enum VTYPE
    {
        RTSP_RTP = 0,
        CAM = 1,
        MP4 = 2
    }
    public partial class MainForm : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        int Key_Q = 81;
        Thread thread;
        ThreadStart ts;
        string _FFMPEGPath = SystemInformation.ComputerName == "DESKTOP-2OGUI9T" ? @"E:\_Works\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe" : @"C:\Users\jisu827\Downloads\ffmpeg-4.3.1-win64-static\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe";

        private bool activeThread;      //thread 활성화 유무
        bool isRecord = false;
        DiagnosticsControl dControl = new DiagnosticsControl();
        Process process = new Process();

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            VideoStream vs = new VideoStream();
            //vs.AVFormatTest();
            //vs.AVFormatTest2();
            //vs.AVFormatTest3();
            //vs.AVFormatTest4();

            //Application.Exit();
            FFmpegBinariesHelper.RegisterFFmpegBinaries();
            Wmp_1.uiMode = "none";
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.stop();
            List<Device> devicelist = dControl.GetDeviceList();
            foreach(Device dv in devicelist)
            {
                if (dv.device == "audio") Cmb_Mic.Items.Add(dv.name);
                else if (dv.device == "video") Cmb_Cam.Items.Add(dv.name);
            }


        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (Cmb_VType.SelectedIndex == 1 && thread.IsAlive)
            //{
            //    activeThread = false;
            //    thread.Join();
            //}
            //easyFFmpeg.DisposeFFmpeg();

            Application.Exit();
        }

        private void Btn_Record_Click(object sender, EventArgs e)
        {
            dControl.PartialRecord(this.DesktopLocation, Wmp_1.Location, process, Cmb_Mic.Text.Trim(), Cmb_Monitor.Text.Trim(), Cmb_Cam.Text.Trim());
        }

        private void Btn_Play_Click(object sender, EventArgs e)
        {
            int type = Cmb_Mic.SelectedIndex;

            if (type == 0)
            {
                //비디오 프레임 디코딩 thread 생성
                ts = new ThreadStart(DecodeAllFramesToImages);
                thread = new Thread(ts);
                activeThread = true;
                if (thread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    thread.Start();
                }
            }
            else
            {
                VideoStream vs = new VideoStream();
                int[] video_size = vs.AVFormatTest3(Txt_URL.Text);
                this.Wmp_1.Size = new System.Drawing.Size(video_size[0], video_size[1]);
                Wmp_1.URL = Txt_URL.Text;
                Wmp_1.Ctlcontrols.play();
            }
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Wmp_1.Ctlcontrols.stop();
        }
        
        private void Btn_RecordStop_Click(object sender, EventArgs e)
        {
            dControl.StopRecord(process);
            try
            {
                process.Kill();
            }
            catch(System.InvalidOperationException ee)
            {
                return;
            }
            this.Refresh();
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            if(dControl.Merge(process) == 1) Txt_URL.Text = Application.StartupPath + "\\output_merge.mp4";
        }

        private void Btn_MergePlay_Click(object sender, EventArgs e)
        {
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.play();
            bool test = false;

            if (test) dControl.OverLay();
        }

        #region WebCam Method
        private unsafe void DecodeAllFramesToImages()
        {
            //video="웹캠 디바이스 이름"
            string device = "video=ABKO APC930 QHD WEBCAM";
            using (var vsd = new VideoStreamDecoderCam(device))
            {
                //Console.WriteLine($"codec name: {vsd.CodecName}");

                var info = vsd.GetContextInfo();
                info.ToList().ForEach(x => Console.WriteLine($"{x.Key} = {x.Value}"));

                var sourceSize = vsd.FrameSize;
                var sourcePixelFormat = vsd.PixelFormat;
                var destinationSize = sourceSize;
                var destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;
                using (var vfc = new VideoFrameConverterCam(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat))
                {
                    var frameNumber = 0;
                    while (vsd.TryDecodeNextFrame(out var frame) && activeThread)
                    {
                        var convertedFrame = vfc.Convert(frame);

                        Bitmap bitmap;

                        bitmap = new Bitmap(convertedFrame.width, convertedFrame.height, convertedFrame.linesize[0], System.Drawing.Imaging.PixelFormat.Format24bppRgb, (IntPtr)convertedFrame.data[0]);
                        Img_video.Image = bitmap;

                        frameNumber++;
                    }
                }
            }
        }

        #endregion
    }
}