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
            btnStop.IsEnabled = false;
        }

        private MediaCapture m_Capture = null;

        private async void OnStart(object sender, RoutedEventArgs e)
        {
            if (App.IsRecording == false)
            {
                try
                {
                    m_Capture = new MediaCapture();
                    MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
                    // 取得ScreenCapture案例
                    ScreenCapture scrcapture = ScreenCapture.GetForCurrentView();
                    // 設定起始化參數
                    settings.AudioSource = scrcapture.AudioSource;
                    settings.VideoSource = scrcapture.VideoSource;
                    // 同時捕捉視訊和音頻
                    settings.StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo;
                    // 起始化MediaCapture物件
                    await m_Capture.InitializeAsync(settings);
                    // 取得視訊庫目錄
                    StorageFolder vdlib = KnownFolders.VideosLibrary;
                    // 建立新檔案
                    string newFileName = DateTime.Now.ToString("yyyy_M_d_HHmmss") + ".mp4";
                    StorageFile newVdFile = await vdlib.CreateFileAsync(newFileName, CreationCollisionOption.ReplaceExisting);
                    // 開始錄制
                    await m_Capture.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), newVdFile);
                    App.IsRecording = true;
                    App.CurrentCapture = m_Capture;

                    btnStart.IsEnabled = false;
                    btnStop.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private async void OnStop(object sender, RoutedEventArgs e)
        {
            if (App.IsRecording)
            {
                // 停止錄制
                await m_Capture.StopRecordAsync();
                App.IsRecording = false;
                btnStop.IsEnabled = false;
                btnStart.IsEnabled = true;
            }
        }

    }
}
