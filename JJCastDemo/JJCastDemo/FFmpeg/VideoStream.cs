using System;
using System.Diagnostics;
using FFmpeg.AutoGen;


namespace JJCastDemo.FFmpeg
{
    public unsafe class VideoStream : IDisposable
    {
        AVFormatContext* _fmtCtx;
        int _vidx = -1, _aidx = -1;
        AVStream* _vStream, _aStream;
        AVCodecParameters* _vPara, _aPara;
        AVCodec* _vCodec, _aCodec;
        AVCodecContext* _vCtx, _aCtx;
        AVPacket _packet;
        AVFrame _vFrame, _aFrame;

        /// <summary>
        /// 스트림 찾기
        /// </summary>
        /// <returns></returns>
        public int AVFormatTest()
        {
            var fmtCtx = _fmtCtx;
            int ret = ffmpeg.avformat_open_input(&fmtCtx, "E:\\_Works\\All of Me (Jon Schmidt) - The Piano Guys.mp4", null, null);
            if (ret != 0) return -1;
            ffmpeg.avformat_find_stream_info(fmtCtx, null);

            Debug.WriteLine("스트림 개수 = {0:d}\n", fmtCtx->nb_streams);

            Debug.WriteLine("시간 = {0:d}초\n", fmtCtx->duration / ffmpeg.AV_TIME_BASE);

            Debug.WriteLine("비트레이트 = {0:d}\n", fmtCtx->bit_rate);
            ffmpeg.avformat_close_input(&fmtCtx);
            return 0;
        }

        /// <summary>
        /// 스트림 정보
        /// </summary>
        /// <returns></returns>
        public int AVFormatTest2()
        {
            var fmtCtx = _fmtCtx;
            int ret = ffmpeg.avformat_open_input(&fmtCtx, "E:\\_Works\\All of Me (Jon Schmidt) - The Piano Guys.mp4", null, null);
            if (ret != 0) return -1;
            ffmpeg.avformat_find_stream_info(fmtCtx, null);

            for (int i = 0; i < fmtCtx->nb_streams; i++)
            {
                if (fmtCtx->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO) _vidx = i;
                if (fmtCtx->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO) _aidx = i;
            }

            Debug.WriteLine("video = " + _vidx.ToString() + "번, audio = " + _aidx.ToString() + "번\n");

            _vidx = ffmpeg.av_find_best_stream(fmtCtx, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, null, 0);
            _aidx = ffmpeg.av_find_best_stream(fmtCtx, AVMediaType.AVMEDIA_TYPE_AUDIO, -1, _vidx, null, 0);

            Debug.WriteLine("video = " + _vidx.ToString() + "번, audio = " + _aidx.ToString() + "번\n");

            ffmpeg.avformat_close_input(&fmtCtx);
            return 0;
        }

        /// <summary>
        /// 코덱 정보 조사
        /// </summary>
        /// <returns></returns>
        public int[] AVFormatTest3(string url)
        {
            int[] rtnval = new int[2];
            var fmtCtx = _fmtCtx;
            int ret = ffmpeg.avformat_open_input(&fmtCtx, url, null, null);
            if (ret != 0) return rtnval;
            ffmpeg.avformat_find_stream_info(fmtCtx, null);

            _vidx = ffmpeg.av_find_best_stream(fmtCtx, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, null, 0);
            _aidx = ffmpeg.av_find_best_stream(fmtCtx, AVMediaType.AVMEDIA_TYPE_AUDIO, -1, _vidx, null, 0);

            _vStream = fmtCtx->streams[_vidx];
            Debug.WriteLine("프레임 개수 = " + _vStream->nb_frames);
            Debug.WriteLine("프레임 레이트 = " + _vStream->avg_frame_rate.num + " / " + _vStream->avg_frame_rate.den);
            Debug.WriteLine("타임 베이스 = " + _vStream->time_base.num + " / " + _vStream->time_base.den);
            _vPara = _vStream->codecpar;
            Debug.WriteLine("폭 = " + _vPara->width);
            Debug.WriteLine("높이 = " + _vPara->height);
            Debug.WriteLine("색상 포맷 = " + _vPara->format);
            Debug.WriteLine("코덱 = " + _vPara->codec_id);
            Debug.WriteLine("--------------------------------");
            rtnval[0] = _vPara->width;
            rtnval[1] = _vPara->height;
            
            _aStream = fmtCtx->streams[_aidx];
            Debug.WriteLine("프레임 개수 = " + _aStream->nb_frames);
            Debug.WriteLine("타임 베이스 = " + _aStream->time_base.num + " / " + _aStream->time_base.den);
            _aPara = _aStream->codecpar;
            Debug.WriteLine("사운드 포맷 = " + _aPara->format);
            Debug.WriteLine("코덱 = " + _aPara->codec_id);
            Debug.WriteLine("채널 = " + _aPara->channels);
            Debug.WriteLine("샘플 레이트 = " + _aPara->sample_rate);

            ffmpeg.avformat_close_input(&fmtCtx);
            return rtnval;
        }

