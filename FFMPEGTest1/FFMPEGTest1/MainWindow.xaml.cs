using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FFmpeg.AutoGen;
using System.Drawing;
using System.IO;
using FFMPEGTest1.FFmpeg;
using FFMPEGTest1.FFmpeg.Decoder;

namespace FFMPEGTest1
{
    public enum VTYPE
    {
        RTSP_RTP = 0,
        CAM
    }
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        Thread thread;
        ThreadStart ts;
        Dispatcher dispatcher = Application.Current.Dispatcher;

        private bool activeThread;      //thread 활성화 유무

        EasyFFmpegManager easyFFmpeg;

        public MainWindow()
        {
            InitializeComponent();

            VideoStream vs = new VideoStream();
            //vs.AVFormatTest();
            //vs.AVFormatTest2();
            //vs.AVFormatTest3();
            vs.AVFormatTest4();
            Application.Current.Shutdown();
            //FFmpeg dll 파일 참조 경로 설정
            FFmpegBinariesHelper.RegisterFFmpegBinaries();
            easyFFmpeg = new EasyFFmpegManager();
        }

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
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
        }
        private void VideoFrameReceived(BitmapImage frame)
        {
            dispatcher.BeginInvoke((Action)(() =>
            {
                Img_video.Source = frame;
            }));
        }

        private void Btn_Record_Checked(object sender, RoutedEventArgs e)
        {
            string fileName = DateTime.Now.ToString("yyMMdd_hh.mm.ss") + ".mp4";
            easyFFmpeg.RecordVideo(fileName);
        }

        private void Btn_Record_Unchecked(object sender, RoutedEventArgs e)
        {
            easyFFmpeg.StopRecord();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Cmb_VType.SelectedIndex == 1 && thread.IsAlive)
            {
                activeThread = false;
                thread.Join();
            }
            easyFFmpeg.DisposeFFmpeg();
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (Cmb_VType.SelectedIndex == 1 && thread.IsAlive)
            {
                activeThread = false;
                thread.Join();
            }

            Btn_Record.IsChecked = false;
            easyFFmpeg.DisposeFFmpeg();                   
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

                        Img_video.Source = bitmapimage;     //image 컨트롤에 웹캠 이미지 표시
                    }
                }
            }));

        }

        #endregion
        /*
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (thread.IsAlive)
            {
                activeThread = false;
                thread.Join();
            }
        }
        */
    }
}
