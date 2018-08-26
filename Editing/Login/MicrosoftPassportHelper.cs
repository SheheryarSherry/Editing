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
    }
}
