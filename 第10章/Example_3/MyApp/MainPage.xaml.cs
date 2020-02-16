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
using Windows.Media.Transcoding;
using Windows.Media.MediaProperties;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void OnTransCode(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;

            // 從專案目錄中取得待轉碼的MP3檔案
            Uri sourceFileUri = new Uri("ms-appx:///1.mp3");
            StorageFile srcFile = await StorageFile.GetFileFromApplicationUriAsync(sourceFileUri);
            // 取得“音樂”檔案目錄
            StorageFolder musicFd = KnownFolders.MusicLibrary;
            // 新檔名
            string newFileName = "new_file.m4a";
            // 建立新檔案
            StorageFile disFile = await musicFd.CreateFileAsync(newFileName, CreationCollisionOption.ReplaceExisting);
            
            // 響應轉碼處理進度
            IProgress<double> progress = new Progress<double>((p) =>
            {
                // 更新進度
                this.pb.Value = p;
            });

            // 案例化MediaTranscoder物件
            MediaTranscoder transcoder = new MediaTranscoder();
            PrepareTranscodeResult result = await transcoder.PrepareFileTranscodeAsync(srcFile, disFile, MediaEncodingProfile.CreateM4a(AudioEncodingQuality.High));
            // 開始轉碼
            if (result.CanTranscode)
            {
                await result.TranscodeAsync().AsTask(progress);
            }

            btn.IsEnabled = true;
        }
    }
}
