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
using Windows.Media.Capture;
using Windows.Media.MediaProperties;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaCapture _capture = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            rdbPhoto.IsChecked = true;
            _capture = (App.Current as App).AppMediaCapture;
            this.capele.Source = _capture;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                // 隱藏系統欄
                await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
                // 強制頁面水平
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Landscape;
            }
            // 開始預覽
            await _capture.StartPreviewAsync();
        }

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // 停止預覽
            await _capture.StopPreviewAsync();
        }

        private void OnRdbtnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string tag = rb.Tag as string;
            if (tag.Equals("1"))
            {
                btnCapPhoto.Visibility = Windows.UI.Xaml.Visibility.Visible;
                btnRec.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                btnCapPhoto.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnRec.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private async void OnCapturePhoto(object sender, RoutedEventArgs e)
        {
            btnCapPhoto.IsEnabled = rdbPhoto.IsEnabled = rdbRecord.IsEnabled = false;

            // 取得照片存放目錄
            StorageFolder cameraRoll = KnownFolders.CameraRoll;
            // 新圖形檔名
            string newFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            // 建立新檔案
            StorageFile newFile = await cameraRoll.CreateFileAsync(newFilename, CreationCollisionOption.ReplaceExisting);
            // 捕捉照片
            await _capture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), newFile);

            btnCapPhoto.IsEnabled = rdbPhoto.IsEnabled = rdbRecord.IsEnabled = true;
        }

        private async void OnStartRecord(object sender, RoutedEventArgs e)
        {
            rdbPhoto.IsEnabled = rdbRecord.IsEnabled = false;

            // 取得視訊目錄
            StorageFolder vdfolder = KnownFolders.VideosLibrary;
            // 新檔名
            string newFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            // 建立新檔案
            StorageFile newFile = await vdfolder.CreateFileAsync(newFilename, CreationCollisionOption.ReplaceExisting);
            // 開始錄制
            await _capture.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), newFile);
            btnRec.Content = "停止錄制";
        }

        private async void OnStopRecord(object sender, RoutedEventArgs e)
        {
            await _capture.StopRecordAsync();
            btnRec.Content = "開始錄制";
            rdbPhoto.IsEnabled = rdbRecord.IsEnabled = true;
        }
    }
}
