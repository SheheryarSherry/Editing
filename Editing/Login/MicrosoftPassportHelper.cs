using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Security.Credentials;

namespace Editing.Login
{
    class MicrosoftPassportHelper
    {
        public static async Task<bool> MicrosoftPassportAvailableCheckasync()
        {
            bool keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
            if (keyCredentialAvailable == false)
            {
                Debug.WriteLine("device is not setup go to setting and setup");

            }
            return false;
        }

        //public static async Task<bool> CreatePassportKeyAsync(Guid userId, string username)
        //{
        //    KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync(username, KeyCredentialCreationOption.ReplaceExisting);
        //}

        //public static async void RemovePassportAccountAsync(UserAccount userAccount)
        //{

        //}
        //public static async Task<bool> GetPassportAuthenticationMessageAsync(UserAccount account)
        //{
        //    KeyCredentialRetrievalResult openKeyResult = await KeyCredentialManager.OpenAsync(account.Username);
        //    if (openKeyResult.Status == KeyCredentialStatus.Success)
        //    {
        //        return true;
        //    }
        //    else if(openKeyResult.Status == KeyCredentialStatus.NotFound)
        //    {
        //        if (await CreatePassportKeyAsync(account.UserId,account.Username))
        //        {
        //            return await GetPassportAuthenticationMessageAsync(account);
        //        }
        //    }
        //    return false;
        //}


    }
}
