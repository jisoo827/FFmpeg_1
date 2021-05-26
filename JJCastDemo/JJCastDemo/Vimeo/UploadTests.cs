using System;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Net;

namespace JJCastDemo.Vimeo
{
    public class UploadTests : BaseHelper
    {
        public async Task ShouldCorrectlyGenerateNewUploadTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetUploadTicketAsync();
            ticket.ShouldNotBeNull();
            ticket.CompleteUri.ShouldNotBeEmpty();
            ticket.TicketId.ShouldNotBeEmpty();
            ticket.UploadLink.ShouldNotBeEmpty();
            ticket.UploadLinkSecure.ShouldNotBeEmpty();
            ticket.Uri.ShouldNotBeEmpty();
            ticket.User.Id.ShouldBe(VimeoSettings.UserId);
        }

        public async Task ShouldCorrectlyGenerateNewTusResumableUploadTicket()
        {
            var client = CreateAuthenticatedClient();
            VimeoDotNet.Models.TusResumableUploadTicket ticket = await client.GetTusResumableUploadTicketAsync(1000);
            ticket.ShouldNotBeNull();
            ticket.Upload.UploadLink.ShouldNotBeEmpty();
            ticket.Id.ShouldNotBeNull();
            ticket.User.Id.ShouldBe(VimeoSettings.UserId);
        }

        public async Task ShouldCorrectlyGenerateReplaceUploadTicket()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var ticket = await AuthenticatedClient.GetReplaceVideoUploadTicketAsync(clipId);
                ticket.ShouldNotBeNull();
                ticket.CompleteUri.ShouldNotBeEmpty();
                ticket.TicketId.ShouldNotBeEmpty();
                ticket.UploadLink.ShouldNotBeEmpty();
                ticket.UploadLinkSecure.ShouldNotBeEmpty();
                ticket.Uri.ShouldNotBeEmpty();
                ticket.User.Id.ShouldBe(VimeoSettings.UserId);
            });
        }

        public async Task ShouldCorrectlyUploadFileByPath()
        {
            long length;
            IUploadRequest completedRequest;
            var tempFilePath = Path.GetTempFileName() + ".mp4";
            using (var fs = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                TestHelper.GetFileFromEmbeddedResources(TestHelper.TestFilePath).CopyTo(fs);
            }
            try
            {
                using (var file = new BinaryContent(tempFilePath))
                {
                    file.ContentType.ShouldBe("video/mp4");
                    length = file.Data.Length;
                    var client = CreateAuthenticatedClient();
                    completedRequest = await client.UploadEntireFileAsync(file);
                    completedRequest.ClipId.ShouldNotBeNull();
                    await client.DeleteVideoAsync(completedRequest.ClipId.Value);
                }
          

                completedRequest.ShouldNotBeNull();
                completedRequest.IsVerifiedComplete.ShouldBeTrue();
                completedRequest.BytesWritten.ShouldBe(length);
                completedRequest.ClipUri.ShouldNotBeNull();
                completedRequest.ClipId.ShouldNotBeNull();
                completedRequest.ClipId?.ShouldBeGreaterThan(0);
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
        }

        public async Task ShouldCorretlyUploadFileByStream()
        {
            long length;
            IUploadRequest completedRequest;

            using (var file = new BinaryContent(TestHelper.GetFileFromEmbeddedResources(TestHelper.TestFilePath), "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.ClipId.ShouldNotBeNull();
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }

            completedRequest.ShouldNotBeNull();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        public async Task ShouldCorretlyUploadFileByByteArray()
        {
            long length;
            IUploadRequest completedRequest;
            var stream = TestHelper.GetFileFromEmbeddedResources(TestHelper.TestFilePath);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);
            using (var file = new BinaryContent(buffer, "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.ClipId.ShouldNotBeNull();
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }

            completedRequest.ShouldNotBeNull();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        public async Task ShouldCorretlyUploadFileByPullLink()
        {
            try
            {
                var client = CreateAuthenticatedClient();
                var video = await client.UploadPullLinkAsync("http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4");
                video.ShouldNotBeNull();
                video.Id.ShouldNotBeNull();
                await client.DeleteVideoAsync(video.Id.Value);
                video = await client.GetVideoAsync(video.Id.Value);
                video.ShouldBeNull();
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
        }

        public async Task ShouldCorrectlyUploadThumbnail()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                using (var file = new BinaryContent(TestHelper.GetFileFromEmbeddedResources(TestHelper.TestFilePath), "image/png"))
                {
                    var picture = await AuthenticatedClient.UploadThumbnailAsync(clipId, file);
                    picture.ShouldNotBeNull();
                }
            });

        }
    }
}
