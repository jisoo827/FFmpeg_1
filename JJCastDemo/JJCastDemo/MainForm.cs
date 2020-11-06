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
using System.Drawing.Imaging;

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

        private bool activeThread;      //thread 활성화 유무
        bool isRecord = false;
        DiagnosticsControl dControl = null;
        private static string rgbHex = string.Empty;
        private static byte colorR = 0;
        private static byte colorG = 0;
        private static byte colorB = 0;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dControl = new DiagnosticsControl();

            SetFfmpegPath();
            InitControl();

            FFmpegBinariesHelper.RegisterFFmpegBinaries();

            Wmp_1.uiMode = "none";
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.stop();

            //VideoStream vs = new VideoStream();
            //int[] video_size = vs.AVFormatTest3(Txt_URL.Text);

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Btn_Record_Click(object sender, EventArgs e)
        {
            dControl.PartialRecord(this.DesktopLocation, Wmp_1.Location, Cmb_Mic.Text.Trim(), Cmb_Monitor.Text.Trim(), Cmb_Cam.Text.Trim());
        }

        private void Btn_RecordStop_Click(object sender, EventArgs e)
        {
            dControl.StopRecord();
            this.Refresh();
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            dControl.OverLay(rgbHex);
            if (dControl.ConcatVideo() == 1) Txt_URL.Text = Application.StartupPath + "\\result.mp4";
        }

        private void Btn_MergePlay_Click(object sender, EventArgs e)
        {
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.play();
        }

        private void Btn_Play_Click(object sender, EventArgs e)
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

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Wmp_1.Ctlcontrols.stop();
        }

        private void SetFfmpegPath()
        {
            var envPathArr = Array.ConvertAll(System.Environment.GetEnvironmentVariable("PATH").Split(';'), Convert.ToString);
            foreach (string envPath in envPathArr)
            {
                if (envPath.ToUpper().IndexOf("FFMPEG") > 0) DiagnosticsControl.FFMPEGPath = envPath + @"\ffmpeg.exe";
            }
        }

        private void InitControl()
        {
            List<Device> devicelist = dControl.GetDeviceList();
            foreach (Device dv in devicelist)
            {
                if (dv.device == "audio") Cmb_Mic.Items.Add(dv.name);
                else if (dv.device == "video") Cmb_Cam.Items.Add(dv.name);
            }
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
                        bitmap = RemoveBackground(bitmap);
                        Img_video.Image = bitmap;

                        frameNumber++;
                    }
                }
            }
        }

        private Bitmap RemoveBackground(Bitmap input)
        {
            Bitmap clone = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
            {
                using (input)
                using (Graphics gr = Graphics.FromImage(clone))
                {
                    gr.DrawImage(input, new Rectangle(0, 0, clone.Width, clone.Height));
                }

                var data = clone.LockBits(new Rectangle(0, 0, clone.Width, clone.Height), ImageLockMode.ReadWrite, clone.PixelFormat);

                var bytes = Math.Abs(data.Stride) * clone.Height;
                byte[] rgba = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, rgba, 0, bytes);

                var pixels = Enumerable.Range(0, rgba.Length / 4).Select(x => new {
                    B = rgba[x * 4],
                    G = rgba[(x * 4) + 1],
                    R = rgba[(x * 4) + 2],
                    A = rgba[(x * 4) + 3],
                    MakeTransparent = new Action(() => rgba[(x * 4) + 3] = 0)
                });

                pixels
                    .AsParallel()
                    .ForAll(p =>
                    {
                        if ((p.R <= colorR + 40 && p.R >= colorR - 40) && (p.G <= colorG + 40 && p.G >= colorG - 40) && (p.B <= colorB + 40 && p.B >= colorB - 40))
                        {
                            p.MakeTransparent();
                        }
                    });

                System.Runtime.InteropServices.Marshal.Copy(rgba, 0, data.Scan0, bytes);
                clone.UnlockBits(data);
                input.Dispose();
                return clone;
            }
        }
        #endregion

        private void Btn_Chromakey_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Btn_Chromakey.BackColor = cd.Color;
                rgbHex = string.Format("{0:X2}{1:X2}{2:X2}", cd.Color.R, cd.Color.G, cd.Color.B);
                colorR = cd.Color.R;
                colorG = cd.Color.G;
                colorB = cd.Color.B;
            }
        }

        private void Btn_Test_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(@"C:\_Works\캡처.png");
            Img_video.Image = RemoveBackground(original);
            //original.Dispose();

        }
    }
}