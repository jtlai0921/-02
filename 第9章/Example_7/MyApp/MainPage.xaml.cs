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
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

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

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFile imgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///1.jpg"));
            // 對圖形進行解碼
            using (IRandomAccessStream streamIn = await imgFile.OpenReadAsync())
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, streamIn);
                // 取得像素資料
                PixelDataProvider provd = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, new BitmapTransform(), ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);
                byte[] srcData = provd.DetachPixelData();
                // 灰階處理
                for (int i = 0; i < srcData.Length; i += 4)
                {
                    // 取得各通道的值
                    double b = srcData[i];
                    double g = srcData[i + 1];
                    double r = srcData[i + 2];
                    // 計算平均值
                    double v = (b + g + r) / 3d;
                    // 置換原來的B、G、R值
                    srcData[i] = srcData[i + 1] = srcData[i + 2] = Convert.ToByte(v);
                }
                // 顯示處理後的圖形
                WriteableBitmap wbitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                srcData.CopyTo(wbitmap.PixelBuffer);
                this.imgGray.Source = wbitmap;
            }
        }
    }
}
