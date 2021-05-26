using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VimeoDotNet.Authorization;
using JJCastDemo.Vimeo.Setting;
using VimeoDotNet;

namespace JJCastDemo.Vimeo
{
    public class BaseHelper
    {
        protected readonly VimeoApiTestSettings VimeoSettings;
        protected readonly IVimeoClient AuthenticatedClient;

        // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        protected const string TestTextTrackFilePath = @"JJCastDemo.Resources.test.vtt";

        protected const string TextThumbnailFilePath = @"JJCastDemo.Resources.test.png";

        protected readonly VimeoDotNet.Models.User mUser;
        protected readonly VimeoDotNet.Models.UserId mUserId;
        protected readonly VimeoApiTestSettings _vimeoSettings;
        protected readonly long Folder_ID;

        public BaseHelper()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            VimeoSettings = SettingsLoader.LoadSettings();
            AuthenticatedClient = CreateAuthenticatedClient();
            mUserId = 108290443;
            Folder_ID = 3865074;
        }

        protected async Task<VimeoClient> CreateUnauthenticatedClient()
        {

            var authorizationClient = new AuthorizationClient(VimeoSettings.ClientId, VimeoSettings.ClientSecret);
            var unauthenticatedToken = await authorizationClient.GetUnauthenticatedTokenAsync();
            return new VimeoClient(unauthenticatedToken.AccessToken);
        }

        protected IVimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(VimeoSettings.AccessToken);
        }
    }
}
