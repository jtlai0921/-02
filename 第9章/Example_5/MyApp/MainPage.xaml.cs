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
using Windows.Graphics.Imaging;

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


        private async void OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            //專案中的圖形檔案
            Uri gifuri = new Uri("ms-appx:///1.gif");
            StorageFile gifFile = await StorageFile.GetFileFromApplicationUriAsync(gifuri);
            using (IRandomAccessStream inputStream = await gifFile.OpenReadAsync())
            {
                // 建立圖形解碼器案例
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.GifDecoderId, inputStream);
                // 取得圖形檔案中的框總數
                uint frameCount = decoder.FrameCount;
                // 分別讀取各個框中圖形，並新增到ListView控制項中
                lvimgs.Items.Clear();
                for (uint n = 0; n < frameCount; n++)
                {
                    BitmapFrame curFrame = await decoder.GetFrameAsync(n);
                    // 取得圖形的像素資料
                    // 在取得資料時，對圖形進行變換處理
                    // 將寬度和高度變為原來的1/3
                    BitmapTransform btf = new BitmapTransform();
                    btf.ScaledWidth = curFrame.PixelWidth / 3;
                    btf.ScaledHeight = curFrame.PixelHeight / 3;
                    var pxprd = await curFrame.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, btf, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);
                    byte[] data = pxprd.DetachPixelData();
                    // 建立記憶體點陣圖物件
                    WriteableBitmap bmp = new WriteableBitmap((int)btf.ScaledWidth, (int)btf.ScaledHeight);
                    data.CopyTo(bmp.PixelBuffer);
                    this.lvimgs.Items.Add(bmp);
                }
            }
            btn.IsEnabled = true;
        }
    }
}
