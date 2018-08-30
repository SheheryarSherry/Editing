using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace Editing.Utils
{
    class Helper
    {
        public static Guid GetDeviceId()
        {
            EasClientDeviceInformation deviceInfoemation = new EasClientDeviceInformation();
            return deviceInfoemation.Id;
        }
    }
}
