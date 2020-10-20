using System.Drawing;
using FFmpeg.AutoGen;

namespace FFMPEGTest1.FFmpeg
{
    public class VideoInfo
    {
        public string CodecName;
        public Size SourceFrameSize;
        public Size DestinationFrameSize;
        public AVPixelFormat SourcePixelFormat;
        public AVPixelFormat DestinationPixelFormat;
        public AVRational Sample_aspect_ratio;
        public AVRational Timebase;
        public AVRational Framerate;
    }
}