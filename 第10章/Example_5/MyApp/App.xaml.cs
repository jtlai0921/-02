using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using System.Threading.Tasks;

namespace MyApp
{
    /// <summary>
    /// 提供特定於套用程式的行為，以補充預設的套用程式類別。
    /// </summary>
    public sealed partial class App : Application
    {

        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        #region 屬性
        public static MediaCapture CurrentCapture { get; set; }
        public static bool IsRecording { get; set; }
        #endregion

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在視窗已包括內容時重復套用程式起始化，
            // 只需確保視窗處於活動狀態
            if (rootFrame == null)
            {
                // 建立要充當導覽上下文的框架，並導覽到第一頁
                rootFrame = new Frame();

                // TODO: 將此值變更為適合您的套用程式的快取大小
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: 從之前暫停的套用程式載入狀態
                }

                // 將框架放在目前視窗中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 當導覽堆堆疊尚未復原時，導覽到第一頁，
                // 並透過將所需訊息作為導覽參數傳入來組態
                // 新頁面
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // 確保目前視窗處於活動狀態
            Window.Current.Activate();
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: 儲存套用程式狀態並停止任何背景活動
            await CleanupCapture();
            deferral.Complete();
        }

        #region 私有方法

        /// <summary>
        /// 清理捕捉元件
        /// </summary>
        private async Task CleanupCapture()
        {
            if (CurrentCapture != null)
            {
                // 若果正在錄制，則停止錄制
                if (IsRecording)
                {
                    await CurrentCapture.StopRecordAsync();
                    IsRecording = false;
                }
                CurrentCapture.Dispose();
                CurrentCapture = null;
            }
        }
        #endregion
    }
}