        /// <summary>
        /// 압축 해제
        /// </summary>
        /// <returns></returns>
        public int AVFormatTest4()
        {
            var fmtCtx = _fmtCtx;
            int ret = ffmpeg.avformat_open_input(&fmtCtx, "C:\\Users\\jisu827\\output.avi", null, null);
            if (ret != 0) return -1;
            ffmpeg.avformat_find_stream_info(fmtCtx, null);

            _vidx = ffmpeg.av_find_best_stream(fmtCtx, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, null, 0);
            _aidx = ffmpeg.av_find_best_stream(fmtCtx, AVMediaType.AVMEDIA_TYPE_AUDIO, -1, _vidx, null, 0);

            var packet = _packet;

            var vFrame = _vFrame;
            var vCtx = _vCtx;
            _vStream = fmtCtx->streams[_vidx];
            _vPara = _vStream->codecpar;
            _vCodec = ffmpeg.avcodec_find_decoder(_vPara->codec_id);
            vCtx = ffmpeg.avcodec_alloc_context3(_vCodec);
            ffmpeg.avcodec_parameters_to_context(vCtx, _vPara);
            ffmpeg.avcodec_open2(vCtx, _vCodec, null);

            var aFrame = _aFrame;
            var aCtx = _aCtx;
            _aStream = fmtCtx->streams[_aidx];
            _aPara = _aStream->codecpar;
            _aCodec = ffmpeg.avcodec_find_decoder(_aPara->codec_id);
            aCtx = ffmpeg.avcodec_alloc_context3(_aCodec);
            ffmpeg.avcodec_parameters_to_context(aCtx, _aPara);
            ffmpeg.avcodec_open2(aCtx, _aCodec, null);

            // 루프를 돌며 패킷을 모두 읽는다.
            int vcount = 0, acount = 0;
            int cnt = 0;
            while (ffmpeg.av_read_frame(fmtCtx, &packet) == 0)
            {
                if (packet.stream_index == _vidx)
                {
                    ret = ffmpeg.avcodec_send_packet(vCtx, &packet);
                    if (ret != 0) { continue; }
                    for (; ; )
                    {
                        ret = ffmpeg.avcodec_receive_frame(vCtx, &vFrame);
                        if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN)) break;
                        if (vcount == 0)
                        {
                            Debug.WriteLine(string.Format("Video format : {0}({1} x {2}).",
                                vFrame.format, vFrame.width, vFrame.height));
                        }
                        Debug.Write(string.Format("V{0,5}(pts={1,5},size={2,5}) : ", vcount++, vFrame.pts, vFrame.pkt_size));
                        for (uint i = 0; i < 3; i++)
                        {
                            Debug.Write(vFrame.linesize[i] + " ");
                        }
                        arDump(vFrame.data[0], 4);
                        arDump(vFrame.data[1], 2);
                        arDump(vFrame.data[2], 2);

                    }

                }

                if (packet.stream_index == _aidx)
                {
                    ret = ffmpeg.avcodec_send_packet(aCtx, &packet);
                    if (ret != 0) { continue; }
                    for (; ; )
                    {
                        ret = ffmpeg.avcodec_receive_frame(aCtx, &aFrame);
                        if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN)) break;
                        if (acount == 0)
                        {
                            Debug.WriteLine(string.Format("Audio format : {0}({1} x {2}).",
                                aFrame.format, aFrame.channels, aFrame.sample_rate));
                        }
                        Debug.Write(string.Format("A{0,5}(pts={1,5},size={2,5}) : ", acount++, aFrame.pts, aFrame.pkt_size));
                        arDump(aFrame.extended_data, 16);
                    }
                }

                ffmpeg.av_packet_unref(&packet);
                Debug.WriteLine("");
                if (cnt == 1000) break;
                cnt++;
            }


            // 메모리 해제
            ffmpeg.av_frame_unref(&vFrame);
            ffmpeg.av_frame_unref(&aFrame);
            ffmpeg.avcodec_free_context(&vCtx);
            ffmpeg.avcodec_free_context(&aCtx);
            ffmpeg.avformat_close_input(&fmtCtx);
            return 0;
        }

        void arDump(void* array, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Debug.Write(string.Format("{0,3:X2}", ((byte*)array + i) == null ? 0 : *((byte*)array + i)));
            }
        }

        public void Dispose()
        {
            var _iFormatContext = _fmtCtx;
            ffmpeg.avformat_close_input(&_iFormatContext);
        }
    }
}
