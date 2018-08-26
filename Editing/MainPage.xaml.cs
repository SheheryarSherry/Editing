using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Threading;
using Editing.Login;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Editing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private UserAccount _account;
        private bool _isExistingAccount;
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckasync())
            {
                if (e.Parameter!=null)
                {
                    _isExistingAccount = true;
                    _account =(UserAccount)e.Parameter;
                    UsernameTextBox.Text = _account.Username;
                    
                }
            }
        }
        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewProject));
        }

        
    }
}
