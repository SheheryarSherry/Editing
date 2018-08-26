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
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.AccessCache;
using Windows.Media.Editing;
using Windows.Storage.FileProperties;
using Windows.Media.Transcoding;
using Windows.Media.Core;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Editing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddVideo : Page
    {


        public AddVideo()
        {
            this.InitializeComponent();
        }
        private StorageItemAccessList storageItemAccessList;
        private MediaComposition Composition;
        private StorageFile pickedFile;
        private MediaStreamSource mediaStreamSource;

        private async void AddmusicButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VideosList));

        }

        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
    }
}
