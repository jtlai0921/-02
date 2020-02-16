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
using Windows.Devices.Enumeration;
using System.Threading.Tasks;

namespace MyApp
{
    /// <summary>
    /// 提供特定於套用程式的行為，以補充預設的套用程式類別。
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// 起始化單一案例套用程式物件。    這是執行的創作程式碼的第一行，
        /// 邏輯上等同於 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.Resuming += this.OnResuming;
        }

        async void OnResuming(object sender, object e)
        {
            await InitializeCapture();
        }

        /// <summary>
        /// 存取MediaCapture物件
        /// </summary>
        public MediaCapture AppMediaCapture { get; private set; }

        /// <summary>
        /// 在套用程式由最終使用者標準啟動時進行呼叫。
        /// 當啟動套用程式以開啟特定的檔案或顯示搜尋結果等動作時，
        /// 將使用其他入口點。
        /// </summary>
        /// <param name="e">有關啟動請求和過程的詳細訊息。</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await this.InitializeCapture();
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
                // 移除用於啟動的旋轉門導覽。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

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

        /// <summary>
        /// 啟動套用程式後復原內容轉換。
        /// </summary>
        /// <param name="sender">附加了處理程式的物件。</param>
        /// <param name="e">有關導覽事件的詳細訊息。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// 在將要暫停套用程式執行時呼叫。    在不知道套用程式
        /// 將被終止還是還原的情況下儲存套用程式狀態，
        /// 並讓記憶體內容保持不變。
        /// </summary>
        /// <param name="sender">暫停的請求的源。</param>
        /// <param name="e">有關暫停的請求的詳細訊息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: 儲存套用程式狀態並停止任何背景活動
            CleanupCapture();

            deferral.Complete();
        }


        #region 私有方法
        /// <summary>
        /// 起始化捕捉元件
        /// </summary>
        private async Task InitializeCapture()
        {
            // 案例化MediaCapture
            this.AppMediaCapture = new MediaCapture();
            // 查詢後置攝影機
            DeviceInformationCollection dvcls = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            DeviceInformation backCamera = dvcls.FirstOrDefault(d => d.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
            if (backCamera == null)
            {
                throw new NullReferenceException("沒有找到後置攝影機");
            }
            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
            // 參考後置攝影機的ID
            settings.VideoDeviceId = backCamera.Id;
            // 不使用音頻輸入裝置
            settings.AudioDeviceId = "";
            // 僅捕捉視訊
            settings.StreamingCaptureMode = StreamingCaptureMode.Video;
            // 起始化MediaCapture物件
            await AppMediaCapture.InitializeAsync(settings);
            System.Diagnostics.Debug.WriteLine("捕捉元件已經起始化。");
        }

        /// <summary>
        /// 清理捕捉元件
        /// </summary>
        private void CleanupCapture()
        {
            if (AppMediaCapture != null)
            {
                AppMediaCapture.Dispose();
                AppMediaCapture = null;
                System.Diagnostics.Debug.WriteLine("捕捉元件已經釋放。");
            }
        }
        #endregion
    }
}