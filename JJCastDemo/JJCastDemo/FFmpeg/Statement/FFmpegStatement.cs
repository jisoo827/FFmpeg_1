using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJCastDemo.FFmpeg.Statement
{
    public class FFmpegStatement
    {
        private string stmt = string.Empty;
        public string GetDeviceLisStmt()
        {
            stmt = @"ffmpeg -list_devices true -f dshow -i dummy && exit";
            return stmt;
        }
        /// <summary>
        /// 데스크탑 화면 녹화 (화면만)
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string DesktopPartialRecordStmt(string offset)
        {
            stmt = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -video_size 1280x720 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"base.mp4\"";
            return stmt;
        }

        /// <summary>
        /// 데스크탑 화면 + 음성 녹화
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="mic"></param>
        /// <returns></returns>
        public string DesktopPartialRecordStmt(string offset, string mic)
        {
            stmt = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -video_size 1280x720 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"base.mp4\" -f dshow -i audio=\"" + mic + "\"";
            return stmt;
        }

        /// <summary>
        /// 캠 화면 녹화
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public string CamRecordStmt(string cam)
        {
            stmt = "ffmpeg -y -f dshow -i video=\"" + cam + "\" -rtbufsize 100M -framerate 45 -video_size 298x144  -c:v libx264 -r 45 -preset ultrafast -tune zerolatency -crf 28 -pix_fmt yuv420p -c:a aac -strict -2 -ac 2 -b:a 128k \"cam.mp4\"";
            return stmt;
        }

        /// <summary>
        /// 비디오 이어붙히기
        /// </summary>
        /// <returns></returns>
        public string ConcatVideoStmt(string front, string back)
        {
            stmt = @"ffmpeg -y -i " + front + " -acodec aac -vcodec libx264 -s hd720  -r 60 -qscale 0.1 -strict experimental front.mp4 && ffmpeg -y -i " + back + " -acodec aac -vcodec libx264 -s hd720  -r 60 -qscale 0.1 -strict experimental back.mp4 &&  ffmpeg -y -f concat -i concatVideo.txt -c copy output_concat.mp4 && exit";
            return stmt;
        }

        public string OverlayVideoStmt()
        {
            stmt = "ffmpeg -y -i cam.mp4 -vf scale=320:240 cam_320.mp4 && ffmpeg -y -i base.mp4 -i cam_320.mp4 -filter_complex \"[1:v]setpts = PTS + 0 / TB[a];[0:v][a]overlay = (W - w):(H - h):enable = gte(t\\, 0):eof_action = pass,format = yuv420p[out]\" -map \"[out]\" -map 0:a? -c:v libx264 -crf 18 -c:a copy output_overlay.mp4 && exit";
            return stmt;
        }

    }
}
