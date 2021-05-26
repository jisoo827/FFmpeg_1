using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VimeoDotNet;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;
using Shouldly;

namespace JJCastDemo.Vimeo
{
    public static class TestHelper
    {
        public const string TestFilePath = @"JJCastDemo.Resources.AllOfMe.mp4";

        public static Stream GetFileFromEmbeddedResources(string relativePath)
        {
            var assembly = typeof(TestHelper).GetTypeInfo().Assembly;
            Stream rtnStream = new FileStream(@"C:\Users\jisu827\AllOfMe.mp4",FileMode.Open);
            Stream rtnStream2 = assembly.GetManifestResourceStream(relativePath);
            return rtnStream;
            //return assembly.GetManifestResourceStream(relativePath);
        }

        /// <summary>
        /// Execute method with test video
        /// </summary>
        /// <param name="client">Vimeo client</param>
        /// <param name="action">Test action with Clip Id</param>
        /// <returns>The result task</returns>
        public static async Task WithTempVideo(this IVimeoClient client, Func<long, Task> action)
        {
            long? tempVideoId = null;
            try
            {
                using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
                {
                    var length = file.Data.Length;
                    var completedRequest = await client.UploadEntireFileAsync(file);
                    
                    completedRequest.ShouldNotBeNull();
                    completedRequest.IsVerifiedComplete.ShouldBeTrue();
                    completedRequest.BytesWritten.ShouldBe(length);
                    completedRequest.ClipUri.ShouldNotBeNull();
                    completedRequest.ClipId.HasValue.ShouldBeTrue();
                    completedRequest.ClipId.ShouldNotBeNull();
                    
                    tempVideoId = completedRequest.ClipId;
                }

                await action(tempVideoId.Value);
            }
            finally
            {
                if (tempVideoId != null)
                {
                    await client.DeleteVideoAsync(tempVideoId.Value);
                }
            }
        }

        /// <summary>
        /// Execute action with test album
        /// </summary>
        /// <param name="client">Vimeo client</param>
        /// <param name="action">Test action with clip Id</param>
        /// <returns>The result task</returns>
        public static async Task WithTestAlbum(this IVimeoClient client, Func<long, Task> action)
        {
            long? tempAlbumId = null;
            try
            {
                var album = await client.CreateAlbumAsync(UserId.Me, new EditAlbumParameters
                {
                    Name = "JJCONTENTS"
                });
                tempAlbumId = album.GetAlbumId();

                await action(tempAlbumId.Value);
            }
            finally
            {
                if (tempAlbumId != null)
                {
                    await client.DeleteAlbumAsync(UserId.Me, tempAlbumId.Value);
                }
            }
        }
    }
}
