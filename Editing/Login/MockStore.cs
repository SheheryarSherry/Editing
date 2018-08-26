using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Editing.Login
{
    class MockStore
    {
        private const string USER_ACCOUNT_LIST_FILE_NAME = "userAccountsList.txt";
        private string _userAccountListPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, USER_ACCOUNT_LIST_FILE_NAME);
        private List<UserAccount> _mockDatabaseUserAccountList;

        private async void SaveAccountAsync()
        {
            string accountsXml = serializeAccountListXml();
            if (File.Exists(_userAccountListPath))
            {
                StorageFile accountsFile = await StorageFile.GetFileFromPathAsync(_userAccountListPath);
                await FileIO.WriteTextAsync(accountsFile,accountsXml);
            }
            else
            {
                StorageFile accountFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(USER_ACCOUNT_LIST_FILE_NAME);
                await FileIO.WriteTextAsync(accountFile,accountsXml);
            }
        }

        private async void LoadAccountListAsync()
        {

            if (File.Exists(_userAccountListPath))
            {
                StorageFile accountsfile = await StorageFile.GetFileFromPathAsync(_userAccountListPath);
                string accountsXml = await FileIO.ReadTextAsync(accountsfile);
                DeserializeXmlToAccountList(accountsXml);
            }
            if (!_mockDatabaseUserAccountList.Any(f => f.Username.Equals("Username")))
            {
                InitializeSampleUserAccounts();
            }
        }

        private void InitializeSampleUserAccounts()
        {
            UserAccount SampleUserAccount = new UserAccount()
            {
                UserId = Guid.NewGuid(),
                Username = "Username",
                Password = "password",
            };
            _mockDatabaseUserAccountList.Add(SampleUserAccount);
            SaveAccountAsync();

        }
    
        public Guid GetUserId(string username)
        {
            if (_mockDatabaseUserAccountList.Any())
            {
                UserAccount account = _mockDatabaseUserAccountList.FirstOrDefault(f=>f.Username.Equals(username));
                if (account != null)
                {
                    return account.UserId;
                }
            }
            return Guid.Empty;
        }
        public UserAccount GetUserAccount(Guid userId)
        {
            return _mockDatabaseUserAccountList.FirstOrDefault(f => f.UserId.Equals(userId));
        }

        public List<UserAccount> GetUserAccountsForDevice(Guid deviceId)
        {
            List<UserAccount> usersForDevice = new List<UserAccount>();
            foreach (UserAccount account in _mockDatabaseUserAccountList)
            {
                if (account.devices.Any(f=>f.DeviceId.Equals(deviceId)))
                {
                    usersForDevice.Add(account);
                }
            }
            return usersForDevice;
        }

        public byte[] GetPublicKey(Guid userId ,Guid deviceId)
        {
            UserAccount account = _mockDatabaseUserAccountList.FirstOrDefault(f=>f.UserId.Equals(userId));
            if (account != null)
            {
                if (account.devices.Any())
                {
                    return account.devices.FirstOrDefault(p=>p.DeviceId.Equals(deviceId)).publicKey;
                }
            }
            return null;
        }

        private List<UserAccount> DeserializeXmlToAccountList(string listAsXml)
        {
            XmlSerializer xmlizer = new XmlSerializer(typeof(List<UserAccount>));
            TextReader textreader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(listAsXml)));
            return _mockDatabaseUserAccountList = (xmlizer.Deserialize(textreader)) as List<UserAccount>;
        }

        private string serializeAccountListXml()
        {
            XmlSerializer xmlizer = new XmlSerializer(typeof(List<UserAccount>));
            StringWriter writer = new StringWriter();
            xmlizer.Serialize(writer,_mockDatabaseUserAccountList);
            return writer.ToString();
        }

        public UserAccount AddAccount(string username)
        {
            UserAccount newAccount = null;
            try
            {
                newAccount = new UserAccount()
                {
                    UserId = Guid.NewGuid(),
                    Username = username,

                };
                _mockDatabaseUserAccountList.Add(newAccount);
                SaveAccountAsync();
            }
            catch (Exception)
            {
             
                throw;
            }
            return newAccount;
        }

        public bool RemoveAccount(Guid userId)
        {
            UserAccount userAccount = GetUserAccount(userId);
            if (userAccount != null)
            {
                _mockDatabaseUserAccountList.Remove(userAccount);
                SaveAccountAsync();
                return true;
            }
            return false;
        }

        public bool RemoveDevice(Guid userId, Guid deviceId)
        {
            UserAccount userAccount = GetUserAccount(userId);
            Device devicetoRemove = null;

            if (userAccount != null)
            {
                foreach (Device device in userAccount.devices)
                {
                    if (device.DeviceId.Equals(deviceId))
                    {
                        devicetoRemove = device;
                        break;
                    }
                }
            }
            if (devicetoRemove != null)
            {
                userAccount.devices.Remove(devicetoRemove);
                SaveAccountAsync();
            }


            return true;
        }
        public void deviceupdateDetails(Guid userId,Guid deviceId, byte[] publicKey, KeyCredentialAttestationResult keyAttestationResult)
        {
            UserAccount existinguserAccount = GetUserAccount(userId);
            if (existinguserAccount != null)
            {
                if (!existinguserAccount.devices.Any(f=> f.DeviceId.Equals(deviceId)))
                {
                    existinguserAccount.devices.Add(new Device()
                    {
                        DeviceId = deviceId,
                        publicKey = publicKey,
                    });
                }
            }
            SaveAccountAsync();
        }
     }
}
