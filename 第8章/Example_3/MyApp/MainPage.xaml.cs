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


namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Storyboard storybd = null;
        bool isPaused = false;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            // 從資源中找到Storyboard案例
            storybd = this.Resources["std"] as Storyboard;
        }

        private void OnPlay(object sender, RoutedEventArgs e)
        {
            storybd.Begin();
            isPaused = false;
        }

        private void OnPauseAndResume(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                storybd.Resume();
                isPaused = false;
            }
            else
            {
                storybd.Pause();
                isPaused = true;
            }
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
            storybd.Stop();
            isPaused = false;
        }

    }
}
