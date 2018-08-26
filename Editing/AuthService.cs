using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editing.Login;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace Editing
{
    class AuthService
    {
        private static AuthService _instance;
        public AuthService instance
        {
            get
            {
                if (null==_instance)
                {
                    _instance = new AuthService();
                }
                return _instance;
            }
        }

        private MockStore _mockstore = new MockStore();

        public Guid GetUserId(string username)
        {
            return _mockstore.GetUserId(username);
        }
        public UserAccount GetUserAccount(Guid userId)
        {
            return _mockstore.GetUserAccount(userId);
        }

        public List<UserAccount> GetUserAccountsForDevice(Guid deviceId)
        {
            return _mockstore.GetUserAccountsForDevice(deviceId);
        }

        public void Register(string username)
        {
            _mockstore.AddAccount(username);
        }

        public bool RemoveUser(Guid userId)
        {
            return _mockstore.RemoveAccount(userId);
        }

        public bool RemoveDevice(Guid userId,Guid deviceId)
        {
            return _mockstore.RemoveDevice(userId,deviceId);
        }

        public void UpdateDetails(Guid userId, Guid DeviceId,byte[] publicKey,KeyCredentialAttestationResult keyAttestationResult)
        {
            _mockstore.deviceupdateDetails(userId,DeviceId,publicKey,keyAttestationResult);
        }

        public bool ValidationCredentials(string username,string password)
        {
            Guid userId = GetUserId(username);
            if (userId != Guid.Empty)
            {
                UserAccount account = GetUserAccount(userId);
                if (account != null)
                {
                    if (string.Equals(password,account.Password))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public IBuffer RequestChallenge()
        {
            return CryptographicBuffer.ConvertStringToBinary("ServerChallange", BinaryStringEncoding.Utf8);
        }

        public bool SendServerSignedChallenge(Guid userId,Guid deviceId, byte[] signedChallenge)
        {
            byte[] userPublicKey = _mockstore.GetPublicKey(userId,deviceId);
            return true;
        }

        private AuthService()
        {

        }
    }
}
