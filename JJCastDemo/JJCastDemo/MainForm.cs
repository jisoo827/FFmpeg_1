﻿using System;
using System.Linq;
using System.Threading;
using FFmpeg.AutoGen;
using System.Drawing;
using JJCastDemo.FFmpeg;
using JJCastDemo.FFmpeg.Decoder;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing.Imaging;
using JJCastDemo.Common;
using JJCastDemo.Vimeo;


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
        Thread threadDesktop;
        Thread threadTrack;
        Devices monitor;

        private static bool activeThread = false;      //thread 활성화 유무
        private static bool isActiveTrack = false;      //thread 활성화 유무
        DiagnosticsControl dControl = null;
        private static string rgbHex = string.Empty;
        private static byte colorR = 0;
        private static byte colorG = 0;
        private static byte colorB = 0;
        private static bool isCapturingMoves = false;
        private static bool isVideoTowing = false;
        private static bool isVideoTowed = false;
        private static string camName = string.Empty;
        private static Point startPoint = new System.Drawing.Point();
        private static int startValue = 0;
        private static bool isCut = false;
        private static double skipStart = 0;
        private static double skipEnd = 0;
        private static double skipTime = 0;
        private static int cutListIndex = 0;
        private static List<int> cutList = new List<int>();
        private static int playTime = 0;

        public static List<int> CutList { get => cutList; set => cutList = value; }

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
            //Wmp_1.uiMode = "none";
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.stop();

            VideoStream vs = new VideoStream();
            _ = vs.AVFormatTest3(Txt_URL.Text);
            isCapturingMoves = false;
            Txt_URL.Text = @"C:\Users\jisu827\AllOfMe.mp4";

            UploadTests upload = new UploadTests();
            AccountTests accountTests = new AccountTests();
            AuthorizationClientAsyncTests authorization = new AuthorizationClientAsyncTests();
            VideoTests videoTests = new VideoTests();

            //var authorizationTest = authorization.ShouldCorrectlyGetUnauthenticatedToken();
            //authorizationTest = authorization.VerifyAuthenticatedAccess();
            //var test = accountTests.ShouldCorrectlyGetAccountInformation();
            //test = accountTests.ShouldCorrectlyGetUserInformation();
            //test = upload.ShouldCorrectlyGenerateNewUploadTicket();
            //var test = upload.ShouldCorrectlyUploadFileByPath();
            //test = videoTests.ShouldCorrectlyGetUserAlbumVideosByUserId();
            //test = videoTests.ShouldMoveVideoToFolder();

            //test = upload.ShouldCorretlyUploadFileByPullLink();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Btn_Record_Click(object sender, EventArgs e)
        {
            Global.WriteLog("녹화 시작");
            dControl.PartialRecord(Cmb_Mic.Text.Trim(), (Devices)Cmb_Monitor.SelectedItem, Cmb_Cam.Text.Trim());
        }

        private void Btn_RecordStop_Click(object sender, EventArgs e)
        {
            Global.WriteLog("녹화 종료");
            dControl.StopRecord();
            this.Refresh();
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            string rdbCheck = Rdb_RightOut.Checked ? "RIGHTOUT" : Rdb_RightIn.Checked ? "RIGHTIN" : Rdb_RightBottomIn.Checked ? "RIGHTBOTTOMIN" :
                Rdb_RightBottomOut.Checked ? "RIGHTBOTTOMOUT" : Rdb_DiagonalOut.Checked ? "DIAGONALOUT" : "DIAGONALIN";
            Global.WriteLog("오버레이(캠 리사이즈 + 오버레이) 시작");
            dControl.OverLay(rgbHex, rdbCheck, ((Devices)Cmb_Monitor.SelectedItem).Size);
            //Global.WriteLog("인트로 이어붙이기 시작");
            //if (dControl.ConcatVideo("title_01_minecraft.mp4", "cut.mp4","result.mp4") == 1) Txt_URL.Text = Application.StartupPath + "\\result.mp4";
            //Global.WriteLog("합성 종료");
        }

        private void Btn_Cut_Click(object sender, EventArgs e)
        {
            if (Wmp_1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                Wmp_1.Ctlcontrols.pause();
            int timeAdd = 0;
            bool isAdd = true;
            // 2,3 1,3 => 2,3 1,4
            Global.WriteLog("자르기 추가");
            for (int i = 0; i < CutList.Count; i+=2)
            {
                if (TimeSlider.SelectedMin >= CutList[i])
                {
                    timeAdd = CutList[i + 1] - CutList[i];
                    
                }
                else if (TimeSlider.SelectedMin < CutList[i] && TimeSlider.SelectedMax > CutList[i])
                {
                    int timeskip = 0;
                    int cutCnt = 0;
                    for(int j = i; j < cutList.Count; j+=2)
                    {
                        if (TimeSlider.SelectedMin + timeskip <= CutList[j] && TimeSlider.SelectedMax + timeskip >= CutList[j])
                        {
                            timeskip += CutList[j + 1] - CutList[j];
                            cutCnt++;
                        }
                        else
                            break;
                    }
                    while(cutCnt > 1)
                    {
                        CutList.RemoveAt(i + 1 + ((cutCnt - 1) * 2));
                        CutList.RemoveAt(i + ((cutCnt - 1) * 2));
                        cutCnt--;
                    }
                    CutList[i] = TimeSlider.SelectedMin + timeAdd;
                    CutList[i + 1] = TimeSlider.SelectedMax + timeskip + timeAdd;
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {
                CutList.Add(TimeSlider.SelectedMin + timeAdd);
                CutList.Add(TimeSlider.SelectedMax + timeAdd);
            }

            Global.WriteLog("자르기 추가 완료, 타임슬라이더 갱신");

            TimeSlider.Max -= (TimeSlider.SelectedMax - TimeSlider.SelectedMin);
            TimeSlider.SelectedMax = TimeSlider.Max;
            TimeSlider.SelectedMin = 0;
            TimeSlider.Value = TimeSlider.Max / 2;
            isCut = true;

            Image image = TimeSlider.BackgroundImage;
            TimeSlider.BackgroundImage = null;
            if (image != null) image.Dispose();

            Global.FileDeleteForce(Application.StartupPath);

            Global.WriteLog("타임슬라이더용 프레임 추출");
            dControl.ExtractImage(Txt_URL.Text, playTime / 1000, cutList);
            Global.WriteLog("타임슬라이더용 프레임 추출 완료");

            TimeSlider.BackgroundImage = new Bitmap(Application.StartupPath + @"\output.jpg");
            TimeSlider.BackgroundImageLayout = ImageLayout.Stretch;
            Global.WriteLog("타임슬라이더 갱신 완료");
        }

        private void Btn_CutPlay_Click(object sender, EventArgs e)
        {
            if (Txt_URL.Text.Trim().Length <= 0) return;
            Img_DeskTop.Visible = false;
            if (CutList.Count > 0)
            {
                skipTime = 0;
                skipStart = CutList[0];
                skipEnd = CutList[1];
                cutListIndex = 2;
                isCut = true;
            }

            Wmp_1.Visible = true;
            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.play();

            threadTrack = new Thread(new ThreadStart(UpdateTrackThreadProc));
            if (threadTrack.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                threadTrack.Start();
                isActiveTrack = true;
            }
        }

        private void Btn_Split_Click(object sender, EventArgs e)
        {
            //8.608 30.415 62.62 93.608 115.707 143.812
            if (Wmp_1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                Wmp_1.Ctlcontrols.pause();
            List<string> split = new List<string>();
            cutList.Sort();
            split.Add("0");
            foreach(int time in cutList)
            {
                split.Add(((double)time / 1000).ToString());
            }
            split.Add(((double)playTime / 1000).ToString());

            string url = Txt_URL.Text;
            if (split.Count <= 0 || split.Count % 2 == 1)
            {
                MessageBox.Show("자르기 실패");
                return;
            }
            Global.WriteLog("스플릿+컨캣 시작");
            dControl.Split(url, split);
            Global.WriteLog("스플릿+컨캣 종료");
        }

        private void Btn_MergePlay_Click(object sender, EventArgs e)
        {
            if (Txt_URL.Text.Trim().Length <= 0) return;
            Global.WriteLog("합성 영상 재생");
            Img_DeskTop.Visible = false;
            Wmp_1.Visible = true;

            skipTime = 0;
            isCut = false;
            cutList.Clear();

            VideoStream vs = new VideoStream();
            playTime = vs.AVFormatTest(Txt_URL.Text);

            //timeLabel1.Parent = Wmp_1;

            TimeSlider.Max = playTime;
            TimeSlider.SelectedMax = playTime;
            TimeSlider.Value = playTime / 2;

            Image image = TimeSlider.BackgroundImage;
            TimeSlider.BackgroundImage = null;
            if (image != null) image.Dispose();
            Global.FileDeleteForce(Application.StartupPath);

            Global.WriteLog("타임슬라이더용 프레임 추출");
            dControl.ExtractImage(Txt_URL.Text, playTime/1000, cutList);
            Global.WriteLog("타임슬라이더용 프레임 추출 완료");

            TimeSlider.BackgroundImage = new Bitmap(Application.StartupPath + @"\output.jpg");
            TimeSlider.BackgroundImageLayout = ImageLayout.Stretch;

            Wmp_1.URL = Txt_URL.Text;
            Wmp_1.Ctlcontrols.play();

            threadTrack = new Thread(new ThreadStart(UpdateTrackThreadProc));
            if (threadTrack.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                threadTrack.Start();
                isActiveTrack = true;
            }
        }

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

        private void Btn_Play_Click(object sender, EventArgs e)
        {
            Wmp_1.Ctlcontrols.stop();
            if (isActiveTrack)
            {
                isActiveTrack = false;
                threadTrack.Join();
                //threadTrack.Interrupt();
                //threadTrack.Abort();
                isActiveTrack = false;
            }

            Img_DeskTop.Visible = true;
            monitor = (Devices)Cmb_Monitor.SelectedItem;
            camName = Cmb_Cam.Text.Trim();
            //비디오 프레임 디코딩 thread 생성
            if (!activeThread)
            {
                label5.Text = "";
                if (Cmb_Cam.Text.Trim().Length > 0)
                {
                    threadCam = new Thread(new ThreadStart(DecodeAllFramesToImages));
                    if (threadCam.ThreadState == System.Threading.ThreadState.Unstarted)
                    {
                        threadCam.Start();
                    }
                }
                threadDesktop = new Thread(new ThreadStart(ScreenCapture));
                if (threadDesktop.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    threadDesktop.Start();
                }
                activeThread = true;
            }
            else
            {
                label5.Text = "Thread activing!!";
            }
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Wmp_1.Ctlcontrols.stop();
            Wmp_1.Visible = false;
            if (isActiveTrack)
            {
                isActiveTrack = false;
                threadTrack.Join();
                //threadTrack.Interrupt();
                //threadTrack.Abort();
                //isActiveTrack = false;
            }
            if (activeThread)
            {
                activeThread = false;
                if (threadCam != null)
                {
                    threadCam.Join();
                    //threadCam.Interrupt();
                    //threadCam.Abort();
                }
                if (threadDesktop != null)
                {
                    threadDesktop.Join();
                    //threadDesktop.Interrupt();
                    //threadDesktop.Abort();
                }
                activeThread = false;
                label5.Text = "";
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
            List<Devices> devicelist = dControl.GetDeviceList();
            List<Devices> monitorlist = new List<Devices>();
            foreach (Devices dv in devicelist)
            {
                if (dv.Device == "audio") Cmb_Mic.Items.Add(dv.Name);
                else if (dv.Device == "video") Cmb_Cam.Items.Add(dv.Name);
            }
            if (Cmb_Cam.Items.Count > 0) Cmb_Cam.SelectedIndex = 0;
            if (Cmb_Mic.Items.Count > 0) Cmb_Mic.SelectedIndex = 0;
            Screen[] screenlist = Screen.AllScreens;
            foreach (Screen screen in screenlist)
            {
                monitorlist.Add(new Devices { Device = "monitor", Name = screen.DeviceName.Substring(screen.DeviceName.IndexOf(".\\") + 2), Size = screen.Bounds.Size, Point = screen.Bounds.Location });
            }

            Cmb_Monitor.DataSource = new BindingSource(monitorlist, null);
            Cmb_Monitor.DisplayMember = "name";
        }
        #region RecordTest Method
        private unsafe void DecodeAllFramesToImages()
        {
            try
            {
                //video="웹캠 디바이스 이름"
                string device = "video=" + camName;
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
            catch (Exception e)
            {
                activeThread = false;
                threadCam.Interrupt();
                threadCam.Abort();
                threadDesktop.Interrupt();
                threadDesktop.Abort();
                Img_video.Image = null;
                Img_DeskTop.Image = null;
                Global.WriteLog(e.Message);
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
                    Bitmap bitmap = new Bitmap(monitor.Size.Width, monitor.Size.Height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.CopyFromScreen(monitor.Point.X, monitor.Point.Y, 0, 0, monitor.Size);

                    Img_DeskTop.Image = bitmap;
                    graphics.Dispose();
                }
            }
            catch (Exception e)
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
                Global.WriteLog(e.Message);
            }

        }

        private void UpdateTrackThreadProc()
        {
            try
            {
                while (isActiveTrack)
                {
                    this.BeginInvoke(new MethodInvoker(UpdateTrack));
                    System.Threading.Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private void UpdateTrack()
        {
            if(isCut)
            {
                if (Wmp_1.Ctlcontrols.currentPosition * 1000 >= skipStart)
                {
                    if (!isVideoTowed) Wmp_1.Ctlcontrols.currentPosition = skipEnd / 1000;
                    skipTime += skipEnd - skipStart;
                    if (cutList.Count > cutListIndex)
                    {
                        skipStart = CutList[cutListIndex++];
                        skipEnd = CutList[cutListIndex++];
                    }
                    else isCut = false;
                }
            }
            isVideoTowed = false;
            if (!isVideoTowing) TimeSlider.Value = Convert.ToInt32((Wmp_1.Ctlcontrols.currentPosition * 1000) - skipTime);
        }

        #endregion

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

        private void Btn_Pause_Click(object sender, EventArgs e)
        {
            if (Wmp_1.playState == WMPLib.WMPPlayState.wmppsPaused || Wmp_1.playState == WMPLib.WMPPlayState.wmppsReady)
                Wmp_1.Ctlcontrols.play();
            else if (Wmp_1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                Wmp_1.Ctlcontrols.pause();
        }

        private void TimeSlider_ValueChanged(object sender, EventArgs e)
        {
            //if (Wmp_1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            //Wmp_1.Ctlcontrols.currentPosition = Convert.ToDouble(timeSlider.Value) / 1000;

            // if(timeSlider.Value == )
            int currentX = (int)((double)TimeSlider.Size.Width * ((double)TimeSlider.Value / (double)TimeSlider.Max));
            Lbl_Current.Location = new Point(TimeSlider.Location.X + currentX - 5, 538);
            TimeSpan ts = TimeSpan.FromSeconds((double)TimeSlider.Value / 1000);
            Lbl_Current.Text = ts.ToString("hh\\:mm\\:ss\\.fff");
        }

        private void TimeSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isVideoTowing) return;
        }

        private void TimeSlider_MouseDown(object sender, MouseEventArgs e)
        {
            isVideoTowing = true;
            startValue = TimeSlider.Value;
        }

        private void TimeSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (isVideoTowing)
            {
                if (startValue != TimeSlider.Value)
                {
                    skipTime = 0;
                    isCut = false;
                    if (cutList.Count > 0)
                    {
                        skipStart = CutList[0];
                        skipEnd = CutList[1];
                        cutListIndex = 2;
                        isCut = true;
                        for (int i = 0; i < CutList.Count; i += 2)
                        {
                            if (TimeSlider.Value + skipTime >= CutList[i])
                            {
                                skipTime += skipEnd - skipStart;
                                if (cutList.Count > cutListIndex)
                                {
                                    skipStart = CutList[cutListIndex++];
                                    skipEnd = CutList[cutListIndex++];
                                }
                                else
                                    isCut = false;
                            }
                        }
                    }
                    Wmp_1.Ctlcontrols.currentPosition = Convert.ToDouble(TimeSlider.Value + skipTime) / 1000;

                    isVideoTowed = true;
                }
                isVideoTowing = false;
            }

        }

        private void TimeSlider_SelectionChanged(object sender, EventArgs e)
        {
            int minX = (int)((double)TimeSlider.Size.Width * ((double)TimeSlider.SelectedMin / (double)TimeSlider.Max));
            Lbl_Min.Location = new Point(TimeSlider.Location.X + minX - 5, 538);
            TimeSpan ts = TimeSpan.FromSeconds((double)TimeSlider.SelectedMin / 1000);
            Lbl_Min.Text = ts.ToString("hh\\:mm\\:ss\\.fff");
            int maxX = (int)((double)TimeSlider.Size.Width * ((double)TimeSlider.SelectedMax / (double)TimeSlider.Max));
            Lbl_Max.Location = new Point(TimeSlider.Location.X + maxX - 5, 538);
            ts = TimeSpan.FromSeconds((double)TimeSlider.SelectedMax / 1000);
            Lbl_Max.Text = ts.ToString("hh\\:mm\\:ss\\.fff");
            //Lbl_Max.Text = DateTime.ParseExact(string.Format("{0:F3}",(double)timeSlider.SelectedMax / 1000) , "ss.fff", null).ToString();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            int minX = (int)((double)TimeSlider.Size.Width * ((double)TimeSlider.SelectedMin / (double)TimeSlider.Max));
            Lbl_Min.Location = new Point(TimeSlider.Location.X + minX - 5, 538);
            int maxX = (int)((double)TimeSlider.Size.Width * ((double)TimeSlider.SelectedMax / (double)TimeSlider.Max));
            Lbl_Max.Location = new Point(TimeSlider.Location.X + maxX - 5, 538);
        }
    }
}