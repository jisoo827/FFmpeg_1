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

        Thread threadCam;
        ThreadStart threadStartCam;
        Thread threadDesktop;
        ThreadStart threadStartDesktop;
        Device monitor;

        private static bool activeThread = false;      //thread 활성화 유무
        DiagnosticsControl dControl = null;
        private static string rgbHex = string.Empty;
        private static byte colorR = 0;
        private static byte colorG = 0;
        private static byte colorB = 0;
        private static bool isCapturingMoves = false;
        private static Point startPoint = new System.Drawing.Point();


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

            VideoStream vs = new VideoStream();
            _ = vs.AVFormatTest3(Txt_URL.Text);
            isCapturingMoves = false;

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Btn_Record_Click(object sender, EventArgs e)
        {
            dControl.PartialRecord(Cmb_Mic.Text.Trim(), (Device)Cmb_Monitor.SelectedItem, Cmb_Cam.Text.Trim());
        }

        private void Btn_RecordStop_Click(object sender, EventArgs e)
        {
            dControl.StopRecord();
            this.Refresh();
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            string rdbCheck = Rdb_RightOut.Checked ? "RIGHTOUT" : Rdb_RightIn.Checked ? "RIGHTIN" : Rdb_RightBottomIn.Checked ? "RIGHTBOTTOMIN" : 
                Rdb_RightBottomOut.Checked ? "RIGHTBOTTOMOUT" : Rdb_DiagonalOut.Checked ? "DIAGONALOUT" : "DIAGONALIN";
            dControl.OverLay(rgbHex, rdbCheck, ((Device)Cmb_Monitor.SelectedItem).size);
            if (dControl.ConcatVideo() == 1) Txt_URL.Text = Application.StartupPath + "\\result.mp4";
        }

        private void Btn_MergePlay_Click(object sender, EventArgs e)
        {
            Img_DeskTop.Visible = false;
            Wmp_1.Visible = true;
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.play();
        }

        private void Btn_Play_Click(object sender, EventArgs e)
        {
            Img_DeskTop.Visible = true;
            monitor = (Device)Cmb_Monitor.SelectedItem;
            //비디오 프레임 디코딩 thread 생성
            if (!activeThread)
            {
                if (Cmb_Cam.Text.Trim().Length > 0)
                { 
                    threadStartCam = new ThreadStart(DecodeAllFramesToImages);
                    threadCam = new Thread(threadStartCam);
                    if (threadCam.ThreadState == System.Threading.ThreadState.Unstarted)
                    {
                        threadCam.Start();
                    }
                }
                threadStartDesktop = new ThreadStart(ScreenCapture);
                threadDesktop = new Thread(threadStartDesktop);
                if (threadDesktop.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    threadDesktop.Start();
                }
                activeThread = true;
            }
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Wmp_1.Ctlcontrols.stop();
            Wmp_1.Visible = false;

            if (activeThread)
            {
                if (threadCam != null)
                {
                    threadCam.Interrupt();
                    threadCam.Abort();
                }
                if (threadDesktop!= null)
                {
                    threadDesktop.Interrupt();
                    threadDesktop.Abort();
                }
                activeThread = false;
            }
            Img_video.Image = null;
            Img_DeskTop.Image = null;


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
            List<Device> monitorlist = new List<Device>();
            foreach (Device dv in devicelist)
            {
                if (dv.device == "audio") Cmb_Mic.Items.Add(dv.name);
                else if (dv.device == "video") Cmb_Cam.Items.Add(dv.name);
            }
            if(Cmb_Cam.Items.Count > 0) Cmb_Cam.SelectedIndex = 0;
            if (Cmb_Mic.Items.Count > 0) Cmb_Mic.SelectedIndex = 0;
            Screen[] screenlist = Screen.AllScreens;
            foreach(Screen screen in screenlist)
            {
                monitorlist.Add(new Device { device = "monitor", name = screen.DeviceName.Substring(screen.DeviceName.IndexOf(".\\") + 2), size = screen.Bounds.Size, point = screen.Bounds.Location });
            }
            
            Cmb_Monitor.DataSource = new BindingSource(monitorlist,null);
            Cmb_Monitor.DisplayMember = "name";
        }

        #region RecordTest Method
        private unsafe void DecodeAllFramesToImages()
        {
            try
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
                            //362, 235
                            bitmap = new Bitmap(convertedFrame.width, convertedFrame.height, convertedFrame.linesize[0], System.Drawing.Imaging.PixelFormat.Format24bppRgb, (IntPtr)convertedFrame.data[0]);
                            int width = Img_video.Width; int height = Img_video.Height;
                            Size resize = new Size(width, height);
                            Bitmap resizeImage = new Bitmap(bitmap, resize);
                            //bitmap = new Bitmap(362, 235, convertedFrame.linesize[0], System.Drawing.Imaging.PixelFormat.Format24bppRgb, (IntPtr)convertedFrame.data[0]);
                            bitmap.Dispose();
                            resizeImage = RemoveBackground(resizeImage);
                        
                            Img_video.Image = resizeImage;
                            //bitmap.Dispose();

                            frameNumber++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                activeThread = false;
                threadCam.Interrupt();
                threadCam.Abort();
                threadDesktop.Interrupt();
                threadDesktop.Abort();
                Img_video.Image = null;
                Img_DeskTop.Image = null;
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

        private void ScreenCapture()
        {
            try
            {
                while (activeThread)
                {                    
                    Bitmap bitmap = new Bitmap(monitor.size.Width, monitor.size.Height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.CopyFromScreen(monitor.point.X, monitor.point.Y, 0, 0, monitor.size);

                    Img_DeskTop.Image = bitmap;
                    graphics.Dispose();
                }
            }
            catch(Exception)
            {
                activeThread = false;
                if (threadCam != null)
                {
                    threadCam.Interrupt();
                    threadCam.Abort();
                }
                threadDesktop.Interrupt();
                threadDesktop.Abort();
                Img_video.Image = null;
                Img_DeskTop.Image = null;
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

        private void Img_video_MouseDown(object sender, MouseEventArgs e)
        {
            return;
            Btn_MergePlay.Visible = true;
            if (e.Button != MouseButtons.Left) return;
           
            isCapturingMoves = true;
            startPoint.X = e.X;
            startPoint.Y = e.Y;
            return;
        }

        private void Img_video_MouseMove(object sender, MouseEventArgs e)
        {
            return;
            if (!isCapturingMoves) return;
            Img_video.Location = new System.Drawing.Point(e.X - startPoint.X + Img_video.Location.X, Img_video.Location.Y - startPoint.Y + e.Y);
        }

        private void Img_video_MouseUp(object sender, MouseEventArgs e)
        {
            return;
            isCapturingMoves = false;
        }
    }
}