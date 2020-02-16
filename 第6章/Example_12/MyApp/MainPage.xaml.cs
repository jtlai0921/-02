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

            string[] data = 
            {
                "王三", "王靖", "張一合", "張六", "張東", "李兆", "李騰梁", "崔正", "崔可紀", "崔吾",
                "李山", "何小小", "張玉", "周源星", "周樂", "周瑩", "馬紅", "馬宇", "馬小波", "馬德",
                "方許許", "方宣", "方燕", "方為", "方歷", "趙明", "趙小英", "趙雷", "趙生", "趙迕克",
                "張凡凡", "馮雲兒", "李蓴", "李於山", "馮一裡", "馮俊", "唐獻", "唐啟昔", "唐肅", "唐旭"
            };
            var groups = from s in data group s by s[0];
            this.cvsData.Source = groups;
        }

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: 準備此處顯示的頁面。

            // TODO: 若果您的套用程式包括多個頁面，請確保
            // 透過登錄以下事件來處理硬體“後退”按鈕:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed 事件。
            // 若果使用由某些範本提供的 NavigationHelper，
            // 則系統會為您處理該事件。
        }
    }
}
