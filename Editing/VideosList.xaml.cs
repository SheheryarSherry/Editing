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
using Windows.Storage;
using Windows.UI.Xaml.Input;
using Windows.Media.Playback;
using Windows.Media.Core;
using System.Threading.Tasks;
using Windows.Media.Transcoding;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.Media.Editing;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.AccessCache;
using Windows.Media.FaceAnalysis;
using System.Threading;
using Windows.System.Threading;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Media.Capture.Frames;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Editing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideosList : Page
    {
        private MediaStreamSource mediaStreamSource;
        public VideosList()
        {
            this.InitializeComponent();
        }
        private MediaComposition composition = new MediaComposition();

        MediaElement DynMelement;



        GridViewItem GVI;

        private StorageItemAccessList storageItemAccessList;
        // private MainPage rootPage ;
        //private MediaComposition composition;
        private StorageFile pickedFile;

        private StorageFile secondVideoFile;
        private IRandomAccessStream writeStream;
        private void bckbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddVideo));
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            mediaElement1.Source = null;
            mediaStreamSource = null;
            base.OnNavigatedFrom(e);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            // Make sure we don't run out of entries in StoreItemAccessList.
            // As we don't need to persist this across app sessions/pages
            // every time should be sufficient for us.
            storageItemAccessList = StorageApplicationPermissions.FutureAccessList;
            storageItemAccessList.Clear();
        }
        private async void  Button_Click(object sender, RoutedEventArgs e)
        {
           

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            pickedFile = await picker.PickSingleFileAsync();
            var storageItemAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
            storageItemAccessList.Add(pickedFile);

            var clip = await MediaClip.CreateFromFileAsync(pickedFile);

            if (pickedFile == null)
            {
                return;
            }


            else
            {
                composition = new MediaComposition();
                composition.Clips.Add(clip);
                mediaElement1.Position = TimeSpan.Zero;
                mediaStreamSource = composition.GeneratePreviewMediaStreamSource(500,200);
                mediaElement1.SetMediaStreamSource(mediaStreamSource);
                import_btn.IsEnabled = false;
            }

        }
        public async Task<string> GetFileSize(StorageFile file)
        {
            var basicProperties = await  file.GetBasicPropertiesAsync();
            var length = basicProperties.Size;
            return Calculatesize(length);
        }

        public string Calculatesize(double sizeInBytes)
        {
            const double terabyte = 1099511627776;
            const double gigabyte = 1073741824;
            const double megabyte = 1048576;
            const double kilobyte = 1024;

            string result;
            double theSize = 0;
            string units;

            if (sizeInBytes <= 0.1)
            {
                result = "0" + " " + "bytes";
                return result;
            }

            if (sizeInBytes >= terabyte)
            {
                theSize = sizeInBytes / terabyte;
                units = " TB";
            }
            else
            {
                if (sizeInBytes >= gigabyte)
                {
                    theSize = sizeInBytes / gigabyte;
                    units = " GB";
                }
                else
                {
                    if (sizeInBytes >= megabyte)
                    {
                        theSize = sizeInBytes / megabyte;
                        units = " MB";
                    }
                    else
                    {
                        if (sizeInBytes >= kilobyte)
                        {
                            theSize = sizeInBytes / kilobyte;
                            units = " KB";
                        }
                        else
                        {
                            theSize = sizeInBytes;
                            units = " bytes";
                        }
                    }
                }
            }
            return theSize +" "+ units;
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            secondVideoFile = await picker.PickSingleFileAsync();
            txtfilename1.Text = secondVideoFile.DisplayName;
            txtfps1.Text = await GetFileSize(secondVideoFile);
            txtsize.Text = secondVideoFile.FileType;
            if (secondVideoFile == null)
            {
                // rootPage.NotifyUser("File picking cancelled", NotifyType.ErrorMessage);
                return;
            }
            mediaElement.SetSource(await secondVideoFile.OpenReadAsync(), secondVideoFile.ContentType);
        }

        private async void merge_btn_Click(object sender, RoutedEventArgs e)
        {
            var clip = await MediaClip.CreateFromFileAsync(pickedFile);
            var secondClip = await MediaClip.CreateFromFileAsync(secondVideoFile);

            composition = new MediaComposition();
            composition.Clips.Add(clip);
            composition.Clips.Add(secondClip);

            // Render to MediaElement.
            mediaElement1.Position = TimeSpan.Zero;
            mediaStreamSource = composition.GeneratePreviewMediaStreamSource(500, 200);
            mediaElement1.SetMediaStreamSource(mediaStreamSource);
        }

        private async void savebtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeChoices.Add("MP4 files", new List<string>() { ".mp4" });
            picker.SuggestedFileName = "RenderedComposition.mp4";

            Windows.Storage.StorageFile pickedFile = await picker.PickSaveFileAsync();
            if (pickedFile != null)
            {
                // Call RenderToFileAsync
                var saveOperation = composition.RenderToFileAsync(pickedFile, MediaTrimmingPreference.Precise);

                
                saveOperation.Completed = new AsyncOperationWithProgressCompletedHandler<TranscodeFailureReason, double>(async (info, status) =>
                {
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                    {
                        try
                        {
                            var results = info.GetResults();
                            if (results != TranscodeFailureReason.None || status != AsyncStatus.Completed)
                            {
                                //ShowErrorMessage("Saving was unsuccessful");
                               
                            }
                            else
                            {
                                // ShowErrorMessage("Trimmed clip saved to file");
                                
                                mediaElement.Source = null;
                                savebtn.IsEnabled = false;
                            }
                        }
                        finally
                        {

                        }

                    }));
                });
            }
            
        }

     

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExportVideo));
        }
    }
}
