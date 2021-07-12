using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using File = System.IO.File;

namespace JJCastDemo.Vimeo
{
    public class VideoHelper : BaseHelper
    {
        /// <summary>
        /// 특정 비디오 정보 가져오기
        /// </summary>
        /// <param name="clipId"></param>
        /// <returns></returns>
        public Video ShouldCorrectlyRetrievesVideosByMe(long clipId)
        {
            var awaiter = AuthenticatedClient.GetVideoAsync(clipId: clipId, userId: mUserId.Id).GetAwaiter();
            Video video = new Video();
            int cnt = 0;
            while (!awaiter.IsCompleted)
            {
                Task.Delay(100);
                if (cnt++ == 300) break;
            }
            video = awaiter.GetResult();
            return video;
        }

        /// <summary>
        /// 폴더 내 비디오 정보 가져오기
        /// </summary>
        /// <returns></returns> 
        public Paginated<Video> ShouldCorrectlyGetUserFolderVideosByUserId()
        {
            var awaiter = AuthenticatedClient.GetVideosAsync(mUserId.Id, folderId: Folder_ID).GetAwaiter();
            Paginated<Video> videos = new Paginated<Video>();
            while (!awaiter.IsCompleted)
            {
                Task.Delay(100);
            }
            videos = awaiter.GetResult();
            return videos;
        }

        /// <summary>
        /// 계정정보 가져오기
        /// </summary>
        /// <returns></returns>
        public User ShouldCorrectlyGetAccountInformation()
        {
            var client = CreateAuthenticatedClient();
            var awaiter = client.GetAccountInformationAsync().GetAwaiter();
            while (!awaiter.IsCompleted)
            {
                Task.Delay(100);
            }
            return awaiter.GetResult();

        }

        /// <summary>
        /// 파일경로로 비디오 업로드
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public long ShouldCorrectlyUploadFileByPath(string filePath)
        {
            long rtnVal = 0;
            var tempFilePath = Path.GetTempFileName() + ".mp4";
            IUploadRequest completedRequest;
            using (var fs = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                Stream stream = new FileStream(filePath, FileMode.Open);
                stream.CopyTo(fs);
            }

            using (var file = new BinaryContent(tempFilePath))
            {
                var client = CreateAuthenticatedClient();
                var awaiter = client.UploadEntireFileAsync(file).GetAwaiter();
                while (!awaiter.IsCompleted)
                {
                    Task.Delay(100);
                }
                completedRequest = awaiter.GetResult();
            }
            if (completedRequest == null) rtnVal = 0;
            if (completedRequest.ClipId != null) rtnVal = Convert.ToInt64(completedRequest.ClipId);
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            return rtnVal;
        }

        /// <summary>
        /// 비디오 폴더 이동
        /// </summary>
        /// <param name="clipId"></param>
        /// <returns></returns>
        public bool ShouldCorrectlyMovetoFolderVideoByMe(long clipId)
        {
            var client = CreateAuthenticatedClient();
            var awaiter = client.MoveVideoToFolder(clipId: clipId, projectId: 3865074).GetAwaiter();
            int cnt = 0;
            while (!awaiter.IsCompleted)
            {
                Task.Delay(100);
                if (cnt++ == 300) break;
            }
            return true;
        }

        /// <summary>
        /// 비디오 이름 변경
        /// </summary>
        /// <param name="clipId"></param>
        /// <param name="reName"></param>
        /// <returns></returns>
        public bool ShouldCorrectlyUpdateVideoName(long clipId, string reName)
        {
            var awaiter = AuthenticatedClient.UpdateVideoMetadataAsync(clipId, new VideoUpdateMetadata
            {
                Name = reName
            }).GetAwaiter();

            int cnt = 0;
            while (!awaiter.IsCompleted)
            {
                Task.Delay(100);
                if (cnt++ == 300) break;
            }
            return true;

        }

        /// <summary>
        /// 비디오 삭제
        /// </summary>
        /// <param name="clipId"></param>
        /// <returns></returns>
        public bool ShouldDeleteVideoByMe(long clipId)
        {
            var client = CreateAuthenticatedClient();
            var awaiter = client.DeleteVideoAsync(clipId).GetAwaiter();

            int cnt = 0;
            while (!awaiter.IsCompleted)
            {
                Task.Delay(100);
                if (cnt++ == 300) break;
            }
            return true;
        }

        /// <summary>
        /// 자막 업로드
        /// </summary>
        /// <param name="clipId"></param>
        /// <param name="filename"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool SubtitleUploadByVideoId(long clipId, string filename, string filepath)
        {
            var client = CreateAuthenticatedClient();
            TextTrack newTextTrack;
            TaskAwaiter<TextTrack> awaiter;
            string textTrackName = filename;
            const string textTrackLanguage = "ko";
            using (var file = new BinaryContent(new FileStream(filepath, FileMode.Open),
                "application/octet-stream"))
            {
                awaiter = client.UploadTextTrackFileAsync(
                    file,
                    clipId,
                    new TextTrack
                    {
                        Active = true,
                        Name = filename,
                        Language = textTrackLanguage,
                        Type = TextTrackType.Captions
                    }).GetAwaiter();

                newTextTrack = awaiter.GetResult();
                if (newTextTrack == null) return false;
                if (newTextTrack.Name != textTrackName || newTextTrack.Uri.Trim().Length == 0 || newTextTrack.Link.Trim().Length == 0) return false;
            }


            return true;

            #region 자막 수정
            /*
                var uri = newTextTrack.Uri;
                var trackId = Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));

                var textTrack = await client.GetTextTrackAsync(clipId, trackId);
                textTrack.ShouldNotBeNull();
                textTrack.Active.ShouldBeFalse();
                textTrack.Name.ShouldBe("UploadtTest.vtt");
                textTrack.Type.ShouldBe(TextTrackType.Captions);
                textTrack.Language.ShouldBe("en");
                textTrack.Uri.ShouldNotBeEmpty();
                textTrack.Link.ShouldNotBeEmpty();

                const string testName = "NewTrackName";
                const TextTrackType testType = TextTrackType.Metadata;
                const string testLanguage = "fr";
                const bool testActive = false;

                var updated = await client.UpdateTextTrackAsync(
                    clipId,
                    trackId,
                    new TextTrack
                    {
                        Name = testName,
                        Type = testType,
                        Language = testLanguage,
                        Active = testActive
                    });

                // inspect the result and ensure the values match what we expect...
                updated.Name.ShouldBe(testName);
                updated.Type.ShouldNotBeNull();
                updated.Type.ShouldBe(testType);
                updated.Language.ShouldBe(testLanguage);
                updated.Active.ShouldBeFalse();
                */
            #endregion

            #region 자막 삭제
            /*
                await client.DeleteTextTrackAsync(clipId, trackId);

                var deletedTrack = await client.GetTextTrackAsync(clipId, trackId);
                deletedTrack.ShouldBeNull();
                */
            #endregion
        }
    }
}
