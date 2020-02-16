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
using Windows.UI.Xaml.Media.Animation;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Storyboard m_storyboard = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            // 起始化示範圖板
            m_storyboard = new Storyboard();
            // 定義時間線
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = TimeSpan.FromSeconds(2d); //時間線持續時間
            da.From = 0d; //初值
            da.To = 360d; //最終值
            // 設定為無限循環播放
            da.RepeatBehavior = RepeatBehavior.Forever;
            // 將時間線與RotateTransform物件關聯
            Storyboard.SetTarget(da, rttrf);
            Storyboard.SetTargetProperty(da, "Angle");
            // 將時間線加入Storyboard的時間線集合中
            m_storyboard.Children.Add(da);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 開始播放動畫
            m_storyboard.Begin();
        }
    }
}
