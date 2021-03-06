using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using FFmpeg.AutoGen;
using System.Drawing;
using System.IO;
using FFMPEGTest1.FFmpeg;
using FFMPEGTest1.FFmpeg.Decoder;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.ComponentModel;


namespace FFMPEGTest1
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
        Dispatcher dispatcher = System.Windows.Application.Current.Dispatcher;
        string _FFMPEGPath = @"C:\Users\jisu827\Downloads\ffmpeg-4.3.1-win64-static\ffmpeg-4.3.1-win64-static\bin\ffmpeg.exe";

        private bool activeThread;      //thread 활성화 유무

        bool isRecord = false;

        EasyFFmpegManager easyFFmpeg;
        public MainForm()
        {
            InitializeComponent();

            VideoStream vs = new VideoStream();
            //vs.AVFormatTest();
            //vs.AVFormatTest2();
            //vs.AVFormatTest3();
            //vs.AVFormatTest4();

            //Application.Exit();
            FFmpegBinariesHelper.RegisterFFmpegBinaries();
            easyFFmpeg = new EasyFFmpegManager();
            Wmp_1.uiMode = "none";
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.stop();

        }

        private void Btn_Record_Click(object sender, EventArgs e)
        {

            Point point = this.DesktopLocation;
            point.X += Wmp_1.Location.X;
            point.Y += Wmp_1.Location.Y;
            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = _FFMPEGPath;
            //process.StartInfo.Arguments = @"-y -rtbufsize 100M -f gdigrab -framerate 30 -probesize 10M -draw_mouse 1 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p ""output.avi""";
            process.StartInfo.Arguments = @"-f gdigrab -framerate 30 -offset_x " + point.X.ToString() + " -offset_y " + point.Y.ToString() + " -video_size 640x480 -show_region 1 -i desktop output1.avi";
            
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            #region old
            //if (!isRecord)
            //{
            //    string fileName = DateTime.Now.ToString("yyMMdd_hh.mm.ss") + ".mp4";
            //    easyFFmpeg.RecordVideo(fileName);

            //    isRecord = true;
            //}
            //else
            //{
            //    easyFFmpeg.StopRecord();
            //    isRecord = false;
            //} 
            #endregion
        }

        /*private void Btn_Play_Click(object sender, EventArgs e)
        {
            ////thread 시작 
            //if (thread.ThreadState == ThreadState.Unstarted)
            //{
            //    thread.Start();
            //}
            string url = Txt_URL.Text;
            int type = Cmb_VType.SelectedIndex;
            if (type == 1)
            {
                //비디오 프레임 디코딩 thread 생성
                ts = new ThreadStart(DecodeAllFramesToImages);
                thread = new Thread(ts);
                activeThread = true;
                if (thread.ThreadState == ThreadState.Unstarted)
                {
                    thread.Start();
                }
            }
            else
            {
                easyFFmpeg.InitializeFFmpeg(url, (VIDEO_INPUT_TYPE)type);

                easyFFmpeg.PlayVideo();
                easyFFmpeg.VideoFrameReceived += VideoFrameReceived;
            }
        }*/

        private void Btn_Play_Click(object sender, EventArgs e)
        {
            VideoStream vs = new VideoStream();
            int[] video_size = vs.AVFormatTest3(Txt_URL.Text);
            this.Wmp_1.Size = new System.Drawing.Size(video_size[0], video_size[1]);
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.play();
        }

        /*private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (Cmb_VType.SelectedIndex == 1 && thread.IsAlive)
            {
                activeThread = false;
                thread.Join();
            }

            if(isRecord) Btn_Record_Click(null, null);
            easyFFmpeg.DisposeFFmpeg();
        }
        */
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Wmp_1.Ctlcontrols.stop();
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

        private void VideoFrameReceived(BitmapImage frame)
        {
            dispatcher.BeginInvoke((Action)(() =>
            {
                Img_video.Image = BitmapImage2Bitmap(frame);
            }));
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
                        BitmapToImageSource(bitmap);

                        frameNumber++;
                    }
                }
            }
        }
        void BitmapToImageSource(Bitmap bitmap)
        {
            //UI thread에 접근하기 위해 dispatcher 사용
            dispatcher.BeginInvoke((Action)(() =>
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    if (thread.IsAlive)
                    {
                        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                        memory.Position = 0;
                        BitmapImage bitmapimage = new BitmapImage();
                        bitmapimage.BeginInit();
                        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                        //bitmapimage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        bitmapimage.StreamSource = memory;
                        bitmapimage.EndInit();

                        Img_video.Image = BitmapImage2Bitmap(bitmapimage);     //image 컨트롤에 웹캠 이미지 표시
                    }
                }
            }));

        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        #endregion

        private void btn_RecordStop_Click(object sender, EventArgs e)
        {
            string process = "ffmpeg";
            Process p = Process.GetProcessesByName(process).FirstOrDefault();

            if (p != null)
            {
                IntPtr hWnd = p.MainWindowHandle;
                PostMessage(hWnd, 0x100, Key_Q, 0);

                try
                {
                    p.Kill();
                }
                catch (InvalidOperationException) { }
                catch (Win32Exception) { }
            }
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            
            StreamWriter writer;
            writer = File.CreateText(Application.StartupPath + "\\mergeVideo.txt");        
            writer.WriteLine("file output2.avi");
            writer.WriteLine("file output1.avi");
            writer.Close();

            string filename = Path.GetTempFileName();

            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = _FFMPEGPath;
            process.StartInfo.Arguments = @"-f concat -i mergeVideo.txt -c copy output_merge.avi";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            Txt_URL.Text = Application.StartupPath + "\\output_merge.avi";
        }
    }
}
