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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 從套用專案中取得圖形檔案
            StorageFile imgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///test.jpeg"));
            // 開啟檔案流
            IRandomAccessStream inputStream = await imgFile.OpenReadAsync();
            // 案例化點陣圖物件
            BitmapImage bitmap = new BitmapImage();
            // 設定圖形來源
            bitmap.SetSource(inputStream);
            // 將記憶體圖形與Image控制項關聯
            this.img.Source = bitmap;
        }

    }
}
