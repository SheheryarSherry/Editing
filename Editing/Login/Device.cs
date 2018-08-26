using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.Login
{
    class Device
    {
        public Guid DeviceId { get; set; }
        public byte[] publicKey { get; set; }
    }
}
