using System;
using System.Collections.Generic;
using System.Drawing;
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
            stmt = @"ffmpeg -list_devices true -f dshow -i dummy";
            return stmt;
        }
        /// <summary>
        /// 데스크탑 화면 녹화 (화면만)
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string DesktopPartialRecordStmt(string offset)
        {
            stmt = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"base.mp4\"";
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
            stmt = "ffmpeg -y -rtbufsize 100M -f gdigrab -framerate 30 -draw_mouse 1 " + offset + " -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 25 -pix_fmt yuv420p \"base.mp4\" -f dshow -i audio=\"" + mic + "\"";
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
        public string ConcatVideoStmt(string front, string back, string result)
        {
            stmt = "ffmpeg -y -i " + back + " -c copy -bsf:v h264_mp4toannexb -f mpegts back.ts &&  ffmpeg -y -i " + front + " -c copy -bsf:v h264_mp4toannexb -f mpegts front.ts && ffmpeg -y -i \"concat:front.ts|back.ts\" -c copy -bsf:a aac_adtstoasc " + result + " && exit";
            return stmt;
        }

        public string OverlayVideoStmt(string pad, string crop, string overlay, string rgbHex, string size = "320:240")
        {
            stmt = "ffmpeg -y -i cam.mp4 -vf scale=" + size  + " -preset ultrafast -tune zerolatency -crf 18 cam_resize.mp4 && ffmpeg -y -i base.mp4 -i cam_resize.mp4 -filter_complex \"[0:v]pad = " + pad + ": color = white[a];[1:v]setpts = PTS - 0.1 / TB[b];" + crop + "chromakey = 0x" + rgbHex + " : 0.1 : 0.1[ckout];[a][ckout]overlay = " + overlay + ":enable = gte(t\\, 0):eof_action = pass,format = yuv420p[out]\" -map \"[out]\" -map 0:a? -c:v libx264 -preset ultrafast -tune zerolatency -crf 18 -c:a copy  output_overlay.mp4 && exit";
            return stmt;
        }

        public string CutVideoStmt(string url, string start, string end, int seq)
        {
            stmt = "ffmpeg -y -i " + url + " -ss " + start + " -to " + end + " -vcodec copy -acodec copy output_cut" + seq.ToString() + ".mp4 && exit";
            return stmt;
        }

    }
}
