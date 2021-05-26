using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Authorization;
using JJCastDemo.Vimeo.Setting;
using System;

namespace JJCastDemo.Vimeo
{
    public class AuthorizationClientAsyncTests
    {
        private readonly VimeoApiTestSettings _vimeoSettings;

        public AuthorizationClientAsyncTests()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            _vimeoSettings = SettingsLoader.LoadSettings();
        }
        
        public async Task ShouldCorrectlyGetUnauthenticatedToken()
        {
            try
            {
                var client = new AuthorizationClient(_vimeoSettings.ClientId, _vimeoSettings.ClientSecret);

                var token = await client.GetUnauthenticatedTokenAsync();
                token.AccessToken.ShouldNotBeNull();
                token.User.ShouldBeNull();
            }
            catch(Exception e)
            { }
        }
        
        public async Task VerifyAuthenticatedAccess()
        {
            try
            {
                var client = new AuthorizationClient(_vimeoSettings.ClientId, _vimeoSettings.ClientSecret);

                var b = await client.VerifyAccessTokenAsync(_vimeoSettings.AccessToken);
                b.ShouldBeTrue();
                b = await client.VerifyAccessTokenAsync("abadaccesstoken");
                b.ShouldBeFalse();
            }
            catch (Exception e)
            { }
        }
    }
}
